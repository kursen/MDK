using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.SKA.Models.ViewModels;

namespace SKA.Areas.SKA.Controllers
{
    public class MonthlyReportsSetupController : BaseController
    {
        //
        // GET: /SKA/MonthlyReportsSetup/
        private SKAEntities entities;

        public MonthlyReportsSetupController()
        {
            entities = new SKAEntities();
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (TempData["SearchValue"] != null)
            {
                ViewBag.SearchValue = TempData["SearchValue"].ToString();
            }
            else
            {
                ViewBag.SearchValue = string.Empty;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string searchValue)
        {
            TempData["SearchValue"] = searchValue;
            return RedirectToAction("Index");
        }

        [GridAction]
        public ActionResult GetList(string searchValue)
        {
            var model = from c in entities.MonthlyReportSetups
                        select new MonthlyReportsSetupViewModel 
                        { 
                            Id = c.Id,
                            Name = c.Name,
                            CheckerName = c.CheckerName,
                            CheckerPosition = c.CheckerPosition,
                            MakerName = c.MakerName,
                            MakerPosition = c.MakerPosition
                        };

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Name.Contains(searchValue)
                    || a.CheckerName.Contains(searchValue)
                    || a.CheckerPosition.Contains(searchValue)
                    || a.MakerName.Contains(searchValue)
                    || a.MakerPosition.Contains(searchValue));
            }
            return View(new GridModel<MonthlyReportsSetupViewModel>(model));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = (from c in entities.MonthlyReportSetups
                        where c.Id == id
                        select new MonthlyReportsSetupViewModel
                        {
                            Id = c.Id,
                            Name = c.Name,
                            CheckerName = c.CheckerName,
                            CheckerPosition = c.CheckerPosition,
                            MakerName = c.MakerName,
                            MakerPosition = c.MakerPosition
                        }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MonthlyReportsSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateItem = (from c in entities.MonthlyReportSetups
                                      where c.Id == model.Id
                                      select c).FirstOrDefault();

                    if (updateItem == null)
                    {
                        return RedirectToAction("Index");
                    }

                    updateItem.Name = model.Name;
                    updateItem.CheckerName = model.CheckerName;
                    updateItem.CheckerPosition = model.CheckerPosition;
                    updateItem.MakerName = model.MakerName;
                    updateItem.MakerPosition = model.MakerPosition;

                    entities.SaveChanges();
                    saveHistory();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    Logger.Error("Error while executing MonthlyReportsSetupController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }
    }
}
