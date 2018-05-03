using System;

namespace WD.Entity.Sub
{
    public class Base_ArticleCategory_Entity
    {
        public int Id { get; set; }
        //[Required,StringLength(10)]
        /// <summary>
        /// 文章类别名称
        /// </summary>
        public string ArticleCategoryName { get; set; }
        //[Required, StringLength(10)]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")] //限制用户可以输入的字符，吃住只能为字母
        /// <summary>
        /// 文章类别名称对应的Code
        /// </summary>
        public string ArticleCategoryCode { get; set; }

    }
}
