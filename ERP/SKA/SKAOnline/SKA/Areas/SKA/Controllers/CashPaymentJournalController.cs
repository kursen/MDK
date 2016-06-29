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
    public class CashPaymentJournalController : BaseController
    {
        private SKAEntities entities;

        private string CashPaymentJournalSessionName = "CashPaymentSession";

        public CashPaymentJournalController()
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
            var model = from m in entities.CashPaymentJournals
                        select new CashPaymentJournalViewModel
                        {
                            Id = m.Id,
                            JBKDate = m.JBKDate,
                            PaymentDate = m.PaymentDate,
                            VoucherNumber = m.Number,
                            CheckNumber = m.CheckNumber,
                            Description = m.Description,
                            ClosingDate = entities.Closings.Select(a => a.ClosingDate).FirstOrDefault()
                        };

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.VoucherNumber.Contains(searchValue) ||
                    a.Description.Contains(searchValue));
            }
            return View(new GridModel<CashPaymentJournalViewModel>(model));
        }

        [HttpPost]
        public ActionResult GetAccountCodeSelectList(string text)
        {
            var branchAccount = entities.ProcJournalAccount("Jurnal Bayar Kas", text);
            var accounts = (from a in branchAccount
                            select a.AccountCode
                            ).ToList();

            return new JsonResult { Data = accounts };
        
        }

        [GridAction]
        public ActionResult _SelectDetail(Guid? id)
        {
            var model = (List<CashPaymentDetailViewModel>)Session[CashPaymentJournalSessionName];
            //var branch = GetCurrentUserBranchId();

            if (model == null)
            {
                model = (from data in entities.CashPaymentJournalDetails
                         where data.CashPaymentJournalId == id
                         select new CashPaymentDetailViewModel
                         {
                             CashPaymentJournalId = data.CashPaymentJournalId,
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
                Session[CashPaymentJournalSessionName] = model;
            }
            return View(new GridModel<CashPaymentDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpPost]
        [GridAction()]
        public ActionResult _InsertDetail()
        {
            var model = (List<CashPaymentDetailViewModel>)Session[CashPaymentJournalSessionName];
            CashPaymentDetailViewModel postmodel = new CashPaymentDetailViewModel();
            var branch = GetCurrentUserBranchId();
            if (model == null)
                model = new List<CashPaymentDetailViewModel>();

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
                        else
                        {
                            if (getFirstBalance != null)
                            {
                                if (getFirstBalance.Debet == null && getFirstBalance.Kredit == null)
                                {
                                    ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                    return View(new GridModel<CashPaymentDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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
                                return View(new GridModel<CashPaymentDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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
                    }

                    int maxId = 0;

                    if (model.Count > 0)
                        maxId = model.Max(m => m.Id);

                    postmodel.Id = maxId + 1;

                    model.Insert(0, postmodel);

                    Session[CashPaymentJournalSessionName] = model;
                }
                else
                {
                    ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                }
            }

            return View(new GridModel<CashPaymentDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpPost]
        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<CashPaymentDetailViewModel>)Session[CashPaymentJournalSessionName];
            CashPaymentDetailViewModel postmodel = new CashPaymentDetailViewModel();
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
                                        return View(new GridModel<CashPaymentDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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
                                    return View(new GridModel<CashPaymentDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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

            return View(new GridModel<CashPaymentDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetail()
        {
            IList<CashPaymentDetailViewModel> model = (IList<CashPaymentDetailViewModel>)Session[CashPaymentJournalSessionName];
            var detail = new CashPaymentDetailViewModel();

            if (!TryUpdateModel<CashPaymentDetailViewModel>(detail))
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

            return View(new GridModel<CashPaymentDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpGet]
        public ActionResult Create()
        {
            Session.Remove(CashPaymentJournalSessionName);

            var model = new CashPaymentJournalViewModel();
            model.JBKDate = DateTime.Now;
            model.PaymentDate = DateTime.Now;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CashPaymentJournalViewModel model)
        {
            var CashPaymentSessionList = (List<CashPaymentDetailViewModel>)(Session[CashPaymentJournalSessionName]);
            BranchViewModel branch;
            branch = GetCurrentUserBranchId();

            if (CashPaymentSessionList == null || CashPaymentSessionList.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail jurnal.");

            string jbkDateString = Request.Form["JBKDate"];
            string paymentDateString = Request.Form["PaymentDate"];
            DateTime jbkDate;
            DateTime.TryParse(jbkDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out jbkDate);
            DateTime paymentDate;
            DateTime.TryParse(jbkDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out paymentDate);
            model.JBKDate = jbkDate;
            model.PaymentDate = paymentDate;

            var dttransaction = new DateTime(jbkDate.Year, jbkDate.Month, 1);
            var transmonth = DateTime.Now.Month;
            var transkyear = DateTime.Now.Year;
            var transday = DateTime.Now.Day;
            var transvoucher = new DateTime(transkyear, transmonth, 1);
            int resultCek = DateTime.Compare(dttransaction, transvoucher);

            var errorItem = ModelState.FirstOrDefault(m => m.Key == "JBKDate");
            var errorPayment = ModelState.FirstOrDefault(m => m.Key == "PaymentDate");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (errorPayment.Value != null)
                ModelState.Remove(errorPayment);

            if (resultCek != 0)
            {
                var nextmonth = jbkDate.Month + 1;
                if (nextmonth == transmonth && jbkDate.Year == transkyear)
                {
                    if (transday > 4)
                    {
                        ModelState.AddModelError("JBKDate", "Jurnal Pembayaran Kas "
                                                   + string.Format("{0:y}", jbkDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("JBKDate", "Jurnal Pembayaran Kas "
                                                + string.Format("{0:y}", jbkDate) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.AmountDebet == model.AmountKredit)
                {
                    //try
                    //{
                        var paymentPrev = (from a in entities.CashPaymentJournals
                                           where a.Number == model.VoucherNumber
                                           select a).FirstOrDefault();

                        if (paymentPrev == null)
                        {
                            var newDataCashPayment = new CashPaymentJournal();

                            //var nextNumber = 0;
                            //var recordCount = (from vrc in entities.VoucherRecordCounters
                            //where vrc.BranchId == branch.Id
                            //select vrc).FirstOrDefault();

                            //if (recordCount != null)
                            //    nextNumber = recordCount.LastCounter;

                            //string number = string.Format("{0:000000}", nextNumber + 1);

                            newDataCashPayment.Id = Guid.NewGuid();
                            newDataCashPayment.Number = model.VoucherNumber;
                            newDataCashPayment.Description = model.Description;
                            newDataCashPayment.JBKDate = model.JBKDate;
                            newDataCashPayment.PaymentDate = model.PaymentDate;
                            newDataCashPayment.CheckNumber = model.CheckNumber;
                            newDataCashPayment.BranchId = branch.Id;


                            entities.CashPaymentJournals.AddObject(newDataCashPayment);

                            foreach (var item in CashPaymentSessionList)
                            {
                                var detailCashPayment = new CashPaymentJournalDetail
                                {
                                    AccountId = item.AccountId,
                                    CashPaymentJournalId = newDataCashPayment.Id,
                                    Debet = item.Debet,
                                    Kredit = item.Kredit,
                                    BranchId = item.BranchId,
                                    MasterBranch = item.MasterBranch 
                                };
                                entities.CashPaymentJournalDetails.AddObject(detailCashPayment);
                            }

                            //if (recordCount != null)
                            //    recordCount.LastCounter = nextNumber + 1;
                            //else
                            //{
                            //    var newCounter = new VoucherRecordCounter { BranchId = branch.Id, LastCounter = 1 };
                            //    entities.VoucherRecordCounters.AddObject(newCounter);
                            //}

                            entities.SaveChanges();
                            saveHistory(model.CheckNumber);
                            Session.Remove(CashPaymentJournalSessionName);
                            //var newAdded = entities.CashPaymentJournals.Select(a => a.Id == newDataCashPayment.Id);

                            return RedirectToAction("Confirmation", new { id = newDataCashPayment.Id });
                        }
                        else {
                            ModelState.AddModelError("VoucherNumber","Nomor sudah ada. Coba masukkan nomor lain.");
                        }
                        
                    }
                //    catch (Exception ex)
                //    {
                //        Logger.Error("Error while executing DHHDJournalController.Create[Post].", ex);
                //        SetErrorMessageViewData("Data tidak dapat ditambah.");
                //    }
                //}
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
            Session.Remove(CashPaymentJournalSessionName);
            var model = (from m in entities.CashPaymentJournals
                         where m.Id == id
                         select new CashPaymentJournalViewModel
                         {
                             VoucherNumber = m.Number,
                             Description = m.Description,
                             CheckNumber = m.CheckNumber,
                             JBKDate = m.JBKDate,
                             PaymentDate = m.PaymentDate
                         }).FirstOrDefault();

            ViewData["VoucherNumber"] = model.VoucherNumber;

            var payment = entities.VoucherPayments.Where(a => a.Voucher.Number == model.VoucherNumber).FirstOrDefault();
            var jbk = "jbk";
            if (payment != null)
            {
                ViewData["Id"] = payment.VoucherId;
                ViewData["JBK"] = jbk;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CashPaymentJournalViewModel model)
        {
            var cashPaymentJournalDetailSessionList = (List<CashPaymentDetailViewModel>)(Session[CashPaymentJournalSessionName]);

            if (cashPaymentJournalDetailSessionList == null || cashPaymentJournalDetailSessionList.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail jurnal.");
            string jbkDateString = Request.Form["JBKDate"];
            string paymentDateString = Request.Form["PaymentDate"];
            DateTime jbkDate;
            DateTime.TryParse(jbkDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out jbkDate);
            DateTime paymentDate;
            DateTime.TryParse(jbkDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out paymentDate);
            model.JBKDate = jbkDate;
            model.PaymentDate = paymentDate;
            model.JBKDateVoucher = jbkDate;

            var dttransaction = new DateTime(jbkDate.Year, jbkDate.Month, 1);
            var transmonth = DateTime.Now.Month;
            var transkyear = DateTime.Now.Year;
            var transday = DateTime.Now.Day;
            var transvoucher = new DateTime(transkyear, transmonth, 1);
            int resultCek = DateTime.Compare(dttransaction, transvoucher);

            var errorItem = ModelState.FirstOrDefault(m => m.Key == "JBKDate");
            var errorPayment = ModelState.FirstOrDefault(m => m.Key == "PaymentDate");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (errorPayment.Value != null)
                ModelState.Remove(errorPayment);

            if (resultCek != 0)
            {
                var nextmonth = jbkDate.Month + 1;
                if (nextmonth == transmonth && jbkDate.Year == transkyear)
                {
                    if (transday > 4)
                    {
                        ModelState.AddModelError("JBKDate", "Jurnal Pembayaran Kas "
                                                   + string.Format("{0:y}", jbkDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("JBKDate", "Jurnal Pembayaran Kas "
                                                + string.Format("{0:y}", jbkDate) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.AmountDebet == model.AmountKredit)
                {
                    try
                    {
                        var newDetail = (from c in entities.CashPaymentJournals
                                         where c.Id == model.Id
                                         select c).FirstOrDefault();

                        if (newDetail == null)
                        {
                            return RedirectToAction("Index");
                        }

                        newDetail.Number = model.VoucherNumber;
                        newDetail.JBKDate = model.JBKDate;
                        newDetail.Description = model.Description;
                        newDetail.PaymentDate = model.PaymentDate;
                        newDetail.CheckNumber = model.CheckNumber;

                        // delete old data first, before inserting new detail
                        foreach (var itemJBK in entities.CashPaymentJournalDetails.Where(p => p.CashPaymentJournalId == newDetail.Id))
                        {
                            entities.CashPaymentJournalDetails.DeleteObject(itemJBK);
                        }

                        // insert new detail
                        foreach (var item in cashPaymentJournalDetailSessionList)
                        {
                            var detailCashPayment = new CashPaymentJournalDetail
                            {
                                AccountId = item.AccountId,
                                CashPaymentJournalId = newDetail.Id,
                                Debet = item.Debet,
                                Kredit = item.Kredit,
                                BranchId = item.BranchId,
                                MasterBranch = item.MasterBranch 
                            };

                            entities.CashPaymentJournalDetails.AddObject(detailCashPayment);
                        }

                        //var getVoucherPayment = entities.VoucherPayments.Where(a => a.VoucherId == newDetail.VoucherId).FirstOrDefault();

                        //if (getVoucherPayment != null)
                        //{
                        //    getVoucherPayment.PaymentDate = model.PaymentDate;
                        //    getVoucherPayment.JBKDate = model.JBKDateVoucher;
                        //    getVoucherPayment.CheckNumber = model.CheckNumber;

                        //    foreach (var itemPayment in entities.VoucherDetails.Where(p => p.VoucherId == newDetail.VoucherId))
                        //    {
                        //        entities.VoucherDetails.DeleteObject(itemPayment);
                        //    }

                        //    foreach (var item in cashPaymentJournalDetailSessionList)
                        //    {
                        //        var detailPayment = new VoucherDetail
                        //        {
                        //            AccountId = item.AccountId,
                        //            VoucherId = getVoucherPayment.VoucherId,
                        //            Debet = item.Debet,
                        //            Kredit = item.Kredit,
                        //            BranchId = item.BranchId
                        //        };

                        //        entities.VoucherDetails.AddObject(detailPayment);
                        //    }
                        //}

                        entities.SaveChanges();
                        saveHistory(model.CheckNumber);
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error while executing CashPaymentJournalController.Edit [Post].", ex);
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
            var model = new CashPaymentJournalViewModel();

            try
            {
                var deleteItem = (from m in entities.CashPaymentJournals
                                  where m.Id == id
                                  select m).FirstOrDefault();
                if (deleteItem == null)
                {
                    SetErrorMessageViewData("Data cannot be found");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.VoucherNumber = deleteItem.Number;
                    model.CheckNumber = deleteItem.CheckNumber;
                    model.Description = deleteItem.Description;
                    model.JBKDate = deleteItem.JBKDate;
                    model.PaymentDate = deleteItem.PaymentDate;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing CashPaymentJournalController.Delete[Get].", ex);
                SetErrorMessageViewData("Data tidak dapat ditampilkan");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(CashPaymentJournalViewModel model)
        {
            try
            {
                var deleteItemCashPayment = (from d in entities.CashPaymentJournals
                                             where d.Id == model.Id
                                             select d).FirstOrDefault();

                var voucher = (from a in entities.Vouchers
                               where a.Number == deleteItemCashPayment.Number
                               select a).FirstOrDefault();

                //var voucherPayment = from b in entities.VoucherPayments
                //                         select b;

                //if (voucher != null)
                //{
                    
                
                //}
                var dhhd = (from duhd in entities.DHHDJournals
                            where duhd.Number == deleteItemCashPayment.Number && duhd.Status == 2
                            select duhd).FirstOrDefault();

                //if (voucherPayment != null)
                //{
                //    entities.VoucherPayments.DeleteObject(voucherPayment.FirstOrDefault());
                //}

                if (dhhd != null)
                {
                    foreach (var itemDhhd in entities.DHHDJorurnalDetails.Where(p => p.DHHDJournalId == dhhd.Id))
                    {
                        entities.DHHDJorurnalDetails.DeleteObject(itemDhhd);
                    }
                    entities.DHHDJournals.DeleteObject(dhhd);
                }

                if (deleteItemCashPayment != null)
                {
                    foreach (var itemCashPayment in entities.CashPaymentJournalDetails.Where(p => p.CashPaymentJournalId == deleteItemCashPayment.Id))
                    {
                        entities.CashPaymentJournalDetails.DeleteObject(itemCashPayment);
                    }
                    entities.CashPaymentJournals.DeleteObject(deleteItemCashPayment);
                }

                if (voucher != null)
                {
                    var voucherPayment = (from b in entities.VoucherPayments
                                          where b.VoucherId.Equals(voucher.Id)
                                          select b).FirstOrDefault();
                    entities.VoucherPayments.DeleteObject(voucherPayment);

                    foreach (var itemVoucher in entities.VoucherDetails.Where(p => p.VoucherId == voucher.Id))
                    {
                        entities.VoucherDetails.DeleteObject(itemVoucher);
                    }

                    entities.Vouchers.DeleteObject(voucher);
                }

                entities.SaveChanges();
                saveHistory(deleteItemCashPayment.CheckNumber);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing CashPaymentJournalController.Delete [Post].", ex);
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
                var confirmation = (from c in entities.CashPaymentJournals
                                    where c.Id == id
                                    select new CashPaymentJournalViewModel
                                    {
                                        VoucherNumber = c.Number,
                                        JBKDate = c.JBKDate,
                                        PaymentDate = c.PaymentDate,
                                        CheckNumber = c.CheckNumber,
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
                Logger.Error("Error while executing CashPaymentJournalController.Confirmation [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Confirmation(CashPaymentJournalViewModel model)
        {
            try
            {
                var deleteItem = (from c in entities.CashPaymentJournals
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
                Logger.Error("Error while executing CashPaymentJournalController.Confirmation [Post].", ex);
                return RedirectToAction("Index");
            }
        }

        public ActionResult detail(Guid id)
        {
            Session.Remove(CashPaymentJournalSessionName);

            var model = (from m in entities.CashPaymentJournals
                         where m.Id == id
                         select new CashPaymentJournalViewModel
                         {
                             VoucherNumber = m.Number,
                             CheckNumber = m.CheckNumber,
                             JBKDate = m.JBKDate,
                             PaymentDate = m.PaymentDate,
                             Description = m.Description
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult CheckDebetKredit(string AccountCode)
        {
            var code = AccountCode.Substring(0, 8);

            var getCode = entities.Accounts.Where(a => a.Code == code).FirstOrDefault();

            var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 5).FirstOrDefault();

            var value = getCodeStatus.AccountSide;
            return new JsonResult { Data = value };
        }

        [HttpPost]
        public ActionResult CheckNumber(string VoucherNumber, Guid Id)
        {
            var value = "";

            if (Id != null)
            {
                var existingNumber = (from c in entities.CashPaymentJournals
                                      where c.Number == VoucherNumber
                                      select c.Number).FirstOrDefault();


                if (existingNumber != null)
                {
                    value = "Nomor yang Anda masukkan sudah ada, silahkan isi dengan nomor yang lain.";
                }
                //var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 2).FirstOrDefault();
            }
            else
            {
                var existingNumberEdit = (from c in entities.CashPaymentJournals
                                      where c.Number == VoucherNumber && c.Id != Id
                                      select c.Number).FirstOrDefault();


                if (existingNumberEdit != null)
                {
                    value = "Nomor yang Anda masukkan sudah ada, silahkan isi dengan nomor yang lain.";
                }
            }
            //var value = getCodeStatus.AccountSide;
            return new JsonResult { Data = value };
        }
    }
}
