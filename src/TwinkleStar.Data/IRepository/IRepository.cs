using System.Linq;

namespace TwinkleStar.Data
{
    /// <summary>
    /// 数据基础操作规范
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> 
        where TEntity : EntityBase
    {
        /// <summary>
        /// 添加实体并提交到数据服务器
        /// </summary>
        /// <param name="item">需要添加数据项</param>
        /// <returns>受影响条数</returns>
        int Insert(TEntity item);

        /// <summary>
        /// 移除实体并提交到数据服务器
        /// 如果表存在约束，需要先删除子表信息
        /// </summary>
        /// <param name="item">需要删除的数据项</param>
        /// <returns>受影响条数</returns>
        int Delete(TEntity item);

        /// <summary>
        /// 修改实体并提交到数据服务器
        /// </summary>
        /// <param name="item">需要修改的数据项</param>
        /// <returns>受影响条数</returns>
        int Update(TEntity item);

        /// <summary>
        /// 得到指定的实体集合（延时结果集）
        /// </summary>
        /// <returns>实体集合</returns>
        IQueryable<TEntity> GetModel();

        /// <summary>
        /// 根据主键得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Find(params object[] id);
    }
}
