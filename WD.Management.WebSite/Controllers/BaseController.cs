using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WD.DataService;

namespace WD.Management.WebSite.Controllers
{
    public class BaseController: Controller
    {
        //protected readonly PubComService _PubComService = null;
        //public BaseController(PubComService pubComService)
        //{
        //    _PubComService = pubComService;
        //}

        public bool CheckLogin()
        {
            byte[] o = null;
            if (HttpContext.Session != null && HttpContext.Session.TryGetValue("_octocean__user_", out o))
            {
                if (o != null && System.Text.Encoding.Default.GetString(o) == "121222")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
