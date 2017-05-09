using MrHai.Core.Data.Repositories;
using System.ComponentModel.Composition;

namespace MrHai.Core
{
    /// <summary>
    /// MrHai 业务核心业务实现
    /// </summary>
    public abstract partial class MrHaiService : MrHaiServiceBase, IMrHaiService
    {
        #region 属性
        /// <summary>
        /// 获取或设置 菜单
        /// </summary>
        [Import]
        protected ICategoryRepository CategoryRepository { get; set; }

        /// <summary>
        /// 获取或设置 全局设置
        /// </summary>
        [Import]
        protected IGlobalConfigRepository GlobalConfigRepository { get; set; }

        /// <summary>
        /// 获取或设置 资讯
        /// </summary>
        [Import]
        protected IInfomationRepository InfomationRepository { get; set; }

        /// <summary>
        /// 获取或设置 作品
        /// </summary>
        [Import]
        protected IWorksRepository WorksRepository { get; set; }

        /// <summary>
        /// 获取或设置 附件
        /// </summary>
        [Import]
        protected IAttachmentRepository AttachmentRepository { get; set; }

        /// <summary>
        /// 获取或设置 用户
        /// </summary>
        [Import]
        protected IUserRepository UserRepository { get; set; }
        #endregion
    }
}
