using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;

namespace SKA.Areas.SKA.Controllers
{
    public class GeneralLedgerController : BaseController
    {
        //
        // GET: /SKA/GeneralLedger/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowReport(string reportName)
        {
            var branch = GetCurrentUserBranchId();
            ViewData["Branch"] = branch.Code;
            ViewData["reportname"] = reportName;
            if (!branch.Code.Equals("00"))
            {
                ViewData["ParameterName"] = "BranchCode";
                ViewData["ParameterValue"] = branch.Code;
            }
            return View();
        }

    }
}
