﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.SKA.Models.ViewModels;

namespace SKA.Areas.SKA.Controllers
{
    public class AdditionalCorrectionController : BaseController
    {
        //
        // GET: /SKA/AdditionalCorrection/
        private SKAEntities entities;
        private string additionalCorrectionSessionName = "AdditionalCorrectionSession";

        public AdditionalCorrectionController()
        {
            entities = new SKAEntities();
        }

        [GridAction]
        public ActionResult GetList() 
        {
            var model = from c in entities.CorrectionandBackups
                        where c.Name == "Koreksi Penambahan"
                        select new AdditionalCorrectionViewModel 
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            Year = c.Year
                        };
            return View(new GridModel<AdditionalCorrectionViewModel>(model));
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            Session.Remove(additionalCorrectionSessionName);

            var model = new AdditionalCorrectionViewModel();
            model.Name = "Koreksi Penambahan";
            model.Year = DateTime.Now.Year;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AdditionalCorrectionViewModel model)
        {
            var additionalCorrectionDetailSessionlist = (List<AdditionalCorrectionDetailViewModel>)(Session[additionalCorrectionSessionName]);

            //if (additionalCorrectionDetailSessionlist == null || additionalCorrectionDetailSessionlist.Count <= 0)
            //    ModelState.AddModelError("", "Silahkan isi detail koreksi penambahan.");

            if (ModelState.IsValid)
            {
                try
                {
                    var newAdditionalCorrection = new CorrectionandBackup();
                    var existItem = entities.CorrectionandBackups.Where(a => a.Year == model.Year && a.Name != "Koreksi Pengurangan" && a.Name != "Cadangan").FirstOrDefault();

                    if (existItem == null)
                    {
                        newAdditionalCorrection.Name = model.Name;
                        newAdditionalCorrection.Description = model.Description;
                        newAdditionalCorrection.Year = model.Year;

                        entities.CorrectionandBackups.AddObject(newAdditionalCorrection);

                        foreach (var item in additionalCorrectionDetailSessionlist)
                        {
                            var detail = new CorrectionandBackupDetail
                            {
                                CorrectionandBackupId = newAdditionalCorrection.Id,
                                AccountId = item.AccountId,
                                Amount = item.Amount,
                                BranchId = item.BranchId
                            };

                            entities.CorrectionandBackupDetails.AddObject(detail);
                        }

                        entities.SaveChanges();
                        saveHistory(model.Name);
                        Session.Remove(additionalCorrectionSessionName);

                        return RedirectToAction("Index");
                    }
                    else {
                        SetErrorMessageViewData("Data has existed.");
                    }
                    
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing AdditionalCorrectionController.Create[Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }

        [GridAction]
        public ActionResult _SelectDetail(int? id)
        {
            var model = (List<AdditionalCorrectionDetailViewModel>)Session[additionalCorrectionSessionName];
            var branch = GetCurrentUserBranchId();
            if (model == null)
            {
                model = (from data in entities.CorrectionandBackupDetails
                         where data.CorrectionandBackupId.Equals(id.Value)
                         select new AdditionalCorrectionDetailViewModel
                         {
                             CorrectionandBackupId = data.CorrectionandBackupId,
                             AccountId = data.AccountId,
                             AccountCode = (data.Account.Code.Length.Equals(8)) ? data.Account.Code + "." + branch.Code + " - " + data.Account.Name + " " + branch.Name : data.Account.Code + " - " + data.Account.Name,
                             AccountName = data.Account.Name,
                             Amount = data.Amount,
                             BranchId = data.BranchId
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[additionalCorrectionSessionName] = model;
            }

            return View(new GridModel<AdditionalCorrectionDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _InsertDetail()
        {
            var model = (List<AdditionalCorrectionDetailViewModel>)Session[additionalCorrectionSessionName];
            AdditionalCorrectionDetailViewModel postmodel = new AdditionalCorrectionDetailViewModel();
            var branch = GetCurrentUserBranchId();
            if (model == null)
                model = new List<AdditionalCorrectionDetailViewModel>();

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

                        //var getFirstBalance = entities.BeginningBalanceBranches.Where(a => a.AccountId == account.FirstOrDefault().Id && a.BranchId == getBranch.Id).FirstOrDefault();

                        //if (account.FirstOrDefault().Code.StartsWith("8") || account.FirstOrDefault().Code.StartsWith("9"))
                        //{
                        //    postmodel.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                        //    postmodel.AccountId = account.FirstOrDefault().Id;
                        //    postmodel.BranchId = getBranch.Id;
                        //}
                        //else
                        //{
                        //    if (getFirstBalance != null)
                        //    {
                        //        if (getFirstBalance.Debet == null && getFirstBalance.Kredit == null)
                        //        {
                        //            ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                        //            return View(new GridModel<AdditionalCorrectionDetailViewModel>(model));
                        //        }
                        //        else
                        //        {
                        postmodel.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                        postmodel.AccountId = account.FirstOrDefault().Id;
                        postmodel.BranchId = getBranch.Id;
                        //postmodel.CorrectionandBackupId = postmodel.CorrectionandBackupId;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                        //        return View(new GridModel<AdditionalCorrectionDetailViewModel>(model));
                        //    }
                        //}

                    }
                    else
                    {
                        var getUserBranch = GetCurrentUserBranchId();
                        postmodel.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                        postmodel.AccountId = account.FirstOrDefault().Id;
                        postmodel.BranchId = getUserBranch.Id;
                        //postmodel.CorrectionandBackupId = postmodel.CorrectionandBackupId;
                    }


                    int maxId = 0;

                    if (model.Count > 0)
                        maxId = model.Max(m => m.Id);

                    postmodel.Id = maxId + 1;

                    model.Insert(0, postmodel);

                    Session[additionalCorrectionSessionName] = model;
                }
                else
                {
                    ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                }
            }

            return View(new GridModel<AdditionalCorrectionDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<AdditionalCorrectionDetailViewModel>)Session[additionalCorrectionSessionName];
            AdditionalCorrectionDetailViewModel postmodel = new AdditionalCorrectionDetailViewModel();

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

                            //var getFirstBalance = entities.BeginningBalanceBranches.Where(a => a.AccountId == account.FirstOrDefault().Id && a.BranchId == getBranch.Id).FirstOrDefault();

                            //if (account.FirstOrDefault().Code.StartsWith("8") || account.FirstOrDefault().Code.StartsWith("9"))
                            //{
                            item.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                            item.AccountId = account.FirstOrDefault().Id;
                            item.Amount = postmodel.Amount;
                            item.BranchId = getBranch.Id;
                            //}
                            //else
                            //{
                            //    if (getFirstBalance != null)
                            //    {
                            //        if (getFirstBalance.Debet == null && getFirstBalance.Kredit == null)
                            //        {
                            //            ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                            //            return View(new GridModel<PettyCashesDetailViewModel>(model));
                            //        }
                            //        else
                            //        {
                            //            item.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                            //            item.AccountId = account.FirstOrDefault().Id;
                            //            item.Debet = postmodel.Debet;
                            //            item.BranchId = getBranch.Id;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                            //        return View(new GridModel<PettyCashesDetailViewModel>(model));
                            //    }
                            //}

                        }
                        else
                        {
                            var getUserBranch = GetCurrentUserBranchId();
                            item.AccountId = account.FirstOrDefault().Id;
                            item.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                            item.Amount = postmodel.Amount;
                            item.BranchId = getUserBranch.Id;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                    }
                }
            }

            return View(new GridModel<AdditionalCorrectionDetailViewModel>(model));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetail()
        {
            IList<AdditionalCorrectionDetailViewModel> model = (IList<AdditionalCorrectionDetailViewModel>)Session[additionalCorrectionSessionName];
            var detail = new AdditionalCorrectionDetailViewModel();

            if (!TryUpdateModel<AdditionalCorrectionDetailViewModel>(detail))
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

            return View(new GridModel<AdditionalCorrectionDetailViewModel>(model));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Session.Remove(additionalCorrectionSessionName);

            var model = (from d in entities.CorrectionandBackups
                         where d.Id == id
                         select new AdditionalCorrectionViewModel
                         {
                             Name = d.Name,
                             Description = d.Description,
                             Year = d.Year
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AdditionalCorrectionViewModel model)
        {
            var additionalCorrectionDetailSessionlist = (List<AdditionalCorrectionDetailViewModel>)(Session[additionalCorrectionSessionName]);

            //if (additionalCorrectionDetailSessionlist == null || additionalCorrectionDetailSessionlist.Count <= 0)
            //    ModelState.AddModelError("", "Silahkan isi detail koreksi penambahan.");

            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.CorrectionandBackups
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    if (newDetail == null)
                    {
                        return RedirectToAction("Index");
                    }

                    newDetail.Name = model.Name;
                    newDetail.Description = model.Description;
                    newDetail.Year = model.Year;
                    

                    // delete old data first, before inserting new detail
                    foreach (var item in entities.CorrectionandBackupDetails.Where(p => p.CorrectionandBackupId == newDetail.Id))
                    {
                        entities.CorrectionandBackupDetails.DeleteObject(item);
                    }

                    // insert new detail
                    foreach (var item in additionalCorrectionDetailSessionlist)
                    {
                        var detail = new CorrectionandBackupDetail
                        {
                            CorrectionandBackupId = newDetail.Id,
                            AccountId = item.AccountId,
                            Amount = item.Amount,
                            BranchId = item.BranchId
                        };

                        entities.CorrectionandBackupDetails.AddObject(detail);
                    }

                    entities.SaveChanges();
                    saveHistory(model.Name);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing AdditionalCorrectionController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var deleteItem = (from c in entities.CorrectionandBackups
                                  where c.Id == id
                                  select new AdditionalCorrectionViewModel
                                  {
                                      Name = c.Name,
                                      Description = c.Description,
                                      Year = c.Year
                                  }).FirstOrDefault();

                if (deleteItem == null)
                {
                    return RedirectToAction("Index");
                }

                return View(deleteItem);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing AdditionalCorrectionController.Delete [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(AdditionalCorrectionViewModel model)
        {
            try
            {
                var deleteItem = (from c in entities.CorrectionandBackups
                                  where c.Id == model.Id
                                  select c).FirstOrDefault();

                if (deleteItem != null)
                {
                    foreach (var item in entities.CorrectionandBackupDetails.Where(p => p.CorrectionandBackupId == deleteItem.Id))
                    {
                        entities.CorrectionandBackupDetails.DeleteObject(item);
                    }

                    entities.CorrectionandBackups.DeleteObject(deleteItem);
                    entities.SaveChanges();
                    saveHistory(deleteItem.Name);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing AdditionalCorrectionController.Delete [Post].", ex);
                SetErrorMessageViewData("Data cannot be deleted.");
            }

            return RedirectToAction("Index");
        }

    }
}
