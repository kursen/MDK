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
    public class CashReceiptJournalController : BaseController
    {
        private SKAEntities entities;

        private string CashReceiptJournalSessiomName = "CashReceiptJournalSession";

        public CashReceiptJournalController()
        {
            entities = new SKAEntities();
        }

        [GridAction]
        public ActionResult Getlist(string searchValue)
        {
            var model = from m in entities.CashReceiptJournals
                        select new CashReceiptJournalViewModel
                        {
                            Id = m.Id,
                            EvidenceNumber = m.EvidenceNumber,
                            DocumentDate = m.DocumentDate,
                            Description = m.Description,
                            ClosingDate = entities.Closings.Select(a => a.ClosingDate).FirstOrDefault()
                        };

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.EvidenceNumber.Contains(searchValue)
                    || a.Description.Contains(searchValue));
            }

            return View(new GridModel<CashReceiptJournalViewModel>(model));
        }

        [HttpPost]
        public ActionResult GetAccountCodeSelectList(string text)
        {
            var branchAccount = entities.ProcJournalAccount("Jurnal Penerimaan Kas", text);
            var accounts = (from a in branchAccount
                            select a.AccountCode
                            ).ToList();

            return new JsonResult { Data = accounts };
        }

        [GridAction]
        public ActionResult _SelectDetail(Guid? id)
        {
            var model = (List<CashReceiptJournalDetailViewModel>)Session[CashReceiptJournalSessiomName];
            //var branch = GetCurrentUserBranchId();

            if (model == null)
            {
                model = (from data in entities.CashReceiptJournalDetails
                         where data.CashReceiptJournalId == id
                         select new CashReceiptJournalDetailViewModel
                         {
                             CashReceiptJournalId = data.CashReceiptJournalId,
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

                Session[CashReceiptJournalSessiomName] = model;
            }

            return View(new GridModel<CashReceiptJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [GridAction()]
        public ActionResult _InsertDetail()
        {
            var model = (List<CashReceiptJournalDetailViewModel>)Session[CashReceiptJournalSessiomName];
            CashReceiptJournalDetailViewModel postmodel = new CashReceiptJournalDetailViewModel();
            var branch = GetCurrentUserBranchId();
            if (model == null)
                model = new List<CashReceiptJournalDetailViewModel>();

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
                                    return View(new GridModel<CashReceiptJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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
                                return View(new GridModel<CashReceiptJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                            }
                        }
                    }
                    else
                    {
                        var getUserBranch = GetCurrentUserBranchId();
                        postmodel.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                        postmodel.AccountId = account.FirstOrDefault().Id;
                        postmodel.BranchId = getUserBranch.Id;
                        postmodel.MasterBranch = branch.Id;

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

                    Session[CashReceiptJournalSessiomName] = model;
                }
                else
                {
                    ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                }
            }

            return View(new GridModel<CashReceiptJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<CashReceiptJournalDetailViewModel>)Session[CashReceiptJournalSessiomName];
            CashReceiptJournalDetailViewModel postmodel = new CashReceiptJournalDetailViewModel();

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

            return View(new GridModel<CashReceiptJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetail()
        {
            IList<CashReceiptJournalDetailViewModel> model = (IList<CashReceiptJournalDetailViewModel>)Session[CashReceiptJournalSessiomName];
            var detail = new CashReceiptJournalDetailViewModel();

            if (!TryUpdateModel<CashReceiptJournalDetailViewModel>(detail))
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

            return View(new GridModel<CashReceiptJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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
            Session.Remove(CashReceiptJournalSessiomName);
            var model = new CashReceiptJournalViewModel();
            model.DocumentDate = DateTime.Now;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CashReceiptJournalViewModel model)
        {
            var cashJournalDetailSessionlist = (List<CashReceiptJournalDetailViewModel>)(Session[CashReceiptJournalSessiomName]);
            BranchViewModel branch;
            branch = GetCurrentUserBranchId();

            if (cashJournalDetailSessionlist == null || cashJournalDetailSessionlist.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail pembayaran.");

            string documentDateString = Request.Form["DocumentDate"];
            DateTime documentDate;
            DateTime.TryParse(documentDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out documentDate);

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
                    if (transday >= 4)
                    {
                        ModelState.AddModelError("documentDate", "Jurnal Penerimaan Kas "
                                                   + string.Format("{0:y}", documentDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("documentDate", "Jurnal Penerimaan Kas "
                                                + string.Format("{0:y}", documentDate) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.AmountDebet == model.AmountKredit)
                {
                    try
                    {
                        var dataPrev = (from a in entities.CashReceiptJournals
                                        where a.EvidenceNumber == model.EvidenceNumber
                                        select a).FirstOrDefault();

                        if (dataPrev == null)
                        {
                            var newData = new CashReceiptJournal();
                            newData.Id = Guid.NewGuid();
                            newData.EvidenceNumber = model.EvidenceNumber;
                            newData.DocumentDate = model.DocumentDate;
                            newData.Description = model.Description;
                            newData.BranchId = branch.Id;

                            entities.CashReceiptJournals.AddObject(newData);

                            foreach (var item in cashJournalDetailSessionlist)
                            {
                                var detail = new CashReceiptJournalDetail
                                {
                                    CashReceiptJournalId = newData.Id,
                                    AccountId = item.AccountId,
                                    Debet = item.Debet,
                                    Kredit = item.Kredit,
                                    BranchId = item.BranchId,
                                    MasterBranch = item.MasterBranch
                                };

                                entities.CashReceiptJournalDetails.AddObject(detail);
                            }
                            entities.SaveChanges();
                            saveHistory(model.EvidenceNumber);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("EvidenceNumber", "Nomor sudah ada, silahkan isi data dengan nomor yang berbeda");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error while executing CashReceiptJournalController.Create [Post].", ex);
                        SetErrorMessageViewData("Data tidak dapat ditambah.");
                    }
                }
                else {
                    SetErrorMessageViewData("Jumlah debet dan kredit tidak sama");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            Session.Remove(CashReceiptJournalSessiomName);

            var model = new CashReceiptJournalViewModel();

            try
            {
                var result = (from r in entities.CashReceiptJournals
                              where r.Id == id
                              select r).FirstOrDefault();
                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.EvidenceNumber = result.EvidenceNumber;
                    model.DocumentDate = result.DocumentDate;
                    model.Description = result.Description;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing CashReceiptJournalController.Edit[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CashReceiptJournalViewModel model)
        {
            var cashReceiptJournalDetailSessionlist = (List<CashReceiptJournalDetailViewModel>)(Session[CashReceiptJournalSessiomName]);

            if (cashReceiptJournalDetailSessionlist == null || cashReceiptJournalDetailSessionlist.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail jurnal.");
            string documentDateString = Request.Form["DocumentDate"];
            DateTime documentDate;
            DateTime.TryParse(documentDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out documentDate);

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
                    if (transday >= 4)
                    {
                        ModelState.AddModelError("documentDate", "Jurnal Penerimaan Kas "
                                                   + string.Format("{0:y}", documentDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("documentDate", "Jurnal Penerimaan Kas "
                                                + string.Format("{0:y}", documentDate) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.AmountDebet == model.AmountKredit)
                {
                    //try
                    //{
                        var cashReceipt = (from aj in entities.CashReceiptJournals
                                           where aj.Id == model.Id
                                           select aj).FirstOrDefault();
                        cashReceipt.Id = model.Id;
                        cashReceipt.DocumentDate = model.DocumentDate;
                        cashReceipt.Description = model.Description;
                        cashReceipt.EvidenceNumber = model.EvidenceNumber;

                        // delete old data first, before inserting new detail
                        foreach (var item in entities.CashReceiptJournalDetails.Where(p => p.CashReceiptJournalId == cashReceipt.Id))
                        {
                            entities.CashReceiptJournalDetails.DeleteObject(item);
                        }

                        // insert new detail
                        foreach (var item in cashReceiptJournalDetailSessionlist)
                        {
                            var detail = new CashReceiptJournalDetail
                            {
                                CashReceiptJournalId = cashReceipt.Id,
                                AccountId = item.AccountId,
                                Debet = item.Debet,
                                Kredit = item.Kredit,
                                BranchId = item.BranchId,
                                MasterBranch = item.MasterBranch 
                            };

                            entities.CashReceiptJournalDetails.AddObject(detail);
                        }

                        entities.SaveChanges();
                        saveHistory(model.EvidenceNumber);
                        return RedirectToAction("Index");
                    }
                    //catch (Exception ex)
                    //{
                    //    Logger.Error("Error while executing CashReceiptJournalController.Edit[Post].", ex);
                    //    SetErrorMessageViewData("Data tidak dapat diubah");
                    //}
                //}
                else {
                    SetErrorMessageViewData("Jumlah debet dan kredit tidak sama");
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
                var model = (from m in entities.CashReceiptJournals
                             where m.Id == id
                             select new CashReceiptJournalViewModel
                             {
                                 EvidenceNumber = m.EvidenceNumber,
                                 DocumentDate = m.DocumentDate,
                                 Description = m.Description,
                             }).FirstOrDefault();
                if (model == null)
                {
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing CashReceiptJournalController.Delete [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(CashReceiptJournalViewModel model)
        {
            try
            {
                var deleteItem = (from di in entities.CashReceiptJournals
                                  where di.Id == model.Id
                                  select di).FirstOrDefault();

                if (deleteItem != null)
                {
                    foreach (var item in entities.CashReceiptJournalDetails.Where(p => p.CashReceiptJournalId == deleteItem.Id))
                    {
                        entities.CashReceiptJournalDetails.DeleteObject(item);
                    }

                    entities.CashReceiptJournals.DeleteObject(deleteItem);
                    entities.SaveChanges();
                    saveHistory(deleteItem.EvidenceNumber);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing CashReceiptJournalController.Delete [Post].", ex);
                SetErrorMessageViewData("Data cannot be deleted.");
            }
            return RedirectToAction("Index");

        }

        public ActionResult detail(Guid id)
        {
            Session.Remove(CashReceiptJournalSessiomName);

            var model = (from m in entities.CashReceiptJournals
                         where m.Id == id
                         select new CashReceiptJournalViewModel
                         {
                            EvidenceNumber = m.EvidenceNumber,
                            DocumentDate = m.DocumentDate,
                            Description = m.Description
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult CheckDebetKredit(string AccountCode)
        {
            var code = AccountCode.Substring(0, 8);

            var getCode = entities.Accounts.Where(a => a.Code == code).FirstOrDefault();

            var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 3).FirstOrDefault();

            var value = getCodeStatus.AccountSide;
            return new JsonResult { Data = value };
        }

        [HttpPost]
        public ActionResult CheckNumber(string EvidenceNumber, Guid Id)
        {
            var value = "";
            if (Id != null)
            {
                var existingNumber = (from c in entities.CashReceiptJournals
                                      where c.EvidenceNumber == EvidenceNumber
                                      select c.EvidenceNumber).FirstOrDefault();
                
                if (existingNumber != null)
                {
                    value = "Nomor yang Anda masukkan sudah ada, silahkan isi dengan nomor yang lain.";
                }
            }
            else {
                var existingNumberEdit = (from c in entities.CashReceiptJournals
                                      where c.EvidenceNumber == EvidenceNumber && c.Id != Id
                                      select c.EvidenceNumber).FirstOrDefault();

                if (existingNumberEdit != null)
                {
                    value = "Nomor yang Anda masukkan sudah ada, silahkan isi dengan nomor yang lain.";
                }
            }
            
            //var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 2).FirstOrDefault();

            //var value = getCodeStatus.AccountSide;
            return new JsonResult { Data = value };
        }
    }
}
