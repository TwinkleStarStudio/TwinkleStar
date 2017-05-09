
namespace TwinkleStar.Demo.Core.Data.Configurations
{
    partial class MemberExtendConfiguration
    {
        partial void MemberExtendConfigurationAppend()
        {
            HasRequired(m => m.Member).WithOptional(n => n.Extend);
        }
    }
}
