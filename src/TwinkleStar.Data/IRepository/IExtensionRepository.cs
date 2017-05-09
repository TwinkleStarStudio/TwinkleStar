using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TwinkleStar.Data
{
    /// <summary>
    /// 扩展的仓储接口
    /// </summary>
    public interface IExtensionRepository<TEntity> :
        IRepository<TEntity>,
        IOrderableRepository<TEntity>
        where TEntity : EntityBase
    {
        /// <summary>
        /// 添加集合[集合数目不大时用此方法，超大集合使用BatchInsert]
        /// </summary>
        /// <param name="items"></param>
        /// <returns>受影响的行数</returns>
        int Insert(IEnumerable<TEntity> items);

        /// <summary>
        /// 删除集合[集合数目不大时用此方法，超大集合使用BatchInsert]
        /// </summary>
        /// <param name="items"></param>
        /// <returns>受影响的行数</returns>
        int Delete(IEnumerable<TEntity> items);

        /// <summary>
        /// 更新集合[集合数目不大时用此方法，超大集合使用BatchInsert]
        /// </summary>
        /// <param name="items"></param>
        /// <returns>受影响的行数</returns>
        int Update(IEnumerable<TEntity> items);

        /// <summary>
        /// 扩展更新方法，只对EF支持
        /// 注意本方法不能和GetModel()一起使用，它的表主键可以通过post或get方式获取
        /// </summary>
        /// <param name="entity"></param>
        int Update<T>(Expression<Action<T>> entity) where T : class;

        /// <summary>
        /// 获取集合[集合数目不大时用此方法，超大集合使用BatchInsert]
        /// </summary>
        /// <param name="items"></param>
        /// <returns>受影响的行数</returns>
        IQueryable<TEntity> GetModel(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 查找集合[集合数目不大时用此方法，超大集合使用BatchInsert]
        /// </summary>
        /// <param name="items"></param>
        /// <returns>受影响的行数</returns>
        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 批量插入集合[集合数目不大时用此方法，超大集合使用BatchInsert]
        /// </summary>
        /// <param name="items"></param>
        /// <returns>受影响的行数</returns>
        int BatchInsert(IEnumerable<TEntity> items);

        /// <summary>
        /// 批量更新集合[集合数目不大时用此方法，超大集合使用BatchInsert]
        /// </summary>
        /// <param name="items"></param>
        /// <returns>受影响的行数</returns>
        int BatchUpdate(IEnumerable<TEntity> item, params string[] fieldParams);

        /// <summary>
        /// 批量删除集合[集合数目不大时用此方法，超大集合使用BatchInsert]
        /// </summary>
        /// <param name="items"></param>
        /// <returns>受影响的行数</returns> 
        int BatchDelete(IEnumerable<TEntity> item);
    }
}
