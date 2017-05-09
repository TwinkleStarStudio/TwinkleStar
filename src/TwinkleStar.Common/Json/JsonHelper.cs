using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwinkleStar.Common.Json
{
    public class JsonHelper
    {
        /// <summary>
        /// 序列化json
        /// </summary>
        /// <param name="value">对象</param>
        /// <param name="isIndented">是否缩进</param>
        /// <returns></returns>
        public static string Serialize(object value, bool isIndented)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value is null.");
            }

            return JsonConvert.SerializeObject(value, isIndented ? Formatting.Indented : Formatting.None);
        }

        /// <summary>
        /// 将字符反序列化成对象
        /// </summary>
        /// <typeparam name="TEntity">泛型对象</typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static TEntity Deserialize<TEntity>(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data is null or empty.");
            }

            return JsonConvert.DeserializeObject<TEntity>(data);
        }
    }
}
