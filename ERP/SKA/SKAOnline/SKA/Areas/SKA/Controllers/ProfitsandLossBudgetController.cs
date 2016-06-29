using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.SKA.Models.ViewModels;
using System.Collections;

namespace SKA.Areas.SKA.Controllers
{
    public class ProfitsandLossBudgetController : BaseController
    {
        //        //
        //        // GET: /SKA/ProfitandLossBudget/
        private SKAEntities entities;
        private string profitandLostBudgetSessionName = "ProfitandLostBudgetSession";
        private static int getYear;
        public ProfitsandLossBudgetController()
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

        [HttpGet]
        public ActionResult Index(int year)
        {
            var model = new ProfitsandLossBudgetViewModel();
            model.FiscalYear = year;
            getYear = year;
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
            return RedirectToAction("Index", new {year = getYear });
        }

        [GridAction]
        public ActionResult GetList(int year, string searchValue)
        {
            var getYear = entities.ProfitandLossBudgets.Where(a => a.FiscalYear == year).FirstOrDefault();

            var template = (from c in entities.ProfitandLossBudgetTemplates
                            select c).ToList();

            try
            {
                if (getYear == null)
                {

                    for (int i = 0; i < template.Count; i++)
                    {
                        var newBudget = new ProfitandLossBudget();
                        newBudget.Name = template[i].Name;
                        newBudget.AccountCode = template[i].AccountCode;
                        newBudget.FiscalYear = year;
                        entities.ProfitandLossBudgets.AddObject(newBudget);

                    }
                    entities.SaveChanges();
             //       var accountCodeList = (from c in entities.Accounts
            //                       from d in entities.Branches
            //                       where c.Code.StartsWith(accountCode.AccountCode)
            //                       select new { 
            //                            AccountId = c.Id,
            //                            AccountCode = c.Code,
            //                            AccountName = c.Name,
            //                            BranchId = d.Id,
            //                            BranchCode = d.Code,
            //                            BranchName = d.Name
            //                       }).ToList();
            //try
                    var model = from c in entities.ProfitandLossBudgets
                                let jan = c.ProfitandLossBudgetDetails.Sum(m => m.January)
                                let feb = c.ProfitandLossBudgetDetails.Sum(m => m.February)
                                let mar = c.ProfitandLossBudgetDetails.Sum(m => m.March)
                                let april = c.ProfitandLossBudgetDetails.Sum(m => m.April)
                                let may = c.ProfitandLossBudgetDetails.Sum(m => m.May)
                                let jun = c.ProfitandLossBudgetDetails.Sum(m => m.June)
                                let jul = c.ProfitandLossBudgetDetails.Sum(m => m.July)
                                let aug = c.ProfitandLossBudgetDetails.Sum(m => m.August)
                                let sep = c.ProfitandLossBudgetDetails.Sum(m => m.September)
                                let oct = c.ProfitandLossBudgetDetails.Sum(m => m.October)
                                let nov = c.ProfitandLossBudgetDetails.Sum(m => m.November)
                                let dec = c.ProfitandLossBudgetDetails.Sum(m => m.December)
                                where c.FiscalYear == year
                                select new ProfitsandLossBudgetViewModel
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    FiscalYear = c.FiscalYear,
                                    AccountCode = c.AccountCode,
                                    Amount = c.Amount,
                                    January = jan,
                                    February = feb,
                                    March = mar,
                                    April = april,
                                    May = may,
                                    June = jun,
                                    July = jul,
                                    August = aug,
                                    September = sep,
                                    October = oct,
                                    November = nov,
                                    December = dec
                                };

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        model = model.Where(a => a.Name.Contains(searchValue)
                            || a.AccountCode.Contains(searchValue));
                    }
                    return View(new GridModel<ProfitsandLossBudgetViewModel>(model));
                    
                }
                else
                {
                    var model = from c in entities.ProfitandLossBudgets
                                let jan = c.ProfitandLossBudgetDetails.Sum(m => m.January)
                                let feb = c.ProfitandLossBudgetDetails.Sum(m => m.February)
                                let mar = c.ProfitandLossBudgetDetails.Sum(m => m.March)
                                let april = c.ProfitandLossBudgetDetails.Sum(m => m.April)
                                let may = c.ProfitandLossBudgetDetails.Sum(m => m.May)
                                let jun = c.ProfitandLossBudgetDetails.Sum(m => m.June)
                                let jul = c.ProfitandLossBudgetDetails.Sum(m => m.July)
                                let aug = c.ProfitandLossBudgetDetails.Sum(m => m.August)
                                let sep = c.ProfitandLossBudgetDetails.Sum(m => m.September)
                                let oct = c.ProfitandLossBudgetDetails.Sum(m => m.October)
                                let nov = c.ProfitandLossBudgetDetails.Sum(m => m.November)
                                let dec = c.ProfitandLossBudgetDetails.Sum(m => m.December)
                                where c.FiscalYear == year
                                select new ProfitsandLossBudgetViewModel
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    FiscalYear = c.FiscalYear,
                                    AccountCode = c.AccountCode,
                                    Amount = c.Amount,
                                    January = jan,
                                    February = feb,
                                    March = mar,
                                    April = april,
                                    May = may,
                                    June = jun,
                                    July = jul,
                                    August = aug,
                                    September = sep,
                                    October = oct,
                                    November = nov,
                                    December = dec
                                };

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        model = model.Where(a => a.Name.Contains(searchValue)
                            || a.AccountCode.Contains(searchValue));
                    }
                    return View(new GridModel<ProfitsandLossBudgetViewModel>(model));
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error. Detail: ", ex);
                return RedirectToAction("GetYear");
            }
        }

        [GridAction]
        public ActionResult _SelectDetail(int? id)
        {
            var model = (List<ProfitsandLossBudgetDetailViewModel>)Session[profitandLostBudgetSessionName];
            if (model == null)
            {
                model = (from data in entities.ProfitandLossBudgetDetails
                         where data.ProfitandLossBudgetId.Equals(id.Value)
                         select new ProfitsandLossBudgetDetailViewModel
                         {
                             ProfitandLossBudgetId = data.ProfitandLossBudgetId,
                             AccountId = data.AccountId,
                             AccountCode = data.Account.Code + "." +data.Branch.Code,
                             AccountName = data.Account.Name + " " + data.Branch.Name,
                             BranchId = data.BranchId,
                             January = data.January,
                             February = data.February,
                             March = data.March,
                             April = data.April,
                             May = data.May,
                             June = data.June,
                             July = data.July,
                             August = data.August,
                             September = data.September,
                             October = data.October,
                             November = data.November,
                             December = data.December,
                             Amount = data.Amount
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[profitandLostBudgetSessionName] = model;
            }

            return View(new GridModel<ProfitsandLossBudgetDetailViewModel>(model));
        }
        [GridAction]
        public ActionResult _SelectDetailAmount(int? id, int? accountid, int? branchId)
        {
            var model = (List<ProfitsandLossBudgetDetailViewModel>)Session[profitandLostBudgetSessionName];
            var detail = (from d in model
                         where d.ProfitandLossBudgetId == id && d.AccountId == accountid && d.BranchId == branchId
                          select new ProfitsandLossBudgetDetailViewModel
                               {
                                   January = d.January,
                                   February = d.February,
                                   March = d.March,
                                   April = d.April,
                                   May = d.May,
                                   June = d.June,
                                   July = d.July,
                                   August = d.August,
                                   September = d.September,
                                   October = d.October,
                                   November = d.November,
                                   December = d.December
                               }).ToList();
            

            return View(new GridModel<ProfitsandLossBudgetDetailViewModel>(detail));
        }

        [GridAction()]
        public ActionResult _UpdateDetail(int? accountid, int? branchId)
        {
            var model = (List<ProfitsandLossBudgetDetailViewModel>)Session[profitandLostBudgetSessionName];
          
            ProfitsandLossBudgetDetailViewModel postmodel = new ProfitsandLossBudgetDetailViewModel();

            if (TryUpdateModel(postmodel))
            {
                var item = model.Where(m => m.ProfitandLossBudgetId == postmodel.ProfitandLossBudgetId && m.AccountId == accountid.Value && m.BranchId == branchId.Value).FirstOrDefault();
                if (item != null)
                {
                    var amount = postmodel.Amount / 12;
                    int eachAmount = (int)amount;
                    int modAmount = eachAmount - (eachAmount % 100);
                    int total11Amount = modAmount * 11;

                    item.Amount = postmodel.Amount;
                    item.January = modAmount;
                    item.February = modAmount;
                    item.March = modAmount;
                    item.April = modAmount;
                    item.May = modAmount;
                    item.June = modAmount;
                    item.July = modAmount;
                    item.August = modAmount;
                    item.September = modAmount;
                    item.October = modAmount;
                    item.November = modAmount;
                    item.December = postmodel.Amount - total11Amount;
                }
            }

            return View(new GridModel<ProfitsandLossBudgetDetailViewModel>(model));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Session.Remove(profitandLostBudgetSessionName);

            //var existingId = entities.ProfitandLossBudgetDetails.Where(a => a.ProfitandLossBudgetId == id).FirstOrDefault();
            var accountCode = entities.ProfitandLossBudgets.Where(c => c.Id == id).FirstOrDefault();

            var budget = entities.ProcBudget(id);
            try 
            { 
                var model = (from c in entities.ProfitandLossBudgets
                                where c.Id == id
                                select new ProfitsandLossBudgetViewModel
                                {
                                    Name = c.Name,
                                    Amount = c.Amount,
                                    FiscalYear = c.FiscalYear
                                }).FirstOrDefault();

                ViewData["Year"] = model.FiscalYear;
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error("Error. Detail: ", ex);
                return RedirectToAction("Index", new { year = accountCode.FiscalYear });
            }
            
             //return RedirectToAction("Index", new { year = accountCode.FiscalYear });
            //var accountCodeList = (from c in entities.Accounts
            //                       from d in entities.Branches
            //                       where c.Code.StartsWith(accountCode.AccountCode)
            //                       select new { 
            //                            AccountId = c.Id,
            //                            AccountCode = c.Code,
            //                            AccountName = c.Name,
            //                            BranchId = d.Id,
            //                            BranchCode = d.Code,
            //                            BranchName = d.Name
            //                       }).ToList();
            //try
            //{
            //    if (existingId == null)
            //    {
                    
            //        for (int i = 0; i < accountCodeList.Count; i++)
            //        {
            //            var newBudgetDetail = new ProfitandLossBudgetDetail();
            //            newBudgetDetail.ProfitandLossBudgetId = id;
            //            newBudgetDetail.AccountId = accountCodeList[i].AccountId;
            //            newBudgetDetail.BranchId = accountCodeList[i].BranchId;

            //            entities.ProfitandLossBudgetDetails.AddObject(newBudgetDetail);

            //        }
            //        entities.SaveChanges();
            //        var model = (from c in entities.ProfitandLossBudgets
            //                     where c.Id == id
            //                     select new ProfitsandLossBudgetViewModel
            //                     {
            //                         Name = c.Name,
            //                         Amount = c.Amount,
            //                         FiscalYear = c.FiscalYear
            //                     }).FirstOrDefault();
            //        ViewData["Year"] = model.FiscalYear;
            //        return View(model);
            //    }
            //    else
            //    {
            //        var model = (from c in entities.ProfitandLossBudgets
            //                     where c.Id == id
            //                     select new ProfitsandLossBudgetViewModel
            //                     {
            //                         Name = c.Name,
            //                         Amount = c.Amount,
            //                         FiscalYear = c.FiscalYear
            //                     }).FirstOrDefault();
            //        ViewData["Year"] = model.FiscalYear;
            //        return View(model);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error("Error. Detail: ", ex);
            //    return RedirectToAction("Index", new { year = accountCode.FiscalYear});
            //}
        }

        [HttpPost]
        public ActionResult Edit(ProfitsandLossBudgetViewModel model)
        {
            var profitandLossDetailSessionlist = (List<ProfitsandLossBudgetDetailViewModel>)(Session[profitandLostBudgetSessionName]);

            //if (pettyCashDetailSessionlist == null || pettyCashDetailSessionlist.Count <= 0)
            //    ModelState.AddModelError("", "Silahkan isi detail pembayaran.");

            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.ProfitandLossBudgets
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    if (newDetail == null)
                    {
                        return RedirectToAction("Index", new { year = newDetail.FiscalYear });
                    }

                    decimal? amount = 0;

                    foreach (var itemAmount in profitandLossDetailSessionlist)
                    {
                        amount += (itemAmount.Amount == null) ? 0 : itemAmount.Amount;
                    }
                    newDetail.Amount = amount;

                    // delete old data first, before inserting new detail
                    foreach (var item in entities.ProfitandLossBudgetDetails.Where(p => p.ProfitandLossBudgetId == newDetail.Id))
                    {
                        entities.ProfitandLossBudgetDetails.DeleteObject(item);
                    }

                    // insert new detail
                    foreach (var item in profitandLossDetailSessionlist)
                    {
                        var detail = new ProfitandLossBudgetDetail
                        {
                            ProfitandLossBudgetId = newDetail.Id,
                            AccountId = item.AccountId,
                            BranchId = item.BranchId,
                            Amount = item.Amount,
                            January = item.January,
                            February = item.February,
                            March = item.March,
                            April = item.April,
                            May = item.May,
                            June = item.June,
                            July = item.July,
                            August = item.August,
                            September = item.September,
                            October = item.October,
                            November = item.November,
                            December = item.December
                        };

                        entities.ProfitandLossBudgetDetails.AddObject(detail);
                    }

                    entities.SaveChanges();
                    saveHistory("Tahun " + model.FiscalYear.ToString());
                    var getYear = entities.ProfitandLossBudgets.Where(a => a.Id == model.Id).FirstOrDefault();
                    return RedirectToAction("Index", new { year = getYear .FiscalYear});
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing ProfitsandLossBudgetController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GetAccountCodeSelectList(string text)
        {

            var branch = GetCurrentUserBranchId();

            var branchAccount = entities.GetAccountBranch(branch.Code, text);
            var accounts = (from a in branchAccount
                            select new
                            {
                                AccountCode = a.AccountCode
                            }).ToList();

            return new JsonResult { Data = new SelectList(accounts, "AccountCode", "AccountCode") };
        }
    }
}
