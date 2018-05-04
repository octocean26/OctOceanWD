using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using WD.Entity.Sub;

namespace WD.DataService.Sub
{
    public class Base_ArticleTag_Dal
    {
        IDbConnection connection = null;
        public Base_ArticleTag_Dal()
        {
            this.connection = new SqlConnection(Utils.OctOceanGlobal.Config.DefaultConnectionString);
        }

        public int InsertArticleTag(Base_ArticleTag_Entity entity)
        {
            string sql = "INSERT INTO Base_ArticleTag(ArticleTagName, ArticleTagCode ) VALUES(@ArticleTagName,@ArticleTagCode)";
            return connection.Execute(sql, new { ArticleTagName = entity.ArticleTagName, ArticleTagCode = entity.ArticleTagCode });

        }

        /// <summary>
        /// 物理删除数据，真删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeleteArticleTag(int Id)
        {
            return connection.Execute("DELETE FROM Base_ArticleTag WHERE Id=@Id", new { Id = Id });
        }
 

        public int UpdateArticleTag(Base_ArticleTag_Entity entity)
        {
            string sql = "UPDATE Base_ArticleTag SET ArticleTagCode=@ArticleTagCode, ArticleTagName=@ArticleTagName WHERE Id=@Id;";
            return connection.Execute(sql, new { ArticleTagName = entity.ArticleTagName, ArticleTagCode = entity.ArticleTagCode,  Id = entity.Id });
        }

        public IList<Base_ArticleTag_Entity> GetAllArticleTag(string where="", object parobj=null)
        {
            string sql = "select  Id , ArticleTagName, ArticleTagCode  from Base_ArticleTag   ";
            if (!string.IsNullOrWhiteSpace(where))
            {
                sql += where;
            }
            var query = connection.Query<Base_ArticleTag_Entity>(sql, parobj).AsList();
            return query;
 
        }


        public Base_ArticleTag_Entity GetArticleTag(int Id)
        {
            string sql = "select  Id , ArticleTagName, ArticleTagCode from Base_ArticleTag where Id=@Id";

            var query = connection.Query<Base_ArticleTag_Entity>(sql, new { Id = Id }).AsList();
            if (query != null && query.Count > 0)
                return query[0];
            return null;
        }
    }
}
