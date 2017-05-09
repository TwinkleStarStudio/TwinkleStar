using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TwinkleStar.Common.ObjectId;
using TwinkleStar.Data;

namespace MrHai.Core.Models
{

    /// <summary>
    /// 全局配置
    /// </summary>
    [Description("全局配置")]
    public partial class GlobalConfig
        : EntityBase
    {
        [Key]
        public string Id { get; set; } = ObjectId.NewObjectId();

        [Required]
        public string ConfigType { get; set; }

        [Required]
        public string Config { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? DeletedTime { get; set; }

        public bool? Enabled { get; set; }
    }
}
