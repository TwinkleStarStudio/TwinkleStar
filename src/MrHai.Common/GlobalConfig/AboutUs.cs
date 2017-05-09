using System;
using System.Collections.Generic;
using TwinkleStar.Common;

namespace MrHai.Common.GlobalConfig
{
    /// <summary>
    /// 关于我们配置实体
    /// </summary>
    [Serializable]
    public class AboutUs
         : ConfigBase
    {
        public AboutUs()
        {
            AboutUsSetting = new List<AboutUsSetting>();
        }

        /// <summary>
        /// 关于我们实体
        /// </summary>
        public IList<AboutUsSetting> AboutUsSetting { get; set; }
        //public AboutUsSetting AboutUsSetting { get; set; }
    }

    [Serializable]
    public class AboutUsSetting
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }
}
