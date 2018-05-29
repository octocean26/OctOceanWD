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
    public class ArticleTagController : BaseController
    {

        readonly Base_ArticleTag_DataService _base_ArticleTag_DataService = null;
        public ArticleTagController(PubComService pubComService)
        {
            _base_ArticleTag_DataService = pubComService._Base_ArticleTag_DataService;
        }


        public IActionResult Index()
        {
            var all = _base_ArticleTag_DataService.GetAllArticleTag().Select(a => new VM_ArticleTag()
            {
                ArticleTagCode = a.ArticleTagCode,
                ArticleTagName = a.ArticleTagName,
                Id = a.Id
            });
            return View(all);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {

            var _Base_ArticleTag_Entity = _base_ArticleTag_DataService.GetArticleTag(id);

            VM_ArticleTag entity = new VM_ArticleTag()
            {
                Id = _Base_ArticleTag_Entity == null ? 0 : _Base_ArticleTag_Entity.Id,
                ArticleTagCode = _Base_ArticleTag_Entity == null ? "" : _Base_ArticleTag_Entity.ArticleTagCode,
                ArticleTagName = _Base_ArticleTag_Entity == null ? "" : _Base_ArticleTag_Entity.ArticleTagName
            };
            return View(entity);
        }

        [HttpPost]
        public IActionResult Edit(VM_ArticleTag entity)
        {
            if (ModelState.IsValid)
            {

                var _Base_ArticleTag_Entity = new Entity.Sub.Base_ArticleTag_Entity()
                {
                    Id = entity.Id,
                    ArticleTagCode = entity.ArticleTagCode,
                    ArticleTagName = entity.ArticleTagName
                };

                if (entity.Id > 0)
                {
                    _base_ArticleTag_DataService.UpdateArticleTag(_Base_ArticleTag_Entity);

                }
                else
                {

                    //创建
                    entity.Id = _base_ArticleTag_DataService.InsertArticleTag(_Base_ArticleTag_Entity);

                }
            }

            return RedirectToAction("Index");

        }


        public IActionResult Delete(int Id)
        {
            _base_ArticleTag_DataService.DeleteArticleTag(Id);
            return RedirectToAction("Index");
        }
    }
}