using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using MrHai.Common.GlobalConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwinkleStar.Common.Others;

namespace MrHai.BMS.Controllers.Config
{
    [Export]
    public class WebsiteController : BaseController
    {
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        // GET: Website
        public ActionResult Index()
        {
            var webSite = (MrHaiApplication.GetOfficialWebsite()?.AppendData) as OfficialWebsite;
            ViewBag.webSite = webSite ?? new OfficialWebsite() { OfficialWebsiteSetting = new OfficialWebsiteSetting() { Name = "", URL = ""} };
            return View();
        }

        [HttpPost]
        public ActionResult Save()
        {
            char[] spiltChar = new char[] { ',' };
            string id = Request["ID"];
            string[] names = Request["Name"].Split(spiltChar);
            string[] urls = Request["Url"].Split(spiltChar);
            OfficialWebsite dbItem = new OfficialWebsite();
            dbItem.ID = id;
            for (int i = 0; i < names.Length; i++)
            {
                OfficialWebsiteSetting item = new OfficialWebsiteSetting
                {
                    Name = names[i],
                    URL = urls[i],
                };
                dbItem.OfficialWebsiteSetting = item;
            }
            MrHaiApplication.SaveOfficialWebsite(dbItem);
            return RedirectToAction("");
        }
    }
}