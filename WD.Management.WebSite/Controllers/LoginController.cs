using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WD.Management.WebSite.Models;

namespace WD.Management.WebSite.Controllers
{
    public class LoginController:BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Active(VM_LoginUser loginUser)
        {

            //if (loginUser.UserName == "OctOcean" && loginUser.PassWord == "smallzgogo")
            //{

            //    HttpContext.Session.Set("_octocean__user_", System.Text.Encoding.Default.GetBytes("121222"));
            //    return RedirectToPage("/Index");
            //}
            //else
            //{
            //    return View("Index");
            //}
            return View("Index");
        }

        public IActionResult Logout()
        {
            //HttpContext.Session.Remove("_octocean__user_");

            return Redirect("/Login/Index");
        }
    }
}
