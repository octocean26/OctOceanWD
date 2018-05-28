using System;

namespace WD.Entity.Sub
{
    public class Base_ArticleCategory_Entity
    {
        public int Id { get; set; }
        
        /// <summary>
        /// 文章类别名称
        /// </summary>
        public string ArticleCategoryName { get; set; }
        
        /// <summary>
        /// 文章类别名称对应的Code
        /// </summary>
        public string ArticleCategoryCode { get; set; }

    }
}
