using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Filters;

namespace SKA.Controllers
{
    public class HomeController : BaseController
    {
        [BranchCodeFilter]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            return View();
        }

        //[HttpPost]
        //public ActionResult Index(FormCollection form)
        //{

        //}

        public ActionResult About()
        {
            return View();
        }
    }
}
