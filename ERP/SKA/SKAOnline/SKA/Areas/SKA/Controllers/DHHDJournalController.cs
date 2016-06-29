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

namespace SKA.Areas.SKA.Controllers
{
    public class DHHDJournalController : BaseController
    {
        private SKAEntities entities;

        private string DHHDJournalSessionName = "DHHDJournalSession";

        public DHHDJournalController()
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
        public ActionResult Getlist(string searchValue)
        {
            var model = from m in entities.DHHDJournals
                        where m.PaymentDate == null && m.Status == 1
                        select new DHHDJournalViewModel
                        {
                            Id = m.Id,
                            DateVoucher = m.DateVoucher,
                            VoucherNumber = m.Number,
                            Description = m.Description,
                            ClosingDate = entities.Closings.Select(a => a.ClosingDate).FirstOrDefault()
                        };

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.VoucherNumber.Contains(searchValue) ||
                    a.Description.Contains(searchValue));
            }
            return View(new GridModel<DHHDJournalViewModel>(model));
        }

        [HttpPost]
        public ActionResult GetAccountCodeSelectList(string text)
        {
            var branchAccount = entities.ProcJournalAccount("Jurnal DHHD", text);
            var accounts = (from a in branchAccount
                            select a.AccountCode
                            ).ToList();

            return new JsonResult { Data = accounts };
        }
       
        [GridAction]
        public ActionResult _SelectDetail(Guid? id)
        {
            var model = (List<DHHDJournalDetailViewModel>)Session[DHHDJournalSessionName];
            //var branch = GetCurrentUserBranchId();

            if (model == null)
            {
                model = (from data in entities.DHHDJorurnalDetails
                         where data.DHHDJournalId == id
                         select new DHHDJournalDetailViewModel
                         {
                             DHHDJournalId = data.DHHDJournalId,
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
                Session[DHHDJournalSessionName] = model;
            }
            return View(new GridModel<DHHDJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [GridAction()]
        public ActionResult _InsertDetail()
        {
            var model = (List<DHHDJournalDetailViewModel>)Session[DHHDJournalSessionName];
            DHHDJournalDetailViewModel postmodel = new DHHDJournalDetailViewModel();
            var branch = GetCurrentUserBranchId();

            if (model == null)
                model = new List<DHHDJournalDetailViewModel>();

            if (TryUpdateModel(postmodel))
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
                        }
                        else { 
                            if (getFirstBalance != null)
                            {
                                if (getFirstBalance.Debet == null && getFirstBalance.Kredit == null)
                                {
                                    ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                    return View(new GridModel<DHHDJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                                }
                                else
                                {
                                    postmodel.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                                    postmodel.AccountId = account.FirstOrDefault().Id;
                                    postmodel.BranchId = getBranch.Id;
                                    postmodel.MasterBranch = branch.Id;
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                return View(new GridModel<DHHDJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                            }
                        }
                    }
                    else
                    {
                        var getBranchUser = GetCurrentUserBranchId();
                        postmodel.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                        postmodel.AccountId = account.FirstOrDefault().Id;
                        postmodel.BranchId = getBranchUser.Id;
                        postmodel.MasterBranch = branch.Id;
                    }

                    int maxId = 0;

                    if (model.Count > 0)
                        maxId = model.Max(m => m.Id);

                    postmodel.Id = maxId + 1;

                    model.Insert(0, postmodel);

                    Session[DHHDJournalSessionName] = model;
                }
                else
                {
                    ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                }
            }

            return View(new GridModel<DHHDJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<DHHDJournalDetailViewModel>)Session[DHHDJournalSessionName];
            DHHDJournalDetailViewModel postmodel = new DHHDJournalDetailViewModel();
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
                            else {
                                if (getFirstBalance != null)
                                {
                                    if (getFirstBalance.Debet == null && getFirstBalance.Kredit == null)
                                    {
                                        ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                        return View(new GridModel<DHHDJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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
                                    return View(new GridModel<DHHDJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                                }
                            }
                        }
                        else {
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

            return View(new GridModel<DHHDJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetail()
        {
            IList<DHHDJournalDetailViewModel> model = (IList<DHHDJournalDetailViewModel>)Session[DHHDJournalSessionName];
            var detail = new DHHDJournalDetailViewModel();

            if (!TryUpdateModel<DHHDJournalDetailViewModel>(detail))
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

            return View(new GridModel<DHHDJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpGet]
        public ActionResult Create()
        {
            Session.Remove(DHHDJournalSessionName);

            var model = new DHHDJournalViewModel();
            model.DateVoucher = DateTime.Now;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DHHDJournalViewModel model)
        {
            var DHHDSessionList = (List<DHHDJournalDetailViewModel>)(Session[DHHDJournalSessionName]);

            if (DHHDSessionList == null || DHHDSessionList.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail jurnal.");

            string dateVoucherString = Request.Form["DateVoucher"];
            DateTime dateVoucher;
            DateTime.TryParse(dateVoucherString, new CultureInfo("id-ID"), DateTimeStyles.None, out dateVoucher);

            model.DateVoucher = dateVoucher;

            var dttransaction = new DateTime(dateVoucher.Year, dateVoucher.Month, 1);
            var transmonth = DateTime.Now.Month;
            var transkyear = DateTime.Now.Year;
            var transday = DateTime.Now.Day;
            var transvoucher = new DateTime(transkyear, transmonth, 1);
            int resultCek = DateTime.Compare(dttransaction, transvoucher);
            var errorItem = ModelState.FirstOrDefault(m => m.Key == "DateVoucher");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (resultCek != 0)
            {
                var nextmonth = dateVoucher.Month + 1;
                if (nextmonth == transmonth && dateVoucher.Year == transkyear)
                {
                    if (transday > 4)
                    {
                        ModelState.AddModelError("DateVoucher", "DHHD Journal "
                                                   + string.Format("{0:y}", dateVoucher) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("DateVoucher", "DHHD Journal "
                                                + string.Format("{0:y}", dateVoucher) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                var branch = GetCurrentUserBranchId();
                if (model.AmountDebet == model.AmountKredit)
                {
                    try
                    {
                        //-----Save new object to DHHDJournal---------//
                        var newDataDHHD = new DHHDJournal();

                        var nextNumber = 0;
                        var recordCount = (from vrc in entities.VoucherRecordCounters
                                           where vrc.BranchId == branch.Id
                                           select vrc).FirstOrDefault();

                        if (recordCount != null)
                            nextNumber = recordCount.LastCounter;

                        string number = string.Format("{0:000000}", nextNumber + 1);

                        newDataDHHD.Id = Guid.NewGuid();
                        newDataDHHD.Number = string.Format("{0}{1}", branch.Code, number);
                        newDataDHHD.Description = model.Description;
                        newDataDHHD.DateVoucher = model.DateVoucher;
                        newDataDHHD.BranchId = branch.Id;
                        newDataDHHD.Status = 1;
                        entities.DHHDJournals.AddObject(newDataDHHD);
                        //-------Save to VoucherPayment---------//

                        var newVoucherPayment = new Voucher();
                        newVoucherPayment.Id = Guid.NewGuid();
                        newVoucherPayment.Number = string.Format("{0}{1}", branch.Code, number);
                        newVoucherPayment.Description = model.Description;
                        newVoucherPayment.TransactionDate = model.DateVoucher;
                        newVoucherPayment.BranchId = branch.Id;
                        newVoucherPayment.VoucherStatusId = 2;
                        newVoucherPayment.PaymentStatus = 1;

                        foreach (var dhhd in DHHDSessionList)
                        {
                            if (dhhd.AccountCode.Contains("UTANG") || dhhd.AccountCode.Contains("Utang") || dhhd.AccountCode.Contains("utang")) 
                            {
                                var id = dhhd.AccountId;
                                var partner = entities.Partners.Where(a => a.AccountId.Equals(id)).FirstOrDefault();

                                newVoucherPayment.PartnerId = partner.Id;
                            }
                        }

                        entities.Vouchers.AddObject(newVoucherPayment);

                        foreach (var item in DHHDSessionList)
                        {
                            var detailDHHD = new DHHDJorurnalDetail
                            {
                                AccountId = item.AccountId,
                                DHHDJournalId = newDataDHHD.Id,
                                Debet = item.Debet,
                                Kredit = item.Kredit,
                                BranchId = item.BranchId,
                                MasterBranch = item.MasterBranch 
                            };
                            entities.DHHDJorurnalDetails.AddObject(detailDHHD);
                            var detailVoucher = new VoucherDetail
                            {
                                AccountId = item.AccountId,
                                VoucherId = newVoucherPayment.Id,
                                Debet = item.Debet,
                                Kredit = item.Kredit,
                                BranchId = item.BranchId,
                                MasterBranch = item.MasterBranch
                                
                            };
                            entities.VoucherDetails.AddObject(detailVoucher);
                        }

                        if (recordCount != null)
                            recordCount.LastCounter = nextNumber + 1;
                        else
                        {
                            var newCounter = new VoucherRecordCounter { BranchId = branch.Id, LastCounter = 1 };
                            entities.VoucherRecordCounters.AddObject(newCounter);
                        }

                        entities.SaveChanges();
                        saveHistory(string.Format("{0}{1}", branch.Code, number));
                        //Session.Remove(DHHDJournalSessionName);
                        var newAdded = newDataDHHD.Id ;

                        return RedirectToAction("Confirmation", new { id = newAdded });
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error while executing DHHDJournalController.Create[Post].", ex);
                        SetErrorMessageViewData("Data tidak dapat ditambah.");
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
            Session.Remove(DHHDJournalSessionName);
            var model = (from m in entities.DHHDJournals
                         where m.Id == id
                         select new DHHDJournalViewModel
                         {
                             VoucherNumber = m.Number,
                             Description = m.Description,
                             DateVoucher = m.DateVoucher
                         }).FirstOrDefault();

            ViewData["VoucherNumber"] = model.VoucherNumber;
            var voucher = entities.Vouchers.Where(a => a.Number == model.VoucherNumber).FirstOrDefault();
            var dhhd = "dhhd";
            if (voucher != null)
            {
                ViewData["Id"] = voucher.Id;
                ViewData["dhhd"] = dhhd;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DHHDJournalViewModel model)
        {
            var branch = GetCurrentUserBranchId();
            var DHHDJournalDetailSessionList = (List<DHHDJournalDetailViewModel>)(Session[DHHDJournalSessionName]);

            if (DHHDJournalDetailSessionList == null || DHHDJournalDetailSessionList.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail jurnal.");

            string dateVoucherString = Request.Form["DateVoucher"];
            DateTime dateVoucher;
            DateTime.TryParse(dateVoucherString, new CultureInfo("id-ID"), DateTimeStyles.None, out dateVoucher);

            model.DateVoucher = dateVoucher;
            var dttransaction = new DateTime(dateVoucher.Year, dateVoucher.Month, 1);
            var transmonth = DateTime.Now.Month;
            var transkyear = DateTime.Now.Year;
            var transday = DateTime.Now.Day;
            var transvoucher = new DateTime(transkyear, transmonth, 1);
            int resultCek = DateTime.Compare(dttransaction, transvoucher);

            var errorItem = ModelState.FirstOrDefault(m => m.Key == "DateVoucher");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (resultCek != 0)
            {
                var nextmonth = dateVoucher.Month + 1;
                if (nextmonth == transmonth && dateVoucher.Year == transkyear)
                {
                    if (transday > 4)
                    {
                        ModelState.AddModelError("DateVoucher", "DHHD Journal "
                                                   + string.Format("{0:y}", dateVoucher) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("DateVoucher", "DHHD Journal "
                                                + string.Format("{0:y}", dateVoucher) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.AmountDebet == model.AmountKredit)
                {
                    try
                    {
                        var newDetail = (from c in entities.DHHDJournals
                                         where c.Id == model.Id
                                         select c).FirstOrDefault();

                        var updateVoucher = (from c in entities.Vouchers
                                             where c.Number == newDetail.Number
                                             select c).FirstOrDefault();

                        if (newDetail == null && updateVoucher == null)
                        {
                            return RedirectToAction("Index");
                        }

                        //newDetail.Number = model.VoucherNumber;
                        newDetail.DateVoucher = model.DateVoucher;
                        newDetail.Description = model.Description;
                        //newDataDHHD.BranchId = branch.Id;
                        //newDataDHHD.Status = 1;

                        //updateVoucher.Number = model.VoucherNumber;
                        updateVoucher.TransactionDate = model.DateVoucher;
                        updateVoucher.Description = model.Description;
                        // delete old data first, before inserting new detail
                        foreach (var itemDHHD in entities.DHHDJorurnalDetails.Where(p => p.DHHDJournalId == newDetail.Id))
                        {
                            entities.DHHDJorurnalDetails.DeleteObject(itemDHHD);
                        }

                        foreach (var itemVoucher in entities.VoucherDetails.Where(p => p.VoucherId == updateVoucher.Id))
                        {
                            entities.VoucherDetails.DeleteObject(itemVoucher);
                        }
                        // insert new detail
                        foreach (var dhhd in DHHDJournalDetailSessionList)
                        {
                            if (dhhd.AccountCode.Contains("Utang"))
                            {
                                var id = dhhd.AccountId;
                                var partner = entities.Partners.Where(a => a.AccountId.Equals(id)).FirstOrDefault();

                                updateVoucher.PartnerId = partner.Id;
                            }
                        }
                        
                        foreach (var item in DHHDJournalDetailSessionList)
                        {
                            var detailDHHD = new DHHDJorurnalDetail
                            {
                                AccountId = item.AccountId,
                                DHHDJournalId = newDetail.Id,
                                Debet = item.Debet,
                                Kredit = item.Kredit,
                                BranchId = item.BranchId,
                                MasterBranch = branch.Id 
                            };

                            entities.DHHDJorurnalDetails.AddObject(detailDHHD);

                            var detailVoucher = new VoucherDetail
                            {
                                AccountId = item.AccountId,
                                VoucherId = updateVoucher.Id,
                                Debet = item.Debet,
                                Kredit = item.Kredit,
                                BranchId = item.BranchId,
                                MasterBranch = branch.Id 
                            };

                            entities.VoucherDetails.AddObject(detailVoucher);
                        }
                        entities.SaveChanges();
                        saveHistory(model.VoucherNumber);
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error while executing DHHDJournalController.Edit [Post].", ex);
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
        public ActionResult PaymentJournal(Guid id)
        {
            Session.Remove(DHHDJournalSessionName);
            var model = (from m in entities.DHHDJournals
                         where m.Id == id
                         select new DHHDJournalViewModel
                         {
                             VoucherNumber = m.Number,
                             Description = m.Description,
                             DateVoucher = m.DateVoucher,
                             PaymentDate = DateTime.Now 
                         }).FirstOrDefault();

            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult PaymentJournal(Guid id, DHHDJournalViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //string PaymentDateString = Request.Form["PaymentDate"];
                    //DateTime PaymentDate;
                    //DateTime.TryParse(PaymentDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out PaymentDate);

                    //model.PaymentDate = PaymentDate;

                    var newDataJBK = new CashPaymentJournal();
                    //var newDataDHHD = new DHHDJournal();

                    var DHHD = (from d in entities.DHHDJournals
                                where d.Id == id
                                select d).FirstOrDefault();

                    DHHD.PaymentDate = model.PaymentDate;

                    newDataJBK.DHHDId = id;
                    newDataJBK.Number = DHHD.Number;
                    newDataJBK.PaymentDate = model.PaymentDate;
                    newDataJBK.CheckNumber = model.CheckNumber;
                    newDataJBK.Description = DHHD.Description;

                    entities.CashPaymentJournals.AddObject(newDataJBK);
                    //entities.DHHDJournals.AddObject(newDataDHHD);
                    entities.SaveChanges();
                    saveHistory(DHHD.Number);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing DHHDJournalController.PaymentJournal [Post].", ex);
                    SetErrorMessageViewData("Data tidak dapat dibuat.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var model = new DHHDJournalViewModel();

            try
            {
                var deleteItem = (from m in entities.DHHDJournals
                                  where m.Id == id
                                  select m).FirstOrDefault();
                if (deleteItem == null)
                {
                    SetErrorMessageViewData("Data tidak dapat ditemukan");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.VoucherNumber = deleteItem.Number;
                    model.DateVoucher = deleteItem.DateVoucher;
                    model.Description = deleteItem.Description;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing DHHDJournalController.Delete[Get].", ex);
                SetErrorMessageViewData("Data tidak dapat ditampilkan");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(DHHDJournalViewModel model)
        {
            try
            {
                var deleteItemDHHD = (from d in entities.DHHDJournals
                                      where d.Id == model.Id
                                      select d).FirstOrDefault();

                var voucher = (from a in entities.Vouchers
                               where a.Number == deleteItemDHHD.Number
                               select a).FirstOrDefault();

                if (deleteItemDHHD != null && voucher != null)
                {
                    foreach (var itemDHHD in entities.DHHDJorurnalDetails.Where(p => p.DHHDJournalId == deleteItemDHHD.Id))
                    {
                        entities.DHHDJorurnalDetails.DeleteObject(itemDHHD);
                    }

                    foreach (var itemVoucher in entities.VoucherDetails.Where(p => p.VoucherId == voucher.Id))
                    {
                        entities.VoucherDetails.DeleteObject(itemVoucher);
                    }

                    entities.DHHDJournals.DeleteObject(deleteItemDHHD);
                    entities.Vouchers.DeleteObject(voucher);
                    entities.SaveChanges();
                    saveHistory(voucher.Number);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing DHHDJournalController.Delete [Post].", ex);
                SetErrorMessageViewData("Data tidak dapat dihapus.");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Confirmation(Guid id)
        {
            ViewData["Id"] = id;
            try
            {
                var confirmation = (from c in entities.DHHDJournals
                                    where c.Id == id
                                    select new DHHDJournalViewModel
                                    {
                                        VoucherNumber = c.Number,
                                        DateVoucher = c.DateVoucher,
                                        Description = c.Description,
                                    }).FirstOrDefault();

                if (confirmation == null)
                {
                    return RedirectToAction("Index");
                }

                return View(confirmation);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing DHHDJournalController.Confirmation [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Confirmation(DHHDJournalViewModel model)
        {
            try
            {
                var deleteItem = (from c in entities.DHHDJournals
                                  where c.Id == model.Id
                                  select c).FirstOrDefault();

                if (deleteItem != null)
                {
                    return RedirectToAction("Edit", new { id = deleteItem.Id });
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing DHHDJournalController.Confirmation [Post].", ex);
                return RedirectToAction("Index");
            }
        }

        public ActionResult detail(Guid id)
        {
            Session.Remove(DHHDJournalSessionName);

            var model = (from m in entities.DHHDJournals
                         where m.Id == id
                         select new DHHDJournalViewModel
                         {
                             VoucherNumber = m.Number,
                             DateVoucher = m.DateVoucher,
                             Description = m.Description
                         }).FirstOrDefault();

            return View(model);
        }
        [HttpPost]
        public ActionResult CheckDebetKredit(string AccountCode)
        {
            string[] arrCode = AccountCode.Trim().Split('.');
            string code = arrCode[0]; //code.Substring(0, 8);

            var getCode = entities.Accounts.Where(a => a.Code == code).FirstOrDefault();

            var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 1).FirstOrDefault();

            var value = getCodeStatus.AccountSide;
            return new JsonResult { Data = value };
        }
    }
}
