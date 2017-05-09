using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using MrHai.Common.GlobalConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MrHai.BMS.Controllers.Config
{
    [Export]
    public class AboutUsController : BaseController
    {
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }

        // GET: AboutUs
        public ActionResult Index()
        {
            var aboutUs = (MrHaiApplication.GetAboutUs()?.AppendData) as AboutUs;
            ViewBag.list = aboutUs ?? new AboutUs();
            return View();
        }

        [HttpPost]
        public ActionResult Save()
        {
            char[] spiltChar = new char[] { ',' };
            string id = Request["ID"];
            string[] emails = !string.IsNullOrEmpty(Request["EMail"]) ? Request["EMail"].Split(spiltChar) : new string[0];
            AboutUs dbItem = new AboutUs();
            dbItem.ID = id;

            for (int i = 0; i < emails.Length; i++)
            {
                if (!string.IsNullOrEmpty(emails[i]))
                {
                    AboutUsSetting item = new AboutUsSetting
                    {
                        Email = emails[i]
                    };
                    dbItem.AboutUsSetting.Add(item);
                }
            }
            MrHaiApplication.SaveAboutUs(dbItem);
            return RedirectToAction("");
        }
    }
}