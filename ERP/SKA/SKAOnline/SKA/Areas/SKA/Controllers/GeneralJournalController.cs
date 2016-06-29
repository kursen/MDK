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
    public class GeneralJournalController : BaseController
    {
        private SKAEntities entities;

        private string GeneralJournalSessionName = "GeneralJournalSession";

        public GeneralJournalController()
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
            var model = from m in entities.GeneralJournals
                        select new GeneralJournalViewModel
                        {
                            Description = m.Description,
                            EvidenceNumber = m.EvidenceNumber,
                            Id = m.Id,
                            DocumentDate = m.DocumentDate,
                            ClosingDate = entities.Closings.Select(a => a.ClosingDate).FirstOrDefault()
                        };

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.EvidenceNumber.Contains(searchValue)
                    || a.Description.Contains(searchValue));
            }
            return View(new GridModel<GeneralJournalViewModel>(model));
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

        [GridAction]
        public ActionResult _SelectDetail(Guid? id)
        {
            var model = (List<GeneralJournalDetailViewModel>)Session[GeneralJournalSessionName];
            var branch = GetCurrentUserBranchId();
            if (model == null)
            {
                model = (from data in entities.GeneralJournalDetails
                         where data.GeneralJournalId == id
                         select new GeneralJournalDetailViewModel
                         {
                             GeneralJournalId = data.GeneralJournalId,
                             AccountId = data.AccountId,
                             AccountCode = (data.Account.Code.Length.Equals(8)) ? data.Account.Code + "." + data.Branch.Code + " - " + data.Account.Name + " " + data.Branch.Name : data.Account.Code + " - " + data.Account.Name,
                             AccountName = data.Account.Name,
                             Debet = data.Debit,
                             Kredit = data.Credit,
                             BranchId = data.BranchId,
                             MasterBranch = data.MasterBranch 
                         }).ToList();

                int counter = 0;
                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }
                Session[GeneralJournalSessionName] = model;
            }
            return View(new GridModel<GeneralJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [GridAction()]
        public ActionResult _InsertDetail()
        {
            var model = (List<GeneralJournalDetailViewModel>)Session[GeneralJournalSessionName];
            GeneralJournalDetailViewModel postmodel = new GeneralJournalDetailViewModel();
            var branch = GetCurrentUserBranchId();
            if (model == null)
                model = new List<GeneralJournalDetailViewModel>();

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
                            var getUserBranch = GetCurrentUserBranchId();
                            postmodel.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                            postmodel.AccountId = account.FirstOrDefault().Id;
                            postmodel.BranchId = getBranch.Id;
                            postmodel.MasterBranch = getUserBranch.Id;

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
                                    return View(new GridModel<GeneralJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                                }
                                else
                                {
                                    var getUserBranch = GetCurrentUserBranchId();
                                    postmodel.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                                    postmodel.AccountId = account.FirstOrDefault().Id;
                                    postmodel.BranchId = getBranch.Id;
                                    postmodel.MasterBranch = getUserBranch.Id;

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
                                return View(new GridModel<GeneralJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                            }
                        }
                    }
                    else
                    {
                        var getUserBranch = GetCurrentUserBranchId();
                        postmodel.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                        postmodel.AccountId = account.FirstOrDefault().Id;
                        postmodel.BranchId = getUserBranch.Id;
                        postmodel.MasterBranch = getUserBranch.Id;

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

                    Session[GeneralJournalSessionName] = model;
                }
                else
                {
                    ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                }
            }

            return View(new GridModel<GeneralJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }
        

        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<GeneralJournalDetailViewModel>)Session[GeneralJournalSessionName];
            GeneralJournalDetailViewModel postmodel = new GeneralJournalDetailViewModel();

            var branch = GetCurrentUserBranchId();

            bool cek = TryUpdateModel(postmodel);
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
                                        return View(new GridModel<GeneralJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                                    }
                                    else
                                    {
                                        item.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                                        item.AccountId = account.FirstOrDefault().Id;
                                        item.Debet = postmodel.Debet;
                                        item.Kredit = postmodel.Kredit;
                                        item.BranchId = getBranch.Id;
                                        item.MasterBranch = branch.Id;

                                        var error2= ModelState.ToList();
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
                                    return View(new GridModel<GeneralJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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

                            var error3 = ModelState.ToList();
                            if (error3.Count > 0)
                            {
                                foreach (var a in error3)
                                {
                                    ModelState.Remove(a);
                                }
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                    }
                }
            }

            return View(new GridModel<GeneralJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetail()
        {
            IList<GeneralJournalDetailViewModel> model = (IList<GeneralJournalDetailViewModel>)Session[GeneralJournalSessionName];
            var detail = new GeneralJournalDetailViewModel();

            if (!TryUpdateModel<GeneralJournalDetailViewModel>(detail))
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

            return View(new GridModel<GeneralJournalDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

       
        [HttpGet]
        public ActionResult Create()
        {
			Session.Remove(GeneralJournalSessionName);
			var model = new GeneralJournalViewModel();
            model.DocumentDate = DateTime.Now;

            var branch = (from c in entities.Branches
                         select c);

            var branchCode = new SelectList(branch.ToList(), "Id", "Name");
            ViewBag.Branch = branchCode;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(GeneralJournalViewModel model)
        {
            
            var generalJournalDetailSessionlist = (List<GeneralJournalDetailViewModel>)(Session[GeneralJournalSessionName]);
            BranchViewModel branch;
            branch = GetCurrentUserBranchId();

            if (generalJournalDetailSessionlist == null || generalJournalDetailSessionlist.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail jurnal.");

            var cabang = (from c in entities.Branches
                          select c);

            var branchCode = new SelectList(cabang.ToList(), "Id", "Name");
            ViewBag.Branch = branchCode;

            string transactionDateString = Request.Form["DocumentDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);
            model.DocumentDate = transactionDate;
            var dttransaction = new DateTime(transactionDate.Year, transactionDate.Month, 1);
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
                var nextmonth = transactionDate.Month + 1;
                if (nextmonth == transmonth && transactionDate.Year == transkyear)
                {
                    if (transday > 4)
                    {
                        ModelState.AddModelError("DocumentDate", "Jurnal Umum Bulan "
                                                   + string.Format("{0:y}", transactionDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("DocumentDate", "Jurnal Umum Bulan "
                                                + string.Format("{0:y}", transactionDate) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.AmountDebet == model.AmountKredit)
                {
                    try
                    {

                        var newData = new GeneralJournal();
                        var prevData = (from a in entities.GeneralJournals
                                        where a.EvidenceNumber == model.EvidenceNumber
                                        select a).FirstOrDefault();

                        if (prevData == null)
                        {
                            newData.Id = Guid.NewGuid();
                            newData.EvidenceNumber = model.EvidenceNumber;
                            newData.DocumentDate = model.DocumentDate;
                            newData.Description = model.Description;
                            if (model.BranchCode == null)
                            {
                                newData.BranchId = branch.Id;
                            }
                            else {
                                newData.BranchId = model.BranchCode;
                                newData.Month = 1;
                            }
                           

                            entities.GeneralJournals.AddObject(newData);

                            foreach (var item in generalJournalDetailSessionlist)
                            {
                                if (model.BranchCode == null)
                                {
                                    var detail = new GeneralJournalDetail
                                    {
                                        GeneralJournalId = newData.Id,
                                        AccountId = item.AccountId,
                                        Debit = item.Debet,
                                        Credit = item.Kredit,
                                        BranchId = item.BranchId,
                                        MasterBranch = item.MasterBranch
                                    };
                                    entities.GeneralJournalDetails.AddObject(detail);
                                }
                                else
                                {
                                    var tempbranchid = model.BranchCode;
                                    var detail = new GeneralJournalDetail
                                    {
                                        GeneralJournalId = newData.Id,
                                        AccountId = item.AccountId,
                                        Debit = item.Debet,
                                        Credit = item.Kredit,
                                        BranchId = (int)tempbranchid,
                                        MasterBranch = tempbranchid,
                                       
                                    };
                                    entities.GeneralJournalDetails.AddObject(detail);
                                }

                            }

                            entities.SaveChanges();
                            saveHistory(model.EvidenceNumber);
                            Session.Remove(GeneralJournalSessionName);

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("EvidenceNumber","Nomor bukti sudah ada, silahkan masukkan data dengan nomor bukti yang berbeda.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error while executing GeneralJournalController.Create [Post].", ex);
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
            Session.Remove(GeneralJournalSessionName);

            var model = new GeneralJournalViewModel();

            try
            {
                var result = (from r in entities.GeneralJournals
                              where r.Id == id
                              select r).FirstOrDefault();
                var cabang = (from c in entities.Branches
                              select c);

                var branchCode = new SelectList(cabang.ToList(), "Id", "Name",result.BranchId);
                ViewBag.Branch = branchCode;

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
                    model.Month = result.Month;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing GeneralJournalController.Edit[Get].", ex);
                SetErrorMessageViewData("Data tidak dapat ditampilkan");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(GeneralJournalViewModel model)
        {
            var GeneralJournalDetailSessionlist = (List<GeneralJournalDetailViewModel>)(Session[GeneralJournalSessionName]);

            if (GeneralJournalDetailSessionlist == null || GeneralJournalDetailSessionlist.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail jurnal.");

            var cabang = (from c in entities.Branches
                          select c);

            var branchCode = new SelectList(cabang.ToList(), "Id", "Name");
            ViewBag.Branch = branchCode;

            string transactionDateString = Request.Form["DocumentDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);
            model.DocumentDate = transactionDate;
            var dttransaction = new DateTime(transactionDate.Year, transactionDate.Month, 1);
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
                var nextmonth = transactionDate.Month + 1;
                if (nextmonth == transmonth && transactionDate.Year == transkyear)
                {
                    if (transday > 4)
                    {
                        ModelState.AddModelError("DocumentDate", "Jurnal Umum Bulan "
                                                   + string.Format("{0:y}", transactionDate) + " telah ditutup, Hubungi Administrator!");
                    }
                }
                else
                {
                    ModelState.AddModelError("DocumentDate", "Jurnal Umum Bulan "
                                                + string.Format("{0:y}", transactionDate) + " telah ditutup, Hubungi Administrator!");
                }
            }

            if (ModelState.IsValid)
            {
                if (model.AmountDebet == model.AmountKredit)
                {
                    try
                    {

                        var general = (from p in entities.GeneralJournals
                                       where p.Id == model.Id
                                       select p).FirstOrDefault();
                        //general.Id = model.Id;

                        var prevData = (from a in entities.GeneralJournals
                                        where a.EvidenceNumber == model.EvidenceNumber && a.Id != model.Id
                                        select a).FirstOrDefault();

                        if (prevData == null)
                        {
                            general.DocumentDate = model.DocumentDate;
                            general.Description = model.Description;
                            general.EvidenceNumber = model.EvidenceNumber;
                            if (model.BranchCode != null)
                            {
                                general.BranchId = model.BranchCode;
                            }

                            if (model.BranchCode == null)
                            {
                                general.Month = null;
                            }
                            else
                            {
                                general.Month = 1;
                            }

                            // delete old data first, before inserting new detail
                            foreach (var item in entities.GeneralJournalDetails.Where(p => p.GeneralJournalId == general.Id))
                            {
                                entities.GeneralJournalDetails.DeleteObject(item);
                            }

                            // insert new detail
                            foreach (var item in GeneralJournalDetailSessionlist)
                            {
                                if (model.BranchCode == null)
                                {
                                    var detail = new GeneralJournalDetail
                                    {
                                        GeneralJournalId = general.Id,
                                        AccountId = item.AccountId,
                                        Debit = item.Debet,
                                        Credit = item.Kredit,
                                        BranchId = item.BranchId,
                                        MasterBranch = item.MasterBranch
                                    };
                                    entities.GeneralJournalDetails.AddObject(detail);
                                }
                                else
                                {

                                    var tempbranchid = model.BranchCode;
                                    var detail = new GeneralJournalDetail
                                    {
                                        GeneralJournalId = general.Id,
                                        AccountId = item.AccountId,
                                        Debit = item.Debet,
                                        Credit = item.Kredit,
                                        BranchId = (int)tempbranchid,
                                        MasterBranch = tempbranchid,

                                    };
                                    entities.GeneralJournalDetails.AddObject(detail);
                                }
                               
                            }

                            entities.SaveChanges();
                            saveHistory(model.EvidenceNumber);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("EvidenceNumber", "Nomor bukti sudah ada, silahkan masukkan data dengan nomor bukti yang berbeda.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error while executing GeneralJournalController.Edit[Post].", ex);
                        SetErrorMessageViewData("Data tidak dapat diubah");
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
                var model = (from m in entities.GeneralJournals
                             where m.Id == id
                             select new GeneralJournalViewModel
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
                Logger.Error("Error while executing GeneralJournalController.Delete [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(GeneralJournalViewModel model)
        {
            try
            {
                var deleteItem = (from di in entities.GeneralJournals
                                  where di.Id == model.Id
                                  select di).FirstOrDefault();

                if (deleteItem != null)
                {
                    foreach (var item in entities.GeneralJournalDetails.Where(p => p.GeneralJournalId == deleteItem.Id))
                    {
                        entities.GeneralJournalDetails.DeleteObject(item);
                    }

                    entities.GeneralJournals.DeleteObject(deleteItem);
                    entities.SaveChanges();
                    saveHistory(deleteItem.EvidenceNumber);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing GeneralJournalController.Delete [Post].", ex);
                SetErrorMessageViewData("Data cannot be deleted.");
            }
            return RedirectToAction("Index");

        }
        public ActionResult detail(Guid id)
        {
            Session.Remove(GeneralJournalSessionName);

            var model = (from m in entities.GeneralJournals
                         where m.Id == id
                         select new GeneralJournalViewModel
                         {
                             EvidenceNumber = m.EvidenceNumber,
                             DocumentDate = m.DocumentDate,
                             Description = m.Description
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult CheckNumber(string EvidenceNumber, Guid Id)
        {
            var value = "";

            if (Id != null)
            {
                var existingNumber = (from c in entities.GeneralJournals
                                      where c.EvidenceNumber == EvidenceNumber
                                      select c.EvidenceNumber).FirstOrDefault();


                if (existingNumber != null)
                {
                    value = "Nomor yang Anda masukkan sudah ada, silahkan isi dengan nomor yang lain.";
                }
                //var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 2).FirstOrDefault();
            }
            else
            {
                var existingNumberEdit = (from c in entities.GeneralJournals
                                      where c.EvidenceNumber == EvidenceNumber && c.Id != Id
                                      select c.EvidenceNumber).FirstOrDefault();


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
