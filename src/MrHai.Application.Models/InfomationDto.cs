using MrHai.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MrHai.Application.Models
{
    public class InfomationDto
    {
        public string Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 资讯类型
        /// </summary>
        public InfomationType InfoType { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 所属菜单
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 所属作品
        /// </summary>
        public string WorksId { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumb { get; set; }

        /// <summary>
        /// 前端跳转链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int? CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeletedTime { get; set; }

        public string WorkName { set; get; }

        public string CategoryName { set; get; }


    }
}
