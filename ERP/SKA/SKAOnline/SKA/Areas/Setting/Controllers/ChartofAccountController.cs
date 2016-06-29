using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.Setting.Models.ViewModels;
using SKA.Filters;
using System.Globalization;
using System.Collections;

namespace SKA.Areas.Setting.Controllers
{
    [BranchCodeFilter]
    public class ChartofAccountController : BaseController
    {
        //
        // GET: /Setting/ChartofAccount/
        private SKAEntities entities;
        private string BeginningBalanceSessionName = "BeginningBalanceSession";
        public ChartofAccountController()
        {
            entities = new SKAEntities();
            //ViewBag.Value = this.getSetupValueLabel();
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
            var model = from at in entities.Accounts
                        select new ChartofAccountViewModel
                        {
                            Id = at.Id,
                            Code = at.Code,
                            Name = at.Name

                            //CreditBeginningBalance = at.CreditBeginningBalance, 
                            //DebitBeginningBalance = at.DebitBeginningBalance
                        };

            //var dateBalance = (from c in entities.BeginningBalanceBranches
            //                   select c.TransactionDate).FirstOrDefault();

            var countBranch = from c in entities.Branches
                               select new BranchViewModel
                               {
                                    Id = c.Id
                               };

            //ViewData["CodeBranch"] = countBranch.Count();
            ////var accountBranch = (from c in entities.Accounts
            ////                         from p in entities.Branches
            ////                         select new { c.Id, p.Code}).ToList();


            var accountBalance = from c in entities.BeginningBalanceBranches
                                  select new BeginningBalanceViewModel
                                  {
                                    Id = c.Id,
                                    AccountId = c.AccountId,
                                    BranchId = c.BranchId
                                  };

            var aa = countBranch.Select(b => b.Id);
            
            //ViewData["AccountBranch"] = accountBranch;
 
            //ArrayList count = new ArrayList();
            
            //for (int i = 0; i < accountBranch.Count(); i++)
            //{ 
            //    count.Add(accountBranch.ElementAt(i));
            //    ViewData["count" + i] = count[i];    
            //}

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Name.Contains(searchValue)
                    || a.Code.Contains(searchValue));
            }
            return View(new GridModel<ChartofAccountViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ChartofAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldAccount = (from c in entities.Accounts
                                      where c.Code == model.Code
                                      select c).FirstOrDefault();

                    if (oldAccount != null)
                    {
                        ModelState.AddModelError("Code", "Kode Perkiraan sudah ada");
                    }
                    else { 
                        var newAccount = new Account();
                    
                        newAccount.Code = model.Code;
                        newAccount.Name = model.Name;
                        
                        entities.Accounts.AddObject(newAccount);
                        entities.SaveChanges();
                        saveHistory(model.Code);
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing ChartofAccountController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }
        [GridAction()]
        public ActionResult _UpdateDetail(int? id)
        {
            var model = (List<BeginningBalanceViewModel>)Session[BeginningBalanceSessionName];
            BeginningBalanceViewModel postmodel = new BeginningBalanceViewModel();

            string transactionDateString = Request.Form["TransactionDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);

            int getThisYear = DateTime.Now.Year;
            postmodel.TransactionDate = transactionDate;

            //var errorItem = ModelState.Keys.Where(m => m == "TransactionDate");

            //if (errorItem != null)
            //    ModelState["TransactionDate"].Errors.Clear();

            if (TryUpdateModel(postmodel, new string[] { "Debet", "Kredit", "AccountId", "BranchId", "BeginningBalanceBranchId" }))
            {
                var item = model.Where(m => m.BeginningBalanceBranchId == postmodel.BeginningBalanceBranchId).FirstOrDefault();

                if (item != null)
                {
                    item.Debet = postmodel.Debet;
                    item.Kredit = postmodel.Kredit;
                    item.TransactionDate = postmodel.TransactionDate;
                }
            }

            return View(new GridModel<BeginningBalanceViewModel>(model));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new ChartofAccountViewModel();

            try
            {

                var result = (from at in entities.Accounts
                              where at.Id == id
                              select at).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.Code = result.Code;
                    model.Name = result.Name;
                    //model.DebitBeginningBalance = result.DebitBeginningBalance;
                    //model.CreditBeginningBalance = result.CreditBeginningBalance;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing ChartofAccountController.Edit [Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed.");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ChartofAccountViewModel model)
        {
            var updateAccount = (from at in entities.Accounts
                                 where at.Id == model.Id
                                 select at).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    var oldAccount = (from a in entities.Accounts
                                      where a.Code == model.Code && a.Id != model.Id
                                      select a).FirstOrDefault();

                    if (oldAccount != null)
                    {
                        ModelState.AddModelError("Code", "Kode Perkiraan sudah ada");
                    }
                    else if(updateAccount.Code == "11.02.03")
                    {
                        ModelState.AddModelError("Code","Kode perkiraan tidak dapat diubah karena digunakan untuk voucher kas kecil");   
                    }
                    else { 
                        updateAccount.Code = model.Code;
                        updateAccount.Name = model.Name;
                        //updateAccount.DebitBeginningBalance = model.DebitBeginningBalance;
                        //updateAccount.CreditBeginningBalance = model.CreditBeginningBalance;

                        entities.SaveChanges();
                        saveHistory(model.Code);
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing ChartofAccountController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data tidak dapat diedit.");
                }
            }

            model.Code = updateAccount.Code;
            model.Name = updateAccount.Name;

            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = new ChartofAccountViewModel();

            try
            {
                
                var result = (from at in entities.Accounts
                              where at.Id == id
                              select at).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.Code = result.Code;
                    model.Name = result.Name;
                    //model.DebitBeginningBalance = result.DebitBeginningBalance;
                    //model.CreditBeginningBalance = result.CreditBeginningBalance;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing AccountTypeController.Delete [Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed.");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(ChartofAccountViewModel model)
        {
            var result = (from at in entities.Accounts
                          where at.Id == model.Id
                          select at).FirstOrDefault();
            try
            {
                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                }
                else if (result.Code.Equals("11.02.03"))
                {
                    SetErrorMessageViewData("Kode Perkiraan tidak bisa dihapus, karena telah digunakan untuk keperluan Voucher Kas Kecil");
                }
                else
                {
                    entities.Accounts.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory(result.Code);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing ChartofAccountController.Delete [Post].", ex);
                SetErrorMessageViewData("Kode Perkiraan tidak bisa dihapus, karena telah digunakan oleh transaksi lain");
            }

            model.Id = result.Id;
            model.Code = result.Code;
            model.Name = result.Name;
            //model.CreditBeginningBalance = result.CreditBeginningBalance;
            //model.DebitBeginningBalance = result.DebitBeginningBalance;

            return View(model);
        }

        [HttpGet]
        public ActionResult BeginningBalance(int id)
        {
            Session.Remove(BeginningBalanceSessionName);
            DateTime getDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var beginningBalance = entities.SetFirstBalance(id);

            var model = (from c in entities.BeginningBalanceBranches
                         where c.AccountId == id
                         select new BeginningBalanceViewModel
                         {
                             AccountId = c.AccountId,
                             AccountName = c.Account.Name,
                             AccountCode = c.Account.Code,
                             Debet = c.Debet,
                             Kredit = c.Kredit,
                             BranchId = c.BranchId,
                             TransactionDate = c.TransactionDate,
                             BeginningBalanceBranchId = c.Id
                         }).FirstOrDefault();

            return View(model);

            //var getAllBranch = entities.Branches.ToList();

            //var existingId = entities.BeginningBalanceBranches.Where(a => a.AccountId == id && a.TransactionDate == getDate).FirstOrDefault();

            //var accountCodeList = (from c in entities.Accounts
            //                       from d in entities.Branches
            //                       where c.Id == id
            //                       select new
            //                       {
            //                           AccountId = c.Id,
            //                           AccountCode = c.Code,
            //                           AccountName = c.Name,
            //                           BranchId = d.Id,
            //                           BranchCode = d.Code,
            //                           BranchName = d.Name
            //                       }).ToList();


            ////if (existingId != null)
            ////{ 
            ////    if(getAllBranch.Count != existingId.Count)
            ////    {
            ////        foreach (var detail in existingId)
            ////        {
            ////            //if(detail.BranchId.ToString().){} 
            ////        }
            ////    }
            ////}

            //try
            //{
            //    if (existingId == null)
            //    {

            //        for (int i = 0; i < accountCodeList.Count; i++)
            //        {
            //            DateTime firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //            var newBudgetDetail = new BeginningBalanceBranch();
            //            newBudgetDetail.AccountId = accountCodeList[i].AccountId;
            //            newBudgetDetail.BranchId = accountCodeList[i].BranchId;
            //            newBudgetDetail.Debet = 0;
            //            newBudgetDetail.TransactionDate = firstDate;
            //            entities.BeginningBalanceBranches.AddObject(newBudgetDetail);

            //        }
            //        entities.SaveChanges();
            //        var model = (from c in entities.BeginningBalanceBranches
            //                     where c.AccountId == id
            //                     select new BeginningBalanceViewModel
            //                     {
            //                         AccountName = c.Account.Name,
            //                         AccountCode = c.Account.Code,
            //                         Debet = c.Debet,
            //                         Kredit = c.Kredit,
            //                         BranchId = c.BranchId,
            //                         TransactionDate = c.TransactionDate
            //                     }).FirstOrDefault();
            //        return View(model);
            //    }
            //    else
            //    {
            //        var model = (from c in entities.BeginningBalanceBranches
            //                     where c.AccountId == id
            //                     select new BeginningBalanceViewModel
            //                     {
            //                         AccountId = c.AccountId,
            //                         AccountName = c.Account.Name,
            //                         AccountCode = c.Account.Code,
            //                         Debet = c.Debet,
            //                         Kredit = c.Kredit,
            //                         BranchId = c.BranchId,
            //                         TransactionDate = c.TransactionDate
            //                     }).FirstOrDefault();
            //        return View(model);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.Error("Error. Detail: ", ex);
            //    return RedirectToAction("Index");
            //}
        }

        [HttpPost]
        public ActionResult BeginningBalance(BeginningBalanceViewModel model)
        {
            var beginningBalanceSessionlist = (List<BeginningBalanceViewModel>)(Session[BeginningBalanceSessionName]);

            //if (pettyCashDetailSessionlist == null || pettyCashDetailSessionlist.Count <= 0)
            //    ModelState.AddModelError("", "Silahkan isi detail pembayaran.");


            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.Accounts
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    if (newDetail == null)
                    {
                        return RedirectToAction("Index");
                    }


                    // delete old data first, before inserting new detail
                    //DateTime getDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    foreach (var item in entities.BeginningBalanceBranches.Where(p => p.AccountId == newDetail.Id))
                    {
                        entities.BeginningBalanceBranches.DeleteObject(item);
                    }

                   
                    // insert new detail
                    foreach (var item in beginningBalanceSessionlist)
                    {
                        var detail = new BeginningBalanceBranch
                        {
                            AccountId = item.AccountId,
                            BranchId = item.BranchId,
                            Debet = item.Debet,
                            Kredit = item.Kredit,
                            TransactionDate = item.TransactionDate
                        };

                        entities.BeginningBalanceBranches.AddObject(detail);

                    }

                    entities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing ChartodAccountController.BeginningBalanceBranch [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }

        [GridAction]
        public ActionResult _SelectBeginningBalance(int? id)
        {
            var model = (List<BeginningBalanceViewModel>)Session[BeginningBalanceSessionName];
            if (model == null)
            {
                model = (from data in entities.BeginningBalanceBranches
                         where data.AccountId.Equals(id.Value)
                         select new BeginningBalanceViewModel
                         {
                             Id = data.Id,
                             AccountId = data.AccountId,
                             AccountCode = data.Account.Code + "." + data.Branch.Code,
                             AccountName = data.Account.Name + " " + data.Branch.Name,
                             BranchId = data.BranchId,
                             Debet = data.Debet,
                             Kredit = data.Kredit,
                             TransactionDate = data.TransactionDate,
                             BeginningBalanceBranchId = data.Id
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[BeginningBalanceSessionName] = model;
            }

            return View(new GridModel<BeginningBalanceViewModel>(model));
        }

        //public ActionResult SetFirstBalance()
        //{ 
        //    var accountCodeList = (from c in entities.Accounts
        //                           from d in entities.Branches
        //                           select new
        //                           {
        //                               AccountId = c.Id,
        //                               AccountCode = c.Code,
        //                               AccountName = c.Name,
        //                               BranchId = d.Id,
        //                               BranchCode = d.Code,
        //                               BranchName = d.Name
        //                           }).ToList();


        //    for (int i = 0; i < accountCodeList.Count; i++)
        //    {
        //        DateTime firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //        var newBudgetDetail = new BeginningBalanceBranch();
        //        newBudgetDetail.AccountId = accountCodeList[i].AccountId;
        //        newBudgetDetail.BranchId = accountCodeList[i].BranchId;
        //        newBudgetDetail.Debet = 0;
        //        newBudgetDetail.TransactionDate = firstDate;
        //        entities.BeginningBalanceBranches.AddObject(newBudgetDetail);

        //    }
        //    entities.SaveChanges();

        //    return RedirectToAction("Index");
        //}
    }
}
