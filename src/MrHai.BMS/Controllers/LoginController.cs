using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MrHai.BMS.Controllers
{
    [Export]
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        #region 属性

        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }

        #endregion

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string pwd)
        {
            try
            {
                var user = MrHaiApplication.Login(userName, pwd)?.AppendData as User;

                if (user == null)
                {
                    throw new Exception("用户名或者密码错误。");
                }

                CurrentUser = new LoginUser()
                {
                    Name = user.UserName,
                    UserCode = user.UserCode,
                    UserId = user.UserId
                };

                return Succeed(CurrentUser);
            }
            catch (Exception ex)
            {
                return Failed(ex.Message);
            }
        }
    }
}