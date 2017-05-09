using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MrHai.BMS.Models
{
    [Serializable]
    public class JsonResultWithoutData
    {
        public JsonResultWithoutData() : this(true, null)
        {

        }

        public JsonResultWithoutData(bool succeed, string msg)
        {
            Succeed = succeed;
            Msg = msg;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Succeed { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; }
    }


    [Serializable]
    public class JsonResultModel<T> : JsonResultWithoutData
    {
        public JsonResultModel(T data) : this(true, null, data)
        {
        }

        public JsonResultModel(bool succeed, string msg) : this(succeed, msg, default(T))
        {
        }

        public JsonResultModel(bool succeed, string msg, T data) : base(succeed, msg)
        {
            Data = data;
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

    }



    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error
            };
        }

        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            if (this.ContentEncoding != null)
                response.ContentEncoding = this.ContentEncoding;
            if (this.Data == null)
                return;

            response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            var scriptSerializer = JsonSerializer.Create(this.Settings);
            // Serialize the data to the Output stream of the response
            scriptSerializer.Serialize(response.Output, this.Data);
        }
    }


}