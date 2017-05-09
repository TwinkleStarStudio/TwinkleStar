
using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using MrHai.Core.Models;
using MrHai.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Web;
using TwinkleStar.Common;

namespace MrHai.BMS.Controllers.Common
{
    [Export]
    public class AttachmentUpLoad: BaseController
    {
        public AttachmentUpLoad(IMrHaiApplication mrHaiApplication)
        {
            MrHaiApplication = mrHaiApplication;
        }
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        public string UpFile(HttpPostedFileBase file, FileEnum type,int? userID,out string msg)
        {
            Attachment entity = new Attachment();
            
            entity.FileName = Path.GetFileNameWithoutExtension(file.FileName);
            entity.Extension = Path.GetExtension(file.FileName);
            entity.Size = file.ContentLength;
            entity.UserID = userID;
            entity.FileType = type;
            string dbCode = MrHaiApplication.GetModelCode(entity);
            if (!string.IsNullOrEmpty(dbCode))
            {
                msg = "上传成功";
                return dbCode;
            }
            else
            {
                entity.Code = GetCode(type);
            }
            
            Stream img = file.InputStream;
            if (QiNiuHelper.UploadImage(entity.Code, img, entity.Extension))
            {
                if (MrHaiApplication.InsertAttachment(entity)>0)//插入附件表
                {
                    msg = "上传成功";
                    return entity.Code;
                }
                else
                {
                    msg = "插入附件表失败";
                    QiNiuHelper.Delete(entity.Code);
                    return string.Empty;
                }
            }
            else
            {
                msg = $"上传图片服务器失败";
                return string.Empty;
            }
        }
        public string UpFile(HttpPostedFileBase file,string code ,FileEnum type, int userID, out string msg)
        {
            Attachment entity = new Attachment();
            entity.Code = code;
            entity.FileName = Path.GetFileNameWithoutExtension(file.FileName);
            entity.Extension = Path.GetExtension(file.FileName);
            entity.Size = file.ContentLength;
            entity.UserID = userID;
            entity.FileType = type;
            Stream img = file.InputStream;
            if (QiNiuHelper.UploadImage(code, img, entity.Extension))
            {
                if (MrHaiApplication.InsertAttachment(entity) > 0)//插入附件表
                {
                    msg = "上传成功";
                    return entity.Code;
                }
                else
                {
                    msg = "插入附件表失败";
                    QiNiuHelper.Delete(entity.Code);
                    return string.Empty;
                }
            }
            else
            {
                msg = $"上传图片服务器失败";
                return string.Empty;
            }
        }


        public  string GetCode(FileEnum type)
        {
            int id = MrHaiApplication.GetMaxAttachmentID();//获取最大ID//todo
            if (id >= 0)
            {
                return CreateCode(id, type);
            }
            else
            {
                return string.Empty;
            }            
        }
        /// <summary>
        /// 编码生成器，前面是枚举的名字，紧跟后面三位是生成的随机数，然后第五位是枚举的值，再接着三位随机字幕，然后就是附件ID
        /// 以图片为例
        /// IMG +三位随机数+枚举值(1)+三位随机字母+附件ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public  string CreateCode(int id, FileEnum type)
        {
            Random ran = new Random();
            int RandKey = ran.Next(100, 999);
            string code = type.ToString() + RandKey + (int)type + GenerateRandom(3) + ++id;
            return code;
        }

        private static char[] constant = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        public static string GenerateRandom(int Length)
        {
            Random ran = new Random();
            string str = string.Empty;
            for (int i = 0; i < Length; i++)
            {
                str += constant[ran.Next(0, 25)];
            }
            return str;
        }
    }
}