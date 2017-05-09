namespace TwinkleStar.Demo.Core.Data.Configurations
{
    /// <summary>
    /// 实体类-数据表映射——角色信息
    /// </summary>    
	internal partial class RoleConfiguration
    {
        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void RoleConfigurationAppend()
        {
            HasMany(m => m.Members).WithMany(n => n.Roles);
        }
    }
}
