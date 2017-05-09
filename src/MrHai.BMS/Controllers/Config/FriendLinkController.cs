using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using MrHai.Common.GlobalConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwinkleStar.Common.Others;

namespace MrHai.BMS.Controllers.Config
{
    //友情链接
    [Export]
    public class FriendLinkController : BaseController
    {
        // GET: FriendLink
        #region 属性

        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }

        #endregion
        public ActionResult Index()
        {

            FriendlyLinks result = (MrHaiApplication.GetFriendlyLinks()?.AppendData ?? new OperationResult(OperationResultType.Success).AppendData) as FriendlyLinks;

            ViewBag.list = result ?? new FriendlyLinks();

            return View();
        }

        public ActionResult Save()
        {
            char[] spiltChar = new char[] { ',' };
            string[] names = !string.IsNullOrEmpty( Request["Name"]) ? Request["Name"].Split(spiltChar) : new string[0] ;
            string[] urls = !string.IsNullOrEmpty(Request["Url"]) ? Request["Url"].Split(spiltChar) : new string[0];
            string[] orderNumrs = !string.IsNullOrEmpty(Request["OrderNum"]) ? Request["OrderNum"].Split(spiltChar) : new string[0];
            string id = Request["ID"];
            FriendlyLinks dbItem = new FriendlyLinks();
            dbItem.ID = id;
            for (int i = 0; i < names.Length; i++)
            {
                int orderNumr = 0;
                if (!string.IsNullOrEmpty(names[i]) && !string.IsNullOrEmpty(urls[i]))
                {
                    FriendlyLink item = new FriendlyLink
                    {
                        Name = names[i],
                        Url = urls[i],
                        OrderNum = int.TryParse(orderNumrs[i], out orderNumr) ? orderNumr : 1
                    };
                    dbItem.Links.Add(item);
                }
            }
            MrHaiApplication.SaveFriendlyLinks(dbItem);
            return RedirectToAction("");
        }

    }
}