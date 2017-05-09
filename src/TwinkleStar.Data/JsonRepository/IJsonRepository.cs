
namespace TwinkleStar.Data.JsonRepository
{
    /// <summary>
    /// Json仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IJsonRepository<TEntity>
        : IRepository<TEntity>
        where TEntity : JsonEntity
    {
        /// <summary>
        /// 是否缩进
        /// </summary>
        bool IsIndent { get; set; }
        /// <summary>
        /// 按文件存储，直接删除所有文件
        /// </summary>
        void DeleteAll();
        int Count();
    }
}
