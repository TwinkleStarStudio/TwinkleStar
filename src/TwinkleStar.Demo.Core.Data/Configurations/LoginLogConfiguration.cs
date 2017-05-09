
namespace TwinkleStar.Demo.Core.Data.Configurations
{
    /// <summary>
    /// 实体类-数据表映射——登录记录信息
    /// </summary>    
    internal partial class LoginLogConfiguration
    {
        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void LoginLogConfigurationAppend()
        {
            HasRequired(m => m.Member).WithMany(n => n.LoginLogs);
        }
    }
}
