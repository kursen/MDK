using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;

namespace SKA.Areas.SKA.Controllers
{
    public class AnotherListController : BaseController
    {
        //
        // GET: /SKA/AnotherList/

        public ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult ShowReport(string reportName)
        {
            ViewData["ReportName"] = reportName;
            return View();
        }
    }
}
