using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using Telerik.Web.Mvc;
using SKA.Models;
using SKA.Areas.SKA.Models.ViewModels;

namespace SKA.Areas.SKA.Controllers
{
    public class PBIKJournalFormatsController : BaseController
    {
        //
        // GET: /SKA/AccountJournalFormats/
        private SKAEntities entities;

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

        public PBIKJournalFormatsController()
        {
            entities = new SKAEntities();
        }

        [GridAction]
        public ActionResult _SelectDetail(string searchValue)
        {
            var model = from data in entities.PBIKJournalFormats
                         select new PBIKJournalFormatsViewModel
                         {
                             Id = data.Id,
                             Status = data.Status,
                             AccountCode = data.AccountCode,
                             TurnNumber = data.TurnNumber,
                             Name = data.Name
                         };

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.AccountCode.Contains(searchValue));
            }

            return View(new GridModel<PBIKJournalFormatsViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(PBIKJournalFormatsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newPBIKJournalFormat = new PBIKJournalFormat();
                    
                    newPBIKJournalFormat.AccountCode = model.AccountCode;
                    newPBIKJournalFormat.Status = model.Status;
                    newPBIKJournalFormat.TurnNumber = model.TurnNumber;
                    newPBIKJournalFormat.Name = model.Name;

                    entities.PBIKJournalFormats.AddObject(newPBIKJournalFormat);
                    entities.SaveChanges();
                    saveHistory(model.AccountCode);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing PBIKJournalFormatsController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = (from d in entities.PBIKJournalFormats
                         where d.Id == id
                         select new PBIKJournalFormatsViewModel
                         {
                             AccountCode = d.AccountCode,
                             Status = d.Status,
                             TurnNumber = d.TurnNumber,
                             Name = d.Name
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PBIKJournalFormatsViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.PBIKJournalFormats
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    if (newDetail == null)
                    {
                        return RedirectToAction("Index");
                    }
                    //var account = entities.Accounts.Where(a => a.Code == model.AccountCode).FirstOrDefault();

                    newDetail.AccountCode = model.AccountCode;
                    newDetail.Status = model.Status;
                    newDetail.TurnNumber = model.TurnNumber;
                    newDetail.Name = model.Name;

                    entities.SaveChanges();
                    saveHistory(model.AccountCode);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing PBIKJournalFormatsController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = new PBIKJournalFormatsViewModel();

            try
            {
                var result = (from b in entities.PBIKJournalFormats
                              where b.Id == id
                              select b).FirstOrDefault();


                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.AccountCode = result.AccountCode;
                    model.Status = result.Status;
                    model.TurnNumber = result.TurnNumber;
                    model.Name = result.Name;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error where executing PBIKJournalFormatsController.Delete[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(PBIKJournalFormatsViewModel model)
        {
            try
            {
                var result = (from b in entities.PBIKJournalFormats
                              where b.Id == model.Id
                              select b).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                }
                else
                {
                    entities.PBIKJournalFormats.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory(result.AccountCode);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while execute PBIKJournalFormatsController.Delete[Post].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }
    }
}
