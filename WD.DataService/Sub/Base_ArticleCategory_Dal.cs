using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using WD.Entity.Sub;

namespace WD.DataService.Sub
{
    public class Base_ArticleCategory_Dal
    {
        IDbConnection connection = null;
        public Base_ArticleCategory_Dal()
        {
            this.connection = new SqlConnection(Utils.OctOceanGlobal.Config.DefaultConnectionString);
        }

        public int InsertArticleCategory(Base_ArticleCategory_Entity entity)
        {
            string sql = "INSERT INTO Base_ArticleCategory(ArticleCategoryName, ArticleCategoryCode,UpdateTime ) VALUES(@ArticleCategoryName,@ArticleCategoryCode,GETDATE())";
            return connection.Execute(sql, new { ArticleCategoryName = entity.ArticleCategoryName, ArticleCategoryCode = entity.ArticleCategoryCode  });

        }

        /// <summary>
        /// 物理删除数据，真删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteArticleCategory(int Id)
        {
            return connection.Execute("DELETE FROM Base_ArticleCategory WHERE Id=@Id", new { Id = Id });
        }
 

        public int UpdateArticleCategory(Base_ArticleCategory_Entity entity)
        {
            string sql = "UPDATE Base_ArticleCategory SET ArticleCategoryCode=@ArticleCategoryCode, ArticleCategoryName=@ArticleCategoryName,  UpdateTime=GETDATE() WHERE Id=@Id;";
            return connection.Execute(sql, new { ArticleCategoryName = entity.ArticleCategoryName, ArticleCategoryCode = entity.ArticleCategoryCode,  Id = entity.Id });
        }

        public IList<Base_ArticleCategory_Entity> GetAllArticleCategory(string where="", object parObj=null)
        {
            string sql = "select  Id , ArticleCategoryName, ArticleCategoryCode, UpdateTime from Base_ArticleCategory   ";
            if (!string.IsNullOrWhiteSpace(where))
            {
                sql += where;
            }
            var query = connection.Query<Base_ArticleCategory_Entity>(sql, parObj).AsList();
            return query;
        }

      


        public Base_ArticleCategory_Entity GetArticleCategory(int Id)
        {
            string sql = "select  Id , ArticleCategoryName, ArticleCategoryCode,UpdateTime from Base_ArticleCategory where  Id=@Id";

            var query = connection.Query<Base_ArticleCategory_Entity>(sql, new { Id = Id }).AsList();
            if (query != null && query.Count > 0)
                return query[0];
            return null;
        }
    }
}
