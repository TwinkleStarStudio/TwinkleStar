using MrHai.Application;
using MrHai.Application.Models.ApiModels;
using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TwinkleStar.Common.Others;

namespace MrHai.Api.Controllers
{
    /// <summary>
    /// 资讯
    /// </summary>
    [Export]
    public class InformationController : ApiController
    {
        /// <summary>
        /// 注入
        /// </summary>
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }
        ///// <summary>
        ///// 根据菜单id， 获取资讯列表
        ///// </summary>
        ///// <param name="id">父节点Id</param> 
        //public ResponseDto<List<Infomation>> GetInfos(string id)
        //{
        //    var re = new ResponseDto<List<Infomation>>() { Data = new List<Infomation>() }; 
        //    var or = MrHaiApplication.GetInfoByPid(id);
        //    if (or.ResultType != OperationResultType.Success)
        //    {
        //        re.Status = ResponseStatus.Error.GetHashCode();
        //        re.Msg = or.Message;
        //    }
        //    re.Data = (or.AppendData as List<Infomation>) ?? new List<Infomation>();
        //    return re;
        //}

        /// <summary>
        /// 获取资讯
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Information/{id}")]
        public ResponseDto<Infomation> GetInfo(string id)
        {
            var re = new ResponseDto<Infomation>();
            var or = MrHaiApplication.GetInfo(id);
            if (or.ResultType != OperationResultType.Success)
            {
                re.Status = ResponseStatus.Error.GetHashCode();
                re.Msg = or.Message;
                return re;
            }

            re.Data = (or.AppendData as Infomation);
            return re;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/Information/Bg/{id}")]
        public ResponseDto<string> GetBg(string id)
        {
            var re = new ResponseDto<string>();
            var or = MrHaiApplication.GetInfoByPid(id, InfomationType.BackgroundImage);
            if (or.ResultType != OperationResultType.Success)
            {
                re.Status = ResponseStatus.Error.GetHashCode();
                re.Msg = or.Message;
                return re;
            }
            var url = (or.AppendData as List<Infomation>)?.FirstOrDefault().Content;
            re.Data = url;
            return re;

        }

        private InfomationType Map(string name)
        {
            InfomationType type = InfomationType.About;
            if (name == "about")
            {
                type = InfomationType.About;
            }
            if (name == "film trailer")
            {
                type = InfomationType.Video;
            }
            if (name == "film stills")
            {
                type = InfomationType.FilmStill;
            }
            if (name == "installation")
            {
                type = InfomationType.Installation;
            }
            if (name == "exhibitions")
            {
                type = InfomationType.Exhibitions;
            }
            if (name == "related texts")
            {
                type = InfomationType.Article;
            }
            if (name == "publications")
            {
                type = InfomationType.Comments;
            }
            return type;

        }

    }
}
