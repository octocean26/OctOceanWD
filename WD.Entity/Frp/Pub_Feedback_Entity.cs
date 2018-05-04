using System;
using System.Collections.Generic;
using System.Text;

namespace WD.Entity.Frp
{
    public class Pub_Feedback_Entity
    {
        public int Id { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string ProblemDesc { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
