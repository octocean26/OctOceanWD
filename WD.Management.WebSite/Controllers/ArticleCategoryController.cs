using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WD.DataService;
using WD.DataService.Sub;

namespace WD.Management.WebSite.Controllers
{
    public class ArticleCategoryController : BaseController
    {
        private readonly Base_ArticleCategory_DataService _base_ArticleCategoryService = null;
        public ArticleCategoryController(PubComService pubComService)
        {
            _base_ArticleCategoryService = pubComService._Base_ArticleCategoryService;

        }

        public IActionResult Index()
        {

            var all= _base_ArticleCategoryService.GetAllArticleCategory();

            return View(all);
        }


        [HttpGet]
        public IActionResult Edit(int ArticleCategoryId)
        {
            return View();
        }
    }
}