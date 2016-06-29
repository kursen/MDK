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
    public class AnnualReportsSetupController : BaseController
    {
        //
        // GET: /SKA/AnnualReportsSetup/
        private SKAEntities entities;

        public AnnualReportsSetupController()
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
            var model = from c in entities.AnnualReportSetups
                        select new AnnualReportsSetupViewModel 
                        { 
                            Id = c.Id,
                            Name = c.Name,
                            ApproverName = c.ApproverName,
                            ApproverPosition = c.ApproverPosition,
                            KnownByName = c.KnownByName,
                            KnownByPosition = c.KnownByPosition
                        };

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Name.Contains(searchValue)
                    || a.ApproverName.Contains(searchValue)
                    || a.ApproverPosition.Contains(searchValue)
                    || a.KnownByName.Contains(searchValue)
                    || a.KnownByPosition.Contains(searchValue));
            }
            return View(new GridModel<AnnualReportsSetupViewModel>(model));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            var model = (from c in entities.AnnualReportSetups
                         where c.Id == id
                         select new AnnualReportsSetupViewModel
                         {
                             Id = c.Id,
                            Name = c.Name,
                            ApproverName = c.ApproverName,
                            ApproverPosition = c.ApproverPosition,
                            KnownByName = c.KnownByName,
                            KnownByPosition = c.KnownByPosition
                         }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AnnualReportsSetupViewModel model)
        {
            if(ModelState.IsValid)
            {
                try 
                {
                    var updateItem = (from c in entities.AnnualReportSetups
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    if (updateItem == null)
                    {
                        return RedirectToAction("Index");
                    }

                    updateItem.Name = model.Name;
                    updateItem.ApproverName = model.ApproverName;
                    updateItem.ApproverPosition = model.ApproverPosition;
                    updateItem.KnownByName = model.KnownByName;
                    updateItem.KnownByPosition = model.KnownByPosition;

                    entities.SaveChanges();
                    saveHistory(model.Name);
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    Logger.Error("Error while executing AnnualReportsSetupController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }
    }
}
