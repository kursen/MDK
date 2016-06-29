using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;

namespace SKA.Areas.SKA.Controllers
{
    public class JournalController : BaseController
    {
        //
        // GET: /SKA/Journal/

        public ActionResult Index()
        {
            return View();
        }

    }
}
