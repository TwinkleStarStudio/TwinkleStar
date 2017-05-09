using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using TwinkleStar.Common.Others;
using TwinkleStar.Data.Extensions;

namespace TwinkleStar.Data
{
    /// <summary>
    /// DbContext上下文仓储功能类，领域上下文可以直接继承它
    /// 生命周期：数据上下文的生命周期为一个HTTP请求的结束
    /// 相关说明：
    /// 1 领域对象使用声明IRepository和IExtensionRepository接口得到不同的操作规范
    /// 2 可以直接为上下注入Action<string>的委托实例，用来记录savechanges产生的异常
    /// 3 可以订阅BeforeSaved和AfterSaved两个事件，用来在方法提交前与提交后实现代码注入
    /// 4 所有领域db上下文都要继承iUnitWork接口，用来实现工作单元，这对于提升程序性能与为重要
    /// </summary>
    
    public abstract class DbContextRepository<TEntity> :
         ISpecificationRepository<TEntity>
         where TEntity : EntityBase
    {
        /// <summary>
        ///     获取 仓储上下文的实例
        /// </summary>
        [Import]
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        ///     获取 EntityFramework的数据仓储上下文
        /// </summary>
        protected UnitOfWorkContextBase EFContext
        {
            get
            {
                if (UnitOfWork is UnitOfWorkContextBase)
                {
                    return UnitOfWork as UnitOfWorkContextBase;
                }
                throw new DataAccessException(string.Format("数据仓储上下文对象类型不正确，应为UnitOfWorkContextBase，实际为 {0}", UnitOfWork.GetType().Name));
            }
        }

        #region Constructors
        ///使用IOC注入， 不需要构造
        //public DbContextRepository(IUnitOfWork db, Action<string> logger)
        //{
        //    UnitWork = db;
        //    Db = (DbContext)db;
        //    Logger = logger;
        //    ((IObjectContextAdapter)Db).ObjectContext.CommandTimeout = 0;
        //}

        //public DbContextRepository(IUnitOfWork db)
        //    : this(db, null)
        //{ }
        #endregion

        #region Properties
        /// <summary>
        /// 数据上下文
        /// </summary>
        protected DbContext Db
        {
            get
            {
                return this.EFContext.DbContext;
            }
        }

        #endregion

        #region Fields
        /// <summary>
        /// 数据总数
        /// </summary>
        int _dataTotalCount = 0;

        /// <summary>
        /// 数据总页数
        /// </summary>
        int _dataTotalPages = 0;

        /// <summary>
        /// 数据页面大小（每次向数据库提交的记录数）
        /// </summary>
        private const int DataPageSize = 10000;

        #endregion

        #region Delegates & Event
        /// <summary>
        /// 保存之后
        /// </summary>
        public event Action<SavedEventArgs> AfterSaved;
        /// <summary>
        /// 保存之前
        /// </summary>
        public event Action<SavedEventArgs> BeforeSaved;
        #endregion

        public int BatchDelete(IEnumerable<TEntity> item)
        {
            DataPageProcess(item, (currentsItems) =>
            {
                ((IObjectContextAdapter)Db).ObjectContext.CommandTimeout = 0;
                Db.Database.ExecuteSqlCommand(DoSql(currentsItems, SqlType.Delete));
            });
            return item.Count();
        }

        public int BatchInsert(IEnumerable<TEntity> items)
        {
            return BatchInsert(items, false);
        }

        public int BatchInsert(IEnumerable<TEntity> items, bool isRemoveIdentity)
        {
            string startTag = "", endTag = "";
            if (isRemoveIdentity)
            {
                startTag = "SET IDENTITY_INSERT " + typeof(TEntity).Name + " ON;";
                endTag = "SET IDENTITY_INSERT " + typeof(TEntity).Name + "  OFF;";
            }
            DataPageProcess(items, (currentItems) =>
            {
                ((IObjectContextAdapter)Db).ObjectContext.CommandTimeout = 0;//永不超时
                Db.Database.ExecuteSqlCommand(startTag
                    + DoSql(currentItems, SqlType.Insert)
                    + endTag);
            });

            return items.Count();
        }

        public int BatchUpdate(IEnumerable<TEntity> item, params string[] fieldParams)
        {
            DataPageProcess(item, (currentItems) =>
            {
                ((IObjectContextAdapter)Db).ObjectContext.CommandTimeout = 0;//永不超时
                Db.Database.ExecuteSqlCommand(DoSql(currentItems, SqlType.Update, fieldParams));
            });

            return item.Count();
        }

        public virtual int Delete(TEntity item)
        {
            OnBeforeSaved(new SavedEventArgs(item, SaveAction.Delete));
            Db.Set<TEntity>().Attach(item);
            Db.Set<TEntity>().Remove(item);
            this.SaveChanges();
            OnAfterSaved(new SavedEventArgs(item, SaveAction.Delete));

            return 1;
        }

        public virtual int Delete(IEnumerable<TEntity> items)
        {
            items.ToList().ForEach(i =>
            {
                Db.Set<TEntity>().Attach(i);
                Db.Set<TEntity>().Remove(i);
            });
            this.SaveChanges();
            return 1;
        }

        public TEntity Find(params object[] id)
        {
            return Db.Set<TEntity>().Find(id);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return GetModel(predicate).FirstOrDefault();
        }

        public TEntity Find(ISpecification<TEntity> specification)
        {
            return GetModel(specification).FirstOrDefault();
        }

        public IQueryable<TEntity> GetModel()
        {
            return Db.Set<TEntity>();////ObjectStateManager 中已存在具有同一键的对象。ObjectStateManager 无法跟踪具有相同键的多个对象。
        }

        /// <summary>
        /// 得到原生态结果集
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetEntities()
        {
            return Db.Set<TEntity>();
        }

        public IQueryable<TEntity> GetModel(Action<IOrderable<TEntity>> orderBy)
        {
            var linq = new Orderable<TEntity>(GetModel());
            orderBy(linq);
            return linq.Queryable;
        }

        public IQueryable<TEntity> GetModel(Expression<Func<TEntity, bool>> predicate)
        {
            return GetModel().Where(predicate);
        }

        public IQueryable<TEntity> GetModel(ISpecification<TEntity> specification)
        {
            return GetModel().Where(specification.Expression);
        }

        public IQueryable<TEntity> GetModel(Action<IOrderable<TEntity>> orderBy, Expression<Func<TEntity, bool>> predicate)
        {
            var linq = new Orderable<TEntity>(GetModel(predicate));
            orderBy(linq);
            return linq.Queryable;
        }

        public IQueryable<TEntity> GetModel(Action<IOrderable<TEntity>> orderBy, ISpecification<TEntity> specification)
        {
            var linq = new Orderable<TEntity>(GetModel(specification));
            orderBy(linq);
            return linq.Queryable;
        }

        public virtual int Insert(TEntity item)
        {
            OnBeforeSaved(new SavedEventArgs(item, SaveAction.Insert));
            Db.Entry<TEntity>(item);
            Db.Set<TEntity>().Add(item);
            this.SaveChanges();
            OnAfterSaved(new SavedEventArgs(item, SaveAction.Insert));

            return 1;
        }

        public virtual int Insert(IEnumerable<TEntity> items)
        {
            items.ToList().ForEach(i =>
            {
                Db.Entry<TEntity>(i);
                Db.Set<TEntity>().Add(i);
            });
            this.SaveChanges();

            return items.Count();
        }

        public virtual int Update(TEntity item)
        {
            OnBeforeSaved(new SavedEventArgs(item, SaveAction.Update));

            Db.Set<TEntity>().Attach(item);
            Db.Entry(item).State = EntityState.Modified;
            try
            {
                this.SaveChanges();
            }
            catch (System.Data.OptimisticConcurrencyException)//并发冲突异常
            {
                ((IObjectContextAdapter)Db).ObjectContext.Refresh(RefreshMode.ClientWins, item);
                this.SaveChanges();
            }

            OnAfterSaved(new SavedEventArgs(item, SaveAction.Update));

            return 1;
        }

        public virtual int Update(IEnumerable<TEntity> items)
        {
            items.ToList().ForEach(i =>
            {
                Db.Set<TEntity>().Attach(i);
                Db.Entry(i).State = EntityState.Modified;
            });
            try
            {
                this.SaveChanges();
            }
            catch (System.Data.OptimisticConcurrencyException)//并发冲突异常
            {
                ((IObjectContextAdapter)Db).ObjectContext.Refresh(RefreshMode.ClientWins, items);
                this.SaveChanges();
            }
            return 1;
        }

        public int Update<T>(Expression<Action<T>> experssion) where T : class
        {
            T newEntity = typeof(T).GetConstructor(Type.EmptyTypes).Invoke(null) as T;//建立指定类型的实例
            List<string> propertyNameList = new List<string>();
            MemberInitExpression param = experssion.Body as MemberInitExpression;
            foreach (var item in param.Bindings)
            {
                string propertyName = item.Member.Name;
                object propertyValue;
                var memberAssignment = item as MemberAssignment;
                if (memberAssignment.Expression.NodeType == ExpressionType.Constant)
                {
                    propertyValue = (memberAssignment.Expression as ConstantExpression).Value;
                }
                else
                {
                    propertyValue = Expression.Lambda(memberAssignment.Expression, null).Compile().DynamicInvoke();
                }
                typeof(T).GetProperty(propertyName).SetValue(newEntity, propertyValue, null);
                propertyNameList.Add(propertyName);
            }

            try
            {
                Db.Set<T>().Attach(newEntity);
            }
            catch (Exception)
            {
                throw new Exception("本方法不能和GetModel()一起使用，请使用Update(TEntity entity)方法");
            }

            Db.Configuration.ValidateOnSaveEnabled = false;
            var ObjectStateEntry = ((IObjectContextAdapter)Db).ObjectContext.ObjectStateManager.GetObjectStateEntry(newEntity);
            propertyNameList.ForEach(x => ObjectStateEntry.SetModifiedProperty(x.Trim()));

            try
            {
                this.SaveChanges();

            }
            catch (System.Data.OptimisticConcurrencyException)//并发冲突异常
            {
                ((IObjectContextAdapter)Db).ObjectContext.Refresh(RefreshMode.ClientWins, newEntity);
                this.SaveChanges();
            }

            return 1;
        }

        #region Protected Methods
        /// <summary>
        /// 根据工作单元的IsNotSubmit的属性，去判断是否提交到数据库
        /// 一般地，在多个repository类型进行组合时，这个IsNotSubmit都会设为true，即不马上提交，
        /// 而对于单个repository操作来说，它的值不需要设置，使用默认的false，将直接提交到数据库，这也保证了操作的原子性。
        /// </summary>
        protected void SaveChanges()
        {
            try
            {
                Db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)//捕获实体验证异常
            {
                var sb = new StringBuilder();
                dbEx.EntityValidationErrors.First().ValidationErrors.ToList().ForEach(i =>
                {
                    sb.AppendFormat("属性为：{0}，信息为：{1}\n\r", i.PropertyName, i.ErrorMessage);
                });
                throw new Exception(sb.ToString());
            }
            catch (Exception ex)//捕获所有异常
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        ///  计数更新,与SaveChange()是两个SQL链接，走分布式事务
        ///  子类可以根据自己的逻辑，去复写
        ///  tableName:表名
        ///  param:索引0为主键名，1表主键值，2为要计数的字段，3为增量
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="param">参数列表，索引0为主键名，1表主键值，2为要计数的字段，3为增量</param>
        protected virtual void UpdateForCount(string tableName, params object[] param)
        {
            string sql = "update [" + tableName + "] set [{2}]=ISNULL([{2}],0)+{3} where [{0}]={1}";
            var listParasm = new List<object>
            {
                param[0],
                param[1],
                param[2],
                param[3],
            };
            Db.Database.ExecuteSqlCommand(string.Format(sql, listParasm.ToArray()));
        }
        #endregion

        #region Virtual Methods

        /// <summary>
        ///  Called after data saved
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnAfterSaved(SavedEventArgs e)
        {
            AfterSaved?.Invoke(e);
        }

        /// <summary>
        /// Called before saved
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBeforeSaved(SavedEventArgs e)
        {
            BeforeSaved?.Invoke(e);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 分页进行数据提交的逻辑
        /// </summary>
        /// <param name="item">原列表</param>
        /// <param name="method">处理方法</param>
        private void DataPageProcess(IEnumerable<TEntity> item, Action<IEnumerable<TEntity>> method)
        {
            if (item != null && item.Any())
            {
                _dataTotalCount = item.Count();
                this._dataTotalPages = item.Count() / DataPageSize;
                if (_dataTotalCount % DataPageSize > 0)
                {
                    _dataTotalPages += 1;
                }

                for (int pageIndex = 0; pageIndex <= _dataTotalPages; pageIndex++)
                {
                    var currentItems = item.Skip((pageIndex - 1) * DataPageSize).Take(DataPageSize).ToList();
                    method(currentItems);
                }
            }
        }

        private static string GetEqualStatment(string fieldName, int paramId, Type pkType)
        {
            if (pkType.IsValueType)
            {
                return string.Format("{0} = {1}", fieldName, GetParamTag(paramId));
            }

            return string.Format("{0} = '{1}'", fieldName, GetParamTag(paramId));
        }

        private static string GetParamTag(int paramId)
        {
            return "{" + paramId + "}";
        }

        /// <summary>
        /// 得到实体键EntityKey
        /// </summary>
        /// <typeparam name="EdmMember"></typeparam>
        /// <returns></returns>
        protected ReadOnlyMetadataCollection<EdmMember> GetPrimaryKey()
        {
            EntitySetBase primaryKey = ((IObjectContextAdapter)Db).ObjectContext.GetEntitySet(typeof(TEntity));

            if (primaryKey == null)
                return null;
            ReadOnlyMetadataCollection<EdmMember> arr = primaryKey.ElementType.KeyMembers;
            return arr;
        }

        /// <summary>
        /// 构建Update语句串
        /// 注意：如果本方法过滤了int,decimal类型更新为０的列，如果希望更新它们需要指定FieldParams参数
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldParams">要更新的字段</param>
        /// <returns></returns>
        private Tuple<string, object[]> CreateUpdateSql(TEntity entity, params string[] fieldParams)
        {
            if (entity == null)
            {
                throw new ArgumentException("The database entity can not be null.");
            }
            var pkList = GetPrimaryKey().Select(i => i.Name).ToList();

            var entityType = entity.GetType();
            var tableFields = new List<PropertyInfo>();
            if (fieldParams != null && fieldParams.Count() > 0)
            {
                tableFields = entityType.GetProperties().Where(i => fieldParams.Contains(i.Name, new StringComparisonIgnoreCase<String>())).ToList();
            }
            else
            {
                tableFields = entityType.GetProperties().Where(i =>
                            !pkList.Contains(i.Name)
                            && i.GetValue(entity, null) != null
                            && !i.PropertyType.IsEnum
                            && !(i.PropertyType == typeof(ValueType) && Convert.ToInt64(i.GetValue(entity, null)) == 0)
                            && !(i.PropertyType == typeof(DateTime) && Convert.ToDateTime(i.GetValue(entity, null)) == DateTime.MinValue)
                            && i.PropertyType != typeof(EntityState)
                            //&& i.GetCustomAttributes(false).Where(j => j.GetType() == typeof(NavigationAttribute)) != null//过滤导航属性
                            && (i.PropertyType.IsValueType || i.PropertyType == typeof(string))
                            ).ToList();
            }

            if (pkList == null || pkList.Count == 0)
            {
                throw new ArgumentException("The Table entity have not a primary key.");
            }
            var arguments = new List<object>();
            var builder = new StringBuilder();

            foreach (var change in tableFields)
            {
                if (pkList.Contains(change.Name))
                {
                    continue;
                }
                if (arguments.Count != 0)
                {
                    builder.Append(", ");
                }
                builder.Append(change.Name + "={" + arguments.Count + "}");
                if (change.PropertyType == typeof(string)
                   || change.PropertyType == typeof(DateTime)
                   || change.PropertyType == typeof(DateTime?)
                   || change.PropertyType == typeof(bool?)
                   || change.PropertyType == typeof(bool))
                    arguments.Add("'" + change.GetValue(entity, null).ToString().Replace("'", "char(39)") + "'");
                else
                    arguments.Add(change.GetValue(entity, null));
            }
            if (builder.Length == 0)
                throw new Exception("没有任何属性进行更新");

            builder.Insert(0, " UPDATE " + string.Format("[{0}]", entityType.Name) + " SET ");

            builder.Append(" WHERE ");
            bool firstPrimaryKey = true;

            foreach (var primaryField in pkList)
            {
                if (firstPrimaryKey)
                {
                    firstPrimaryKey = false;
                }
                else
                {
                    builder.Append(" AND ");
                }

                object val = entityType.GetProperty(primaryField).GetValue(entity, null);
                Type pkType = entityType.GetProperty(primaryField).GetType();
                builder.Append(GetEqualStatment(primaryField, arguments.Count, pkType));
                arguments.Add(val);
            }
            return new Tuple<string, object[]>(builder.ToString(), arguments.ToArray());
        }

        /// <summary>
        /// 构建Delete语句串
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Tuple<string, object[]> CreateDeleteSql(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("The database entity can not be null.");
            }

            Type entityType = entity.GetType();
            List<string> pkList = GetPrimaryKey().Select(i => i.Name).ToList();
            if (pkList == null || pkList.Count == 0)
            {
                throw new ArgumentException("The Table entity have not a primary key.");
            }

            var argumengts = new List<object>();
            var builder = new StringBuilder();
            builder.Append(" DELETE FROM " + string.Format("[{0}]", entityType.Name));

            builder.Append(" WHERE ");
            bool firstPrimaryKey = true;
            foreach (var primaryField in pkList)
            {
                if (firstPrimaryKey)
                {
                    firstPrimaryKey = false;
                }
                else
                {
                    builder.Append(" AND ");
                }

                Type pkType = entityType.GetProperty(primaryField).GetType();
                object val = entityType.GetProperty(primaryField).GetValue(entity, null);
                builder.Append(GetEqualStatment(primaryField, argumengts.Count, pkType));
                argumengts.Add(val);
            }

            return new Tuple<string, object[]>(builder.ToString(), argumengts.ToArray());
        }

        /// <summary>
        /// 构建Insert语句串
        /// 主键为自增时，如果主键值为0，我们将主键插入到SQL串中
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Tuple<string, object[]> CreateInsertSql(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("The database entity can not be null.");

            Type entityType = entity.GetType();
            var table = entityType.GetProperties().Where(i => i.PropertyType != typeof(EntityKey)
                 && i.PropertyType != typeof(EntityState)
                 && i.Name != "IsValid"
                 && i.GetValue(entity, null) != null
                 && !i.PropertyType.IsEnum
                 //&& i.GetCustomAttributes(false).Where(j => j.GetType() == typeof(NavigationAttribute)) != null
                 && (i.PropertyType.IsValueType || i.PropertyType == typeof(string))).ToArray();//过滤主键，航行属性，状态属性等

            var pkList = new List<string>();
            if (GetPrimaryKey() != null)//有时主键可能没有设计，这对于添加操作是可以的
                pkList = GetPrimaryKey().Select(i => i.Name).ToList();
            var arguments = new List<object>();
            var fieldbuilder = new StringBuilder();
            var valuebuilder = new StringBuilder();

            fieldbuilder.Append(" INSERT INTO " + string.Format("[{0}]", entityType.Name) + " (");

            foreach (var member in table)
            {
                if (pkList.Contains(member.Name) && Convert.ToString(member.GetValue(entity, null)) == "0")
                    continue;
                object value = member.GetValue(entity, null);
                if (value != null)
                {
                    if (arguments.Count != 0)
                    {
                        fieldbuilder.Append(", ");
                        valuebuilder.Append(", ");
                    }

                    fieldbuilder.Append(member.Name);
                    if (member.PropertyType == typeof(string)
                        || member.PropertyType == typeof(DateTime)
                        || member.PropertyType == typeof(DateTime?)
                        || member.PropertyType == typeof(bool?)
                        || member.PropertyType == typeof(bool)
                        )
                        valuebuilder.Append("'{" + arguments.Count + "}'");
                    else
                        valuebuilder.Append("{" + arguments.Count + "}");
                    if (value is string)
                        value = value.ToString().Replace("'", "char(39)");
                    arguments.Add(value);

                }
            }


            fieldbuilder.Append(") Values (");

            fieldbuilder.Append(valuebuilder.ToString());
            fieldbuilder.Append(");");
            return new Tuple<string, object[]>(fieldbuilder.ToString(), arguments.ToArray());
        }

        /// <summary>
        /// /// <summary>
        /// 执行SQL，根据SQL操作的类型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list"></param>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        private string DoSql(IEnumerable<TEntity> list, SqlType sqlType)
        {
            return DoSql(list, sqlType, null);
        }
        /// <summary>
        /// 执行SQL，根据SQL操作的类型
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list"></param>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        private string DoSql(IEnumerable<TEntity> list, SqlType sqlType, params string[] fieldParams)
        {
            var sqlstr = new StringBuilder();
            switch (sqlType)
            {
                case SqlType.Insert:
                    list.ToList().ForEach(i =>
                    {
                        Tuple<string, object[]> sql = CreateInsertSql(i);
                        sqlstr.AppendFormat(sql.Item1, sql.Item2);
                    });
                    break;
                case SqlType.Update:
                    list.ToList().ForEach(i =>
                    {
                        Tuple<string, object[]> sql = CreateUpdateSql(i, fieldParams);
                        sqlstr.AppendFormat(sql.Item1, sql.Item2);
                    });
                    break;
                case SqlType.Delete:
                    list.ToList().ForEach(i =>
                    {
                        Tuple<string, object[]> sql = CreateDeleteSql(i);
                        sqlstr.AppendFormat(sql.Item1, sql.Item2);
                    });
                    break;
                default:
                    throw new ArgumentException("请输入正确的参数");
            }
            return sqlstr.ToString();
        } 
        #endregion

        /// <summary>
        /// SQL操作类型
        /// </summary>
        protected enum SqlType
        {
            Insert,
            Update,
            Delete,
        }
    }
}
