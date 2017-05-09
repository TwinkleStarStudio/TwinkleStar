using MrHai.BMS;
using MrHai.BMS.Filters;
using MrHai.BMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwinkleStar.Common;
using TwinkleStar.Common.Json;
using TwinkleStar.Common.Logging;
//using TwinkleStar.Common.Base64;

namespace MrHai.BMS.Controllers.Base
{
    [TSAuthroize]
    public class BaseController : Controller
    {
        protected ResponseMessage responseMessage = new ResponseMessage();//返回对象的定义
        // GET: Base
        protected int? UserID
        {
            get
            {
                return CurrentUser?.UserId;
            }
        }

        protected string UserName
        {
            get
            {
                return CurrentUser?.Name;
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public LoginUser CurrentUser
        {
            get
            {
                string currentJson = Request.Cookies["CurrentUser"].Value;

                if (!string.IsNullOrEmpty(currentJson))
                {
                    //return JsonHelper.Deserialize<LoginUser>(Encrypt.DecryptDES(currentJson, "MRHAIMRSEA"));
                    return JsonHelper.Deserialize<LoginUser>(currentJson);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    //Response.Cookies["CurrentUser"].Value = Encrypt.EncryptDES(JsonHelper.Serialize(value, false), "MRHAIMRSEA");
                    Response.Cookies["CurrentUser"].Value = JsonHelper.Serialize(value, false);
                    Response.Cookies["CurrentUser"].Expires = DateTime.Now.AddYears(1);
                }
            }
        }

        protected string RequestUrl
        {
            get { return Request.RawUrl; }
        }
        protected JsonResult JsonBase(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult Succeed(string msg)
        {
            return Json(new JsonResultWithoutData(true, msg), JsonRequestBehavior.AllowGet);
        }

        protected ActionResult Succeed<T>(T model)
        {
            return Json(new JsonResultModel<T>(model), JsonRequestBehavior.AllowGet);
        }

        protected ActionResult Failed(string msg)
        {
            return Json(new JsonResultWithoutData(false, msg), JsonRequestBehavior.AllowGet);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Logger.GetLogger(filterContext.Exception.Source).Error(filterContext.Exception.ToString());
            filterContext.ExceptionHandled = true;
            if (filterContext.HttpContext.Request.IsAjaxRequest())//判断是不是ajax请求
            {
                ResponseMessage message = new ResponseMessage();
                message.result.errcode = 500;
                message.result.msg = filterContext.Exception.Message;
                filterContext.Result =Json(message);
            }
            else
            {
                // 跳转到错误页
                filterContext.Result = new RedirectResult(Url.Action("S500", "Sorry"));
            }
           
          
        }
    }
}