using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MrHai.Core;
using System.IO;
using System.Reflection;
using MrHai.Core.Data.Initialize;
using MrHai.Core.Data.Repositories;
using MrHai.Core.Models;
using MrHai.Common.GlobalConfig;
using System.Collections.Generic;

namespace MrHai.BMS.Tests
{
    [TestClass]
    [Export]
    public class UnitTest1
    {
        private static CompositionContainer _container;
        [Import]
        public IMrHaiService MrHaiService { get; set; }

        public UnitTest1()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory()));
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            _container = new CompositionContainer(catalog);

            new MrHaiDatabaseInitializer().Initialize();


        }

        [TestMethod]
        public void TestMethod1()
        {
            ICategoryRepository categoryRepository = _container.GetExportedValue<ICategoryRepository>();

            Category category = new Category()
            {
                ParentId = null,
                CategoryType = NavigationType.MainNavigation,
                Enabled = true,
                IsDeleted = false,
                Level = 1,
                Name = "Works",
                Icon = "Icon"
            };

            Category category1 = new Category()
            {
                ParentId = category.Id,
                CategoryType = NavigationType.MainNavigation,
                Enabled = true,
                IsDeleted = false,
                Level = 1,
                Name = "Mr.Sea",
                Icon = "Icon"
            };

            categoryRepository.Insert(category);
            //categoryRepository.Insert(category1);

            var c = categoryRepository.GetModel();
        }

        [TestMethod]
        public void TestGlobalConfig()
        {
            //IGlobalConfigRepository globalConfigRepository = _container.GetExportedValue<IGlobalConfigRepository>();

            //FriendlyLinksConfig config = new FriendlyLinksConfig(globalConfigRepository);

            ////config.SaveConfig(new FriendlyLinks()
            ////{
            ////    Links = new List<FriendlyLink>()
            ////    {
            ////        new FriendlyLink() {Name = "百度", Url = "www.baidu.com" },
            ////        new FriendlyLink() {Name = "新浪", Url = "www.sina.com" },
            ////        new FriendlyLink() {Name = "腾讯", Url = "www.qq.com" },
            ////        new FriendlyLink() {Name = "淘宝", Url = "www.taobao.com" },
            ////    }
            ////});


            //var conf = config.GetConfig();
        }
    }
}
