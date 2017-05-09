using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwinkleStar.Common;
using TwinkleStar.Common.Others;

namespace MrHai.BMS.Controllers.Logo
{
    [Export]
    public class LogoController : BaseController
    {
        #region 属性
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }

        #endregion
        // GET: Logo
        public ActionResult Index()
        {
            Random rand = new Random();
            int value = rand.Next(1, 100000);
            ViewBag.video = $"{Configs.GetConfig("Url")}BackgroundVideo?v={value}";
            ViewBag.logo = $"{Configs.GetConfig("Url")}Logo?v={value}";
            return View();
        }

        public ActionResult Save()
        {
            return View();
        }
    }
}