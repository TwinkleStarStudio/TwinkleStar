using System;
using System.Linq;
using System.Linq.Expressions;

namespace TwinkleStar.Data
{
    public interface IOrderableRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 带排序的结果集
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetModel(Action<IOrderable<TEntity>> orderBy);

        /// <summary>
        /// 根据指定lambda表达式和排序方式,得到延时结果集
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetModel(Action<IOrderable<TEntity>> orderBy, Expression<Func<TEntity, bool>> predicate);
    }
}
