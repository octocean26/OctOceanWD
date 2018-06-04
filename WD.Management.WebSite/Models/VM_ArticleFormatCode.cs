using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WD.Management.WebSite.Models
{
    public class VM_ArticleFormatCode
    {
        public string ContentText { get; set; }
        public string OldString { get; set; } = "<";
        public string NewString { get; set; } = "&lt;";
    }
}
