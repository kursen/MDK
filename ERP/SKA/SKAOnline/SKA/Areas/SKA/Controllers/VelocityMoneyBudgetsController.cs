using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Areas.SKA.Models.ViewModels;
using Telerik.Web.Mvc;
using SKA.Models;
using System.Collections.Specialized;

namespace SKA.Areas.SKA.Controllers
{
    public class VelocityMoneyBudgetsController : BaseController
    {
        //
        // GET: /SKA/VelocityMoneyBudgets/
        private SKAEntities entities;
        private static int tempYear;
        public VelocityMoneyBudgetsController()
        {
            entities = new SKAEntities();
        }

        public ActionResult GetYear()
        {
            Dictionary<int, int> years = new Dictionary<int, int>();

            for (int i = DateTime.Now.Year + 1; i >= 1990; i--)
            {
                years.Add(i, i);
            }

            var newYear = from a in years
                          select a;

            ViewBag.Year = new SelectList(newYear.ToList(), "Key", "Value");

            return View();
        }

        public ActionResult Index(int year)
        {
            var model = new VelocityMoneyBudgetsViewModel();
            model.FiscalYear = year;
            tempYear = year;
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
            return RedirectToAction("Index", new { year = tempYear });
        }

        [GridAction]
        public ActionResult GetList(int year, string searchValue)
        {
            var getYear = entities.VelocityMoneyBudgets.Where(a => a.FiscalYear == year).FirstOrDefault();

            var template = (from c in entities.VelocityMoneyBudgetTemplates
                            select c).ToList();

            try
            {
                if (getYear == null)
                {

                    for (int i = 0; i < template.Count; i++)
                    {
                        var newVelocity = new VelocityMoneyBudget();
                        newVelocity.Month = template[i].Month;
                        newVelocity.FiscalYear = year;
                        entities.VelocityMoneyBudgets.AddObject(newVelocity);

                    }
                    entities.SaveChanges();

                    var model = from c in entities.VelocityMoneyBudgets
                                where c.FiscalYear == year
                                select new VelocityMoneyBudgetsViewModel
                                {
                                    Id = c.Id,
                                    Month = c.Month,
                                    FiscalYear = c.FiscalYear,
                                    OperationalAcceptance = c.OperationalAcceptance,
                                    NonOperationalAcceptance = c.NonOperationalAcceptance,
                                    OperationalExpenses = c.OperationalExpenses,
                                    NonOperationalExpenses = c.NonOperationalExpenses,
                                    AcceptanceAmount = c.AcceptanceAmount,
                                    ExpensesAmount = c.ExpensesAmount,
                                    CashIncrementDecrement = c.CashIncrementDecrement,
                                    FirstBalance = c.FiscalYear,
                                    LastBalance = c.LastBalance
                                };

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        model = model.Where(a => a.Month.Contains(searchValue));
                    }
                    return View(new GridModel<VelocityMoneyBudgetsViewModel>(model));

                }
                else
                {
                    var model = from c in entities.VelocityMoneyBudgets
                                where c.FiscalYear == year
                                select new VelocityMoneyBudgetsViewModel
                                {
                                    Id = c.Id,
                                    Month = c.Month,
                                    FiscalYear = c.FiscalYear,
                                    OperationalAcceptance = c.OperationalAcceptance,
                                    NonOperationalAcceptance = c.NonOperationalAcceptance,
                                    OperationalExpenses = c.OperationalExpenses,
                                    NonOperationalExpenses = c.NonOperationalExpenses,
                                    AcceptanceAmount = c.AcceptanceAmount,
                                    ExpensesAmount = c.ExpensesAmount,
                                    CashIncrementDecrement = c.CashIncrementDecrement,
                                    FirstBalance = c.FiscalYear,
                                    LastBalance = c.LastBalance
                                };

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        model = model.Where(a => a.Month.Contains(searchValue));
                    }
                    return View(new GridModel<VelocityMoneyBudgetsViewModel>(model));
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error. Detail: ", ex);
                return RedirectToAction("GetYear");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            
            var templateId = entities.VelocityMoneyBudgets.Where(b => b.Id == id).FirstOrDefault();

            try {
                var model = (from c in entities.VelocityMoneyBudgets
                                where c.Id == id
                                select new VelocityMoneyBudgetsViewModel
                                {
                                    Month = c.Month,
                                    FiscalYear = c.FiscalYear,
                                    OperationalAcceptance = c.OperationalAcceptance,
                                    NonOperationalAcceptance = c.NonOperationalAcceptance,
                                    AcceptanceAmount = c.AcceptanceAmount,
                                    OperationalExpenses = c.OperationalExpenses,
                                    NonOperationalExpenses = c.NonOperationalExpenses,
                                    ExpensesAmount = c.ExpensesAmount,
                                    CashIncrementDecrement = c.CashIncrementDecrement,
                                    FirstBalance = c.FirstBalance,
                                    LastBalance = c.LastBalance
                                }).FirstOrDefault();

                var getData = entities.VelocityMoneyBudgets.Where(a => a.FiscalYear == model.FiscalYear && a.Id <= id).ToList();

                decimal? operationalAcceptance = 0;
                decimal? nonOperationalAcceptance = 0;
                decimal? operationalExpenses = 0;
                decimal? nonOperationalExpenses = 0;
                decimal? acceptanceAmount = 0;
                decimal? expensesAmount = 0;
                decimal? cashIncrementDecrement = 0;
                decimal? firstBalance = getData[0].FirstBalance;
                decimal? lastBalance = 0;

                if (getData.Count == 1)
                {
                    operationalAcceptance = model.OperationalAcceptance;
                    nonOperationalAcceptance = model.NonOperationalAcceptance;
                    operationalExpenses = model.OperationalExpenses;
                    nonOperationalExpenses = model.NonOperationalExpenses;
                    acceptanceAmount = model.AcceptanceAmount;
                    expensesAmount = model.ExpensesAmount;
                    cashIncrementDecrement = model.CashIncrementDecrement;
                    lastBalance = model.LastBalance;
                }
                else { 
                    for (int i = 0; i < getData.Count; i++)
                    {
                        operationalAcceptance += (getData[i].OperationalAcceptance != null) ? getData[i].OperationalAcceptance : 0;
                        nonOperationalAcceptance += (getData[i].NonOperationalAcceptance != null) ? getData[i].NonOperationalAcceptance : 0;
                        operationalExpenses += (getData[i].OperationalExpenses != null) ? getData[i].OperationalExpenses : 0;
                        nonOperationalExpenses += (getData[i].NonOperationalExpenses != null) ? getData[i].NonOperationalExpenses : 0;
                        acceptanceAmount += (getData[i].AcceptanceAmount != null) ? getData[i].AcceptanceAmount : 0;
                        expensesAmount += (getData[i].ExpensesAmount != null) ? getData[i].ExpensesAmount : 0;
                        cashIncrementDecrement += (getData[i].CashIncrementDecrement != null) ? getData[i].CashIncrementDecrement : 0;
                        lastBalance = cashIncrementDecrement + firstBalance;
                    }
                }
                ViewData["operationalAccept"] = operationalAcceptance;
                ViewData["nonOperationalAccept"] = nonOperationalAcceptance;
                ViewData["operationalExpenses"] = operationalExpenses;
                ViewData["nonOperationalExpenses"] = nonOperationalExpenses;
                ViewData["acceptance"] = acceptanceAmount;
                ViewData["expenses"] = expensesAmount;
                ViewData["cashIncrementDecrement"] = cashIncrementDecrement;
                ViewData["firstBalance"] = firstBalance;
                ViewData["lastBalance"] = lastBalance;

                ViewData["Year"] = model.FiscalYear;

                return View(model);
            }
            catch(Exception ex)
            {
                Logger.Error("Error. Detail: ", ex);
                return RedirectToAction("Index", new { year = templateId.FiscalYear});
            }
        }

        [HttpPost]
         public ActionResult Edit(VelocityMoneyBudgetsViewModel model)
         {
             if (ModelState.IsValid)
             {
                 try
                 {
                     var updatedItem = (from c in entities.VelocityMoneyBudgets
                                      where c.Id == model.Id
                                      select c).FirstOrDefault();
                     var getData = entities.VelocityMoneyBudgets.Where(a => a.Id == (model.Id - 1) && a.FiscalYear == updatedItem.FiscalYear).FirstOrDefault();

                     if (updatedItem == null)
                     {
                         return RedirectToAction("Index", new {year = updatedItem.FiscalYear });
                     }

                     if (updatedItem.Month == "Januari")
                     {
                         updatedItem.OperationalAcceptance = model.OperationalAcceptance;
                         updatedItem.NonOperationalAcceptance = model.NonOperationalAcceptance;
                         updatedItem.AcceptanceAmount = model.AcceptanceAmount;
                         updatedItem.OperationalExpenses = model.OperationalExpenses;
                         updatedItem.NonOperationalExpenses = model.NonOperationalExpenses;
                         updatedItem.ExpensesAmount = model.ExpensesAmount;
                         updatedItem.CashIncrementDecrement = model.CashIncrementDecrement;
                         updatedItem.FirstBalance = model.FirstBalance;
                         updatedItem.LastBalance = model.LastBalance;
                     }
                     else {
                         var getFirstBalance = (getData.LastBalance != null) ? getData.LastBalance : 0;
                         var getLastBalance = getFirstBalance + model.CashIncrementDecrement;

                         updatedItem.OperationalAcceptance = model.OperationalAcceptance;
                         updatedItem.NonOperationalAcceptance = model.NonOperationalAcceptance;
                         updatedItem.AcceptanceAmount = model.AcceptanceAmount;
                         updatedItem.OperationalExpenses = model.OperationalExpenses;
                         updatedItem.NonOperationalExpenses = model.NonOperationalExpenses;
                         updatedItem.ExpensesAmount = model.ExpensesAmount;
                         updatedItem.CashIncrementDecrement = model.CashIncrementDecrement;
                         updatedItem.FirstBalance = getFirstBalance;
                         updatedItem.LastBalance = getLastBalance;
                     }
                        
                     entities.SaveChanges();
                     saveHistory();
                     return RedirectToAction("Index", new { year = updatedItem.FiscalYear});
                 }
                 catch (Exception ex)
                 {
                     Logger.Error("Error while executing VelocityMoneyBudgetsController.Edit [Post].", ex);
                     SetErrorMessageViewData("Data cannot be edited.");
                 }
             }
             return View(model);
         }
    }
}
