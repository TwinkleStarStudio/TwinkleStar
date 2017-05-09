
using System.Configuration;

namespace TwinkleStar.Common
{
    /// <summary>
    /// 数据库连接字符串(配置文件字符串命名为"TwinkleDB")
    /// </summary>
    public class DBConnectionStrings
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["TwinkleDB"] != null)
                {
                    return ConfigurationManager.ConnectionStrings["TwinkleDB"].ToString();
                }
                throw new ConfigurationErrorsException("未配置key为TwinkleDB的连接字符串。");
            }
        }
    }
}
