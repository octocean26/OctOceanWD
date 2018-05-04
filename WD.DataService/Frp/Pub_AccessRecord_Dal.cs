using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using WD.Entity.Frp;

namespace WD.DataService.Frp
{
    public class Pub_AccessRecord_Dal
    {
        IDbConnection connection = null;
        public Pub_AccessRecord_Dal()
        {
            this.connection = new SqlConnection(Utils.OctOceanGlobal.Config.DefaultConnectionString);
        }
        public void InsertAccessRecord(Pub_AccessRecord_Entity arEntity, Pub_ArticleBrowseLog_Entity ablEntity)
        {
            if (arEntity != null)
            {
                string sql = "INSERT INTO Pub_AccessRecord ( PageTag, SessionID, IP, AccessUrl, CreateTime )VALUES  ( @PageTag, @SessionID, @IP, @AccessUrl, GETDATE()   )";
                connection.Execute(sql, new { arEntity.PageTag, arEntity.SessionID, arEntity.IP, arEntity.AccessUrl });
            }
            if (ablEntity != null)
            {
                string sql2 = "INSERT INTO Pub_ArticleBrowseLog(ArticleKey, IP, SessionID, AccessUrl, CreateTime) VALUES(@ArticleKey, @IP, @SessionID, @AccessUrl, GETDATE())";
                connection.Execute(sql2, new { ablEntity.ArticleKey, ablEntity.IP, ablEntity.SessionID, ablEntity.AccessUrl });
            }
        }
    }
}
