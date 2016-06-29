using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Filters;

namespace SKA.Areas.Setting.Controllers
{
    [BranchCodeFilter]
    public class ListAccountController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
