using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinkleStar.Common;

namespace MrHai.Common.GlobalConfig
{
    /// <summary>
    /// 碎片设置
    /// </summary>
    [Serializable]
    public class Fragment
        : ConfigBase
    {
        public Fragment()
        {
            Fragments = new List<FragmentSetting>();
        }

        /// <summary>
        /// 友情链接集合（名称，Url）
        /// </summary>
        public IList<FragmentSetting> Fragments { get; set; }
    }

    /// <summary>
    /// 碎片设置
    /// </summary>
    public class FragmentSetting
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// 作品ID
        /// </summary>
        public string WorksId { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; }
    }
}
