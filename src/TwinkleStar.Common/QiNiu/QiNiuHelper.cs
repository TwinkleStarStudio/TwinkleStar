using System;
using Qiniu.IO;
using System.IO;
using Qiniu.RS;
using Qiniu.RPC;
using TwinkleStar.Common.Logging;

namespace TwinkleStar.Common
{
    public class QiNiuHelper
    {
        static string bucket = Configs.GetConfig("Bucket");
        static string access_key = Configs.GetConfig("Access_Key");
        static string secret_key = Configs.GetConfig("Secret_Key");
        public static bool UploadImage(string name, Stream img, string type)
        {
            if (string.IsNullOrEmpty(name) || img == null)
            {
                return false;
            }
            Qiniu.Conf.Config.ACCESS_KEY = access_key;
            Qiniu.Conf.Config.SECRET_KEY = secret_key;
            IOClient target = new IOClient();
            PutExtra extra = new PutExtra();
            extra.MimeType = type;
            String key = name;
            PutPolicy put = new PutPolicy(bucket + ":" + key, 3600);
         
            string upToken = put.Token();
            PutRet ret = target.Put(upToken, key, img, extra);
            if (ret != null && ret.key != null && ret.key.Equals(key))  // upload successfully
            {
                return true;
            }
            else
            {
                Logger.GetLogger(ret?.Response ?? "").Error(ret?.ToString() ?? "");
                return false;
            }
        }

        public static bool Delete(string key)
        {
            Qiniu.Conf.Config.ACCESS_KEY = access_key;
            Qiniu.Conf.Config.SECRET_KEY = secret_key;
            //实例化一个RSClient对象，用于操作BucketManager里面的方法
            RSClient client = new RSClient();
            CallRet ret = client.Delete(new EntryPath(bucket, key));
            if (ret.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
