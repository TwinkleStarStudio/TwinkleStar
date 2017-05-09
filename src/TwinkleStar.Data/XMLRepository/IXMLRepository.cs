
namespace TwinkleStar.Data.XMLRepository
{
    /// <summary>
    /// XML仓储接口
    /// </summary>
     public interface IXMLRepository<TEntity>
        : IRepository<TEntity>
        where TEntity : XMLEntity
    {
    }
}
