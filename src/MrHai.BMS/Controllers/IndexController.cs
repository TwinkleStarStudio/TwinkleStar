using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MrHai.BMS.Controllers
{
    [Export]
    public class IndexController : BaseController
    {
        #region 属性

        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }

        #endregion

        // GET: Index
        public ActionResult Index()
        {

            return View();
        }
    }
}