using System;

namespace WD.Management.WebSite.Models
{
    //ע�Ͷ���
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}