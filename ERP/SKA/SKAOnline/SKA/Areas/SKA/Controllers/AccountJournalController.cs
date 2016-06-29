using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.SKA.Models.ViewModels;
using System.Globalization;
using SKA.Areas.Setting.Models.ViewModels;

namespace SKA.Areas.SKA.Controllers
{
    public class AccountJournalController : BaseController
    {
        private SKAEntities entities;
        private string accountJournalSessionName = "AccountJournalSession";

        public AccountJournalController()
        {
            entities = new SKAEntities();
        }

        [GridAction]
        public ActionResult Getlist(string searchValue)
        {
            var model = from m in entities.AccountJournals
                        select new AccountJournalViewModel
                        {
                            Id = m.Id,
                            DRDNumber = m.DRDNumber,
                            DocumentDate = m.DocumentDate,
                            Description = m.Description,
                            WaterBill = m.WaterBill,
                            NonWaterBill = m.NonWaterBill,
                            ClosingDate = entities.Closings.Select(a => a.ClosingDate).FirstOrDefault()
                        };
           
            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.DRDNumber.Contains(searchValue)
                    || a.Description.Contains(searchValue));
            }
            return View(new GridModel<AccountJournalViewModel>(model));
        }

        [HttpPost]
        public ActionResult GetAccountCodeSelectList(string text)
        {
            var branch = GetCurrentUserBranchId();

            var branchAccount = entities.ProcJournalAccount("Jurnal Rekening", text);
            var accounts = (from a in branchAccount
                            select a.AccountCode
                            ).ToList();

            return new JsonResult { Data = accounts };
            //var branch = GetCurrentUserBranchId();

            //var account1 = from c in entities.JournalAccounts
            //               where c.JournalType.JournalName == "Jurnal Rekening" && c.Account.Code.Length.Equals(11)
            //               select new
            //               {
            //                   Id = c.Id,
            //                   Fullname = c.Account.Code + " - " + c.Account.Name
            //               };


            //var account2 = from c in entities.JournalAccounts
            //               where c.JournalType.JournalName == "Jurnal Rekening" && c.Account.Code.Length.Equals(8)
            //               select new
            //               {
            //                   Id = c.Id,
            //                   Fullname = c.Account.Code + "." + branch.Code + " - " + c.Account.Name + " " + branch.Name
            //               };

            //var accounts = account2.Union(account1);
            //if (text != null)
            //{
            //    accounts = accounts.Where(a => a.Fullname.Contains(text));
            //}

            //return new JsonResult { Data = new SelectList(accounts.ToList(), "Id", "Fullname") };
        }

        [GridAction]
        public ActionResult _SelectDetail(Guid? id)
        {
            var model = (List<AccountJournalDetailViewModel>)Session[accountJournalSessionName];

            if (model == null)
            {
                var branch = GetCurrentUserBranchId();
                model = (from data in entities.AccountJournalDetails
                         where data.AccountJournalId == id
                         select new AccountJournalDetailViewModel
                         {
                             AccountJournalId = data.AccountJournalId,
                             AccountId = data.AccountId,
                             AccountCode = (data.Account.Code.Length.Equals(8)) ? data.Account.Code + "." + data.Branch.Code + " - " + data.Account.Name + " " + data.Branch.Name : data.Account.Code + " - " + data.Account.Name,
                             AccountName = data.Account.Name,
                             Debet = data.Debet,
                             Kredit = data.Kredit,
                             BranchId = data.BranchId, 
                             MasterBranch = data.MasterBranch 
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[accountJournalSessionName] = model;
            }

            return View(new GridModel<AccountJournalDetailViewModel>(model.OrderByDescending(p=>p.Debet)));
        }

        [GridAction()]
        public ActionResult _InsertDetail()
        {
            var model = (List<AccountJournalDetailViewModel>)Session[accountJournalSessionName];
            AccountJournalDetailViewModel postmodel = new AccountJournalDetailViewModel();
           var branch = GetCurrentUserBranchId();
           if (model == null)
           {
               model = new List<AccountJournalDetailViewModel>();
           }
           bool cek = TryUpdateModel(postmodel);
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
                    if (account.FirstOrDefault().Code.Length.Equals(8))
                    {
                        var getBranch = entities.Branches.Where(a => a.Code == getBranchCode).FirstOrDefault();

                        var getFirstBalance = entities.BeginningBalanceBranches.Where(a => a.AccountId == account.FirstOrDefault().Id && a.BranchId == getBranch.Id).FirstOrDefault();

                        if (account.FirstOrDefault().Code.StartsWith("8") || account.FirstOrDefault().Code.StartsWith("9"))
                        {
                            postmodel.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                            postmodel.AccountId = account.FirstOrDefault().Id;
                            postmodel.BranchId = getBranch.Id;
                            postmodel.MasterBranch = branch.Id;

                            var error1 = ModelState.ToList();
                            if (error1.Count > 0)
                            {
                                foreach (var a in error1)
                                {
                                    ModelState.Remove(a);
                                }
                            }
                        }
                        else
                        {
                            if (getFirstBalance != null)
                            {
                                if (getFirstBalance.Debet == null && getFirstBalance.Kredit == null)
                                {
                                    ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                    return View(new GridModel<AccountJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                                }
                                else
                                {
                                    //var getUserBranch = GetCurrentUserBranchId();
                                    postmodel.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                                    postmodel.AccountId = account.FirstOrDefault().Id;
                                    postmodel.BranchId = getBranch.Id;
                                    postmodel.MasterBranch  = branch.Id;

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
                                return View(new GridModel<AccountJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                            }
                        }
                    }
                    else
                    {
                        postmodel.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                        postmodel.AccountId = account.FirstOrDefault().Id;

                        var error3 = ModelState.ToList();
                        if (error3.Count > 0)
                        {
                            foreach (var a in error3)
                            {
                                ModelState.Remove(a);
                            }
                        }
                    }

                    int maxId = 0;
                    if (model.Count > 0)
                        maxId = model.Max(m => m.Id);

                    postmodel.Id = maxId + 1;

                    model.Insert(0, postmodel);

                    Session[accountJournalSessionName] = model;
                }
                else
                {
                    ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                }
            }
            return View(new GridModel<AccountJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }


        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<AccountJournalDetailViewModel>)Session[accountJournalSessionName];
            AccountJournalDetailViewModel postmodel = new AccountJournalDetailViewModel();

            var branch = GetCurrentUserBranchId();

            if (TryUpdateModel(postmodel))
            {
                var item = model.Where(m => m.Id == postmodel.Id).FirstOrDefault();

                if (item != null)
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
                        if (account.FirstOrDefault().Code.Length.Equals(8))
                        {
                            var getBranch = entities.Branches.Where(a => a.Code == getBranchCode).FirstOrDefault();

                            var getFirstBalance = entities.BeginningBalanceBranches.Where(a => a.AccountId == account.FirstOrDefault().Id && a.BranchId == getBranch.Id).FirstOrDefault();

                            if (account.FirstOrDefault().Code.StartsWith("8") || account.FirstOrDefault().Code.StartsWith("9"))
                            {
                                item.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                                item.AccountId = account.FirstOrDefault().Id;
                                item.Debet = postmodel.Debet;
                                item.Kredit = postmodel.Kredit;
                                item.BranchId = getBranch.Id;
                                item.MasterBranch = branch.Id;
                            }
                            else
                            {
                                if (getFirstBalance != null)
                                {
                                    if (getFirstBalance.Debet == null && getFirstBalance.Kredit == null)
                                    {
                                        ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                        return View(new GridModel<AccountJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                                    }
                                    else
                                    {
                                        item.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                                        item.AccountId = account.FirstOrDefault().Id;
                                        item.Debet = postmodel.Debet;
                                        item.Kredit = postmodel.Kredit;
                                        item.BranchId = getBranch.Id;
                                        item.MasterBranch = branch.Id;
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                    return View(new GridModel<AccountJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                                }
                            }
                            
                        }
                        else
                        {
                            var getUserBranch = GetCurrentUserBranchId();
                            item.AccountId = account.FirstOrDefault().Id;
                            item.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                            item.Debet = postmodel.Debet;
                            item.Kredit = postmodel.Kredit;
                            item.BranchId = getUserBranch.Id;
                            item.MasterBranch = branch.Id;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                    }
                }
            }

            return View(new GridModel<AccountJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetail()
        {
            IList<AccountJournalDetailViewModel> model = (IList<AccountJournalDetailViewModel>)Session[accountJournalSessionName];
            var detail = new AccountJournalDetailViewModel();

            if (!TryUpdateModel<AccountJournalDetailViewModel>(detail))
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

            return View(new GridModel<AccountJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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


        [HttpGet]
        public ActionResult Create()
        {
            Session.Remove(accountJournalSessionName);

            var model = new AccountJournalViewModel();
            model.DocumentDate = DateTime.Now;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AccountJournalViewModel model)
        {
            BranchViewModel branch;
            branch = GetCurrentUserBranchId();
            var accountJournalDetailSessionlist = (List<AccountJournalDetailViewModel>)(Session[accountJournalSessionName]);

            if (accountJournalDetailSessionlist == null || accountJournalDetailSessionlist.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail pembayaran.");

            string documentDateString = Request.Form["DocumentDate"];
            DateTime documentDate;
            DateTime.TryParse(documentDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out documentDate);

            int getThisYear = DateTime.Now.Year;
            model.DocumentDate = documentDate;
            var dttransaction = new DateTime(documentDate.Year, documentDate.Month, 1);
            var transmonth = DateTime.Now.Month;
            var transkyear = DateTime.Now.Year;
            var transday = DateTime.Now.Day;
            var transvoucher = new DateTime(transkyear, transmonth, 1);
            int resultCek = DateTime.Compare(dttransaction, transvoucher);

            var errorItem = ModelState.FirstOrDefault(m => m.Key == "DocumentDate");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (resultCek != 0)
            {
                var nextmonth = documentDate.Month + 1;
                if (nextmonth == transmonth && documentDate.Year == transkyear)
                {
                    if (transday > 4)
                    {
                        ModelState.AddModelError("documentDate", "Journal Rekening "
                                                   + string.Format("{0:y}", documentDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("documentDate", "Journal Rekening "
                                                + string.Format("{0:y}", documentDate) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.AmountDebet == model.AmountKredit)
                {
                    try
                    {
                        var dataPrev = (from a in entities.AccountJournals
                                        where a.DRDNumber == model.DRDNumber
                                        select a).FirstOrDefault();

                        if (dataPrev == null)
                        {
                            var newData = new AccountJournal();
                            newData.Id = Guid.NewGuid();
                            newData.DRDNumber = model.DRDNumber;
                            newData.DocumentDate = model.DocumentDate;
                            newData.Description = model.Description;
                            newData.WaterBill = model.WaterBill;
                            newData.NonWaterBill = model.NonWaterBill;
                            newData.BranchId = branch.Id;

                            entities.AccountJournals.AddObject(newData);

                            foreach (var item in accountJournalDetailSessionlist)
                            {
                                var detail = new AccountJournalDetail
                                {
                                    AccountJournalId = newData.Id,
                                    AccountId = item.AccountId,
                                    Debet = item.Debet,
                                    Kredit = item.Kredit,
                                    BranchId = item.BranchId,
                                    MasterBranch = item.MasterBranch 
                                };

                                entities.AccountJournalDetails.AddObject(detail);
                            }
                            entities.SaveChanges();
                            saveHistory(model.DRDNumber);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("DRDNumber","Nomor sudah ada, silahkan isi kembali nomor yang berbeda.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error while executing AccountJournalController.Create [Post].", ex);
                        SetErrorMessageViewData("Data cannot be created.");
                    }
                }
                else {
                    SetErrorMessageViewData("Jumlah debet dan kredit tidak sama.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            Session.Remove(accountJournalSessionName);

            var model = (from m in entities.AccountJournals
                         where m.Id == id
                         select new AccountJournalViewModel
                         {
                             DRDNumber = m.DRDNumber,
                             Description = m.Description,
                             DocumentDate = m.DocumentDate,
                             WaterBill = m.WaterBill,
                             NonWaterBill = m.NonWaterBill
                         }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AccountJournalViewModel model)
        {
            var accountJournalDetailSessionlist = (List<AccountJournalDetailViewModel>)(Session[accountJournalSessionName]);

            if (accountJournalDetailSessionlist == null || accountJournalDetailSessionlist.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail jurnal.");

            string documentDateString = Request.Form["DocumentDate"];
            DateTime documentDate;
            DateTime.TryParse(documentDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out documentDate);

            int getThisYear = DateTime.Now.Year;
            model.DocumentDate = documentDate;

            var dttransaction = new DateTime(documentDate.Year, documentDate.Month, 1);
            var transmonth = DateTime.Now.Month;
            var transkyear = DateTime.Now.Year;
            var transday = DateTime.Now.Day;
            var transvoucher = new DateTime(transkyear, transmonth, 1);
            int resultCek = DateTime.Compare(dttransaction, transvoucher);

            var errorItem = ModelState.FirstOrDefault(m => m.Key == "DocumentDate");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (resultCek != 0)
            {
                var nextmonth = documentDate.Month + 1;
                if (nextmonth == transmonth && documentDate.Year == transkyear)
                {
                    if (transday > 4)
                    {
                        ModelState.AddModelError("documentDate", "Journal Rekening "
                                                   + string.Format("{0:y}", documentDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("documentDate", "Journal Rekening "
                                                + string.Format("{0:y}", documentDate) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.AmountDebet == model.AmountKredit)
                {
                    try
                    {
                        var dataPrev = (from a in entities.AccountJournals
                                        where a.DRDNumber == model.DRDNumber && a.Id != model.Id
                                        select a).FirstOrDefault();

                        if (dataPrev == null)
                        {
                            var accountJournal = (from aj in entities.AccountJournals
                                                  where aj.Id == model.Id
                                                  select aj).FirstOrDefault();
                            accountJournal.Id = model.Id;
                            accountJournal.NonWaterBill = model.NonWaterBill;
                            accountJournal.WaterBill = model.WaterBill;
                            accountJournal.DocumentDate = model.DocumentDate;
                            accountJournal.Description = model.Description;
                            accountJournal.DRDNumber = model.DRDNumber;

                            // delete old data first, before inserting new detail
                            foreach (var item in entities.AccountJournalDetails.Where(p => p.AccountJournalId == accountJournal.Id))
                            {
                                entities.AccountJournalDetails.DeleteObject(item);
                            }

                            // insert new detail
                            foreach (var item in accountJournalDetailSessionlist)
                            {
                                var detail = new AccountJournalDetail
                                {
                                    AccountJournalId = accountJournal.Id,
                                    AccountId = item.AccountId,
                                    Debet = item.Debet,
                                    Kredit = item.Kredit,
                                    BranchId = item.BranchId,
                                    MasterBranch = item.MasterBranch 
                                };

                                entities.AccountJournalDetails.AddObject(detail);
                            }

                            entities.SaveChanges();
                            saveHistory(model.DRDNumber);
                            return RedirectToAction("Index");
                        }
                        else {
                            ModelState.AddModelError("DRDNumber", "Nomor sudah ada, silahkan isi dengan nomor yang berbeda");
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error while executing AccountJournalController.Edit[Post].", ex);
                        SetErrorMessageViewData("Data tidak dapat diubah.");
                    }
                }
                else {
                    SetErrorMessageViewData("Jumlah debet dan kredit tidak sama.");
                    return View(model);
                }
                
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var model = (from m in entities.AccountJournals
                             where m.Id == id
                             select new AccountJournalViewModel
                             {
                                 DRDNumber = m.DRDNumber,
                                 DocumentDate = m.DocumentDate,
                                 Description = m.Description,
                                 WaterBill = m.WaterBill,
                                 NonWaterBill = m.NonWaterBill
                             }).FirstOrDefault();
                if (model == null)
                {
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing AccountJournalController.Delete [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(AccountJournalViewModel model)
        {
            try
            {
                var deleteItem = (from di in entities.AccountJournals
                                  where di.Id == model.Id
                                  select di).FirstOrDefault();

                if (deleteItem != null)
                {
                    foreach (var item in entities.AccountJournalDetails.Where(p => p.AccountJournalId == deleteItem.Id))
                    {
                        entities.AccountJournalDetails.DeleteObject(item);
                    }

                    entities.AccountJournals.DeleteObject(deleteItem);
                    entities.SaveChanges();
                    saveHistory(deleteItem.DRDNumber);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing AccountJournalController.Delete [Post].", ex);
                SetErrorMessageViewData("Data cannot be deleted.");
            }
            return RedirectToAction("Index");

        }

        public ActionResult detail(Guid id)
        {
            Session.Remove(accountJournalSessionName);

            var model = (from m in entities.AccountJournals
                         where m.Id == id
                         select new AccountJournalViewModel
                         {
                             DRDNumber = m.DRDNumber,
                             DocumentDate = m.DocumentDate,
                             Description = m.Description,
                             WaterBill = m.WaterBill,
                             NonWaterBill = m.NonWaterBill
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult CheckDebetKredit(string AccountCode) {
            var code = AccountCode.Substring(0, 8);

            var getCode = entities.Accounts.Where(a => a.Code == code).FirstOrDefault();

            var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 2).FirstOrDefault();

            var value = getCodeStatus.AccountSide;
            return new JsonResult { Data = value};
        }

        [HttpPost]
        public ActionResult CheckNumber(string DRDNumber, Guid Id)
        {
            var value = "";
            if (Id == null)
            {
                var existingNumber = (from c in entities.AccountJournals
                                      where c.DRDNumber == DRDNumber
                                      select c.DRDNumber).FirstOrDefault();



                if (existingNumber != null)
                {
                    value = "Nomor yang Anda masukkan sudah ada, silahkan isi dengan nomor yang lain.";
                }
            }
            else
            {
                var existingNumberEdit = (from c in entities.AccountJournals
                                          where c.DRDNumber == DRDNumber && c.Id != Id
                                          select c.DRDNumber).FirstOrDefault();

                if (existingNumberEdit != null)
                {
                    value = "Nomor yang Anda masukkan sudah ada, silahkan isi dengan nomor yang lain.";
                }
            }
            //var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 2).FirstOrDefault();

            //var value = getCodeStatus.AccountSide;
            return new JsonResult { Data = value };
        }

        //[HttpPost]
        //public ActionResult CheckNumberEdit(string DRDNumber)
        //{
        //    var existingNumberEdit = (from c in entities.AccountJournals
        //                          where c.DRDNumber == DRDNumber 
        //                          select c.DRDNumber).FirstOrDefault();

        //    var value = "";

        //    if (existingNumberEdit != null)
        //    {
        //        value = "Nomor yang Anda masukkan sudah ada, silahkan isi dengan nomor yang lain.";
        //    }
        //    //var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 2).FirstOrDefault();

        //    //var value = getCodeStatus.AccountSide;
        //    return new JsonResult { Data = value };
        //}
    }
}
