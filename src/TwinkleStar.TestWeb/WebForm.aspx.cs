using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TwinkleStar.Common;
using TwinkleStar.Common.Logging;

namespace TwinkleStar.TestWeb
{
    public partial class WebForm : System.Web.UI.Page
    {
        public string sysConfig { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Configs.SetConfig(HttpContext.Current.Request.ApplicationPath, "name", "fuxing");
                //sysConfig = Configs.GetConfig("name");

                Logger logger = Logger.GetLogger(this.GetType());

                logger.Info("Loaded");
            }
            catch (Exception ex)
            {
                sysConfig = ex.Message;
            }
        }
    }
}