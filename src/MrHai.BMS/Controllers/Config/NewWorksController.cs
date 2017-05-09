using MrHai.Application;
using MrHai.Application.Models;
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
    public class NewWorksController : BaseController
    {
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }

        // GET: NewWorks
        public ActionResult Index()
        {
            var newWorks = (MrHaiApplication.GetNewWorks()?.AppendData) as NewWorks ?? new NewWorks() { NewWorksList = new List<NewWorksSetting>()};

            ViewBag.NewWorks = newWorks;

            var works = (MrHaiApplication.GetWorksName()?.AppendData) as List<CategoryListDto>;
            ViewBag.Works = works ?? new List<CategoryListDto>();

            return View();
        }

        [HttpPost]
        public ActionResult Save()
        {
            NewWorks newWorks = new NewWorks();
            newWorks.ID = Request["ID"];
            newWorks.NewWorksList = new List<NewWorksSetting>();

            char[] spiltChar = new char[] { ',' };
            string[] worksIds = !string.IsNullOrEmpty(Request["selWorksId"]) ? Request["selWorksId"].Split(spiltChar) : new string[0];
            string[] orderNumrs = !string.IsNullOrEmpty(Request["OrderNum"]) ? Request["OrderNum"].Split(spiltChar) : new string[0];

            for (int i = 0; i < worksIds.Length; i++)
            {
                int orderNumr = 0;
                if (!string.IsNullOrEmpty(worksIds[i]) && !worksIds.Equals("-1"))
                {
                    newWorks.NewWorksList.Add(new NewWorksSetting()
                    {
                        WorksId = worksIds[i],
                        OrderNum = int.TryParse(orderNumrs[i], out orderNumr) ? orderNumr : 1
                    });
                }
            }

            MrHaiApplication.SaveNewWorks(newWorks);

            return RedirectToAction("");
        }
    }
}