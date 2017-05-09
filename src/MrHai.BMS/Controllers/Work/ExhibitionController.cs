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

namespace MrHai.BMS.Controllers.Work
{
    //展览现场
    [Export]
    public class ExhibitionController : BaseController
    {
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        // GET: Exhibition
        public ActionResult Index()
        {
            var caList = MrHaiApplication.GetCategories(NavigationType.MainNavigation, new List<string> { "1" }).AppendData as List<CategoryListDto>;
            ViewBag.ca = caList;
            string caID = Request.Form["ddCa"];
            ViewBag.caID = caID;
            string workName = Request.Form["txtWorkName"] == null ? "" : Request.Form["txtWorkName"].Trim();
            ViewBag.workName = workName;
            int pageIndex = 0;
            if (!int.TryParse(Request.Form["pageIndex"], out pageIndex))
            {
                pageIndex = 1;
            }
            int pageSize = 20;
            int count = 0;

            var list = MrHaiApplication.CommonSearch(workName, caID, InfomationType.Exhibitions, ref count, pageIndex, pageSize).AppendData as List<InfomationDto> ?? new List<InfomationDto>();
            List<WorkViewModel> modelList = new List<WorkViewModel>();
            foreach (var item in list)
            {
                WorkViewModel entity = new WorkViewModel();
                entity.caName = item.CategoryName;
                entity.name = item.WorkName;
                entity.infoID = item.Id;
                entity.title = item.Title;
                entity.createTime = item.CreateTime;
                modelList.Add(entity);
            }
            ViewBag.modelList = modelList;

            ViewBag.pageIndex = pageIndex;
            ViewBag.pageCount = (int)Math.Ceiling((decimal)count / (decimal)pageSize);
            return View();
        }

        public ActionResult New(string id)
        {
            var caList = MrHaiApplication.GetCategories(NavigationType.MainNavigation, new List<string> { "1" }).AppendData as List<CategoryListDto>;
            ViewBag.ca = caList;
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

                data.infoType = InfomationType.Exhibitions;
                data.userID = UserID;
                AddEWC tool = new AddEWC(MrHaiApplication);
                return Json(tool.CommonAdd(data));
            }
            else//更新
            {

                data.infoType = InfomationType.Exhibitions;
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