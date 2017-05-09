using MrHai.Application;
using MrHai.Application.Models;
using MrHai.BMS.Controllers.Base;
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
    [Export]
    public class WorksController : BaseController
    {
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        // GET: Works
        public ActionResult Index()
        {

            var caList = MrHaiApplication.GetCategories(NavigationType.MainNavigation, new List<string> { "1" }).AppendData as List<CategoryListDto>;
            ViewBag.ca = caList;
            
            string caID = Request.Form["ddCa"];
            ViewBag.caID = caID;
            string workName = Request.Form["txtWorkName"]==null?"": Request.Form["txtWorkName"].Trim();
            ViewBag.workName = workName;
            int pageIndex = 0;
            if (!int.TryParse(Request.Form["pageIndex"], out pageIndex))
            {
                pageIndex = 1;
            }         

            List<WorkViewModel> modelList = new List<WorkViewModel>();            
            int count = 0;
            int pageSize = 20;
            var list = MrHaiApplication.SearchWorks(workName, caID, ref count, pageIndex, pageSize).AppendData  as List<CategoryListDto>?? new List<CategoryListDto>();
            foreach (var item in list)
            {
                WorkViewModel entity = new WorkViewModel();
                entity.caName = item.ParentName;
                entity.name = item.Name;
                entity.infoID = item.Id;
                entity.createTime = item.CreateTime;
                modelList.Add(entity);
            }
            ViewBag.modelList = modelList;
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageCount= (int)Math.Ceiling((decimal)count / (decimal)pageSize);
            return View();
        }


        public ActionResult New(string id)
        {
            var caList = MrHaiApplication.GetCategories(NavigationType.MainNavigation,new List<string> { "1"}).AppendData as List<CategoryListDto>;
            ViewBag.ca = caList;
            WorkViewModel viewModel = new WorkViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                var list = MrHaiApplication.GetInfoByPid(id)?.AppendData as List<Infomation>;
                var caModel = MrHaiApplication.GetCategory(id)?.AppendData as Category;
                viewModel.infoID = id;
                viewModel.name = caModel.Name;
                viewModel.ca = caModel.ParentId;
                viewModel.about = list?.FirstOrDefault(it => it.InfoType == InfomationType.About)?.Content ?? string.Empty;
                viewModel.backgrand = list?.FirstOrDefault(it => it.InfoType == InfomationType.BackgroundImage)?.Content ?? string.Empty;
                viewModel.screenShot = list?.FirstOrDefault(it => it.InfoType == InfomationType.FilmStill)?.Content ?? string.Empty;
                viewModel.assembly = list?.FirstOrDefault(it => it.InfoType == InfomationType.Installation)?.Content ?? string.Empty;
                viewModel.video = list?.FirstOrDefault(it => it.InfoType == InfomationType.Video)?.Content ?? string.Empty;
                viewModel.summary = list?.FirstOrDefault(it => it.InfoType == InfomationType.Summary)?.Content ?? string.Empty;

            }
            ViewBag.model = viewModel;
            return View();
        }

        [HttpPost]
        public ActionResult Add()
        {
            WorkViewModel model= Request.GetPostBody<WorkViewModel>();
            if (model == null)
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "提交数据失败";
                return Json(responseMessage);
            }
            if (string.IsNullOrEmpty(model.name) || string.IsNullOrEmpty(model.backgrand)|| string.IsNullOrEmpty(model.about) ||
                string.IsNullOrEmpty(model.ca) || string.IsNullOrEmpty(model.summary))
            {
                responseMessage.result.errcode = 1;
                responseMessage.result.msg = "缺少关键数据，新增失败";
                return Json(responseMessage);
            }
            string caID = model.infoID;
            if (string.IsNullOrEmpty(caID))
            {
                caID = MrHaiApplication.AddCategoryAndReturnId(model.name, model.ca.ToString(), NavigationType.MainNavigation);
                MrHaiApplication.AddChildCategory(caID);
            }            
            List<Infomation> list = new List<Infomation>();
            //背景图片
            Infomation backgrand = new Infomation();
            backgrand.CategoryId = caID;
            backgrand.Content = model.backgrand;
            backgrand.InfoType = InfomationType.BackgroundImage;
            backgrand.CreateTime = DateTime.Now;
            backgrand.CreateUserId = UserID;
            backgrand.IsDeleted = false;
            list.Add(backgrand);
            //预告视频
            Infomation video = new Infomation();
            video.CategoryId = caID;
            video.Content = model.video;
            video.InfoType = InfomationType.Video;
            video.CreateTime = DateTime.Now;
            video.CreateUserId = UserID;
            video.IsDeleted = false;
            list.Add(video);
            //视频截图
            Infomation screenShot = new Infomation();
            screenShot.CategoryId = caID;
            screenShot.Content = model.screenShot;
            screenShot.InfoType = InfomationType.FilmStill;
            screenShot.CreateTime = DateTime.Now;
            screenShot.CreateUserId = UserID;
            screenShot.IsDeleted = false;
            list.Add(screenShot);
            //组装过程
            Infomation assembly = new Infomation();
            assembly.CategoryId = caID;
            assembly.Content = model.assembly;
            assembly.InfoType = InfomationType.Installation;
            assembly.CreateTime = DateTime.Now;
            assembly.CreateUserId = UserID;
            assembly.IsDeleted = false;
            list.Add(assembly);
            //关于作品
            Infomation about = new Infomation();
            about.CategoryId = caID;
            about.Content = model.about;
            about.InfoType = InfomationType.About;
            about.CreateTime = DateTime.Now;
            about.CreateUserId = UserID;
            about.IsDeleted = false;
            list.Add(about);
            //摘要
            Infomation summary = new Infomation();
            summary.CategoryId = caID;
            summary.Content = model.summary;
            summary.InfoType = InfomationType.Summary;
            summary.CreateTime = DateTime.Now;
            summary.CreateUserId = UserID;
            summary.IsDeleted = false;
            list.Add(summary);
            if (!string.IsNullOrEmpty(model.infoID))//update
            {
                var infoList = MrHaiApplication.GetInfoByPid(model.infoID)?.AppendData as List<Infomation>;
                List<Infomation> newItem = new List<Infomation>();
                foreach (var upitem in list)
                {
                    var dbModel = infoList.FirstOrDefault(it=>it.CategoryId.Equals(upitem.CategoryId)&&it.InfoType==upitem.InfoType);
                    if (dbModel!=null)
                    {
                        dbModel.Title = upitem.Title;
                        dbModel.Content = upitem.Content;
                        dbModel.IsDeleted = false;
                    }
                    else
                    {
                        newItem.Add(upitem);
                    }
                }                
                if (newItem.Count > 0)
                {
                    MrHaiApplication.SaveInfomation(newItem);
                }
                MrHaiApplication.UpdateinfomationChanges(infoList);
                MrHaiApplication.EditCategory(model.name, model.infoID);
                responseMessage.result.errcode = 0;
                responseMessage.result.msg = "更新成功";
                return Json(responseMessage);
            }
            MrHaiApplication.SaveInfomation(list);
            responseMessage.result.errcode = 0;
            responseMessage.result.msg="新增成功";
            return Json(responseMessage);
        }



        public ActionResult Delete(string id )
        {
            if (string.IsNullOrEmpty(id))
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "删除失败，未接收到有效参数";
                return JsonBase(responseMessage);
            }
            var infoList = MrHaiApplication.GetInfoByPid(id)?.AppendData as List<Infomation>;            
            MrHaiApplication.DeleteInfomation(infoList);
            MrHaiApplication.DeleteCategory(id);
            responseMessage.result.errcode = 0;
            responseMessage.result.msg = "删除成功";
            return JsonBase(responseMessage);
        }
    }
}