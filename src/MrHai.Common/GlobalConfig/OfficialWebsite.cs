using System;
using TwinkleStar.Common;

namespace MrHai.Common.GlobalConfig
{
    /// <summary>
    /// 官网配置
    /// </summary>
    [Serializable]
    public class OfficialWebsite
        : ConfigBase
    {
        /// <summary>
        /// 配置实体
        /// </summary>
        public OfficialWebsiteSetting OfficialWebsiteSetting { get; set; }
    }

    [Serializable]
    public class OfficialWebsiteSetting
    {
        /// <summary>
        /// 官网名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 官网链接
        /// </summary>
        public string URL { get; set; }
    }
}
