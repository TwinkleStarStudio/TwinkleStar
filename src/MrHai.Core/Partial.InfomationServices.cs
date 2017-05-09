using MrHai.Core.Models;
using MrHai.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinkleStar.Common.Others;

namespace MrHai.Core
{
    /// <summary>
    /// Information相关
    /// </summary>
    public abstract partial class MrHaiService
    {
        public int SaveInfomation(List<Infomation> list)
        {
            foreach (var item in list)
            {
                string url = string.Empty;
                switch (item.InfoType)
                {
                    case InfomationType.Video:
                        url = GlobalConstants.ProstillsUrl;
                        break;
                    case InfomationType.Article:
                        url = GlobalConstants.RelatedUrl;
                        break;
                    case InfomationType.Comments:
                        url = GlobalConstants.PublicationsUrl;
                        break;
                    case InfomationType.ArtistCV:
                        url = GlobalConstants.ArtistUrl;
                        break;
                    case InfomationType.Exhibitions:
                        url = GlobalConstants.ExhibitionsUrl;
                        break;
                    case InfomationType.Installation:
                        url = GlobalConstants.InstallationUrl;
                        break;
                    case InfomationType.FilmStill:
                        url = GlobalConstants.ProtrailerUrl;
                        break;
                    case InfomationType.BackgroundImage:
                        url = null;
                        break;
                    case InfomationType.About:
                        url = GlobalConstants.ProaboutUrl;
                        break;
                    case InfomationType.Summary:
                        url = null;
                        break;
                    case InfomationType.MainArtistCV:
                        url = GlobalConstants.TextsUrl;
                        break;
                    case InfomationType.News:
                        url = GlobalConstants.News;
                        break;
                    case InfomationType.Other:
                        url = null;
                        break;
                    default:
                        url = null;
                        break;
                }
                item.Url = url + $"?pid={item.CategoryId}&infoid={item.Id}";
            }
            return InfomationRepository.Insert(list);
        }

        public int UpdateinfomationChanges(Infomation entiy)
        {
            string url = string.Empty;
            switch (entiy.InfoType)
            {
                case InfomationType.Video:
                    url = GlobalConstants.ProstillsUrl;
                    break;
                case InfomationType.Article:
                    url = GlobalConstants.RelatedUrl;
                    break;
                case InfomationType.Comments:
                    url = GlobalConstants.PublicationsUrl;
                    break;
                case InfomationType.ArtistCV:
                    url = GlobalConstants.ArtistUrl;
                    break;
                case InfomationType.Exhibitions:
                    url = GlobalConstants.ExhibitionsUrl;
                    break;
                case InfomationType.Installation:
                    url = GlobalConstants.InstallationUrl;
                    break;
                case InfomationType.FilmStill:
                    url = GlobalConstants.ProtrailerUrl;
                    break;
                case InfomationType.BackgroundImage:
                    url = null;
                    break;
                case InfomationType.About:
                    url = GlobalConstants.ProaboutUrl;
                    break;
                case InfomationType.Summary:
                    url = null;
                    break;
                case InfomationType.MainArtistCV:
                    url = GlobalConstants.TextsUrl;
                    break;
                case InfomationType.News:
                    url = GlobalConstants.News;
                    break;
                case InfomationType.Other:
                    url = null;
                    break;
                default:
                    url = null;
                    break;
            }
            entiy.Url = url + $"?pid={entiy.CategoryId}&infoid={entiy.Id}";
            return InfomationRepository.Update(entiy);
        }
        public int UpdateinfomationChanges(List<Infomation> list)
        {
            foreach (var item in list)
            {
                string url = string.Empty;
                switch (item.InfoType)
                {
                    case InfomationType.Video:
                        url = GlobalConstants.ProstillsUrl;
                        break;
                    case InfomationType.Article:
                        url = GlobalConstants.RelatedUrl;
                        break;
                    case InfomationType.Comments:
                        url = GlobalConstants.PublicationsUrl;
                        break;
                    case InfomationType.ArtistCV:
                        url = GlobalConstants.ArtistUrl;
                        break;
                    case InfomationType.Exhibitions:
                        url = GlobalConstants.ExhibitionsUrl;
                        break;
                    case InfomationType.Installation:
                        url = GlobalConstants.InstallationUrl;
                        break;
                    case InfomationType.FilmStill:
                        url = GlobalConstants.ProtrailerUrl;
                        break;
                    case InfomationType.BackgroundImage:
                        url = null;
                        break;
                    case InfomationType.About:
                        url = GlobalConstants.ProaboutUrl;
                        break;
                    case InfomationType.Summary:
                        url = null;
                        break;
                    case InfomationType.MainArtistCV:
                        url = GlobalConstants.TextsUrl;
                        break;
                    case InfomationType.News:
                        url = GlobalConstants.News;
                        break;
                    case InfomationType.Other:
                        url = null;
                        break;
                    default:
                        url = null;
                        break;
                }
                item.Url = url + $"?pid={item.CategoryId}&infoid={item.Id}";
            }

            return InfomationRepository.Update(list);
        }

        public int DeleteInfomation(List<Infomation> list)
        {
            return InfomationRepository.Delete(list);
        }
        public string GetModelByTitle(string title, string workID, InfomationType type)
        {
            var model = InfomationRepository.GetModel(it => it.IsDeleted == false && it.CategoryId == workID && it.Title == title && it.InfoType == type);
            return model.FirstOrDefault()?.Id ?? string.Empty;
        }

        public OperationResult GetInfomationByType(InfomationType type)
        {
            OperationResult or = new OperationResult(OperationResultType.Success);
            var models = InfomationRepository.GetModel(it => it.IsDeleted == false && it.InfoType == type);
            or.AppendData = models.ToList();
            return or;
        }


        public OperationResult GetInfomationModelByType(InfomationType type)
        {
            OperationResult or = new OperationResult(OperationResultType.Success);
            var model = InfomationRepository.Find(it => it.IsDeleted == false && it.InfoType == type);
            or.AppendData = model;
            return or;
        }

        public OperationResult GetInfomations(List<string> categoryIds, List<InfomationType> types)
        {
            OperationResult or = new OperationResult(OperationResultType.Success);
            var model = InfomationRepository.GetModel(p => p.IsDeleted == false && categoryIds.Contains(p.CategoryId) && types.Contains(p.InfoType)).ToList();
            or.AppendData = model.ToList();
            return or;
        }

        public OperationResult GetInfo(InfomationType type)
        {
            OperationResult or = new OperationResult(OperationResultType.Success);
            var model = InfomationRepository.GetModel(p => p.IsDeleted == false && p.InfoType == type).ToList();
            or.AppendData = model.FirstOrDefault();
            return or;
        }


        public OperationResult GetInfoByPid(string pid)
        {
            OperationResult or = new OperationResult(OperationResultType.Success);
            var model = InfomationRepository.GetModel(p => p.IsDeleted == false && p.CategoryId == pid).ToList();
            or.AppendData = model.ToList();
            return or;

        }
        public OperationResult GetInfoByPid(string pid, InfomationType type)
        {
            OperationResult or = new OperationResult(OperationResultType.Success);
            var model = InfomationRepository.GetModel(p => p.IsDeleted == false && p.CategoryId == pid && p.InfoType == type).ToList();
            or.AppendData = model.ToList();
            return or;

        }


    }
}
