using System;
using System.Collections.Generic;
using TwinkleStar.Common;

namespace MrHai.Common.GlobalConfig
{
    /// <summary>
    /// 友情链接集合配置实体
    /// </summary>
    [Serializable]
    public class FriendlyLinks
        : ConfigBase
    {
        public FriendlyLinks()
        {
            Links = new List<FriendlyLink>();
        }

        /// <summary>
        /// 友情链接集合（名称，Url）
        /// </summary>
        public IList<FriendlyLink> Links { get; set; }
    }

    /// <summary>
    /// 友情链接实体
    /// </summary>
    [Serializable]
    public class FriendlyLink
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public int OrderNum { get; set; }
    }
}
