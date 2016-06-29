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
    public class RevenueExpenditureController : BaseController
    {
        //
        // GET: /SKA/Expenditure/
        private SKAEntities entities;

        public RevenueExpenditureController()
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
            var model = from c in entities.RevenuesandExpenditures
                        select new RevenueExpenditureViewModel 
                        {
                            Id = c.Id,
                            AccountId = c.AccountId,
                            Tipe = c.Name,
                            AccountCode = c.Account.Code,
                            AccountName = c.Account.Name
                        };

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.AccountCode.Contains(searchValue)
                    || a.AccountName.Contains(searchValue)
                    || a.Tipe.Contains(searchValue));
            }
            return View(new GridModel<RevenueExpenditureViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            Dictionary<int, string> tipe = new Dictionary<int, string>();

            tipe.Add(1, "Penerimaan Operasional");
            tipe.Add(2, "Pengeluaran Operasional");

            ViewBag.TipeSelected = new SelectList(tipe.ToList(), "Value", "Value");
            return View();
        }

        [HttpPost]
        public ActionResult GetAccountCodeSelectList(string text)
        {
            
            var accounts = from a in entities.Accounts
                            select a;

            if (text != null)
            { 
                accounts = accounts.Where(a => a.Code.Contains(text));
            }
            return new JsonResult { Data = new SelectList(accounts.ToList(), "Id", "Code") };
        }

        [HttpPost]
        public ActionResult Create(RevenueExpenditureViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var account = entities.Accounts.Where(a => a.Code == model.AccountCode).FirstOrDefault();
                    var newItem = new RevenuesandExpenditure();

                    newItem.Id = model.Id;
                    newItem.AccountId = account.Id;
                    newItem.Name = model.Tipe;

                    entities.RevenuesandExpenditures.AddObject(newItem);
                    entities.SaveChanges();
                    saveHistory(model.AccountCode + "-" + model.Tipe);
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    Logger.Error("Error while executing RevenueExpenditureController.Create[Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }   
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Dictionary<int, string> tipe = new Dictionary<int, string>();

            tipe.Add(1, "Penerimaan Operasional");
            tipe.Add(2, "Pengeluaran Operasional");

            ViewBag.TipeSelected = new SelectList(tipe.ToList(), "Value", "Value");

            var model = (from c in entities.RevenuesandExpenditures
                         where c.Id == id
                         select new RevenueExpenditureViewModel 
                         { 
                            AccountId = c.AccountId,
                            AccountCode = c.Account.Code + " - " + c.Account.Name,
                            AccountName = c.Account.Name,
                            Tipe = c.Name
                         }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RevenueExpenditureViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var updateItem = (from c in entities.RevenuesandExpenditures
                                      where c.Id == model.Id
                                      select c).FirstOrDefault();

                    if (updateItem == null)
                    {
                        return RedirectToAction("Index");
                    }

                    var getAccount = entities.Accounts.Where(a => a.Code == model.AccountCode).FirstOrDefault();

                    updateItem.AccountId = getAccount.Id;
                    updateItem.Name = model.Tipe;

                    entities.SaveChanges();
                    saveHistory(model.AccountCode + "-" + model.Tipe);
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    Logger.Error("Error while executing RevenueExpenditureController.Edit[Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try 
            { 
                var model = (from c in entities.RevenuesandExpenditures
                                 where c.Id == id
                                 select new RevenueExpenditureViewModel
                                 {
                                     AccountId = c.AccountId,
                                     AccountCode = c.Account.Code,
                                     AccountName = c.Account.Name,
                                     Tipe = c.Name
                                 }).FirstOrDefault();
                if (model == null)
                {
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch(Exception ex)
            {
                Logger.Error("Error while executing RevenueExpenditureController.Delete [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(RevenueExpenditureViewModel model)
        {
            try 
            {
                var deleteItem = (from c in entities.RevenuesandExpenditures
                                  where c.Id == model.Id
                                  select c).FirstOrDefault();

                if (deleteItem != null)
                { 
                    entities.RevenuesandExpenditures.DeleteObject(deleteItem);
                    entities.SaveChanges();
                    saveHistory(deleteItem.Account.Code + "-" + deleteItem.Name);
                    return RedirectToAction("Index");
                }
            }

            catch(Exception ex)
            {
                Logger.Error("Error while executing RevenueExpenditureController.Delete[Post].", ex);
                SetErrorMessageViewData("Data cannot be created.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult ShowReport(string reportName)
        {
            ViewData["ReportName"] = reportName;
            return View();
        }
    }
}
