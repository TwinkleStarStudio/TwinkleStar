using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MrHai.Core.Models;
using TwinkleStar.Common.Others;
using MrHai.Application.Models;
using TwinkleStar.Common.Extensions;
using MrHai.Core.Models.Enums;

namespace MrHai.Core
{
    /// <summary>
    /// Category相关
    /// </summary>
    public abstract partial class MrHaiService
    {

        /// <summary>
        /// 获取树状菜单列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pids"></param>
        /// <param name="isGetEnabled"></param>
        /// <returns></returns>  
        public virtual OperationResult GetCategories(NavigationType type = NavigationType.MainNavigation, List<string> pids = null, bool isGetEnabled = false)
        {
            var or = new OperationResult(OperationResultType.Success);
            var models = CategoryRepository.GetModel().Where(p => p.CategoryType == type &&
                                                            p.IsDeleted == false);
            if (!isGetEnabled)
            {
                models = models.Where(p => p.Enabled == true);
            }
            if (pids != null && pids.Count > 0)
            {
                models = models.Where(p => pids.Contains(p.ParentId));
            }
            else
            {
                models = models.Where(p => p.ParentId == null);
            }
            var data = models.OrderByDescending(p => p.OrderNum).ToList().MapToList<CategoryListDto>().ToList();
            GetChildCategories(data, type);
            or.AppendData = data;
            return or;
        }
        public virtual OperationResult GetCategories(List<string> ids)
        {
            ids = ids ?? new List<string>();
            var or = new OperationResult(OperationResultType.Success);
            var data = CategoryRepository.GetModel().Where(p => ids.Contains(p.Id)).ToList();
            or.AppendData = data;
            return or;
        }
        public virtual OperationResult GetCategory(string id)
        {
            var or = new OperationResult(OperationResultType.Success);
            var data = CategoryRepository.GetModel().FirstOrDefault(p => p.Id == id);
            or.AppendData = data;
            return or;
        }
       

        public virtual OperationResult GetRootCategories()
        {
            var or = new OperationResult(OperationResultType.Success);
            var data = CategoryRepository.GetModel().Where(p => p.Enabled == true
                                && p.CategoryType == NavigationType.MainNavigation
                                && p.IsDeleted == false && p.Level == 0).OrderByDescending(p => p.OrderNum);
            or.AppendData = data.ToList();
            return or;
        }

        public virtual OperationResult GetChildCategories(string pid, NavigationType type)
        {
            var or = new OperationResult(OperationResultType.Success);
            var data = CategoryRepository.GetModel().Where(p => p.Enabled == true
                                && p.CategoryType == type && p.ParentId == pid
                                && p.IsDeleted == false).OrderByDescending(p=>p.OrderNum);
            or.AppendData = data.ToList();
            return or;
        }
        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <param name="parentCategories"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual IEnumerable<CategoryListDto> GetChildCategories(List<CategoryListDto> parentCategories, NavigationType type)
        {
            var models = new List<CategoryListDto>();
            if (parentCategories != null)
            {
                var pids = parentCategories.Select(p => p.Id).ToList();
                models = CategoryRepository.GetModel().Where(p => pids.Contains(p.ParentId) &&
                                                         p.IsDeleted == false && p.Enabled == true).ToList()?.MapToList<CategoryListDto>();
                if (models != null && models.Count > 0)
                {
                    GetChildCategories(models, type);
                }
                foreach (var category in parentCategories)
                {
                    var mainchilds = models.Where(p => p.ParentId == category.Id && p.CategoryType == type).OrderByDescending(p => p.OrderNum).ToList() ?? new List<CategoryListDto>();
                    var articleChilds = models.Where(p => p.ParentId == category.Id && p.CategoryType != type).OrderByDescending(p => p.OrderNum).ToList() ?? new List<CategoryListDto>();

                    category.ChildCategory = mainchilds;
                    category.HasOtherNavigation = articleChilds != null && articleChilds.Count > 0;
                }

            }
            return models;
        }
        //获取所有作品名
        public virtual OperationResult GetWorksName()
        {
            var or = new OperationResult(OperationResultType.Success);
            List<CategoryListDto> workList = new List<CategoryListDto>();
            var worksPNode = GetCategories(NavigationType.MainNavigation, new List<string> { "1" }).AppendData as List<CategoryListDto>;
            foreach (var item in worksPNode)
            {
                foreach (var child in item.ChildCategory)
                {
                    CategoryListDto entity = new CategoryListDto()
                    {
                        Id = child.Id,
                        Name = child.Name
                    };
                    workList.Add(entity);
                }
            }
            or.AppendData = workList;
            return or;
        }

        public virtual OperationResult GetWorksName(string id)
        {
            var or = new OperationResult(OperationResultType.Success);
            List<CategoryListDto> workList = new List<CategoryListDto>();
            var worksPNode = GetCategories(NavigationType.MainNavigation, new List<string> { id }).AppendData as List<CategoryListDto>;
            foreach (var item in worksPNode)
            {
                foreach (var child in item.ChildCategory)
                {
                    CategoryListDto entity = new CategoryListDto()
                    {
                        Id = child.Id,
                        Name = child.Name
                    };
                    workList.Add(entity);
                }
            }
            or.AppendData = workList;
            return or;
        }

        public virtual int AddCategory(string name, string pid, NavigationType type, bool isThird = false)
        {

            Category parent = null;
            if (!string.IsNullOrEmpty(pid))
            {
                parent = CategoryRepository.Find(pid);
            }
            var maxOrdModel = CategoryRepository.GetModel().OrderByDescending(p => p.OrderNum).FirstOrDefault();
            var max = maxOrdModel?.OrderNum + 1 ?? 1;
            var model = new Category()
            {
                Name = name,
                ParentId = string.IsNullOrEmpty(pid) ? null : pid,
                CategoryType = type,
                Level = parent == null ? 0 : parent.Level + 1,
                OrderNum = max,
                Enabled = true,
                IsDeleted = false,
            };
            if (isThird)
            {
                AddChildCategory(model.Id);
            }
            return CategoryRepository.Insert(model);
        }

        public virtual string AddCategoryAndReturnId(string name, string pid, NavigationType type)
        {
            Category parent = null;
            if (!string.IsNullOrEmpty(pid))
            {
                parent = CategoryRepository.Find(pid);
            }
            var maxOrdModel = CategoryRepository.GetModel().OrderByDescending(p => p.OrderNum).FirstOrDefault();
            var max = maxOrdModel?.OrderNum + 1 ?? 1;
            var model = new Category()
            {
                Name = name,
                ParentId = string.IsNullOrEmpty(pid) ? null : pid,
                CategoryType = type,
                Level = parent == null ? 0 : parent.Level + 1,
                OrderNum = max,
                Enabled = true,
                IsDeleted = false,
            };
            model.Url = GlobalConstants.FragmentUrl + $"?pid={model.ParentId}&infoid={model.Id}";
            CategoryRepository.Insert(model);
            return model.Id;
        }

        public int AddChildCategory(string pid)
        {
            var nameList = new List<string>() { "about", "film trailer", "film stills", "installation", "exhibitions", "related texts", "publications" };
            int orderNum = 6;
            foreach (var name in nameList)
            {
                var model = new Category()
                {
                    Name = name,
                    ParentId = string.IsNullOrEmpty(pid) ? null : pid,
                    CategoryType = NavigationType.ArticleNavigation,
                    Level = 0,
                    OrderNum = orderNum,
                    Enabled = true,
                    IsDeleted = false,
                };
                this.CategoryRepository.Insert(model);
                orderNum--;
            }
            return 1;
        }

        public virtual int EditCategory(string name, string id)
        {
            var editCount = 0;
            var model = this.CategoryRepository.Find(id);
            model.Url= GlobalConstants.ProaboutUrl + $"?pid={model.ParentId}&infoid={model.Id}";
            if (model != null)
            {
                model.Name = name;
                editCount = this.CategoryRepository.Update(model);
            }
            return editCount;
        }

        public virtual int SortCategory(string pid, int num)
        {
            var model = this.CategoryRepository.Find(pid);
            var changeNum = 0;
            Category changeModel = null;
            if (num > 0)
            {
                changeModel = this.CategoryRepository.GetModel(p => p.OrderNum > model.OrderNum && p.ParentId == model.ParentId && p.IsDeleted == false && p.Enabled == true)
                  .OrderBy(p => p.OrderNum)
                  .FirstOrDefault();
                changeNum = changeModel?.OrderNum ?? model.OrderNum + 1;
            }
            else
            {
                changeModel = this.CategoryRepository.GetModel(p => p.OrderNum < model.OrderNum && p.ParentId == model.ParentId && p.IsDeleted == false && p.Enabled == true)
                    .OrderByDescending(p => p.OrderNum)
                    .FirstOrDefault();
                changeNum = changeModel?.OrderNum ?? model.OrderNum - 1;
            }

            if (changeModel != null)
            {
                changeModel.OrderNum = model.OrderNum;
                CategoryRepository.Update(changeModel);
            }
            model.OrderNum = changeNum;
            return CategoryRepository.Update(model);
        }

        public virtual int DeleteCategory(string id, bool isDelChild = true)
        {
            var model = CategoryRepository.GetModel().FirstOrDefault(p => p.Id == id &&
                                                        p.IsDeleted == false && p.Enabled == true);

            if (isDelChild)
            {
                DeleteChild(model.Id);
            }
            model.DeletedTime = DateTime.Now;
            model.IsDeleted = true;
            CategoryRepository.Update(model);
            return 0;
        }

        private void DeleteChild(string pid)
        {
            var items = CategoryRepository.GetModel().Where(p => p.ParentId == pid).ToList();
            foreach (var category in items)
            {
                DeleteChild(category.Id);
                category.DeletedTime = DateTime.Now;
                category.IsDeleted = true;
                this.CategoryRepository.Update(category);
            }
        }

        public virtual int ChangeCategoryEnabled(string id)
        {
            var model = CategoryRepository.Find(id);
            var enabled = !(model.Enabled.HasValue ? model.Enabled : true);
            model.Enabled = enabled;
            return this.CategoryRepository.Update(model);
        }

        //public virtual int DeleteCategory(string id)
        //{
        //    var model =
        //}
    }
}
