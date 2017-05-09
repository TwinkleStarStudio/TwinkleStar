using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MrHai.BMS
{
    /// <summary>
    /// 登录用户
    /// </summary>
    [Serializable]
    public class LoginUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }

        public string UserCode { get; set; }

        public string Name { get; set; }
    }
}