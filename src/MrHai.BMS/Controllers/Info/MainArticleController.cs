using MrHai.Application;
using MrHai.Application.Models;
using MrHai.BMS.Controllers.Base;
using MrHai.BMS.Controllers.Common.Funtion;
using MrHai.BMS.Models.Work;
using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwinkleStar.Common;

namespace MrHai.BMS.Controllers
{
    [Export]
    public class MainArticleController : BaseController
    {
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        // GET: Article
        // GET: MainArticle
        public ActionResult Index()
        {
            string workName = Request.Form["txtWorkName"] == null ? "" : Request.Form["txtWorkName"].Trim();
            var list = MrHaiApplication.GetInfoByPid("3").AppendData as List<Infomation> ?? new List<Infomation>();
            List<WorkViewModel> modelList = new List<WorkViewModel>();
            foreach (var item in list)
            {
                WorkViewModel entity = new WorkViewModel();
                entity.name = item.Title;
                entity.content = item.Content;
                entity.infoID = item.Id;
                modelList.Add(entity);
            }
            ViewBag.modelList = modelList;
            return View();
        }

        public ActionResult New(string id)
        {
            
            viewAddModel model = new viewAddModel();
            if (!string.IsNullOrEmpty(id))
            {
                var dbModel = MrHaiApplication.GetInfo(id).AppendData as Infomation;
                var caModel = MrHaiApplication.GetCategory(dbModel.CategoryId)?.AppendData as Category ?? new Category();
                model.content = dbModel.Content;
                model.title = dbModel.Title;
                model.workId = dbModel.CategoryId;
                model.workName = caModel.Name;
                model.cdID = caModel.ParentId;
                model.infoID = dbModel.Id;
            }
            ViewBag.model = model;
            return View();
        }

        [HttpPost]
        public ActionResult Add()
        {
            var data = Request.GetPostBody<viewAddModel>();
            if (string.IsNullOrEmpty(data.infoID))//新增
            {

                data.infoType = InfomationType.MainArtistCV;
                data.userID = UserID;
                data.workId = "3";
                data.userID = UserID;
                AddEWC tool = new AddEWC(MrHaiApplication);
                return Json(tool.CommonAdd(data));
            }
            else//更新
            {

                data.infoType = InfomationType.MainArtistCV;
                data.userID = UserID;
                data.workId = "3";
                data.userID = UserID;
                AddEWC tool = new AddEWC(MrHaiApplication);
                return Json(tool.CommonUpdate(data));
            }

        }

        public ActionResult Delete(string id)
        {
            AddEWC tool = new AddEWC(MrHaiApplication);
            return JsonBase(tool.CommonDelete(id));
        }
       
    }
}