using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WD.DataService;
using WD.DataService.Sub;
using WD.Management.WebSite.Models;

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

            var all = _base_ArticleCategoryService.GetAllArticleCategory().Select(a => new VM_ArticleCategory()
            {
                ArticleCategoryName = a.ArticleCategoryName,
                ArticleCategoryCode = a.ArticleCategoryCode,
                Id = a.Id
            });

            return View(all);
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {

            var _Base_ArticleCategory_Entity = _base_ArticleCategoryService.GetArticleCategory(id);

            VM_ArticleCategory entity = new VM_ArticleCategory()
            {
                Id = _Base_ArticleCategory_Entity == null ? 0 : _Base_ArticleCategory_Entity.Id,
                ArticleCategoryCode = _Base_ArticleCategory_Entity == null ? "" : _Base_ArticleCategory_Entity.ArticleCategoryCode,
                ArticleCategoryName = _Base_ArticleCategory_Entity == null ? "" : _Base_ArticleCategory_Entity.ArticleCategoryName
            };
            return View(entity);
        }

        [HttpPost]
        public IActionResult Edit(VM_ArticleCategory entity)
        {
            if (ModelState.IsValid)
            {

                var _Base_ArticleCategory_Entity = new Entity.Sub.Base_ArticleCategory_Entity()
                {
                    Id = entity.Id,
                    ArticleCategoryCode = entity.ArticleCategoryCode,
                    ArticleCategoryName = entity.ArticleCategoryName
                };

                if (entity.Id > 0)
                {
                    _base_ArticleCategoryService.UpdateArticleCategory(_Base_ArticleCategory_Entity);

                }
                else
                {

                    //创建
                    entity.Id = _base_ArticleCategoryService.InsertArticleCategory(_Base_ArticleCategory_Entity);

                }
            }

            return RedirectToAction("Index");

        }


        public IActionResult Delete(int Id)
        {
            _base_ArticleCategoryService.DeleteArticleCategory(Id);
            return RedirectToAction("Index");
        }
    }
}