using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;

namespace SKA.Areas.VoucherKas.Controllers
{
    public class ReportVoucherController : BaseController
    {
        //
        // GET: /VoucherKas/ReportVoucher/

        public ActionResult Index()
        {
            var branch = GetCurrentUserBranchId();

            ViewBag.BranchCode = branch.Code;
            return View();
        }

        public ActionResult IndexReport()
        {
            var branch = GetCurrentUserBranchId();

            ViewBag.BranchCode = branch.Code;
            return View();
        }

        public virtual ActionResult ShowReport(string reportName)
        {
            var branch = GetCurrentUserBranchId();
            ViewData["Branch"] = branch.Code;
            ViewData["ReportName"] = reportName;

            if (!branch.Code.Equals("00"))
            {
                ViewData["ParameterName"] = "BranchCode";
                ViewData["ParameterValue"] = branch.Code;
            }
            return View();
        }

    }
}
