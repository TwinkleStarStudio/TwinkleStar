using MrHai.BMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TwinkleStar.Common.Json;

namespace MrHai.BMS
{
    public class TSAuthroizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string currentJson = HttpContext.Current.Request.Cookies["CurrentUser"].Value;

            if (string.IsNullOrEmpty(currentJson) || JsonHelper.Deserialize<LoginUser>(currentJson) == null)
            {
                FormsAuthentication.SignOut();

                if (HttpContext.Current.Request.HttpMethod.Equals(
                    System.Net.Http.HttpMethod.Post.Method, System.StringComparison.OrdinalIgnoreCase))
                {
                    throw new HttpException((int)HttpStatusCode.Unauthorized, "Unauthorized");
                }
                else
                {
                    return false;
                }
            }

            //return base.AuthorizeCore(httpContext);
            return true;
        }
    }
}