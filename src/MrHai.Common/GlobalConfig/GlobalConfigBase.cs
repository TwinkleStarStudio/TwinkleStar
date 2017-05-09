using MrHai.Core.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrHai.Common.GlobalConfig
{
    /// <summary>
    /// 全局配置操作基类
    /// </summary>
    public abstract class GlobalConfigBase
    {
        protected IGlobalConfigRepository m_repository;
        protected string m_configType;

        public GlobalConfigBase(IGlobalConfigRepository repository)
        {
            m_repository = repository;
            m_configType = this.GetType().FullName;
        }
    }
}
