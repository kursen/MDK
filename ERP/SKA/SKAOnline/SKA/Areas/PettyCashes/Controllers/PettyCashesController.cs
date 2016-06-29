using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.PettyCashes.Models.ViewModels;
using System.Collections;
using SKA.Areas.PettyCashes.Models.Repositories;
using System.Text;
using System.Threading;
using System.Globalization;
namespace SKA.Areas.PettyCashes.Controllers
{
    public class PettyCashesController : BaseController
    {
        private SKAEntities entities;
        private string pettyCashSessionName = "PettyCashSession";
        
        public PettyCashesController()
        {
            entities = new SKAEntities();
            //var user = GetCurrentUserBranchId();
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

            //var username = GetCurrentUserId();
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
            var branch = GetCurrentUserBranchId();

            var model = from c in entities.PettyCashes
                        select new PettyCashesViewModel
                        {
                            Id = c.Id,
                            Number = c.Number,
                            TransactionDate = c.TransactionDate,
                            PaidTo = c.PaidTo,
                            Necessity = c.Necessity,
                            ReceiverName = c.ReceiverName,
                            BranchId = c.BranchId,
                            Amount = c.Credit,
                            AccountCodePettyCash = c.AccountCode
                        };

            var getClosingDate = entities.Closings.Select(a => a.ClosingDate).FirstOrDefault();

            if (branch.Code != "00")
            {
                model = model.Where(a => a.BranchId == branch.Id);
            }

            if (getClosingDate != null)
            {
                model = model.Where(a => a.TransactionDate >= getClosingDate);
            }

            if(!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Number.Contains(searchValue) 
                    || a.PaidTo.Contains(searchValue)
                    || a.Necessity.Contains(searchValue)
                    || a.ReceiverName.Contains(searchValue));
            }
            return View(new GridModel<PettyCashesViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            Session.Remove(pettyCashSessionName);
            var branch = GetCurrentUserBranchId();
            var model = new PettyCashesViewModel();
            string value = System.Configuration.ConfigurationManager.AppSettings["VoucherKasKecilCode"];

            var account = (from c in entities.Accounts
                           where c.Code == value// Kode Kas Kecil
                          select c).FirstOrDefault();

            if(account == null) {
                account = new Account();
            }
            
            model.AccountCodePettyCash = account.Code + "."+ branch.Code;
            model.TransactionDate = DateTime.Now;
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(PettyCashesViewModel model)
        {
            var pettyCashDetailSessionlist = (List<PettyCashesDetailViewModel>)(Session[pettyCashSessionName]);

            if (pettyCashDetailSessionlist == null || pettyCashDetailSessionlist.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail voucher kas kecil.");
            
            var branch = GetCurrentUserBranchId();
            //var userid = GetCurrentUserId();

            string transactionDateString = Request.Form["TransactionDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);

            int getThisYear = DateTime.Now.Year;
            model.TransactionDate = transactionDate;

            var dttransaction = new DateTime(transactionDate.Year, transactionDate.Month, 1);
            var transmonth = DateTime.Now.Month;
            var transkyear = DateTime.Now.Year;
            var transday = DateTime.Now.Day;
            var transvoucher = new DateTime(transkyear, transmonth, 1);
            int resultCek = DateTime.Compare(dttransaction, transvoucher);

            var errorItem = ModelState.FirstOrDefault(m => m.Key == "TransactionDate");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (resultCek != 0)
            {
                var nextmonth = transactionDate.Month + 1;
                if (nextmonth == transmonth && transactionDate.Year == transkyear)
                {
                    if (transday > 4)
                    {
                        ModelState.AddModelError("TransactionDate", "Transaksi Voucher "
                                                   + string.Format("{0:y}", transactionDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("TransactionDate", "Transaksi Voucher Bulan "
                                                + string.Format("{0:y}", transactionDate) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newPettyCash = new PettyCash();

                    var nextNumber = 0;
                    var recordCount = (from pty in entities.PettyCashRecordCounters
                                       where pty.BranchId == branch.Id && pty.Year == getThisYear
                                       select pty).FirstOrDefault();

                    if (recordCount != null)
                        nextNumber = recordCount.LastCounter;

                    //var maxTransactionDate = (from pt in entities.PettyCashes
                    //                          where pt.BranchId == branch.Id
                    //                          select pt.TransactionDate).Max();

                    var pettyData = (from v in entities.PettyCashes
                                    where v.BranchId == branch.Id && v.TransactionDate.Year == getThisYear
                                    select v.Number).Max();

                    int no;
                    int lastCounter;
                    int yearPetty;
                    if (pettyData != null)
                    {
                        no = int.Parse(pettyData.Substring(0,4));
                        yearPetty = getThisYear;
                    }
                    else
                    {
                        no = 0;
                        yearPetty = getThisYear;
                    }

                    if (no >= nextNumber)
                    {
                        lastCounter = no;
                    }
                    else
                    {
                        lastCounter = nextNumber;
                    }
                    

                    string number = string.Format("{0:0000}", lastCounter + 1);

                    
                    newPettyCash.Id = Guid.NewGuid();
                    newPettyCash.Number = string.Format("{0}/{1}/VKK/{2}", number, branch.ShortName, getThisYear);

                    newPettyCash.TransactionDate = model.TransactionDate;
                    newPettyCash.PaidTo = model.PaidTo;
                    newPettyCash.Necessity = model.Necessity;
                    newPettyCash.ReceiverName = model.ReceiverName;
                    newPettyCash.BranchId = branch.Id;
                    newPettyCash.AccountCode = model.AccountCodePettyCash;
                    newPettyCash.Credit = model.Amount;

                    entities.PettyCashes.AddObject(newPettyCash);

                    foreach (var item in pettyCashDetailSessionlist)
                    {
                        var detail = new PettyCashDetail
                        {
                            PettyCashId = newPettyCash.Id,
                            AccountId = item.AccountId,
                            Debet = item.Debet,
                            BranchId = item.BranchId,
                            MasterBranch = item.MasterBranch 

                        };

                        entities.PettyCashDetails.AddObject(detail);
                    }

                    if (recordCount != null)
                        recordCount.LastCounter = lastCounter + 1;

                    else
                    {
                        var newCounter = new PettyCashRecordCounter { 
                            Id = Guid.NewGuid(),
                            BranchId = branch.Id, 
                            LastCounter = 1, 
                            Year = getThisYear 
                        };
                        entities.PettyCashRecordCounters.AddObject(newCounter);
                    }
                    entities.SaveChanges();
                    //string address = Request.Url.ToString();
                    string numberVoucher = string.Format("{0}/{1}/VKK/{2}", number, branch.ShortName, getThisYear);
                    saveHistory(numberVoucher);
                    
                    Session.Remove(pettyCashSessionName);
                    //var newAdded = entities.PettyCashes.Select(a => a.Id).Max();

                    return RedirectToAction("Confirmation", new { id = newPettyCash.Id });
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing PettyCashesController.Create[Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GetAccountCodeSelectList(string text)
        {
            var branch = GetCurrentUserBranchId();
            var branchAccount = entities.GetAccountBranch(branch.Code, text).Take(20);
            
               var  accounts = (from a in branchAccount where a.AccountCode != null
                                select new
                                {
                                    AccountCode = a.AccountCode
                                }).ToList();

            
            return new JsonResult { Data = new SelectList(accounts, "AccountCode", "AccountCode") };
        }

        [GridAction]
        public ActionResult _SelectDetail(Guid? id)
        {
            var branch = GetCurrentUserBranchId();
            var model = (List<PettyCashesDetailViewModel>)Session[pettyCashSessionName];

            if (model == null)
            {
                model = (from data in entities.PettyCashDetails
                         where data.PettyCashId == id
                         select new PettyCashesDetailViewModel
                         {
                             PettyCashId = data.PettyCashId,
                             AccountId = data.AccountId,
                             AccountCode = data.Account.Code + "." + data.Branch.Code + " - " + data.Account.Name + " " + data.Branch.Name, //(data.Account.Code.Length.Equals(8)) ? data.Account.Code + "." + data.Branch.Code + " - " + data.Account.Name +" "+ data.Branch.Name : data.Account.Code + " - " + data.Account.Name,
                             AccountName = data.Account.Name,
                             Debet = data.Debet,
                             BranchId = data.BranchId,
                             MasterBranch = branch.Id 
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[pettyCashSessionName] = model;
            }

            return View(new GridModel<PettyCashesDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _InsertDetail()
        {
            var model = (List<PettyCashesDetailViewModel>)Session[pettyCashSessionName];
            PettyCashesDetailViewModel postmodel = new PettyCashesDetailViewModel();
            var branch = GetCurrentUserBranchId();

            if (model == null)
            {
                model = new List<PettyCashesDetailViewModel>();
            }

            TryUpdateModel(postmodel);
            {
                string[] words = postmodel.AccountCode.Split('-');
                var code = words[0];

                char[] reverseCode = code.Trim().ToCharArray();
                Array.Reverse(reverseCode);

                string getBranchCode = new string(reverseCode.Take(2).Reverse().ToArray()); //code.Substring(9, 2);
                string accountCode = new string(reverseCode.Skip(3).Reverse().ToArray()); //code.Substring(0, 8);
                var account = from c in entities.Accounts
                              select c;

                var getBranches = entities.Branches.Where(a => a.Code == getBranchCode).FirstOrDefault();

                if (getBranches != null)
                {
                    account = account.Where(a => a.Code == accountCode);
                }
                else
                {
                    account = account.Where(a => a.Code == code);
                }

                
                if (account != null)
                {
                    //if (account.FirstOrDefault().Code.Length.Equals(8))
                    //{
                        Branch getBranch = entities.Branches.Where(a => a.Code == getBranchCode).FirstOrDefault();
                        int AccountId = account.FirstOrDefault().Id;
                        BeginningBalanceBranch getFirstBalance = entities.BeginningBalanceBranches.Where(a => a.AccountId == AccountId && a.BranchId == getBranch.Id).Select(s => s).FirstOrDefault();

                        //if (account.FirstOrDefault().Code.StartsWith("8") || account.FirstOrDefault().Code.StartsWith("9"))
                        //{
                        //    postmodel.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                        //    postmodel.AccountId = account.FirstOrDefault().Id;
                        //    postmodel.BranchId = getBranch.Id;
                        //    postmodel.MasterBranch = branch.Id;

                        //    var error1 = ModelState.ToList();
                        //    if (error1.Count > 0)
                        //    {
                        //        foreach (var a in error1)
                        //        {
                        //            ModelState.Remove(a);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                            if (getFirstBalance != null)
                            {
                                if (getFirstBalance.Debet == null && getFirstBalance.Kredit == null)
                                {
                                    ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                    return View(new GridModel<PettyCashesDetailViewModel>(model));
                                }
                                else
                                {
                                    postmodel.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                                    postmodel.AccountId = account.FirstOrDefault().Id;
                                    postmodel.BranchId = getBranch.Id;
                                    postmodel.MasterBranch = branch.Id;

                                    var error2 = ModelState.ToList();
                                    if (error2.Count > 0)
                                    {
                                        foreach (var a in error2)
                                        {
                                            ModelState.Remove(a);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                return View(new GridModel<PettyCashesDetailViewModel>(model));
                            }
                       // }
                        
                    //}
                    //else {
                    //    var getUserBranch = GetCurrentUserBranchId();
                    //    postmodel.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                    //    postmodel.AccountId = account.FirstOrDefault().Id;
                    //    postmodel.BranchId = getUserBranch.Id;
                    //    postmodel.MasterBranch = branch.Id;

                    //    var error3 = ModelState.ToList();
                    //    if (error3.Count > 0)
                    //    {
                    //        foreach (var a in error3)
                    //        {
                    //            ModelState.Remove(a);
                    //        }
                    //    }
                    //}
                    
                    int maxId = 0;

                    if (model.Count > 0)
                        maxId = model.Max(m => m.Id);

                    postmodel.Id = maxId + 1;

                    model.Insert(0, postmodel);

                    Session[pettyCashSessionName] = model;
                }
                else
                {
                    ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                }
            }

            return View(new GridModel<PettyCashesDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<PettyCashesDetailViewModel>)Session[pettyCashSessionName];
            PettyCashesDetailViewModel postmodel = new PettyCashesDetailViewModel();
            var branch = GetCurrentUserBranchId();
            if (TryUpdateModel(postmodel))
            {
                var item = model.Where(m => m.Id == postmodel.Id).FirstOrDefault();
                
                if (item != null)
                {

                    string[] words = postmodel.AccountCode.Split('-');
                    string code = words[0];

                    char[] reverseCode = code.Trim().ToCharArray();
                    Array.Reverse(reverseCode);

                    string getBranchCode = new string(reverseCode.Take(2).Reverse().ToArray()); //code.Substring(9, 2);
                    string accountCode = new string(reverseCode.Skip(3).Reverse().ToArray()); //code.Substring(0, 8);
                    var account = from c in entities.Accounts
                                  select c;

                    var getBranches = entities.Branches.Where(a => a.Code == getBranchCode).FirstOrDefault();

                    if (getBranches != null)
                    {
                        account = account.Where(a => a.Code == accountCode);
                    }
                    else
                    {
                        account = account.Where(a => a.Code == code);
                    }



                    if (account != null)
                    {
                        //if (account.FirstOrDefault().Code.Length.Equals(8))
                        //{
                            var getBranch = entities.Branches.Where(a => a.Code == getBranchCode).FirstOrDefault();

                            var getFirstBalance = entities.BeginningBalanceBranches.Where(a => a.AccountId == account.FirstOrDefault().Id && a.BranchId == getBranch.Id).FirstOrDefault();

                            //if (account.FirstOrDefault().Code.StartsWith("8") || account.FirstOrDefault().Code.StartsWith("9"))
                            //{
                            //    item.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                            //    item.AccountId = account.FirstOrDefault().Id;
                            //    item.Debet = postmodel.Debet;
                            //    item.BranchId = getBranch.Id;
                            //    item.MasterBranch = branch.Id;
                            //}
                            //else
                            //{
                                if (getFirstBalance != null)
                                {
                                    if (getFirstBalance.Debet == null && getFirstBalance.Kredit == null)
                                    {
                                        ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                        return View(new GridModel<PettyCashesDetailViewModel>(model));
                                    }
                                    else
                                    {
                                        item.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                                        item.AccountId = account.FirstOrDefault().Id;
                                        item.Debet = postmodel.Debet;
                                        item.BranchId = getBranch.Id;
                                        item.MasterBranch = branch.Id;
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                    return View(new GridModel<PettyCashesDetailViewModel>(model));
                                }
                            }
                            
                        //}
                        //else
                        //{
                        //    var getUserBranch = GetCurrentUserBranchId();
                        //    item.AccountId = account.FirstOrDefault().Id;
                        //    item.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                        //    item.Debet = postmodel.Debet;
                        //    item.BranchId = getUserBranch.Id;
                        //    item.MasterBranch = branch.Id;
                        //}
                    }
                    else
                    {
                        ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                    }
                }
           // }

            return View(new GridModel<PettyCashesDetailViewModel>(model));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetail()
        {
            IList<PettyCashesDetailViewModel> model = (IList<PettyCashesDetailViewModel>)Session[pettyCashSessionName];
            var detail = new PettyCashesDetailViewModel();

            if (!TryUpdateModel<PettyCashesDetailViewModel>(detail))
            {
                // check for member with [Required] attribute. 
                // If it found, clear the error, otherwise, the delete operation will not perform.

                var accountCodeError = ModelState.Keys.Where(e => e == "AccountCode");

                if (accountCodeError != null)
                    ModelState["AccountCode"].Errors.Clear();
            }

            if (detail != null)
            {
                var item = model.Where(c => c.Id == detail.Id).FirstOrDefault();

                if (item != null)
                    model.Remove(item);
            }

            return View(new GridModel<PettyCashesDetailViewModel>(model));
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            Session.Remove(pettyCashSessionName);

            var model = (from d in entities.PettyCashes
                         where d.Id == id
                         select new PettyCashesViewModel
                         {
                             Number = d.Number,
                             TransactionDate = d.TransactionDate,
                             PaidTo = d.PaidTo,
                             Necessity = d.Necessity,
                             ReceiverName = d.ReceiverName,
                             BranchId = d.BranchId,
                             Amount = d.Credit,
                             AccountCodePettyCash = d.AccountCode,
                         }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PettyCashesViewModel model)
        {
            var branch = GetCurrentUserBranchId();
            var pettyCashDetailSessionlist = (List<PettyCashesDetailViewModel>)(Session[pettyCashSessionName]);

            if (pettyCashDetailSessionlist == null || pettyCashDetailSessionlist.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail pembayaran.");

            //var branch = GetCurrentUserBranchId();
            string transactionDateString = Request.Form["TransactionDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);

            int getThisYear = DateTime.Now.Year;
            model.TransactionDate = transactionDate;
            
            var dttransaction = new DateTime(transactionDate.Year, transactionDate.Month, 1);
            var transmonth = DateTime.Now.Month;
            var transkyear = DateTime.Now.Year;
            var transday = DateTime.Now.Day;
            var transvoucher = new DateTime(transkyear, transmonth, 1);
            int resultCek = DateTime.Compare(dttransaction, transvoucher);
            var errorItem = ModelState.FirstOrDefault(m => m.Key == "TransactionDate");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (resultCek != 0)
            {
                var nextmonth = transactionDate.Month + 1;
                if (nextmonth == transmonth && transactionDate.Year == transkyear)
                {
                    if (transday > 4)
                    {
                        ModelState.AddModelError("TransactionDate", "Transaksi Voucher "
                                                   + string.Format("{0:y}", transactionDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("TransactionDate", "Transaksi Voucher Bulan "
                                                + string.Format("{0:y}", transactionDate) + " telah ditutup, Hubungi Administrator!");
                }
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.PettyCashes
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    
                    if (newDetail == null)
                    {
                        return RedirectToAction("Index");
                    }


                    newDetail.Number = model.Number;
                    newDetail.TransactionDate = model.TransactionDate;
                    newDetail.PaidTo = model.PaidTo;
                    newDetail.Necessity = model.Necessity;
                    newDetail.ReceiverName = model.ReceiverName;
                    newDetail.BranchId = model.BranchId ;
                    newDetail.Credit = model.Amount;
                    newDetail.AccountCode = model.AccountCodePettyCash;

                    
                    // delete old data first, before inserting new detail
                    foreach (var item in entities.PettyCashDetails.Where(p => p.PettyCashId == newDetail.Id))
                    {
                        entities.PettyCashDetails.DeleteObject(item);
                    }

                    
                    // insert new detail
                    foreach (var items in pettyCashDetailSessionlist)
                    {
                        var detail = new PettyCashDetail
                        {
                            PettyCashId = newDetail.Id,
                            AccountId = items.AccountId,
                            Debet = items.Debet,
                            BranchId = items.BranchId,
                            MasterBranch = branch.Id
                        };

                        
                        entities.PettyCashDetails.AddObject(detail);
                    }

                    entities.SaveChanges();
                    saveHistory(model.Number);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing PettyCashesController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var deleteItem = (from c in entities.PettyCashes
                                  where c.Id == id
                                  select new PettyCashesViewModel
                                  {
                                      Number = c.Number,
                                      TransactionDate = c.TransactionDate,
                                      PaidTo = c.PaidTo,
                                      Necessity = c.Necessity,
                                      ReceiverName = c.ReceiverName,
                                      BranchId = c.BranchId,
                                      Amount = c.Credit,
                                      AccountCodePettyCash = c.AccountCode
                                  }).FirstOrDefault();

                if (deleteItem == null)
                {
                    return RedirectToAction("Index");
                }

                return View(deleteItem);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing PettyCashesController.Delete [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(PettyCashesViewModel model)
        {
            try
            {
                var deleteItem = (from c in entities.PettyCashes
                                  where c.Id == model.Id
                                  select c).FirstOrDefault();

                
                if (deleteItem != null)
                {
                    foreach (var item in entities.PettyCashDetails.Where(p => p.PettyCashId == deleteItem.Id))
                    {
                        entities.PettyCashDetails.DeleteObject(item);
                    }

                    
                    entities.PettyCashes.DeleteObject(deleteItem);
                    entities.SaveChanges();
                    saveHistory(deleteItem.Number);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing PettyCashesController.Delete [Post].", ex);
                SetErrorMessageViewData("Data cannot be deleted.");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Confirmation(Guid id)
        {
            ViewData["Id"] = id;
            try
            {
                var confirmation = (from c in entities.PettyCashes
                                  where c.Id == id
                                  select new PettyCashesViewModel
                                  {
                                      Number = c.Number,
                                      TransactionDate = c.TransactionDate,
                                      PaidTo = c.PaidTo,
                                      Necessity = c.Necessity,
                                      ReceiverName = c.ReceiverName,
                                      BranchId = c.BranchId,
                                      Amount = c.Credit,
                                      AccountCodePettyCash = c.AccountCode
                                  }).FirstOrDefault();

                if (confirmation == null)
                {
                    return RedirectToAction("Index");
                }

                return View(confirmation);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing PettyCashesController.Confirmation [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Confirmation(PettyCashesViewModel model)
        {
            try
            {
                var deleteItem = (from c in entities.PettyCashes
                                  where c.Id == model.Id
                                  select c).FirstOrDefault();

                if (deleteItem != null)
                {
                    return RedirectToAction("Edit", new { id = deleteItem.Id});
                }
                
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing PettyCashesController.Confirmation [Post].", ex);
                return RedirectToAction("Index");
            }
            
        }

        //public JsonResult GetNumber()
        //{            
        //    var branch = GetCurrentUserBranchId();
        //    int getThisYear = DateTime.Now.Year;
        //    var nextNumber = 0;
        //    var recordCount = (from pty in entities.PettyCashRecordCounters
        //                       where pty.BranchId == branch.Id && pty.Year == getThisYear
        //                       select pty).FirstOrDefault();
        //    if (recordCount != null)
        //        nextNumber = recordCount.LastCounter;
        //    string number = string.Format("{0:0000}", nextNumber + 1);
        //    return Json(number, JsonRequestBehavior.AllowGet);
        //}
    }
}
