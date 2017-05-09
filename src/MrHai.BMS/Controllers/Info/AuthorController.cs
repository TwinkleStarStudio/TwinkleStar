using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwinkleStar.Common;

namespace MrHai.BMS.Controllers.Info
{
    //作者简介
    [Export]
    public class AuthorController : BaseController
    {
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        // GET: Author
        public ActionResult Index()
        {
            var result = MrHaiApplication.GetInfomationModelByType(InfomationType.ArtistCV).AppendData as Infomation;
            ViewBag.content = result?.Content??string.Empty;
            return View();
        }
        [HttpPost]
        public ActionResult Update()
        {
            string data = Request.GetPostBody();
            if (string.IsNullOrEmpty(data))
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "缺少必要参数";
                return Json(responseMessage);
            }
            Infomation model = MrHaiApplication.GetInfomationModelByType(InfomationType.ArtistCV).AppendData as Infomation;
            if (model == null)
            {
                Infomation entity = new Infomation();
                entity.InfoType = InfomationType.ArtistCV;
                entity.IsDeleted = false;
                entity.Content = data;
                entity.CreateUserId = UserID;
                entity.CategoryId = "4";
                entity.CreateUserId = UserID;
                MrHaiApplication.SaveInfomation(new List<Infomation>() { entity});
            }
            else
            {
                model.Content = data;
                model.CreateUserId = UserID;
                model.CategoryId = "4";
                MrHaiApplication.UpdateinfomationChanges(model);
            }
            responseMessage.result.errcode = 0;
            responseMessage.result.msg = "操作成功";
            return Json(responseMessage);
        }

    }
}