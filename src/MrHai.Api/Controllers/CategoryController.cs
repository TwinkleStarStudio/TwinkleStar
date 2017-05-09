using MrHai.Application;
using MrHai.Application.Models;
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
    /// 菜单
    /// </summary>
    [Export]
    public class CategoryController : ApiController
    {
        /// <summary>
        /// 注入
        /// </summary>
        [Import]
        public IMrHaiApplication MrHaiApplication { get; set; }

        string root = Configs.GetConfig("rootUrl");

        /// <summary>
        /// 获取菜单列表
        /// </summary> 
        public ResponseDto<List<CategoryDto>> GetParent()
        {
            var pids = new List<string>();
            var or = MrHaiApplication.GetRootCategories();
            var data = or.AppendData as List<Category>;

            var resultDto = new List<CategoryDto>();

            foreach (var item in data)
            {
                var dto = new CategoryDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Url = string.IsNullOrEmpty(item.Url) ? "" : root + item.Url,
                    Child = new List<CategoryDto>()
                };
                switch (item.Name)
                {
                    case "works"://获取二级菜单
                        //dto.Type = CategoryDtoType.None.GetHashCode();
                        var secondChildOr = MrHaiApplication.GetChildCategories(item.Id, NavigationType.MainNavigation);
                        var secondChild = secondChildOr.AppendData as List<Category>;
                        var secondList = new List<CategoryDto>();
                        foreach (var sec in secondChild)// 获取三级菜单
                        {
                            var second = new CategoryDto()
                            {
                                Name = sec.Name,
                                Id = sec.Id,
                                Url = string.IsNullOrEmpty(sec.Url) ? "" : root + sec.Url,

                                Child = new List<CategoryDto>()
                            };
                            var thirdChildOr = MrHaiApplication.GetChildCategories(sec.Id, NavigationType.MainNavigation);
                            var thirdChild = thirdChildOr.AppendData as List<Category>;
                            foreach (var th in thirdChild)
                            {
                                var thirdInfos = (MrHaiApplication.GetInfoByPid(th.Id, InfomationType.About).AppendData as List<Infomation>)?.FirstOrDefault();
                                var third = new CategoryDto()
                                {
                                    Name = th.Name,
                                    Id = th.Id,
                                    Url = string.IsNullOrEmpty(th.Url) ? "" : root + thirdInfos?.Url,
                                };
                                second.Child.Add(third);
                            }
                            secondList.Add(second);
                        }
                        dto.Child = secondList;
                        break;
                    case "news"://读取配置  然后去拿对应的Category  
                                //var newworklist = (MrHaiApplication.GetNewWorks().AppendData as NewWorks)?.NewWorksList?.OrderBy(p => p.OrderNum);
                                //var ids = newworklist?.Select(p => p.WorksId)?.ToList() ?? new List<string>();
                                //var newCategoryList = MrHaiApplication.GetCategories(ids).AppendData as List<Category>;
                                ////dto.Name = "news/current exhibitions";
                                //foreach (var newwork in newworklist)//newCategoryList)
                                //{
                                //    var thisCate = newCategoryList?.FirstOrDefault(p => p.Id == newwork.WorksId);
                                //    var newsInfo = (MrHaiApplication.GetInfoByPid(newwork.WorksId, InfomationType.About).AppendData as List<Infomation>)?.FirstOrDefault();

                        //    var sds = new CategoryDto()
                        //    {
                        //        Id = newwork?.WorksId,
                        //        Name = thisCate?.Name,
                        //        Url = string.IsNullOrEmpty(newsInfo?.Url) ? "" : root + newsInfo?.Url
                        //    };
                        //    dto.Child.Add(sds);
                        //}
                        foreach (var info in MrHaiApplication.GetInfoByPid(item.Id).AppendData as List<Infomation>)
                        {
                            var infoSec = new CategoryDto()
                            {
                                Id = info.Id,
                                Name = info.Title,
                                Url = string.IsNullOrEmpty(info.Url) ? "" : root + info.Url
                            };
                            dto.Child.Add(infoSec);
                        }
                        break;
                    case "texts/publications"://获取二级菜单
                        foreach (var info in MrHaiApplication.GetInfoByPid(item.Id).AppendData as List<Infomation>)
                        {
                            var infoSec = new CategoryDto()
                            {
                                Id = info.Id,
                                Name = info.Title,
                                Url = string.IsNullOrEmpty(info.Url) ? "" : root + info.Url
                            };
                            dto.Child.Add(infoSec);
                        }
                        break;
                    case "artist cv"://拿资讯 type为 ArtistCV类型的第一个，直接显示  --完成
                        var art = (MrHaiApplication.GetInfomationByType(InfomationType.ArtistCV).AppendData as List<Infomation>)?.FirstOrDefault();
                        dto.Url = string.IsNullOrEmpty(art?.Url) ? "" : root + art?.Url;

                        //dto.Url = string.IsNullOrEmpty(item.Url) ? "" : root + item.Url;
                        break;
                    case "2gg.studio"://读取配置，直接显示  
                        dto.Url = ((MrHaiApplication.GetOfficialWebsite().AppendData as OfficialWebsite)?.OfficialWebsiteSetting)?.URL;
                        break;
                    case "contact"://读取配置，直接显示 
                        dto.Url = string.IsNullOrEmpty(item.Url) ? "" : root + item.Url;
                        break;
                    case "links":
                        dto.Name = "links";
                        foreach (var link in (MrHaiApplication.GetFriendlyLinks()?.AppendData as FriendlyLinks)?.Links)
                        {
                            var sec = new CategoryDto()
                            {
                                Name = link.Name,
                                Url = link.Url
                            };
                            dto.Child.Add(sec);
                        }
                        break;
                    default: continue;
                }
                resultDto.Add(dto);
            }

            var re = new ResponseDto<List<CategoryDto>>() { Data = new List<CategoryDto>() };
            re.Data = resultDto;
            return re;
        }
        /// <summary>
        /// 获取contact 
        /// </summary>
        /// <returns></returns>
        [Route("api/Category/Contact")]
        public ResponseDto<string> GetContact()
        {
            var re = new ResponseDto<string>() { };
            var concacts = MrHaiApplication.GetAboutUs().AppendData as AboutUs;
            re.Data = string.Join("<br/>", concacts?.AboutUsSetting?.Select(p => p.Email));
            return re;
        }

        /// <summary>
        /// 获取作品导航菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Category/child/{pid}")]
        public ResponseDto<List<CategoryDto>> GetChild(string pid)
        {
            var childOr = MrHaiApplication.GetChildCategories(pid, NavigationType.ArticleNavigation);
            var data = childOr.AppendData as List<Category>;
            var list = new List<CategoryDto>();

            foreach (var item in data)
            {
                var dto = new CategoryDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Url = string.IsNullOrEmpty(item.Url) ? "" : root + item.Url,
                    Child = new List<CategoryDto>()
                };
                switch (item.Name)
                {
                    case "about":
                        //dto.Type = CategoryDtoType.Show.GetHashCode();
                        var aboutInfo = (MrHaiApplication.GetInfoByPid(pid, InfomationType.About).AppendData as List<Infomation>)?.FirstOrDefault();
                        dto.Id = aboutInfo?.Id;
                        dto.Url = string.IsNullOrEmpty(aboutInfo.Url) ? "" : root + aboutInfo?.Url;
                        break;
                    case "film trailer":
                        //dto.Type = CategoryDtoType.Show.GetHashCode();
                        //dto.Id = (MrHaiApplication.GetInfoByPid(pid, InfomationType.Video).AppendData as List<Infomation>)?.FirstOrDefault()?.Id;
                        //dto.Content = MrHaiApplication.GetInfoByPid(pid, InfomationType.Video).AppendData;

                        var trailer = (MrHaiApplication.GetInfoByPid(pid, InfomationType.Video).AppendData as List<Infomation>)?.FirstOrDefault();
                        dto.Id = trailer?.Id;
                        dto.Url = string.IsNullOrEmpty(trailer.Url) ? "" : root + trailer?.Url;
                        break;
                    case "film stills":
                        //dto.Type = CategoryDtoType.Show.GetHashCode();
                        var stills = (MrHaiApplication.GetInfoByPid(pid, InfomationType.FilmStill).AppendData as List<Infomation>)?.FirstOrDefault();
                        dto.Id = stills?.Id;
                        dto.Url = string.IsNullOrEmpty(stills.Url) ? "" : root + stills?.Url;
                        //dto.Content = MrHaiApplication.GetInfoByPid(pid, InfomationType.FilmStill).AppendData;
                        break;
                    case "installation":

                        var insDto = (MrHaiApplication.GetInfoByPid(pid, InfomationType.Installation).AppendData as List<Infomation>)?.FirstOrDefault();
                        dto.Id = insDto?.Id;
                        dto.Url = string.IsNullOrEmpty(insDto.Url) ? "" : root + insDto?.Url;
                        //dto.Type = CategoryDtoType.Show.GetHashCode();
                        //dto.Content = MrHaiApplication.GetInfoByPid(pid, InfomationType.Installation).AppendData;
                        break;

                    case "exhibitions":
                        var exList = MrHaiApplication.GetInfoByPid(pid, InfomationType.Exhibitions).AppendData as List<Infomation>;
                        foreach (var ex in exList)
                        {
                            dto.Child.Add(new CategoryDto()
                            {
                                Id = ex.Id,
                                Url = string.IsNullOrEmpty(ex.Url) ? "" : root + ex.Url,
                                Name = ex.Title
                            });
                        }
                        break;
                    case "related texts":
                        var insList = MrHaiApplication.GetInfoByPid(pid, InfomationType.Article).AppendData as List<Infomation>;
                        foreach (var ins in insList)
                        {
                            dto.Child.Add(new CategoryDto()
                            {
                                Id = ins.Id,
                                Url = string.IsNullOrEmpty(ins.Url) ? "" : root + ins.Url,
                                Name = ins.Title
                            });
                        }
                        break;
                    case "publications":
                        var commlist = MrHaiApplication.GetInfoByPid(pid, InfomationType.Comments).AppendData as List<Infomation>;
                        foreach (var comm in commlist)
                        {
                            dto.Child.Add(new CategoryDto()
                            {
                                Id = comm.Id,
                                Url = string.IsNullOrEmpty(comm.Url) ? "" : root + comm.Url,
                                Name = comm.Title
                            });
                        }
                        break;
                    default:
                        continue;
                }
                list.Add(dto);
            }
            var re = new ResponseDto<List<CategoryDto>>() { Data = new List<CategoryDto>() };
            re.Data = list;
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
    /// <summary>
    /// 菜单Dto
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 菜单显示的名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public dynamic Content { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<CategoryDto> Child { get; set; }
    }
    /// <summary>
    /// 类型
    /// </summary>
    public enum CategoryDtoType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Show = 1,
        /// <summary>
        /// 
        /// </summary>
        Url = 2,
        /// <summary>
        /// 
        /// </summary>
        Request = 3
    }
}
