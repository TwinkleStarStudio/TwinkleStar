using MrHai.Application;
using MrHai.BMS.Controllers.Base;
using MrHai.BMS.Models.Work;
using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace MrHai.BMS.Controllers.Common.Funtion
{
    [Export]
    public class AddEWC
    {
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        public AddEWC(IMrHaiApplication haiApplication)
        {
            if (haiApplication!=null)
            {
                MrHaiApplication = haiApplication;
            }
           
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResponseMessage CommonAdd(viewAddModel data)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            if (string.IsNullOrEmpty(data.workId) || string.IsNullOrEmpty(data.title) || string.IsNullOrEmpty(data.content))
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "信息不完整，新增失败";
                return responseMessage;
            }
            if (!string.IsNullOrEmpty(MrHaiApplication.GetModelByTitle(data.title, data.workId, data.infoType)))
            {
                responseMessage.result.errcode = 1;
                responseMessage.result.msg = "信息存在";
                return responseMessage;
            }
            Infomation exhibition = new Infomation()
            {
                Title = data.title,
                CategoryId = data.workId,
                Content = data.content,
                CreateUserId = data.userID,
                CreateTime = DateTime.Now,
                InfoType = data.infoType,
                IsDeleted = false
            };
            int count = MrHaiApplication.SaveInfomation(new List<Infomation> { exhibition });
            if (count > 0)
            {
                responseMessage.result.errcode = 0;
                responseMessage.result.msg = "新增成功";
            }
            else
            {
                responseMessage.result.errcode = 2;
                responseMessage.result.msg = "新增失败";
            }
            return responseMessage;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResponseMessage CommonUpdate(viewAddModel data)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            if (string.IsNullOrEmpty(data.workId) || string.IsNullOrEmpty(data.title) || string.IsNullOrEmpty(data.content))
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "信息不完整，更新失败";
                return responseMessage;
            }
            
            var dbModel = MrHaiApplication.GetInfo(data.infoID).AppendData as Infomation;
            dbModel.CategoryId = data.workId;
            dbModel.Title = data.title;
            dbModel.Content = data.content;
            
            int count = MrHaiApplication.UpdateinfomationChanges(new List<Infomation> { dbModel });
            if (count > 0)
            {
                responseMessage.result.errcode = 0;
                responseMessage.result.msg = "更新成功";
            }
            else
            {
                responseMessage.result.errcode = 2;
                responseMessage.result.msg = "更新失败";
            }
            return responseMessage;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseMessage CommonDelete(string id)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            if (string.IsNullOrEmpty(id))
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "确少必要参数";
                return responseMessage;
            }
            
            var dbModel = MrHaiApplication.GetInfo(id).AppendData as Infomation;
            if (dbModel==null)
            {
                responseMessage.result.errcode = -1;
                responseMessage.result.msg = "该条信息不存在";
                return responseMessage;
            }
            dbModel.IsDeleted = true;
            dbModel.DeletedTime = DateTime.Now;            

            int count = MrHaiApplication.UpdateinfomationChanges(new List<Infomation> { dbModel });
            if (count > 0)
            {
                responseMessage.result.errcode = 0;
                responseMessage.result.msg = "删除成功";
            }
            else
            {
                responseMessage.result.errcode = 2;
                responseMessage.result.msg = "删除失败";
            }
            return responseMessage;
        }

    }
}