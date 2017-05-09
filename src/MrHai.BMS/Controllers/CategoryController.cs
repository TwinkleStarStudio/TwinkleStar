using MrHai.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MrHai.Application.Models;
using MrHai.Application.Models.ApiModels;
using MrHai.Core.Models;
using MrHai.BMS.Controllers.Base;
using MrHai.BMS.Models.Common;

namespace MrHai.BMS.Controllers
{
    [Export]
    public class CategoryController : BaseController
    {

        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        // GET: Category
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult ArtIndex(string pid)
        {
            ViewBag.PId = pid;
            return View();
        }

        public ActionResult GetCategory(NavigationType type, string pid, bool isGetEnabled = false)
        {
            List<string> pids = null;
            if (!string.IsNullOrEmpty(pid))
            {
                pids = new List<string>()
                {
                    pid
                };
            }
            var data = MrHaiApplication.GetCategories(type, pids, isGetEnabled);

            return new JsonResult() { Data = data };
        }
        [HttpPost]
        public ActionResult AddCategory(string pid, string name, int type, bool isThird = false)
        {
            var re = new ResponseDto();
            if (string.IsNullOrEmpty((name)))
            {
                return new JsonResult() { Data = ResponseDto.ErrorResponse("名称不能为空！") };
            }
            if (!Enum.IsDefined(typeof(NavigationType), type))
            {
                return new JsonResult() { Data = ResponseDto.ErrorResponse("上传类型有误！") };
            }

            var navType = (NavigationType)type;
            var result =
                MrHaiApplication.AddCategory(name, pid, navType, isThird);

            return new JsonResult() { Data = re }; ;
        }
        [HttpPost]
        public ActionResult EditCategory(string id, string name)
        {
            var re = new ResponseDto();
            if (string.IsNullOrEmpty((name)))
            {
                return new JsonResult() { Data = ResponseDto.ErrorResponse("名称不能为空！") };
            }
            var result =
                MrHaiApplication.EditCategory(name, id);

            return new JsonResult() { Data = re }; ;
        }

        public ActionResult DeleteCategory(string id)
        {
            var re = new ResponseDto();
            MrHaiApplication.DeleteCategory(id, true);
            return new JsonResult() { Data = re };
        }

        public ActionResult SortCategory(string id, int num)
        {
            var re = new ResponseDto();
            var model = MrHaiApplication.SortCategory(id, num);
            return new JsonResult() { Data = re }; ;
        }

        public ActionResult ChangeEnable(string id)
        {
            var re = new ResponseDto();
            var model = MrHaiApplication.ChangeCategoryEnabled(id);
            return new JsonResult() { Data = re }; ;
        }

        public ActionResult WorkList(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "缺少关键数据";
                return JsonBase(responseMessage);
            }

            var data = MrHaiApplication.GetCategories(NavigationType.MainNavigation, new List<string> { id }).AppendData as List<CategoryListDto>;
            if (data.Count <= 0)
            {
                responseMessage.result.errcode = 1;
                responseMessage.result.msg = "该分类下没有作品";
                return JsonBase(responseMessage);
            }
            List<CategoryViewModel> list = new List<CategoryViewModel>();
            foreach (var item in data)
            {
                CategoryViewModel entity = new CategoryViewModel();
                entity.Id = item.Id;
                entity.Name = item.Name;
                list.Add(entity);
            }
            responseMessage.result.errcode = 0;
            responseMessage.result.msg = "正常";
            responseMessage.body = list;
            return JsonBase(responseMessage);
        }
    }
}