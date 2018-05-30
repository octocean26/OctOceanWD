using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WD.Management.WebSite.Models
{
    public class VM_ArticleManagement
    {
        public SelectList Base_ArticleCategoryddl { get; set; }
        public SelectList Base_ArticleTagddl { get; set; }

       
    }
}
