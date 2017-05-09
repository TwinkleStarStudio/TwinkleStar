using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwinkleStar.Common
{
    /// <summary>
    /// 时间类型
    /// </summary>
    public enum DateType
    {
        TICKS,
        /// <summary>
        /// 毫秒
        /// </summary>
        MILLISECOND,
        /// <summary>
        /// 秒
        /// </summary>
        SECOND,
        /// <summary>
        /// 分
        /// </summary>
        MINUTE,
        /// <summary>
        /// 小时
        /// </summary>
        HOUR,
        /// <summary>
        /// 日
        /// </summary>
        DAY,
        /// <summary>
        /// 月
        /// </summary>
        MONTH,
        /// <summary>
        /// 年
        /// </summary>
        YEAR
    }
}
