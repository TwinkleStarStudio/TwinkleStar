using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using MrHai.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwinkleStar.Common;

namespace MrHai.BMS.Controllers.Common
{
    [Export]
    public class ImageController : BaseController
    {
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        string FileType = ".jpg/.gif/.bmp/.png";//定义上传文件的类型字符串
        // GET: Image
        /// <summary>
        /// 富文本编辑器上传
        /// </summary>
        /// <param name="upload"></param>
        public void Index(HttpPostedFileBase upload)
        {
            
            string filename = Path.GetFileName(upload.FileName);
            
            int filesize = upload.ContentLength;//获取上传文件的大小单位为字节byte
            string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
            String callback = Request["CKEditorFuncNum"];
            if (!FileType.Contains(fileEx))
            {
                Response.Write("<script type=\"text/javascript\">");
                Response.Write("window.parent.CKEDITOR.tools.callFunction(" + callback + ",''," + "'文件格式不正确（必须为.jpg/.gif/.bmp/.png文件）');");
                Response.Write("</script>");
            }
            string msg = string.Empty;
            AttachmentUpLoad AttachmentUpLoad = new AttachmentUpLoad(MrHaiApplication);
            string code = AttachmentUpLoad.UpFile(upload, FileEnum.IMG, UserID, out msg);
            if (!string.IsNullOrEmpty(code))
            {
                string url = Configs.GetConfig("Url") + code;
                Response.Write("<script type=\"text/javascript\">");
                Response.Write("window.parent.CKEDITOR.tools.callFunction(" + callback + ",'" + url + "','')");
                Response.Write("</script>");
            }
            else
            {
                Response.Write("上传失败");
            }
        }
        /// <summary>
        /// 直接上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult UpLoad(HttpPostedFileBase file)
        {
            if (file == null)
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "上传文件失败";
                return JsonBase(responseMessage);
            }

            string filename = Path.GetFileName(file.FileName);
            int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
            string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
            if (!FileType.Contains(fileEx))
            {
                responseMessage.result.errcode = 1;
                responseMessage.result.msg = "为上传指定格式";
                return JsonBase(responseMessage);
            }
            string msg = string.Empty;
            AttachmentUpLoad AttachmentUpLoad = new AttachmentUpLoad(MrHaiApplication);
            string code = AttachmentUpLoad.UpFile(file, FileEnum.IMG, UserID, out msg);
            if (!string.IsNullOrEmpty(code))
            {

                string url = Configs.GetConfig("Url");
                url += code;
                responseMessage.result.errcode = 0;
                responseMessage.result.msg = url;
                return JsonBase(responseMessage);
            }
            else
            {
                responseMessage.result.errcode = 2;
                responseMessage.result.msg = $"上传错误,+{msg}";
                return JsonBase(responseMessage);
            }
        }

        /// <summary>
        /// 直接上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult UpLoadImages(HttpPostedFileBase[] file)
        {
            if (file == null || file.Length <= 0)
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "上传文件失败";
                return JsonBase(responseMessage);
            }
            foreach (var item in file)
            {
                string filename = Path.GetFileName(item.FileName);
                int filesize = item.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
                if (!FileType.Contains(fileEx))
                {
                    responseMessage.result.errcode = 1;
                    responseMessage.result.msg = "有些图片为上传指定格式";
                    return JsonBase(responseMessage);
                }
            }

            List<string> urls = new List<string>();
            foreach (var item in file)
            {
                string msg = string.Empty;
                AttachmentUpLoad AttachmentUpLoad = new AttachmentUpLoad(MrHaiApplication);
                string code = AttachmentUpLoad.UpFile(item, FileEnum.IMG, UserID, out msg);
                if (!string.IsNullOrEmpty(code))
                {

                    string url = Configs.GetConfig("Url");
                    url += code;
                    urls.Add(url);
                }
                else
                {
                    responseMessage.result.errcode = 2;
                    responseMessage.result.msg = $"上传失败,+{msg}";
                    return JsonBase(responseMessage);
                }
            }
            responseMessage.result.errcode = 0;
            responseMessage.result.msg = "上传成功";
            responseMessage.body = urls;
            return JsonBase(responseMessage);
        }

        public  ActionResult UpLoadLogo(HttpPostedFileBase file)
        {

            if (file == null)
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "上传文件失败";
                return JsonBase(responseMessage);
            }

            string filename = Path.GetFileName(file.FileName);
            int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
            string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
            if (!FileType.Contains(fileEx))
            {
                responseMessage.result.errcode = 1;
                responseMessage.result.msg = "为上传指定格式";
                return JsonBase(responseMessage);
            }
            string msg = string.Empty;
            AttachmentUpLoad AttachmentUpLoad = new AttachmentUpLoad(MrHaiApplication);
            string code = AttachmentUpLoad.UpFile(file, "Logo", FileEnum.IMG, UserID.Value, out msg);
            if (!string.IsNullOrEmpty(code))
            {

                string url = Configs.GetConfig("Url");
                Random random = new Random();
                url += code + $"?v={random.Next()}"; ;
                responseMessage.result.errcode = 0;
                responseMessage.result.msg = url;
                return JsonBase(responseMessage);
            }
            else
            {
                responseMessage.result.errcode = 2;
                responseMessage.result.msg = $"上传错误,+{msg}";
                return JsonBase(responseMessage);
            }
        }
    }
}