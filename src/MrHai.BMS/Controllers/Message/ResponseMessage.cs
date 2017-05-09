using System;

namespace MrHai.BMS.Controllers
{
    public class ResponseMessage
    {
        public ResponseMessage()
        {
            result = new ResponseResult();
        }

        public ResponseHead head { set; get; }

        public object body { set; get; }

        public ResponseResult result { set; get; }
    }

    public class ResponseHead
    {
        /// <summary>
        /// 响应状态
        /// <para>200：响应正常</para>
        /// <para>500：系统错误</para>
        /// <para>501：验签失败</para>
        /// </summary>
        public int statuscode { get; set; }

        /// <summary>
        /// 响应格式
        /// 目前只支持 Json,XML
        /// </summary>
        public string restype { get; set; }
    }

    public class ResponseResult
    {
        /// <summary>
        /// 返回错误代码【业务定义】
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 返回消息【业务定义】
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 服务时间【框架提供】
        /// </summary>
        public string sertime { get { return DateTime.Now.ToString(); } }

        /// <summary>
        /// 需要携带的其他信息
        /// </summary>
        public object info { set; get; }

        /// <summary>
        /// 缓存是否过期
        /// <para>0：已过期</para>
        /// <para>1：未过期，Body内容为Null</para>
        /// </summary>
        public int cachestatus { get; set; }
    }
}