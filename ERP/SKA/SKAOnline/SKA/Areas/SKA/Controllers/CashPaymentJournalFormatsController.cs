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
    public class CashPaymentJournalFormatsController : BaseController
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

        public CashPaymentJournalFormatsController()
        {
            entities = new SKAEntities();
        }

        [GridAction]
        public ActionResult _SelectDetail(string searchValue)
        {
            var model = from data in entities.CashPaymentJournalFormats
                         select new CashPaymentJournalFormatsViewModel
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
            return View(new GridModel<CashPaymentJournalFormatsViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(CashPaymentJournalFormatsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newCashPaymentJournalFormat = new CashPaymentJournalFormat();

                    newCashPaymentJournalFormat.AccountCode = model.AccountCode;
                    newCashPaymentJournalFormat.Status = model.Status;
                    newCashPaymentJournalFormat.TurnNumber = model.TurnNumber;
                    newCashPaymentJournalFormat.Name = model.Name;

                    entities.CashPaymentJournalFormats.AddObject(newCashPaymentJournalFormat);
                    entities.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing CashPaymentJournalFormatController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = (from d in entities.CashPaymentJournalFormats
                         where d.Id == id
                         select new CashPaymentJournalFormatsViewModel
                         {
                             AccountCode = d.AccountCode,
                             Status = d.Status,
                             TurnNumber = d.TurnNumber,
                             Name = d.Name
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CashPaymentJournalFormatsViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.CashPaymentJournalFormats
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

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing CashPaymentJournalFormatsViewModelController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = new CashPaymentJournalFormatsViewModel();

            try
            {
                var result = (from b in entities.CashPaymentJournalFormats
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
                Logger.Error("Error where executing CashPaymentJournalFormatsController.Delete[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(CashPaymentJournalFormatsViewModel model)
        {
            try
            {
                var result = (from b in entities.CashPaymentJournalFormats
                              where b.Id == model.Id
                              select b).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                }
                else
                {
                    entities.CashPaymentJournalFormats.DeleteObject(result);
                    entities.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while execute CashPaymentJournalFormatslController.Delete[Post].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }
    }
}
