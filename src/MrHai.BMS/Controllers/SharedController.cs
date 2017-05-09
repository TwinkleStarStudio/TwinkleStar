using MrHai.BMS.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MrHai.BMS.Controllers
{
    public class SharedController : BaseController
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Layout()
        {
            
            return View();
        }
    }
}