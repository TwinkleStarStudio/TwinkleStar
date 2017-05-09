using MrHai.Application.IOC;
using MrHai.Core.Data.Initialize;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MrHai.Api
{
    /// <summary>
    /// 应用程序
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 应用程序起始点
        /// </summary>
        protected void Application_Start()
        {

            //设置MEF依赖注入容器
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            MefWebApiDependencySolver solver = new MefWebApiDependencySolver(catalog);
            GlobalConfiguration.Configuration.DependencyResolver = solver;

            new MrHaiDatabaseInitializer().Initialize();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
