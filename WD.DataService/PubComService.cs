using System;
using System.Collections.Generic;
using System.Text;
using WD.DataService.Sub;
using WD.Utils;

namespace WD.DataService
{
    public class PubComService
    {


        public readonly OctOceanConfig _OctOceanConfig = null;
        public PubComService(OctOceanConfig octOceanConfig)
        {
            _OctOceanConfig = octOceanConfig;
        }

        public Base_ArticleCategory_DataService _Base_ArticleCategoryService
        {
            get { return new Base_ArticleCategory_DataService(_OctOceanConfig); }
        }

        public Pri_ArticleDraft_DataService _Pri_ArticleDraft_DataService
        {
            get { return new Pri_ArticleDraft_DataService(_OctOceanConfig); }
        }


        public Pri_ArticleImage_DataService _Pri_ArticleImage_DataService
        {
            get { return new Pri_ArticleImage_DataService(_OctOceanConfig); }
        }

        public Pri_ArticleDraft_Temp_DataService _Pri_ArticleDraft_Temp_DataService
        {
            get { return new Pri_ArticleDraft_Temp_DataService(_OctOceanConfig); }
        }

        public Pub_Article_DataService _Pub_Article_DataService
        {
            get { return new Pub_Article_DataService(_OctOceanConfig); }
        }

        public Base_ArticleTag_DataService _Base_ArticleTag_DataService
        {
            get { return new Base_ArticleTag_DataService(_OctOceanConfig); }
        }
    }
}
