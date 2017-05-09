using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwinkleStar.Data
{
    /// <summary>
    /// EF底层构架，关于规约功能的仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISpecificationRepository<TEntity> :
        IExtensionRepository<TEntity>
        where TEntity : EntityBase
    {
        /// <summary>
        /// 根据指定规约,得到延时结果集
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetModel(ISpecification<TEntity> specification);

        /// <summary>
        /// 根据指定规约,得到第一个实体
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        TEntity Find(ISpecification<TEntity> specification);

        /// <summary>
        /// 带排序功能的，根据指定规约，得到结果集
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="specification"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetModel(Action<IOrderable<TEntity>> orderBy, ISpecification<TEntity> specification);

        /// <summary>
        /// 保存之后触发
        /// Occurs after data saved
        /// </summary>
        event Action<SavedEventArgs> AfterSaved;

        /// <summary>
        /// 保存之前触发
        /// Occurs before data saved
        /// </summary>
        event Action<SavedEventArgs> BeforeSaved;
    }

}
