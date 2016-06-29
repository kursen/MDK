using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;

namespace SKA.Areas.PettyCashes.Controllers
{
    public class FormController : BaseController
    {
        //
        // GET: /PettyCashes/Form/

        public ActionResult Index()
        {
            var branch = GetCurrentUserBranchId();
            ViewData["Branch"] = branch.Code;
            return View();
        }

        public virtual ActionResult ShowReport(string reportName)
        {
            var branch = GetCurrentUserBranchId();
            ViewData["Branch"] = branch.Code;
            ViewData["ReportName"] = reportName;

            if (!branch.Code.Equals("00")) //cabang
            {
                ViewData["ParameterName"] = "KodeCabang";
                ViewData["ParameterValue"] = branch.Code;
            }
            return View();
        }
    }
}
