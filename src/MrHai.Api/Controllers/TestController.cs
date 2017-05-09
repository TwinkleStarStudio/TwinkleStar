using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using MrHai.Application;

namespace MrHai.Api.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [Export]
    public class TestController : ApiController
    {
        /// <summary>
        /// 注入
        /// </summary>
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        public object TestApplocation()
        {

            var result = MrHaiApplication.GetFriendlyLinks();

            return result;
        }
    }
}
