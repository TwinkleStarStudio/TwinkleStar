using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinkleStar.Common
{
    /// <summary>
    /// 全局配置接口
    /// </summary>
    public interface IGlobalConfig<T> where T: ConfigBase, new()
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="configJson"></param>
        T GetConfig();

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="config"></param>
        int SaveConfig(T config);
    }
}
