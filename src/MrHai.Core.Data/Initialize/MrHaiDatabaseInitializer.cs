using System;
using System.Data.Entity;
using TwinkleStar.Data;
using MrHai.Core.Data.Migrations;
using TwinkleStar.Service.Initialize;

namespace MrHai.Core.Data.Initialize
{
    /// <summary>
    /// 数据库初始化操作类
    /// </summary>
    public class MrHaiDatabaseInitializer : DatabaseInitializer
    {
        /// <summary>
        /// 数据库初始化
        /// </summary>
        public override void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EFDbContext, MrHaiConfiguration>());
        }
    }
}
