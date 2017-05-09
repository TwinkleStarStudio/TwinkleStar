using System;
using System.ComponentModel.Composition;
using TwinkleStar.Data;

namespace TwinkleStar.Service
{
    /// <summary>
    /// 核心业务实现基类
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>
        /// 获取或设置 工作单元对象，用于处理同步业务的事务操作
        /// </summary>
        [Import]
        protected IUnitOfWork UnitOfWork { get; set; }
    }
}
