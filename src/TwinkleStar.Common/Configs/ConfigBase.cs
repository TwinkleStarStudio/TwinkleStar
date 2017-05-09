using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinkleStar.Common
{
    /// <summary>
    /// 配置基类
    /// </summary>
    public abstract class ConfigBase
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool? Enable { get; set; } = true;

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDeleted { get; set; } = false;

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletedTime { get; set; }
    }
}
