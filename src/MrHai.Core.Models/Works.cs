using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TwinkleStar.Common.ObjectId;
using TwinkleStar.Data;

namespace MrHai.Core.Models
{
    /// <summary>
    /// 作品
    /// </summary>
    [Description("作品")]
    public partial class Works
        : EntityBase
    {
        [Key]
        public string Id { get; set; } = ObjectId.NewObjectId();

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeletedTime { get; set; }
    }
}
