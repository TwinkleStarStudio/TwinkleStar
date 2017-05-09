using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwinkleStar.Common.Json;

namespace TwinkleStar.Common
{
    public static class HttpHelper
    {
        public static string GetPostBody(this HttpRequestBase requset)
        {
            using (StreamReader stream = new System.IO.StreamReader(requset.InputStream))
            {
                return stream.ReadToEnd();
            }
        }

        /// <summary>
        /// 获得请求对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requset"></param>
        /// <returns></returns>
        public static T GetPostBody<T>(this HttpRequestBase requset)
        {
            using (StreamReader stream = new System.IO.StreamReader(requset.InputStream))
            {
                string json = stream.ReadToEnd();
                return JsonHelper.Deserialize<T>(json);
            }
        }
    }
}
