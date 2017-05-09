using System.Data.Entity.Migrations;
using TwinkleStar.Data;

namespace TwinkleStar.Service.Migrations
{
    /// <summary>
    /// 数据库迁徙配置
    /// </summary>
    public class Configuration : DbMigrationsConfiguration<EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// 如果有变化，初始化数据库
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(EFDbContext context)
        {

        }
    }
}
