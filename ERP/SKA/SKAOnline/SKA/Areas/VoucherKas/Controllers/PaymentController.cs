using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.VoucherKas.Models.ViewModel;
using SKA.Areas.Setting.Models.ViewModels;
using System.Globalization;

namespace SKA.Areas.VoucherKas.Controllers
{
    public class PaymentController : BaseController
    {
        private SKAEntities entities;

        private string PaymentSessionName = "PaymentDetailSessionList";

        public PaymentController()
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
            var branch = GetCurrentUserBranchId();
            var model = from p in entities.Vouchers
                        select new VoucherViewModel
                        {
                            Id = p.Id,
                            Number = p.Number,
                            PaymentDate = p.VoucherPayment.PaymentDate,
                            TransactionDate = p.TransactionDate,
                            VoucherStatusId = p.VoucherStatusId,
                            BranchId = p.BranchId
                        };

            if (branch.Code != "00")
            {
                model = model.Where(a => a.BranchId == branch.Id && a.VoucherStatusId == 2);
            }
            else
            {
                model = model.Where(a => a.VoucherStatusId == 2);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Number.Contains(searchValue));
            }

            return View(new GridModel<VoucherViewModel>(model));
        }

        [GridAction]
        public ActionResult _GetListDetail(VoucherDetailViewModel model)
        {
            var branch = GetCurrentUserBranchId();
            VoucherDetailViewModel postModel = new VoucherDetailViewModel
            {
                VoucherId = model.VoucherId,
                AccountId = model.AccountId,
                Debet = model.Debet,
                Kredit = model.Kredit,
                MasterBranch = branch.Id
            };
            return View(model);
        }

        [GridAction]
        public ActionResult _SelectDetail(Guid? id)
        {
            var branch = GetCurrentUserBranchId();
            var model = (List<VoucherDetailViewModel>)Session[PaymentSessionName];
            //var branch = GetCurrentUserBranchId();

            if (model == null)
            {
                model = (from data in entities.VoucherDetails
                         where data.VoucherId == id && (!data.AccountId.Equals(data.Voucher.Partner.AccountId))
                         select new VoucherDetailViewModel
                         {
                             VoucherId = data.VoucherId,
                             AccountId = data.AccountId,
                             Debet = data.Debet,
                             Kredit = data.Kredit,
                             AccountCode = (data.Account.Code.Length.Equals(8)) ? data.Account.Code + "." + data.Branch.Code + " - " + data.Account.Name + " " + data.Branch.Name : data.Account.Code + " - " + data.Account.Name,
                             AccountName = data.Account.Name,
                             BranchId = data.BranchId,
                             MasterBranch = branch.Id 
                         }).ToList();

                int counter = 0;
                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }
                Session[PaymentSessionName] = model;
            }
            return View(new GridModel<VoucherDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<VoucherDetailViewModel>)Session[PaymentSessionName];
            VoucherDetailViewModel postmodel = new VoucherDetailViewModel();
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
                                        return View(new GridModel<VoucherDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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
                                    return View(new GridModel<VoucherDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                                }
                            }

                        }
                        else
                        {
                            item.AccountId = account.FirstOrDefault().Id;
                            item.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                            item.Debet = postmodel.Debet;
                            item.Kredit = postmodel.Kredit;
                            item.MasterBranch = branch.Id;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                    }
                }
            }

            return View(new GridModel<VoucherDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetail()
        {
            IList<VoucherDetailViewModel> model = (IList<VoucherDetailViewModel>)Session[PaymentSessionName];
            var detail = new VoucherDetailViewModel();

            if (!TryUpdateModel<VoucherDetailViewModel>(detail))
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

            return View(new GridModel<VoucherDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpGet]
        public ActionResult Paid(Guid id)
        {
            Session.Remove(PaymentSessionName);
            var payment = (from p in entities.Vouchers
                           where p.Id == id
                           select new VoucherPaymentViewModel
                           {
                               VoucherNumber = p.Number,
                               TransactionDate = p.TransactionDate,
                               PartnerName = p.Partner.Name,
                               PartnerId = p.PartnerId,
                               Description = p.Description,
                               ApproveDate = p.ApproveDate,
                               //VoucherStatusId = p.VoucherStatusId,
                               VoucherStatusName = p.SystemVoucherStatu.Description,
                               BranchId = p.BranchId,
                               //Status= p.PaymentStatus,
                               JBKDate = DateTime.Now,
                               PaymentDate = DateTime.Now

                           }).FirstOrDefault();

            return View(payment);
        }

        [HttpPost]
        public ActionResult Paid(Guid id, VoucherPaymentViewModel model)
        {
            BranchViewModel branch;
            branch = GetCurrentUserBranchId();

            var voucher = (from s in entities.Vouchers
                           where s.Id == id
                           select s).FirstOrDefault();

            //string transactionDateString = Request.Form["TransactionDate"];
            string jbkDateString = Request.Form["JBKDate"];
            string paymentDateString = Request.Form["PaymentDate"];
            //DateTime transactionDate;
            //DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);
            DateTime paymentDate;
            DateTime.TryParse(paymentDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out paymentDate);
            DateTime jbkDate;
            DateTime.TryParse(jbkDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out jbkDate);
            model.JBKDate = jbkDate;
            model.PaymentDate = paymentDate;
            //model.TransactionDate = transactionDate;
            DateTime cekJbk;
            DateTime.TryParse(jbkDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out cekJbk);

            var transdatemonth = voucher.TransactionDate.Month;
            var transdateyear = voucher.TransactionDate.Year;
            var transdateday = voucher.TransactionDate.Day;
            var dttransaction = new DateTime(transdateyear,transdatemonth,1);
            var jbkmonth = DateTime.Now.Month; 
            var jbkyear = DateTime.Now.Year;
            var jbkday = DateTime.Now.Day;
            var dtjbk = new DateTime(jbkyear, jbkmonth, 1);
            int resultCek = DateTime.Compare(dttransaction, dtjbk);
            var error = ModelState.ToList();

            if (error.Count > 0)
            {
                foreach (var a in error)
                {
                    ModelState.Remove(a);
                }
            }

           
            //if (resultCek != 0)
            //{
            //    var nextmonth = transdatemonth + 1;
            //    if (nextmonth == jbkmonth && transdateyear == jbkyear)
            //    {
            //        if (jbkday >= 4)
            //        {
            //            ModelState.AddModelError("JBKDate", "Pembayaran Voucher "
            //                                       + string.Format("{0:y}", voucher.TransactionDate) + " telah ditutup, Hubungi Administrator!");
            //        }
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("JBKDate", "Pembayaran Voucher Bulan "
            //                                    + string.Format("{0:y}", voucher.TransactionDate) + " telah ditutup, Hubungi Administrator!");
            //    }
            //}

           
            if (model.CheckNumber == null)
            {
                ModelState.AddModelError("CheckNumber", "Nomor Cek/Giro harus diisi!");

            }

            if (model.AccountCode == null)
            {
                ModelState.AddModelError("AccountId", "No Kas/Bank harus diisi!");
            }

            if (model.Bank == null)
            {
                ModelState.AddModelError("BankId", "Bank harus diisi!");
            }

            if (ModelState.IsValid)
            {

                //try
                //{
                    var paymentVoucher = new VoucherPayment();
                    var cashPaymentJournal = new CashPaymentJournal();

                    //var itemAdded = (from c in entities.Vouchers
                    //                 where c.Id == model.VoucherId
                    //                 select c).FirstOrDefault();

                    var dhhdJournal = entities.DHHDJournals.Where(d => d.Number == model.VoucherNumber).FirstOrDefault();//Get DUHD
                    
                    var voucherDetail = entities.VoucherDetails.Where(d => d.VoucherId == id).ToList();//GET Voucher

                    var deleteDetail = entities.VoucherDetails.Where(d => d.VoucherId == id).ToList(); //get detail from voucher detail

                    //var voucher = (from s in entities.Vouchers
                    //               where s.Id == id
                    //               select s).FirstOrDefault(); //get current voucher 


                    char[] reverseCode = model.AccountCode.Trim().ToCharArray();
                    Array.Reverse(reverseCode);

                    string code = new string(reverseCode.Take(2).Reverse().ToArray()); //model.AccountCode.Substring(9, 2);

                    var getBranchCode = entities.Branches.Where(a => a.Code == code).FirstOrDefault(); // check code branch in table Branch
                    

                    //-------------------------------------- add to payment table ----------------------//
                    paymentVoucher.VoucherId = id;
                    paymentVoucher.CheckNumber = model.CheckNumber;
                    paymentVoucher.PaymentDate = model.PaymentDate;
                    paymentVoucher.JBKDate = model.JBKDate;
                    paymentVoucher.BankId = model.BankId;
                    paymentVoucher.AccountId = model.AccountId;
                    paymentVoucher.BranchId = branch.Id ;
                    entities.SaveChanges();
                    //------------delete object before insert the new one-------------//
                    for (int i = 0; i < deleteDetail.Count; i++)
                    {
                        entities.VoucherDetails.DeleteObject(deleteDetail[i]);
                    }
                    entities.SaveChanges();

                    //---------------insert new detail---------------//
                    for (int i = 0; i < voucherDetail.Count; i++)
                    {
                        var updateVoucherDetail = new VoucherDetail();
                        //updateVoucherDetail.VoucherId = id;
                        //updateVoucherDetail.AccountId = voucherDetail[i].AccountId;
                        //updateVoucherDetail.BranchId = voucherDetail[i].BranchId;
                        //updateVoucherDetail.Debet = voucherDetail[i].Debet;
                        //updateVoucherDetail.Kredit = voucherDetail[i].Kredit;
                        //updateVoucherDetail.MasterBranch = branch.Id;
                        voucherDetail[i].MasterBranch = branch.Id;
                        entities.VoucherDetails.AddObject(voucherDetail[i]);

                        decimal? getAmount = 0;

                        if (voucherDetail[i].Account.Id.Equals(voucherDetail[i].Voucher.Partner.AccountId))
                        {
                            getAmount = voucherDetail[i].Debet + voucherDetail[i].Kredit;
                        }

                        if (i == voucherDetail.Count - 1)
                        {
                            updateVoucherDetail.VoucherId = id;
                            updateVoucherDetail.AccountId = model.AccountId;
                        }

                    }
                    //------Change status voucher---//
                    voucher.PaymentStatus = 2;

                    //-----------------------add to cash payment journal--------------//
                    cashPaymentJournal.Id = Guid.NewGuid();
                    cashPaymentJournal.VoucherId = id;
                    cashPaymentJournal.CheckNumber = model.CheckNumber;
                    cashPaymentJournal.PaymentDate = model.PaymentDate;
                    cashPaymentJournal.JBKDate = model.JBKDate;
                    cashPaymentJournal.Description = voucher.Description;
                    cashPaymentJournal.Number = model.VoucherNumber;
                    cashPaymentJournal.BranchId = branch.Id;
                    entities.VoucherPayments.AddObject(paymentVoucher);

                    entities.CashPaymentJournals.AddObject(cashPaymentJournal);
                    entities.SaveChanges();

                    if (dhhdJournal != null)
                    {
                        //dhhdJournal.VoucherId = id;
                        dhhdJournal.PaymentDate = model.PaymentDate;
                        dhhdJournal.Status = 2;
                        entities.SaveChanges();
                    }

                    //var getNewAdded = (from c in entities.CashPaymentJournals
                    //                   select c.Id).Max();


                    //--------------------add to cash payment detail journal from voucher detail-------------//
                    for (int i = 0; i < voucherDetail.Count; i++)
                    {
                        var newDetailJBK = new CashPaymentJournalDetail();
                        if (voucherDetail[i].Account.Id.Equals(voucherDetail[i].Voucher.Partner.AccountId))
                        {
                            newDetailJBK.CashPaymentJournalId = cashPaymentJournal.Id;
                            newDetailJBK.AccountId = voucherDetail[i].AccountId;
                            newDetailJBK.Debet = voucherDetail[i].Kredit;
                            newDetailJBK.MasterBranch = branch.Id;
                            newDetailJBK.BranchId = voucherDetail[i].BranchId;
                            entities.CashPaymentJournalDetails.AddObject(newDetailJBK);
                            entities.SaveChanges();
                        }
                    }

                    //GET value Utang from JBKDetail
                    var debt = entities.CashPaymentJournalDetails.Where(a => a.CashPaymentJournalId == cashPaymentJournal.Id).FirstOrDefault();
               
                    
                    var newDetailCash = new CashPaymentJournalDetail();
                    newDetailCash.CashPaymentJournalId = cashPaymentJournal.Id;
                    newDetailCash.AccountId = model.AccountId;
                    newDetailCash.Kredit = debt.Debet;
                    newDetailCash.BranchId = getBranchCode.Id;
                    newDetailCash.MasterBranch = branch.Id;

                    entities.CashPaymentJournalDetails.AddObject(newDetailCash);
                    entities.SaveChanges();

                    Session.Remove(PaymentSessionName);
                    saveHistory(model.VoucherNumber);
                    return RedirectToAction("Index");
                }
            //    catch (Exception ex)
            //    {
            //        Logger.Error("Error while executing PaymentController.Paid[Post].", ex);
            //        SetErrorMessageViewData("Data tidak dapat dibuat.");
            //    }
            //}
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id, string jbk = "")
        {
            Session.Remove(PaymentSessionName);

            var voucher = (from v in entities.VoucherPayments
                           where v.VoucherId == id
                           select v).Count();

            if (voucher <= 0)
                return RedirectToAction("Index");

            var payment = (from p in entities.Vouchers
                           where p.Id == id
                           select new VoucherPaymentViewModel
                           {
                               VoucherNumber = p.Number,
                               TransactionDate = p.TransactionDate,
                               PartnerName = p.Partner.Name,
                               PartnerId = p.PartnerId,
                               Description = p.Description,
                               ApproveDate = p.ApproveDate,
                               //VoucherStatusId = p.VoucherStatusId,
                               VoucherStatusName = p.SystemVoucherStatu.Name,
                               BranchId = p.BranchId,
                               CheckNumber = p.VoucherPayment == null ? "" : p.VoucherPayment.CheckNumber,
                               PaymentDate = p.VoucherPayment == null ? null : p.VoucherPayment.PaymentDate,
                               JBKDate = p.VoucherPayment == null ? new DateTime(1900, 1, 1) : p.VoucherPayment.JBKDate,
                               Bank = p.VoucherPayment.Bank.Name,
                               BankId = p.VoucherPayment.BankId,
                               //Status = p.PaymentStatus,
                               AccountId = p.VoucherPayment.AccountId,
                               VoucherStatusDescription = p.VoucherPayment.Voucher.SystemVoucherStatu.Description,
                               AccountCode = (p.VoucherPayment.Branch.Id == null) ? p.VoucherPayment.Account.Code : p.VoucherPayment.Account.Code + "." + p.VoucherPayment.Branch.Code
                           }).FirstOrDefault();
            
            ViewData["JBK"] = jbk;
            return View(payment);
        }

        [HttpPost]
        public ActionResult Edit(VoucherPaymentViewModel model, string jbk = "")
        {
            var branch = GetCurrentUserBranchId();
            var paymentDetailSessionList = (List<VoucherDetailViewModel>)(Session[PaymentSessionName]);
            if (paymentDetailSessionList == null || paymentDetailSessionList.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail pembayaran.");

            var voucher = (from vp in entities.Vouchers
                           where vp.Id == model.Id
                           select vp).FirstOrDefault();

            string transactionDateString = Request.Form["TransactionDate"];
            string jbkDateString = Request.Form["JBKDate"];
            string paymentDateString = Request.Form["PaymentDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);
            DateTime paymentDate;
            DateTime.TryParse(paymentDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out paymentDate);
            DateTime jbkDate;
            DateTime.TryParse(jbkDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out jbkDate);
            model.JBKDate = jbkDate;
            model.PaymentDate = paymentDate;
            model.TransactionDate = transactionDate;

            var transdatemonth = voucher.TransactionDate.Month;
            var transdateyear = voucher.TransactionDate.Year;
            var transdateday = voucher.TransactionDate.Day;
            var dttransaction = new DateTime(transdateyear, transdatemonth, 1);
            var jbkmonth = DateTime.Now.Month;
            var jbkyear = DateTime.Now.Year;
            var jbkday = DateTime.Now.Day;
            var dtjbk = new DateTime(jbkyear, jbkmonth, 1);
            int resultCek = DateTime.Compare(dttransaction, dtjbk);

            var userBranch = GetCurrentUserBranchId();
            var error = ModelState.ToList();
            if (error.Count > 0)
            {
                foreach (var a in error)
                {
                    ModelState.Remove(a);
                }
            }

            if (resultCek != 0)
            {
                var nextmonth = transdatemonth + 1;
                if (nextmonth == jbkmonth && transdateyear == jbkyear)
                {
                    if (jbkday >= 4)
                    {
                        ModelState.AddModelError("JBKDate", "Pembayaran Voucher "
                                                   + string.Format("{0:y}", voucher.TransactionDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("JBKDate", "Pembayaran Voucher Bulan "
                                                + string.Format("{0:y}", voucher.TransactionDate) + " telah ditutup, Hubungi Administrator!");
                }
            }


            if (model.CheckNumber == null)
            {
                ModelState.AddModelError("CheckNumber", "Nomor Cek/Giro harus diisi!");

            }

            if (model.AccountCode == null)
            {
                ModelState.AddModelError("AccountId", "No Kas/Bank harus diisi!");
            }

            if (model.Bank == null)
            {
                ModelState.AddModelError("BankId", "Bank harus diisi!");
            }

            if (ModelState.IsValid)
            {

                //try
                //{
                    var voucherPayment = (from vp in entities.VoucherPayments
                                          where vp.VoucherId == model.Id
                                          select vp).FirstOrDefault();

                    
                    var journalPayment = (from jp in entities.CashPaymentJournals
                                          where jp.VoucherId == voucher.Id
                                          select jp).FirstOrDefault();

                    var journalDHHD = (from a in entities.DHHDJournals
                                       where a.VoucherId == voucher.Id
                                       select a).FirstOrDefault();

                    if (voucherPayment == null && journalPayment == null && journalDHHD == null)
                    {
                        return RedirectToAction("Index");
                    }
                    //voucherPayment.VoucherId = model.VoucherId;


                    char[] reverseCode = model.AccountCode.Trim().ToCharArray();
                    Array.Reverse(reverseCode);

                    string code = new string(reverseCode.Take(2).Reverse().ToArray()); //model.AccountCode.Substring(9, 2);
                    string accountCode = new string(reverseCode.Skip(3).Reverse().ToArray()); //model.AccountCode.Substring(0, 8)
                    var codes = entities.Accounts.Where(a => a.Code == accountCode).FirstOrDefault();
                    var getBranchCode = entities.Branches.Where(a => a.Code == code).FirstOrDefault();

                    voucherPayment.CheckNumber = model.CheckNumber;
                    voucherPayment.PaymentDate = model.PaymentDate;
                    voucherPayment.JBKDate = model.JBKDate;
                    voucherPayment.BankId = model.BankId;
                    voucherPayment.AccountId = model.AccountId;
                    voucherPayment.BranchId = getBranchCode.Id;

                    //---------------------- UPDATE VOUCHER DETAIL ------------------------//
                    foreach (var itemVoucher in entities.VoucherDetails.Where(p => p.VoucherId == voucher.Id))
                    {
                        entities.VoucherDetails.DeleteObject(itemVoucher);
                    }

                        // insert new detail
                    foreach (var item in paymentDetailSessionList)
                    {
                        var detailVoucher = new VoucherDetail
                        {
                            AccountId = item.AccountId,
                            VoucherId = voucher.Id,
                            Debet = item.Debet,
                            Kredit = item.Kredit,
                            BranchId = item.BranchId,
                            MasterBranch = branch.Id 
                        };
                        entities.VoucherDetails.AddObject(detailVoucher);
                    }
                    //entities.SaveChanges();
                    var voucherDetail = new VoucherDetail();

                    voucherDetail.VoucherId = voucher.Id;
                    voucherDetail.AccountId = voucher.Partner.AccountId;
                    voucherDetail.Kredit = model.AmountPaid;
                    voucherDetail.BranchId = voucher.BranchId;
                    voucherDetail.MasterBranch = voucher.BranchId;
                    entities.VoucherDetails.AddObject(voucherDetail);
                    //entities.SaveChanges();

                    //----------------- UPDATE DHHD WHERE STATUS = 2 -----------------//
                    journalDHHD.PaymentDate  = model.PaymentDate;
                    journalDHHD.Status = 2;

                    //----------------- UPDATE DHHD DETAIL ------------------------//
                    foreach (var itemDHHD in entities.DHHDJorurnalDetails.Where(p => p.DHHDJournalId == journalDHHD.Id))
                    {
                        entities.DHHDJorurnalDetails.DeleteObject(itemDHHD);
                    }


                    foreach (var item in paymentDetailSessionList)
                    {
                        var detailDHHD = new DHHDJorurnalDetail
                        {
                            AccountId = item.AccountId,
                            DHHDJournalId = journalDHHD.Id,
                            Debet = item.Debet,
                            Kredit = item.Kredit,
                            BranchId = item.BranchId,
                            MasterBranch = branch.Id 
                        };
                        entities.DHHDJorurnalDetails.AddObject(detailDHHD);
                    }

                    var dhhdDetail = new DHHDJorurnalDetail();

                    dhhdDetail.DHHDJournalId = journalDHHD.Id;
                    dhhdDetail.AccountId = voucher.Partner.AccountId;
                    dhhdDetail.Kredit = model.AmountPaid;
                    dhhdDetail.BranchId = voucher.BranchId;
                    dhhdDetail.MasterBranch = branch.Id;
                    entities.DHHDJorurnalDetails.AddObject(dhhdDetail);
                    //entities.SaveChanges();

                    //------------------------ UPDATE JBK ------------------------//
                    journalPayment.CheckNumber = model.CheckNumber;
                    voucherPayment.PaymentDate = model.PaymentDate;
                    journalPayment.JBKDate = model.JBKDate;

                    //----------------------- UPDATE JBK DETAIL ------------------//

                    foreach (var itemJBK in entities.CashPaymentJournalDetails.Where(p => p.CashPaymentJournalId == journalPayment.Id))
                    {
                        entities.CashPaymentJournalDetails.DeleteObject(itemJBK);
                    }
                    
                    // ------------------------------ TAX -----------------
                    var newJBKDetail = new CashPaymentJournalDetail();
                    newJBKDetail.CashPaymentJournalId = journalPayment.Id;
                    newJBKDetail.AccountId = codes.Id;
                    newJBKDetail.BranchId = getBranchCode.Id;
                    newJBKDetail.Kredit = model.AmountPaid;
                    newJBKDetail.MasterBranch = branch.Id;
                    entities.CashPaymentJournalDetails.AddObject(newJBKDetail);
                    //entities.SaveChanges();

                    // ------------------------------ DEBT --------------------------//
                    var newDetailJBK = new CashPaymentJournalDetail();
                    newDetailJBK.CashPaymentJournalId = journalPayment.Id;
                    newDetailJBK.AccountId = voucher.Partner.AccountId;
                    newDetailJBK.BranchId = voucher.BranchId;
                    newDetailJBK.Debet = model.AmountPaid;
                    newDetailJBK.MasterBranch = branch.Id;
                    //newDetailJBK.MasterBranch = 
                    entities.CashPaymentJournalDetails.AddObject(newDetailJBK);

                    entities.SaveChanges();
                    saveHistory(model.VoucherNumber);

                    if (jbk != "")
                    {
                        return RedirectToAction("../CashPaymentJournal/Index", new { Area = "SKA" });
                    }
                    else { 
                        return RedirectToAction("Index");
                    }
                }
                //catch (Exception ex)
            //    {
            //        ViewData["Message"] = "Data tidak dapat diubah. Detail: " + ex.Message;
            //    }
            //}
            ViewData["JBK"] = jbk;
            return View(model);
        }

        [GridAction]
        public ActionResult GetAccountCodeList()
        {
            string cashAndBankCode = System.Configuration.ConfigurationManager.AppSettings["cashAndBankCode"];
            var branch = GetCurrentUserBranchId();
            var account = from c in entities.Accounts
                          where c.Code.StartsWith(cashAndBankCode)
                          select new ChartofAccountViewModel { 
                            Id = c.Id,
                            Code = c.Code,
                            Name = c.Name
                          };

            if (branch.Code == "00")
            {
                account = from a in account
                          from b in entities.Branches
                          select new ChartofAccountViewModel
                          {
                              Id = a.Id,
                              Code = a.Code + "." + b.Code,
                              Name = a.Name + " " + b.Name
                          };
            }
            else {
                account = from a in account
                          select new ChartofAccountViewModel { 
                            Id = a.Id,
                            Code = a.Code + "." + branch.Code,
                            Name = a.Name + " " + branch.Name
                          };
            }
   
            //var account1 = from c in entities.Accounts
            //               where c.Code.Length.Equals(11)
            //               select new ChartofAccountViewModel
            //               {
            //                   Id = c.Id,
            //                   Code = c.Code,
            //                   Name = c.Name
            //               };

            //var account2 = from c in entities.Accounts
            //               where c.Code.Length.Equals(8)
            //               select new ChartofAccountViewModel
            //               {
            //                   Id = c.Id,
            //                   Code = c.Code + "." + branch.Code,
            //                   Name = c.Name + " " + branch.Name
            //               };

            var model = account;

            return View(new GridModel<ChartofAccountViewModel>(model));
        }

        [GridAction]
        public ActionResult GetBankList()
        {
            var model = from m in entities.Banks
                        select new BankViewModel
                        {
                            Id = m.Id,
                            BankName = m.Name,
                            ACNumber = m.ACNumber
                        };
            return View(new GridModel<BankViewModel>(model));
        }
    }
}

