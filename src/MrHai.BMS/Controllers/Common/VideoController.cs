using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using MrHai.BMS.Controllers.Common;
using MrHai.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwinkleStar.Common;

namespace MrHai.BMS.Controllers.Work
{
    //视频
    [Export]
    public class VideoController : BaseController
    {
        string fileEX = ".ogg .web .mp4";

        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        
        // GET: Video
        public ActionResult Index()
        {
            
            return View();
        }


        public ActionResult UpVideo(HttpPostedFileBase file)
        {
            string message = string.Empty;
            string filename = Path.GetFileName(file.FileName);
            int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
            string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
            if (!fileEX.Contains(fileEx))
            {
                responseMessage.result.errcode = 1;
                responseMessage.result.msg = $"只能上传{fileEX}格式的视频";
                return Json(responseMessage);
            }
            AttachmentUpLoad AttachmentUpLoad = new AttachmentUpLoad(MrHaiApplication);
            string code = AttachmentUpLoad.UpFile(file, FileEnum.Video, UserID, out message);
            string url = Configs.GetConfig("Url");
            url += code;
            responseMessage.result.errcode = 0;
            responseMessage.result.msg = url;
            return Json(responseMessage);
        }

        public ActionResult UpLoadBackGroundVideo(HttpPostedFileBase file)
        {
            string message = string.Empty;
            string filename = Path.GetFileName(file.FileName);
            int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
            string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
            if (!fileEX.Contains(fileEx))
            {
                responseMessage.result.errcode = 1;
                responseMessage.result.msg = $"只能上传{fileEX}格式的视频";
                return Json(responseMessage);
            }
            AttachmentUpLoad AttachmentUpLoad = new AttachmentUpLoad(MrHaiApplication);
            string code = AttachmentUpLoad.UpFile(file, "BackgroundVideo", FileEnum.Video, UserID.Value, out message);
            string url = Configs.GetConfig("Url");
            Random random = new Random();
            url += code + $"?v={random.Next()}";
            responseMessage.result.errcode = 0;
            responseMessage.result.msg = url;
            return Json(responseMessage);
        }
    }
}