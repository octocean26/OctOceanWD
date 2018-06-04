using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WD.Entity.Sub;

namespace WD.Management.WebSite.Models
{
    public class VM_Article
    {
        public string ArticleKey { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleCategory { get; set; }
        public string ContentText { get; set; }
        public string ArticleTag { get; set; }
        public string ArticleDesc { get; set; }
        public string AidStyle { get; set; }
        public bool IsPublish { get; set; }
        public bool CanUploadOrPublish { get; set; }

        public SelectList Base_ArticleCategoryddl { get; set; }

        public IList<Base_ArticleTag_Entity> Base_ArticleTagList { get; set; }

        public IList<Pri_ArticleImage_Entity> Pri_ArticleImageList { get; set; }

        public string ArticlePreviewUrl { get; set; }
    }
}
