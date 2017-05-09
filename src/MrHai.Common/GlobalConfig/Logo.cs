using System;
using System.Collections.Generic;
using TwinkleStar.Common;

namespace MrHai.Common.GlobalConfig
{
    /// <summary>
    /// Logo全局设置实体
    /// </summary>
    [Serializable]
    public class Logo
        : ConfigBase
    {
        /// <summary>
        /// Logo设置
        /// </summary>
        public LogoSetting LogoSetting { get; set; }
    }

    [Serializable]
    public class LogoSetting
    {
        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 鼠标悬浮后显示图片
        /// </summary>
        public string HoverImage { get; set; }

    }
}
