using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TwinkleStar.Common.ObjectId;
using TwinkleStar.Data;

namespace MrHai.Core.Models
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Description("菜单")]
    public partial class Category
        : EntityBase
    {
        [Key]
        public string Id { get; set; } =ObjectId.NewObjectId();

        /// <summary>
        /// 上一个节点的Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        [Required]
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

        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 前端跳转链接
        /// </summary>
        public string Url { get; set; }

        public DateTime? DeletedTime { get; set; }

        public bool? Enabled { get; set; }

        public int OrderNum { get; set; }
    }
}
