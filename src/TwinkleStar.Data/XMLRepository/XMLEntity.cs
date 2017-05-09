using System;

namespace TwinkleStar.Data.XMLRepository
{
    /// <summary>
    /// XML实体基类
    /// </summary>
    public abstract class XMLEntity : EntityBase
    {
        private string id = Guid.NewGuid().ToString();
        /// <summary>
        /// XML实体主键
        /// </summary>
        public string RootID
        {
            get { return id; }
            set { id = value; }
        }
    }
}
