using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using OctOcean.Com.Helper;
using WD.Entity.Aid;
using WD.Entity.Sub;

namespace WD.DataService.Sub
{
    public class Pri_ArticleDraft_Dal
    {

        IDbConnection connection = null;
        public Pri_ArticleDraft_Dal()
        {
            this.connection = new SqlConnection(Utils.OctOceanGlobal.Config.DefaultConnectionString);
        }

        public int InsertPri_ArticleDraft(Pri_ArticleDraft_Entity entity)
        {
            string sql = "INSERT INTO Pri_ArticleDraft(ArticleKey,ArticleTitle,ArticleCategory,ContentText,ArticleTag,ArticleDesc,AidStyle,UpdateTime ) VALUES(@ArticleKey,@ArticleTitle,@ArticleCategory,@ContentText,@ArticleTag,@ArticleDesc,@AidStyle,GETDATE())";
            return connection.Execute(sql, new { entity.ArticleKey, entity.ArticleTitle, entity.ArticleCategory, entity.ContentText, entity.ArticleTag, entity.ArticleDesc, entity.AidStyle });

        }

         
        public int DeletePri_ArticleDraft(int Id)
        {
            return connection.Execute("DELETE FROM Pri_ArticleDraft WHERE Id=@Id", new { Id = Id });
        }

        public int DeletePri_ArticleDraft(string ArticleKey)
        {
            return connection.Execute("DELETE FROM Pri_ArticleDraft WHERE ArticleKey=@ArticleKey", new { ArticleKey = ArticleKey });
        }
 

        /// <summary>
        /// 删除或者清空数据
        /// </summary>
        /// <param name="ArticleKey"></param>
        /// <returns></returns>
        public int DeleteAndClearTemp(string ArticleKey)
        {
            string sql = @"
DELETE FROM Pri_ArticleDraft WHERE ArticleKey=@ArticleKey;
DELETE FROM Pri_ArticleDraft_Temp WHERE ArticleKey=@ArticleKey;
DELETE FROM Pub_Article WHERE ArticleKey=@ArticleKey;
DELETE FROM Pri_ArticleImage WHERE ArticleKey=@ArticleKey;
";
            return connection.Execute(sql, new { ArticleKey = ArticleKey });
        }



        public int UpdatePri_ArticleDraft(Pri_ArticleDraft_Entity entity)
        {
            string sql = "UPDATE Pri_ArticleDraft SET ArticleTitle=@ArticleTitle, ArticleCategory=@ArticleCategory,ContentText=@ContentText,ArticleTag=@ArticleTag,ArticleDesc=@ArticleDesc,AidStyle=@AidStyle, UpdateTime=@UpdateTime WHERE ArticleKey=@ArticleKey;";
            return connection.Execute(sql, new { entity.ArticleTitle, entity.ArticleCategory, entity.ContentText, entity.ArticleTag, entity.ArticleDesc, entity.AidStyle,   entity.UpdateTime, entity.ArticleKey });
        }

        public int UpdatePri_ArticleDraftContentText(string ArticleKey, string ContentText)
        {
            string sql = "UPDATE Pri_ArticleDraft SET ContentText=@ContentText,UpdateTime=GETDATE() WHERE ArticleKey=@ArticleKey; ";
            return connection.Execute(sql, new { ContentText, ArticleKey });
        }



        public IList<Pri_ArticleDraft_Entity> GetAllPri_ArticleDraft(string where="", object parObj=null)
        {
            string sql = "select  Id , ArticleKey,ArticleTitle,ArticleCategory,ContentText,ArticleTag,ArticleDesc,AidStyle,UpdateTime from Pri_ArticleDraft ";
            if (!string.IsNullOrWhiteSpace(where))
            {
                sql += where;
            }
             
            var query = connection.Query<Pri_ArticleDraft_Entity>(sql, parObj).AsList();
            return query;
        }
 


        public Pri_ArticleDraft_Entity GetPri_ArticleDraft(int Id)
        {
            string sql = "select  Id , ArticleKey,ArticleTitle,ArticleCategory,ContentText,ArticleTag,ArticleDesc,AidStyle,UpdateTime from Pri_ArticleDraft where Id=@Id";

            var query = connection.Query<Pri_ArticleDraft_Entity>(sql, new { Id = Id }).AsList();
            if (query != null && query.Count > 0)
                return query[0];
            return null;
        }


        public Pri_ArticleDraft_Entity GetPri_ArticleDraft(string ArticleKey)
        {
            string sql = "select top 1  Id , ArticleKey,ArticleTitle,ArticleCategory,ContentText,ArticleTag,ArticleDesc,AidStyle,UpdateTime from Pri_ArticleDraft where  ArticleKey=@ArticleKey";

            var query = connection.Query<Pri_ArticleDraft_Entity>(sql, new { ArticleKey = ArticleKey }).AsList();
            if (query != null && query.Count > 0)
                return query[0];
            return null;
        }


        public IList<Aux_ArticleDraftPager_Entity> GetPri_ArticleDraftPagerList(string where, int PageIndex, int PageSize, object whereObjPar, string OrderColumn, string OrderType, out int SumCount)
        {
            int start = (PageIndex - 1) * PageSize + 1;
            int end = PageIndex * PageSize;
            string snorderby = OrderColumn;
            string rsorderby = OrderColumn;
            if ("UpdateTime".Equals(OrderColumn, StringComparison.InvariantCultureIgnoreCase))
            {
                snorderby = "d.UpdateTime";
                rsorderby = "d.UpdateTime";
            }
            else if ("ArticleCategoryName".Equals(OrderColumn, StringComparison.InvariantCultureIgnoreCase))
            {
                snorderby = "c.ArticleCategoryName";
                rsorderby = "wt.ArticleCategoryName";
            }
            else if ("ArticleTitle".Equals(OrderColumn, StringComparison.InvariantCultureIgnoreCase))
            {
                snorderby = "d.ArticleTitle";
                rsorderby = "d.ArticleTitle";
            }
            string sqlcount = string.Format(@"
 SELECT count(1) FROM Pri_ArticleDraft d LEFT JOIN Base_ArticleCategory c ON d.ArticleCategory = c.ArticleCategoryCode
 WHERE {0};", where);
            SumCount = ConvertHelper.ToInt32(connection.ExecuteScalar(sqlcount, whereObjPar));

            string sql = string.Format(@"
with wt as 
(
    select ROW_NUMBER() OVER(ORDER BY {3} {5}) AS SNumber, d.Id, d.ArticleKey, c.ArticleCategoryName
    FROM Pri_ArticleDraft d LEFT JOIN Base_ArticleCategory c ON d.ArticleCategory = c.ArticleCategoryCode
    WHERE {0}
)
select wt.SNumber,wt.ArticleKey,d.ArticleTitle,wt.ArticleCategoryName,d.ArticleTag,d.UpdateTime,u.ArticleKey as PubArticleKey,u.UpdateTime as PubUpdateTime
from wt left join Pri_ArticleDraft d on wt.ArticleKey = d.ArticleKey
LEFT JOIN Pub_Article AS u ON u.ArticleKey=wt.ArticleKey
where wt.SNumber BETWEEN {1} AND {2} order by {4} {5}; ", where, start, end, snorderby, rsorderby, OrderType);

            var query = connection.Query<Aux_ArticleDraftPager_Entity>(sql, whereObjPar).AsList();
            return query;

        }

    }
}
