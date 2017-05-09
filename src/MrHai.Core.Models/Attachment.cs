using MrHai.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinkleStar.Data;

namespace MrHai.Core.Models
{
    [Description("附件表")]
    public  class Attachment : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }//扩展名
        public long Size { set; get; }
        public int? UserID { get; set; }
        public FileEnum FileType{ set; get; }
        public DateTime CreateTime { get; set; }
    }
}
