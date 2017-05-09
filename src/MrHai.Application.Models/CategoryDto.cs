using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrHai.Application.Models
{
    public class CategoryListDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 上一个节点的Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 父节点名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary> 
        public NavigationType CategoryType { get; set; }

        /// <summary>
        /// 层级 0-开始
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int OrderNum { get; set; }
        public bool? Enabled { get; set; }
        /// <summary>
        /// 是否拥有作品导航
        /// </summary>
        public bool HasOtherNavigation { get; set; }

        public DateTime CreateTime { set; get; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public IEnumerable<CategoryListDto> ChildCategory { get; set; }
    }
}
