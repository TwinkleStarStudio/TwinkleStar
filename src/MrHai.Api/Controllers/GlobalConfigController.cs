using MrHai.Api.Models;
using MrHai.Application;
using MrHai.Application.Models.ApiModels;
using MrHai.Common.GlobalConfig;
using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TwinkleStar.Common;
using TwinkleStar.Common.Others;

namespace MrHai.Api.Controllers
{
    /// <summary>
    /// 碎片模块
    /// </summary>
    [Export]
    public class GlobalConfigController : ApiController
    {
        /// <summary>
        /// 注入
        /// </summary>
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }

        string root = Configs.GetConfig("rootUrl");

        /// <summary>
        /// 获取碎片列表
        /// </summary>
        /// <returns></returns>
        public ResponseDto Get()
        {
            var re = new ResponseDto<dynamic>();
            var or = MrHaiApplication.GetFragment();
            if (or.ResultType != OperationResultType.Success)
            {
                return ResponseDto.ErrorResponse(or.Message);
            }

            var config = or.AppendData as Fragment;

            var ids = config.Fragments.Select(p => p.WorksId).ToList();
            var infoListOr = MrHaiApplication.GetInfomations(ids, new List<InfomationType>() { InfomationType.Summary, InfomationType.About });
            var categoryOr = MrHaiApplication.GetCategories(ids);

            var infoList = infoListOr.AppendData as List<Infomation>;
            var categoryList = categoryOr.AppendData as List<Category>;

            var frameSetting = or.AppendData as Fragment;
            re.Data = frameSetting.Fragments.Select(p => ConvertToDto(p, categoryList, infoList));
            return re;
        }

        private FragmentDto ConvertToDto(FragmentSetting frame, List<Category> categories, List<Infomation> infos)
        {
            var category = categories?.FirstOrDefault(p => p.Id == frame.WorksId);
            var infoSummary = infos?.FirstOrDefault(p => p.CategoryId == frame.WorksId && p.InfoType ==InfomationType.Summary);
            var infoAbout = infos?.FirstOrDefault(p => p.CategoryId == frame.WorksId && p.InfoType == InfomationType.About);
            return new FragmentDto()
            {
                Num = frame.SerialNumber,
                CategoryId = frame.WorksId,
                Url = string.IsNullOrEmpty(infoAbout?.Url)?"": root+ infoAbout?.Url,
                Summary = infoSummary?.Content,
            };

        }

    }
}
