using System;

namespace WD.Management.WebSite.Models
{
    //×¢ÊÍ¶øÒÑ
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}