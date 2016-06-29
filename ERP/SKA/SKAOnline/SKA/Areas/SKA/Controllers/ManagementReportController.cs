﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;

namespace SKA.Areas.SKA.Controllers
{
    public class ManagementReportController : BaseController
    {
        //
        // GET: /SKA/ManagementReport/

        public ActionResult Index()
        {
            var branch = GetCurrentUserBranchId();
            ViewData["Branch"] = branch.Code;
            return View();
        }

        public ActionResult ShowReport(string reportName)
        {
            var branch = GetCurrentUserBranchId();
            ViewData["Branch"] = branch.Code;
            ViewData["ReportName"] = reportName;

            if (!branch.Code.Equals("00")) //cabang
            {
                ViewData["ParameterName"] = "Branch";
                ViewData["ParameterValue"] = branch.Code;
            }
            return View();
        }

    }
}
