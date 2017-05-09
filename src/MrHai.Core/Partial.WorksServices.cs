using MrHai.Application.Models;
using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TwinkleStar.Common.Extensions;
using TwinkleStar.Common.Others;

namespace MrHai.Core
{
    /// <summary>
    /// works相关
    /// </summary>
    public abstract partial class MrHaiService
    {
        public OperationResult SearchWorks(string name,string pid ,ref int recordCount, int pageIndex, int pageCount)
        {
            OperationResult or = new OperationResult(OperationResultType.Success);
            pageIndex -= 1;
            //获取父级
            var parents = CategoryRepository.GetModel(x => x.ParentId.Equals("1") && !x.IsDeleted.Value && x.Enabled.Value && x.CategoryType == NavigationType.MainNavigation).ToList();
            if (!string.IsNullOrEmpty(pid))
            {
                parents = parents.Where(it => it.Id == pid).ToList();
            }
            var parentIds = parents.Select(x => x.Id);
            //集合数量
            recordCount = CategoryRepository.GetModel(x => x.Name.Contains(name) && parentIds.Contains(x.ParentId) && !x.IsDeleted.Value && x.Enabled.Value && x.CategoryType == NavigationType.MainNavigation).Count();
            //实体
            var entity = CategoryRepository.GetModel(x => x.Name.Contains(name) && parentIds.Contains(x.ParentId) && !x.IsDeleted.Value && x.Enabled.Value && x.CategoryType == NavigationType.MainNavigation)
                .OrderByDescending(x => x.OrderNum).Skip(pageIndex * pageCount).Take(pageCount).MapToList<CategoryListDto>().ToList();

            entity.ForEach(x => x.ParentName = parents.FirstOrDefault(p => p.Id == x.ParentId)?.Name);
            or.AppendData = entity;
            return or;
        }
        public OperationResult SearchWorks(string name, ref int recordCount, int pageIndex, int pageCount)
        {
            OperationResult or = new OperationResult(OperationResultType.Success);
            pageIndex -= 1;
            //获取父级
            var parents = CategoryRepository.GetModel(x => x.ParentId.Equals("1") && !x.IsDeleted.Value && x.Enabled.Value && x.CategoryType == NavigationType.MainNavigation).ToList();
            
            var parentIds = parents.Select(x => x.Id);
            //集合数量
            recordCount = CategoryRepository.GetModel(x => x.Name.Contains(name) && parentIds.Contains(x.ParentId) && !x.IsDeleted.Value && x.Enabled.Value && x.CategoryType == NavigationType.MainNavigation).Count();
            //实体
            var entity = CategoryRepository.GetModel(x => x.Name.Contains(name) && parentIds.Contains(x.ParentId) && !x.IsDeleted.Value && x.Enabled.Value && x.CategoryType == NavigationType.MainNavigation)
                .OrderByDescending(x => x.OrderNum).Skip(pageIndex * pageCount).Take(pageCount).MapToList<CategoryListDto>().ToList();

            entity.ForEach(x => x.ParentName = parents.FirstOrDefault(p => p.Id == x.ParentId)?.Name);
            or.AppendData = entity;
            return or;
        }


        public OperationResult CommonSearch(string name,string pid ,InfomationType infoType, ref int recordCount, int pageIndex, int pageCount)
        {
            pageIndex -= 1;
            OperationResult or = new OperationResult(OperationResultType.Success);
            //获取父级
            var parents = CategoryRepository.GetModel(x => x.ParentId.Equals("1") && !x.IsDeleted.Value && x.Enabled.Value && x.CategoryType == NavigationType.MainNavigation).ToList();
            if (!string.IsNullOrEmpty(pid))
            {
                parents = parents.Where(it => it.Id == pid).ToList();
            }
            var parentIds = parents.Select(x => x.Id);

            var entity = CategoryRepository.GetModel(x => x.Name.Contains(name) && parentIds.Contains(x.ParentId) && !x.IsDeleted.Value
                                                    && x.Enabled.Value && x.CategoryType == NavigationType.MainNavigation).MapToList<CategoryListDto>().ToList();

            var workid = entity.Select(it => it.Id);
            recordCount = InfomationRepository.GetModel(x => workid.Contains(x.CategoryId) && x.InfoType == infoType && !x.IsDeleted.Value).Count();
            var result = InfomationRepository.GetModel(x => workid.Contains(x.CategoryId) && x.InfoType == infoType && !x.IsDeleted.Value)
                .OrderByDescending(x => x.CreateTime).Skip(pageIndex * pageCount).Take(pageCount).MapToList<InfomationDto>().ToList();
            entity.ForEach(x => x.ParentName = parents.FirstOrDefault(p => p.Id == x.ParentId)?.Name);
            result.ForEach((i) =>
            {
                var data = entity.FirstOrDefault(it => it.Id == i.CategoryId);
                i.WorkName = data.Name;
                i.CategoryName = data.ParentName;

            });
            entity.ForEach(x => x.ParentName = parents.FirstOrDefault(p => p.Id == x.ParentId)?.Name);
            or.AppendData = result;
            return or;
        }



        public OperationResult GetInfos(string id, NodeType type)
        {
            OperationResult or = new OperationResult(OperationResultType.Success);
            switch (type)
            {
                case NodeType.Category:
                    or = GetInfoByCategory(id);
                    break;
                case NodeType.Work:
                    or = GetInfoByWorkId(id);
                    break;
            };
            return or;
        }
        public OperationResult GetInfo(string id)
        {
            OperationResult or = new OperationResult(OperationResultType.Success);
            or.AppendData = InfomationRepository.Find(id);
            return or;
        }

        private OperationResult GetInfoByCategory(string id)
        {
            var or = new OperationResult(OperationResultType.Success);
            var models = InfomationRepository.GetModel(p => p.CategoryId == id && p.IsDeleted == false).OrderByDescending(p => p.Order);

            or.AppendData = models.ToList();
            return or;
        }

        private OperationResult GetInfoByWorkId(string id)
        {
            var or = new OperationResult(OperationResultType.Success);
            var models = InfomationRepository.GetModel(p => p.WorksId == id && p.IsDeleted == false).OrderByDescending(p => p.Order);

            or.AppendData = models.ToList();
            return or;
        }
    }
}
