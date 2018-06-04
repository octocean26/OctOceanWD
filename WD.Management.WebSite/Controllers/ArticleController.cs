using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WD.DataService;
using WD.DataService.Sub;
using WD.Entity.Sub;
using WD.Management.WebSite.Models;

namespace WD.Management.WebSite.Controllers
{
    
    public class ArticleController : BaseController
    {
        readonly PubComService _PubComService = null;
        
        public ArticleController(PubComService pubComService)
        {
            _PubComService = pubComService;
        }


        public IActionResult Index()
        {
            VM_ArticleManagement vm = new VM_ArticleManagement();

            vm.Base_ArticleCategoryddl = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _PubComService._Base_ArticleCategoryService.GetAllArticleCategory(), "ArticleCategoryCode", "ArticleCategoryName", "");//默认选择空值

            var tagdal = _PubComService._Base_ArticleTag_DataService;
            var tagsource = tagdal.GetAllArticleTag();
            vm.Base_ArticleTagddl = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tagsource, "ArticleTagCode", "ArticleTagName", "");
            var obj = tagsource.ToDictionary(a => a.ArticleTagCode, b => b.ArticleTagName);
            ViewData["TagJson"] = JsonConvert.SerializeObject(obj);

            ViewData["ArticlePreviewUrl"] = _PubComService._OctOceanConfig.ArticlePreviewUrl;

            return View(vm);
        }

        [Route("Article/Edit/{ArticleKey}")]
        public IActionResult Edit(string ArticleKey)
        {
            VM_Article article = new VM_Article()
            {
                ArticlePreviewUrl = _PubComService._OctOceanConfig.ArticlePreviewUrl + "/" + ArticleKey + "?t=p"
            };

            if (string.IsNullOrEmpty(ArticleKey))
            {
                article.ArticleGuidKey = "A_" + Guid.NewGuid().ToString().Replace("-", "");
                article.IsPublish = false;
                article.CanUploadOrPublish = false;
            }
            else
            {
                var entity = _PubComService._Pri_ArticleDraft_DataService.GetPri_ArticleDraft(ArticleKey);
                if (entity == null)
                {
                    return RedirectToAction("Index");
                }
                article.ArticleGuidKey = ArticleKey;
                article.ArticleTitle = entity.ArticleTitle;
                article.ArticleCategory = entity.ArticleCategory;
                article.ContentText = entity.ContentText;
                article.ArticleTag = entity.ArticleTag;
                article.ArticleDesc = entity.ArticleDesc;
                article.AidStyle = entity.AidStyle;
                article.IsPublish = _PubComService._Pub_Article_DataService.GetPub_Article_Entity(ArticleKey) != null;
                article.CanUploadOrPublish = entity != null;
            }

            article.Base_ArticleCategoryddl = new SelectList(_PubComService._Base_ArticleCategoryService.GetAllArticleCategory(), "ArticleCategoryCode", "ArticleCategoryName", ""); //默认选择空值
            article.Base_ArticleTagList = _PubComService._Base_ArticleTag_DataService.GetAllArticleTag();
            var allimagelist = _PubComService._Pri_ArticleImage_DataService.GetAllPri_ArticleImage();
            if (allimagelist != null)
            {
                article.Pri_ArticleImageList = allimagelist.OrderBy(a => a.UpdateTime).ToList();
            }


            return View(article);
        }


        public IActionResult FormatCode()
        {
            VM_ArticleFormatCode _ArticleFormatCode = new VM_ArticleFormatCode();

            return View(_ArticleFormatCode);
        }

        [HttpPost]
        public IActionResult FormatCode(VM_ArticleFormatCode _ArticleFormatCode)
        {
            string ct = _ArticleFormatCode.ContentText;
            _ArticleFormatCode.ContentText = ct.Replace(_ArticleFormatCode.OldString, _ArticleFormatCode.NewString);
           return View(_ArticleFormatCode);
        }

        [Route("Article/HtmlEdit/{ArticleKey}")]
        public IActionResult HtmlEdit(string ArticleKey)
        {
            var entity=  _PubComService._Pri_ArticleDraft_DataService.GetPri_ArticleDraft(ArticleKey);
            VM_ArticleHtmlEdit articleHtmlEdit = new VM_ArticleHtmlEdit()
            {
                ArticleKey = entity == null ? "" : entity.ArticleKey,
                ContentText = entity.ContentText
            };
            return View(articleHtmlEdit);
        }
        [HttpPost]
        public IActionResult HtmlEdit(VM_ArticleHtmlEdit articleHtmlEdit)
        {
            _PubComService._Pri_ArticleDraft_DataService.UpdatePri_ArticleDraftContentText(articleHtmlEdit.ArticleKey, articleHtmlEdit.ContentText);
            return View(articleHtmlEdit);

        }

        /// <summary>
        /// 分页功能
        /// </summary>
        /// <param name="orderColumn"></param>
        /// <param name="orderType"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="ArticleCategoryCode"></param>
        /// <returns></returns>
        public object Pagination(string orderColumn = "UpdateTime", string orderType = "desc", int PageIndex = 1, int PageSize = 10, string ArticleCategoryCode = "", string ArticleTag = "")
        {
            if ("UpdateTimeF".Equals(orderColumn, StringComparison.InvariantCultureIgnoreCase)) orderColumn = "UpdateTime";
            int sumcount = 0;
            //int PageIndex = page;
            //int PageSize = limit;
            IList<Entity.Aid.Aux_ArticleDraftPager_Entity> data = null;
            string where = " 1=1 ";

            if (!string.IsNullOrWhiteSpace(ArticleCategoryCode))
            {
                where += " AND d.ArticleCategory=@ArticleCategory ";
            }
            if (!string.IsNullOrWhiteSpace(ArticleTag))
            {
                where += " AND ArticleTag LIKE '%:'+@ArticleTag+':%' ";
            }
            var obj = new
            {
                ArticleCategory = ArticleCategoryCode,
                ArticleTag = ArticleTag
            };

            data = _PubComService._Pri_ArticleDraft_DataService.GetPri_ArticleDraftPagerList(where, PageIndex, PageSize, obj, orderColumn, orderType, out sumcount);


            //if (string.IsNullOrEmpty(ArticleCategoryCode))
            //{

            //    data = draftdal.GetPri_ArticleDraftPagerList(where, PageIndex, PageSize, null, orderColumn, orderType, out sumcount);

            //}
            //else
            //{

            //    data = draftdal.GetPri_ArticleDraftPagerList(where, PageIndex, PageSize, new { ArticleCategory = ArticleCategoryCode }, orderColumn, orderType, out sumcount);
            //}
            return new { code = 0, msg = "", count = sumcount, data = data };

        }


        /// <summary>
        /// 删除文章，并彻底删除该文章所有的记录，否则更新draft和发布后的删除状态
        /// </summary>
        /// <param name="ArticleKey"></param>
        /// <returns></returns>
        public object Delete(string ArticleKey)
        {
            int _status = 0;
            string _msg = "";
            try
            {
                //如果没有执行过删除，就更新一下状态，否则就彻底删除
                _PubComService._Pri_ArticleDraft_DataService.DeleteAndClearTemp(ArticleKey);
                _status = 1;
            }
            catch (Exception ex)
            {
                _status = 4;
                _msg = ex.Message;

            }
            return new { status = _status, msg = _msg };


        }


        /// <summary>
        /// 对文章进行发布或者取消发布操作，注意：该方法只是对Pub_Article表进行了操作，能够执行发布的前提是存在draft数据并且不是draft数据没有删除
        /// </summary>
        /// <param name="ArticleKey"></param>
        /// <param name="IsPublish"></param>
        /// <returns></returns>
        public object Publish(string ArticleKey, bool IsPublish)
        {
            string _msg = string.Empty;
            int _status = 0;
            //先判断是否已经存在了Draft中去
            var draftentity = _PubComService._Pri_ArticleDraft_DataService.GetPri_ArticleDraft(ArticleKey);
            //发布按钮展示的时候，已经进行过限制，此处为了保险，使用服务端限制
            if (draftentity == null) //如果没有数据或者数据已经删除，需要重新保存
            {
                _status = 2;
                _msg = "请先保存数据";

            }
            else
            {
                try
                {
                    _PubComService._Pub_Article_DataService.InsertPub_ArticleWithPri_ArticleDraft(ArticleKey, IsPublish);
                    _status = 1;
                }
                catch (Exception ex)
                {
                    _status = 4;
                    _msg = ex.Message;
                }

            }
            return new { status = _status, msg = _msg };

        }


        /// <summary>
        /// 定时自动保存文章到Draft和Temp中去
        /// </summary>
        /// <param name="ArticleKey"></param>
        /// <param name="ArticleTitle"></param>
        /// <param name="ArticleCategory"></param>
        /// <param name="ContentText"></param>
        /// <param name="ArticleTag"></param>
        /// <param name="ArticleDesc"></param>
        /// <param name="AidStyle"></param>
        /// <returns></returns>
        public object Save(string ArticleKey, string ArticleTitle, string ArticleCategory, string ContentText, string ArticleTag, string ArticleDesc, string AidStyle)
        {
            int _status = 0;
            string _msg = string.Empty;
            int savecount = 0;
            //只用判断标题、样式、和内容
            if (ContentText != null)
            {
                var _cont = ContentText.Replace("<p>", "").Replace("<br>", "").Replace("</p>", "").Replace("</br>", ""); //去掉自带的样式
                if (string.IsNullOrWhiteSpace(_cont))
                {
                    ContentText = "";
                }

            }


            if (string.IsNullOrWhiteSpace(ArticleTitle) && string.IsNullOrWhiteSpace(ContentText) && string.IsNullOrWhiteSpace(ArticleDesc) && string.IsNullOrWhiteSpace(AidStyle))
            {
                _status = 3;
                _msg = "未提交数据";
            }
            else
            {
                ContentText = _PubComService._Pri_ArticleImage_DataService.ReplaceImagesPlaceholder(ContentText, ArticleKey);

                //将数据存入到Temp中去
                var tempdal = _PubComService._Pri_ArticleDraft_Temp_DataService;
                var draftdal = _PubComService._Pri_ArticleDraft_DataService;

                try
                {
                    int oi = tempdal.InsertPri_ArticleDraft_Temp(new Pri_ArticleDraft_Temp_Entity()
                    {
                        ArticleKey = ArticleKey,
                        ArticleTag = ArticleTag ?? "",
                        ArticleDesc = ArticleDesc ?? "",
                        AidStyle = AidStyle ?? "",
                        ArticleCategory = ArticleCategory ?? "",
                        ArticleTitle = ArticleTitle ?? "",
                        ContentText = ContentText ?? "",
                        UpdateTime = DateTime.Now

                    });
                    //如果有改动的话，就进行同步修改，注意：仍然要保存旧的删除的状态
                    if (oi > 0)
                    {
                        //查询一下之前旧的状态，是否删除
                        //删除的状态只能在执行删除或者还原的时候进行改变，此处仍然取旧的状态
                        var olddraft = draftdal.GetPri_ArticleDraft(ArticleKey);
                        //只有有旧的记录才更新
                        if (olddraft != null)
                        {
                            //必须在Draft中有这条记录修改才能生效，也就是说，只有点了保存按钮，产生了数据，才能够和temp数据进行关联，这样的话可以减少草稿内容（比如测试、或者打开页面没有做任何事情）的产生。
                            draftdal.UpdatePri_ArticleDraft(new Pri_ArticleDraft_Entity()
                            {
                                ArticleKey = ArticleKey,
                                ArticleTag = ArticleTag ?? "",
                                ArticleDesc = ArticleDesc ?? "",
                                AidStyle = AidStyle ?? "",
                                ArticleCategory = ArticleCategory ?? "",
                                ArticleTitle = ArticleTitle ?? "",
                                ContentText = ContentText ?? "",
                                UpdateTime = DateTime.Now
                            });
                        }
                    }

                    //查询历史总次数
                    savecount = tempdal.GetSaveTempCountByArticleKey(ArticleKey);
                    _status = 1;
                }
                catch (Exception ex)
                {
                    _status = 4;
                    _msg = ex.Message;
                }

            }
            return new { status = _status, msg = _msg, ak = ArticleKey, sc = savecount };

        }





    }
}