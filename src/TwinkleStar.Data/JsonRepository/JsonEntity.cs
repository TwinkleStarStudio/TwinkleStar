using TwinkleStar.Common;

namespace TwinkleStar.Data.JsonRepository
{
    /// <summary>
    /// Json实体基类
    /// </summary>
    public abstract class JsonEntity : EntityBase
    {
        private string id = PublicHelper.NewObjectId();
        /// <summary>
        /// Json实体主键
        /// </summary>
        public string RootID
        {
            get { return id; }
            set { id = value; }
        }
    }
}
