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
    public class CashReceiptJournalFormatsController : BaseController
    {
        //
        // GET: /SKA/AccountJournalFormats/
        private SKAEntities entities;
        public CashReceiptJournalFormatsController()
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
        public ActionResult _SelectDetail(string searchValue)
        {
            var model = from data in entities.CashReceiptJournalFormats
                         select new CashReceiptJournalFormatsViewModel
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

            return View(new GridModel<CashReceiptJournalFormatsViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(CashReceiptJournalFormatsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newCashReceiptJournalFormat = new CashReceiptJournalFormat();

                    newCashReceiptJournalFormat.AccountCode = model.AccountCode;
                    newCashReceiptJournalFormat.Status = model.Status;
                    newCashReceiptJournalFormat.TurnNumber = model.TurnNumber;
                    newCashReceiptJournalFormat.Name = model.Name;

                    entities.CashReceiptJournalFormats.AddObject(newCashReceiptJournalFormat);
                    entities.SaveChanges();
                    saveHistory(model.AccountCode);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing CashReceiptJournalFormatsController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = (from d in entities.CashReceiptJournalFormats
                         where d.Id == id
                         select new CashReceiptJournalFormatsViewModel
                         {
                             AccountCode = d.AccountCode,
                             Status = d.Status,
                             TurnNumber = d.TurnNumber,
                             Name = d.Name
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CashReceiptJournalFormatsViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.CashReceiptJournalFormats
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    if (newDetail == null)
                    {
                        return RedirectToAction("Index");
                    }
                    
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
                    Logger.Error("Error while executing CashReceiptJournalFormatsController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = new CashReceiptJournalFormatsViewModel();

            try
            {
                var result = (from b in entities.CashReceiptJournalFormats
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
                Logger.Error("Error where executing CashReceiptJournalFormatsController.Delete[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(CashReceiptJournalFormatsViewModel model)
        {
            try
            {
                var result = (from b in entities.CashReceiptJournalFormats
                              where b.Id == model.Id
                              select b).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                }
                else
                {
                    entities.CashReceiptJournalFormats.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory(result.AccountCode);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while execute CashReceiptJournalFormatsController.Delete[Post].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }
    }
}
