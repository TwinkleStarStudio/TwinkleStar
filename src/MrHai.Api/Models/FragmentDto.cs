using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MrHai.Api.Models
{
    public class FragmentDto
    {
        public int Num { get; set; }
        public string CategoryId { get; set; }
        public string Url { get; set; }
        public string Summary { get; set; }
    }
}