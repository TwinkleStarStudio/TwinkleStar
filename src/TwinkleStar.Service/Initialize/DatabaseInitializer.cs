using System;
using System.Data.Entity;
using TwinkleStar.Data;
using TwinkleStar.Service.Migrations;

namespace TwinkleStar.Service.Initialize
{
    /// <summary>
    /// 数据库初始化操作类
    /// </summary>
    public class DatabaseInitializer
    {
        /// <summary>
        /// 数据库初始化
        /// </summary>
        public virtual void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EFDbContext, Configuration>());
        }
    }
}
