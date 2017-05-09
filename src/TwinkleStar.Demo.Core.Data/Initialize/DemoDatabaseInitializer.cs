using System;
using System.Data.Entity;
using TwinkleStar.Data;
using TwinkleStar.Demo.Core.Data.Migrations;
using TwinkleStar.Service.Initialize;

namespace TwinkleStar.Demo.Core.Data.Initialize
{
    /// <summary>
    /// 数据库初始化操作类
    /// </summary>
    public class DemoDatabaseInitializer: DatabaseInitializer
    {
        /// <summary>
        /// 数据库初始化
        /// </summary>
        public override void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EFDbContext, DemoConfiguration>());
        }
    }
}
