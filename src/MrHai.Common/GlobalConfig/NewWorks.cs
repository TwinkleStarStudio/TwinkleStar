using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinkleStar.Common;

namespace MrHai.Common.GlobalConfig
{
    /// <summary>
    /// 最新作品
    /// </summary>
    [Serializable]
    public class NewWorks
        : ConfigBase
    {
        public NewWorks()
        {
            NewWorksList = new List<NewWorksSetting>();
        }

        /// <summary>
        /// 最新作品集合
        /// </summary>
        public IList<NewWorksSetting> NewWorksList { get; set; }
    }

    /// <summary>
    /// 最新作品设置
    /// </summary>
    public class NewWorksSetting
    {
        /// <summary>
        /// 作品ID
        /// </summary>
        public string WorksId { get; set; }

        public int OrderNum { get; set; }
    }
}
