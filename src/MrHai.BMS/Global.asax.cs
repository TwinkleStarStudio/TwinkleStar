using MrHai.Application.IOC;
using MrHai.Core.Data.Initialize;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting; 
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MrHai.BMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //设置MEF依赖注入容器
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            MefDependencySolver solver = new MefDependencySolver(catalog);
            DependencyResolver.SetResolver(solver);

            new MrHaiDatabaseInitializer().Initialize();
        }
    }
}
