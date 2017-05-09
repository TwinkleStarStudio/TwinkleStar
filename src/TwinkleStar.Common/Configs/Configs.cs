
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Configuration;

namespace TwinkleStar.Common
{
    /// <summary>
    /// 系统配置文件
    /// </summary>
    public class Configs
    {
        /// <summary>
        /// 根据键值获取配置文件
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static string GetConfig(string key)
        {
            string val = string.Empty;
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                val = ConfigurationManager.AppSettings[key];
            return val;
        }



        /// <summary>
        /// 获取所有配置文件
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetConfig()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
                dict.Add(key, ConfigurationManager.AppSettings[key]);
            return dict;
        }


        /// <summary>
        /// 根据键值获取配置文件
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetConfig(string key, string defaultValue)
        {
            string val = defaultValue;
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                val = ConfigurationManager.AppSettings[key];
            if (val == null)
                val = defaultValue;
            return val;
        }



        /// <summary>
        /// 写配置文件,如果节点不存在则自动创建
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool SetConfig(string key, string value)
        {

            try
            {
                Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (!conf.AppSettings.Settings.AllKeys.Contains(key))
                    conf.AppSettings.Settings.Add(key, value);
                else
                    conf.AppSettings.Settings[key].Value = value;
                conf.Save();
                ConfigurationManager.RefreshSection("AppSettings");
                return true;
            }
            catch { return false; }
        }



        /// <summary>
        /// 写配置文件(用键值创建),如果节点不存在则自动创建 (Web程序无效)
        /// </summary>
        /// <param name="dict">键值集合</param>
        /// <returns></returns>

        public static bool SetConfig(Dictionary<string, string> dict)
        {
            try
            {
                if (dict == null || dict.Count == 0)
                    return false;
                Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                foreach (string key in dict.Keys)
                {
                    if (!conf.AppSettings.Settings.AllKeys.Contains(key))
                        conf.AppSettings.Settings.Add(key, dict[key]);
                    else
                        conf.AppSettings.Settings[key].Value = dict[key];
                }
                conf.Save();
                ConfigurationManager.RefreshSection("AppSettings");
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 写配置文件,如果节点不存在则自动创建(仅web可用)
        /// </summary>
        /// <param name="exePath">服务器上虚拟根地址（HttpContext.Current.Request.ApplicationPath）</param>
        /// <param name="key">键值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool SetConfig(string exePath, string key, string value)
        {

            try
            {
                Configuration conf = WebConfigurationManager.OpenWebConfiguration(exePath);
                if (!conf.AppSettings.Settings.AllKeys.Contains(key))
                    conf.AppSettings.Settings.Add(key, value);
                else
                    conf.AppSettings.Settings[key].Value = value;
                conf.Save();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 写配置文件(用键值创建),如果节点不存在则自动创建 (仅web可用)
        /// </summary>
        /// <param name="exePath">服务器上虚拟根地址（HttpContext.Current.Request.ApplicationPath）</param>
        /// <param name="dict">键值集合</param>
        /// <returns></returns>

        public static bool SetConfig(string exePath, Dictionary<string, string> dict)
        {
            try
            {
                if (dict == null || dict.Count == 0)
                    return false;
                Configuration conf = WebConfigurationManager.OpenWebConfiguration(exePath);
                foreach (string key in dict.Keys)
                {
                    if (!conf.AppSettings.Settings.AllKeys.Contains(key))
                        conf.AppSettings.Settings.Add(key, dict[key]);
                    else
                        conf.AppSettings.Settings[key].Value = dict[key];
                }
                conf.Save();
                return true;
            }
            catch { return false; }
        }
    }
}
