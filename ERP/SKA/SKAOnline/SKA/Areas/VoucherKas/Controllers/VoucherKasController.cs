using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.VoucherKas.Models.ViewModel;
using Telerik.Web.Mvc.UI;
using SKA.Areas.VoucherKas.Models.Repositories;
using System.Threading;
using SKA.Areas.Setting.Models.ViewModels;
using System.Globalization;
using SKA.Areas.SKA.Models.ViewModels;

namespace SKA.Areas.VoucherKas.Controllers
{
    public class VoucherKasController : BaseController
    {
        private SKAEntities entities;

        private string VoucherSessionName = "VoucherDetailSessionList";
        //private string ApproveSessionName = "ApproveDateSessionList";

        public VoucherKasController()
        {
            entities = new SKAEntities();
        }

        [GridAction]
        public ActionResult GetList(string searchValue)
        {
            var branch = GetCurrentUserBranchId();
            var model = from vk in entities.Vouchers
                        where vk.VoucherStatusId == 1
                        select new VoucherViewModel
                        {
                            Id = vk.Id,
                            Number = vk.Number,
                            TransactionDate = vk.TransactionDate,
                            PartnerId = vk.PartnerId,
                            PartnerName = vk.Partner.Name,
                            Description = vk.Description,
                            Attachment = vk.Attachment,
                            BranchId = vk.BranchId,
                            ApproveDate = vk.ApproveDate
                        };

            var getClosingDate = entities.Closings.Select(a => a.ClosingDate).FirstOrDefault();

            if (branch.Code != "00")
            {
                model = model.Where(a => a.BranchId == branch.Id);
            }

            //if (getClosingDate != null)
            //{
            //    model = model.Where(a => a.TransactionDate >= getClosingDate);
            //}

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Number.Contains(searchValue)
                    || a.PartnerName.Contains(searchValue));
            }

            return View(new GridModel<VoucherViewModel>(model));
        }

        [GridAction]
        public ActionResult GetApproveList(string SearchValueApprove)
        {
            IQueryable<VoucherViewModel> model;
            BranchViewModel branch;
            if (String.IsNullOrEmpty(SearchValueApprove))
            {
                branch = GetCurrentUserBranchId();
                model = from vk in entities.Vouchers
                            where vk.ApproveDate == null && vk.VoucherStatusId == 1
                            select new VoucherViewModel
                            {
                                Id = vk.Id,
                                Number = vk.Number,
                                TransactionDate = vk.TransactionDate,
                                PartnerId = vk.PartnerId,
                                PartnerName = vk.Partner.Name,
                                Description = vk.Description,
                                Attachment = vk.Attachment,
                                BranchId = vk.BranchId,
                                ApproveDate = vk.ApproveDate

                            };
            }
            else
            {
                branch = GetCurrentUserBranchId();
                model = from vk in entities.Vouchers
                        where vk.ApproveDate == null && vk.VoucherStatusId == 1 && vk.Number.Contains(SearchValueApprove)
                            select new VoucherViewModel
                            {
                                Id = vk.Id,
                                Number = vk.Number,
                                TransactionDate = vk.TransactionDate,
                                PartnerId = vk.PartnerId,
                                PartnerName = vk.Partner.Name,
                                Description = vk.Description,
                                Attachment = vk.Attachment,
                                BranchId = vk.BranchId,
                                ApproveDate = vk.ApproveDate

                            };
            }

            if (branch.Code != "00")
            {
                model = model.Where(a => a.BranchId == branch.Id);
            }

            Session[VoucherSessionName] = model;
            return View(new GridModel<VoucherViewModel>(model));
        }

        [HttpGet]
        public ActionResult IndexApprove()
        {
            if (TempData["SearchValueApprove"] != null)
            {
                ViewBag.SearchValueApprove = TempData["SearchValueApprove"].ToString();
            }
            else
            {
                ViewBag.SearchValueApprove = string.Empty;
            }
            return View();
        }

        [HttpPost]
        public ActionResult IndexApprove(string searchValue)
        {
            TempData["SearchValueApprove"] = searchValue;
            return RedirectToAction("IndexApprove");
        }

        //[HttpPost]
        //public ActionResult IndexApprove(string searchValueApprove)
        //{
        //    TempData["SearchValueApprove"] = searchValueApprove;
        //    return RedirectToAction("IndexApprove");
        //}

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
        public ActionResult _SelectApprove(Guid? id)
        {
            var branch = GetCurrentUserBranchId();

            var orders = (from o in new SKAEntities().VoucherDetails
                          where o.VoucherId == id
                          select new VoucherDetailViewModel
                          {
                              AccountId = o.AccountId,
                              AccountCode = (o.Account.Code.Length.Equals(8)) ? o.Account.Code + "." + o.Branch.Code + " - " + o.Account.Name + " " + o.Branch.Name : o.Account.Code + " - " + o.Account.Name,
                              Debet = o.Debet,
                              Kredit = o.Kredit,
                              BranchId = o.BranchId,
                              MasterBranch = branch.Id 
                          }).ToList();

            return View(new GridModel<VoucherDetailViewModel>(orders.OrderByDescending(p => p.Debet)));
        }

        [GridAction]
        public ActionResult _UpdateApprove(VoucherViewModel postmodel)
        {
            var branch = GetCurrentUserBranchId();
            
            string approveDateString = Request.Form["ApproveDate"];
            DateTime approveDate;
            DateTime.TryParse(approveDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out approveDate);

            postmodel.ApproveDate = approveDate;
            
            var error = ModelState.ToList();
            if(error.Count > 0)
            {
                foreach(var a in error)
                {
                    ModelState.Remove(a);
                }    
            }

            if (ModelState.IsValid)
            {
                var itemAdded = (from c in entities.Vouchers
                                 where c.Id == postmodel.Id
                                 select c).FirstOrDefault();
                var detailAdded = entities.VoucherDetails.Where(d => d.VoucherId == itemAdded.Id).ToList();

                var dhhd = (from c in entities.DHHDJournals
                            where c.Number == itemAdded.Number
                            select c).FirstOrDefault();
                itemAdded.ApproveDate = postmodel.ApproveDate;
                itemAdded.VoucherStatusId = 2;
                entities.SaveChanges();
                if (dhhd == null)
                {
                    var newAdded = new DHHDJournal();
                    newAdded.Id = Guid.NewGuid();
                    newAdded.DateVoucher = itemAdded.TransactionDate;
                    newAdded.Number = itemAdded.Number;
                    newAdded.Description = itemAdded.Description;
                    newAdded.VoucherId = itemAdded.Id;
                    newAdded.BranchId = itemAdded.BranchId;
                    newAdded.Status = 1;//status DHHD ketika belum dibayar

                    entities.DHHDJournals.AddObject(newAdded);
                    entities.SaveChanges();

                    //var getNewAdded = (from c in entities.DHHDJournals
                    //                      select c.Id).Max();

                    for (int i = 0; i < detailAdded.Count; i++)
                    {
                        var newDetailDHHD = new DHHDJorurnalDetail();
                        newDetailDHHD.DHHDJournalId = newAdded.Id;
                        newDetailDHHD.AccountId = detailAdded[i].AccountId;
                        newDetailDHHD.Debet = detailAdded[i].Debet;
                        newDetailDHHD.Kredit = detailAdded[i].Kredit;
                        newDetailDHHD.BranchId = detailAdded[i].BranchId;
                        newDetailDHHD.MasterBranch = branch.Id;


                        entities.DHHDJorurnalDetails.AddObject(newDetailDHHD);
                        entities.SaveChanges();
                    }
                }
            }

            var model = from vk in entities.Vouchers
                         where vk.ApproveDate == null
                         select new VoucherViewModel
                         {
                             Id = vk.Id,
                             Number = vk.Number,
                             TransactionDate = vk.TransactionDate,
                             PartnerId = vk.PartnerId,
                             PartnerName = vk.Partner.Name,
                             Description = vk.Description,
                             Attachment = vk.Attachment,
                             BranchId = vk.BranchId,
                             ApproveDate = vk.ApproveDate
                         };

            if (branch.Code != "00")
            {
                model = model.Where(a => a.BranchId == branch.Id);
            }
            return View(new GridModel<VoucherViewModel>(model));
        }

        [HttpPost]
        public ActionResult GetPartnerNameSelectList(string text)
        {
            var partners = from a in entities.Partners
                           select a;

            if (text != null)
            {
                partners = partners.Where(a => a.Name.Contains(text));
            }
           return new JsonResult { Data = new SelectList(partners.ToList(), "Id", "Name", JsonRequestBehavior.AllowGet) };

        }

        [GridAction]
        public ActionResult _SelectDetail(Guid? id)
        {
            var branch = GetCurrentUserBranchId();
            var model = (List<VoucherDetailViewModel>)Session[VoucherSessionName];
            //var branch = GetCurrentUserBranchId();
            if (model == null)
            {
                model = (from data in entities.VoucherDetails
                         where data.VoucherId == id //&& (!data.AccountId.Equals(data.Voucher.Partner.AccountId))
                         select new VoucherDetailViewModel
                         {
                             VoucherId = data.VoucherId,
                             AccountId = data.AccountId,
                             AccountCode = data.Account.Code + "." + data.Branch.Code + " - " + data.Account.Name + " " + data.Branch.Name, //(data.Account.Code.Length.Equals(8)) ? data.Account.Code + "." + data.Branch.Code + " - " + data.Account.Name + " " + data.Branch.Name : data.Account.Code + " - " + data.Account.Name,
                             AccountName = data.Account.Name,
                             Debet = data.Debet,
                             Kredit = data.Kredit,
                             BranchId = data.BranchId,
                             MasterBranch = branch.Id 
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[VoucherSessionName] = model;
            }

            return View(new GridModel<VoucherDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }

        [GridAction]
        public ActionResult _InsertDetail()
        {
            var model = (List<VoucherDetailViewModel>)Session[VoucherSessionName];
            VoucherDetailViewModel postmodel = new VoucherDetailViewModel();
            var branch = GetCurrentUserBranchId();

            if (model == null)
            {
                model = new List<VoucherDetailViewModel>();
            }

            //var errors = ModelState
            //    .Where(x => x.Value.Errors.Count > 0)
            //    .Select(x => new { x.Key, x.Value.Errors })
            //    .ToArray();


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
                    //if (account.FirstOrDefault().Code.Length.Equals(8))
                    //{
                        var getBranch = entities.Branches.Where(a => a.Code == getBranchCode).FirstOrDefault();

                        var getFirstBalance = entities.BeginningBalanceBranches.Where(a => a.AccountId == account.FirstOrDefault().Id && a.BranchId == getBranch.Id).FirstOrDefault();

                        //if (account.FirstOrDefault().Code.StartsWith("8") || account.FirstOrDefault().Code.StartsWith("9"))
                        //{
                        //    postmodel.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                        //    postmodel.AccountId = account.FirstOrDefault().Id;
                        //    postmodel.BranchId = getBranch.Id;
                        //    postmodel.MasterBranch = branch.Id;

                        //     var error1 = ModelState.ToList();
                        //     if (error1.Count > 0)
                        //     {
                        //         foreach (var a in error1)
                        //         {
                        //             ModelState.Remove(a);
                        //         }
                        //     }

                        //}
                        //else
                        //{
                            if (getFirstBalance != null)
                            {
                                if (getFirstBalance.Debet == null && getFirstBalance.Kredit == null)
                                {
                                    ModelState.AddModelError("AccountCode", "Isi saldo awal kode perkiraan terlebih dahulu");
                                    return View(new GridModel<VoucherDetailViewModel>(model.OrderByDescending(p => p.Debet)));
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
                                return View(new GridModel<VoucherDetailViewModel>(model.OrderByDescending(p => p.Debet)));
                            }
                       // }
                    //}
                    //else
                    //{
                    //    postmodel.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                    //    postmodel.AccountId = account.FirstOrDefault().Id;
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

                    Session[VoucherSessionName] = model;
                }
                else
                {
                    ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                }
            }
           
                
            //}

            return View(new GridModel<VoucherDetailViewModel>(model.OrderByDescending(p => p.Debet)));
        }


        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<VoucherDetailViewModel>)Session[VoucherSessionName];
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
                        //if (account.FirstOrDefault().Code.Length.Equals(8))
                        //{
                            var getBranch = entities.Branches.Where(a => a.Code == getBranchCode).FirstOrDefault();

                            var getFirstBalance = entities.BeginningBalanceBranches.Where(a => a.AccountId == account.FirstOrDefault().Id && a.BranchId == getBranch.Id).FirstOrDefault();

                            //if (account.FirstOrDefault().Code.StartsWith("8") || account.FirstOrDefault().Code.StartsWith("9"))
                            //{
                            //    item.AccountCode = account.FirstOrDefault().Code + "." + getBranch.Code + " - " + account.FirstOrDefault().Name + " " + getBranch.Name;
                            //    item.AccountId = account.FirstOrDefault().Id;
                            //    item.Debet = postmodel.Debet;
                            //    item.Kredit = postmodel.Kredit;
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
                            //}
                            
                        //}
                        //else 
                        //{
                        //    item.AccountId = account.FirstOrDefault().Id;
                        //    item.AccountCode = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                        //    item.Debet = postmodel.Debet;
                        //    item.Kredit = postmodel.Kredit;
                        //    item.MasterBranch = branch.Id;

                           
                        //}
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
            IList<VoucherDetailViewModel> model = (IList<VoucherDetailViewModel>)Session[VoucherSessionName];
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

        [GridAction]
        public ActionResult ApproveList()
        {
            var model = from vk in entities.Vouchers
                        select new VoucherViewModel
                        {
                            Id = vk.Id,
                            Number = vk.Number,
                            TransactionDate = vk.TransactionDate,
                            PartnerId = vk.PartnerId,
                            PartnerName = vk.Partner.Name,
                            Description = vk.Description,
                            Attachment = vk.Attachment,
                            BranchId = vk.BranchId,
                            ApproveDate = vk.ApproveDate

                        };
            return View(new GridModel<VoucherViewModel>(model));
        }

        [HttpGet]
        public ActionResult ApproveVoucher()
        {
            if (TempData["SearchValueApprove"] != null)
            {
                ViewBag.SearchValueApprove = TempData["SearchValueApprove"].ToString();

            }
            else
            {
                ViewBag.SearchValueApprove = string.Empty;
            }
            return View();
        }

        [HttpPost]
        public ActionResult ApproveVoucher(VoucherViewModel model)
        {
            //TempData["SearchValue"] = searchValueApprove;
            var DHHDSessionList = (List<VoucherDetailViewModel>)(Session[VoucherSessionName]);
            if (ModelState.IsValid)
            {
                try
                {
                    var approve = (from a in entities.Vouchers
                                   where a.Id == model.Id  && a.VoucherStatusId == 1
                                   select a).FirstOrDefault();
                    var dhhd = (from a in entities.DHHDJournals
                                where a.Number == model.Number 
                                select a).FirstOrDefault();
                    if (approve == null)
                    {
                        return RedirectToAction("Index");
                    }
                    approve.ApproveDate = model.ApproveDate;
                    approve.VoucherStatusId = 2;

                    if (dhhd == null)
                    {
                        var newDHHD = new DHHDJournal();
                        newDHHD.Id = model.Id;
                        newDHHD.VoucherId = approve.Id;
                        newDHHD.Number = approve.Number;
                        newDHHD.DateVoucher = approve.TransactionDate;
                        newDHHD.Description = approve.Description;
                        newDHHD.BranchId = approve.BranchId;
                        newDHHD.Status = 1; //DHHD belum dibayar

                        entities.DHHDJournals.AddObject(newDHHD);
                        foreach (var item in DHHDSessionList)
                        {
                            var detail = new DHHDJorurnalDetail
                            {
                                AccountId = item.AccountId,
                                DHHDJournalId = approve.Id,
                                Debet = item.Debet,
                                Kredit = item.Kredit,
                                BranchId = item.BranchId,
                                MasterBranch = item.MasterBranch 
                            };

                            entities.DHHDJorurnalDetails.AddObject(detail);
                        }
                    }
                   
                    
                    /*var getPettyCashDetail = (from c in entities.PettyCashDetails
                                              where c.PettyCashId == c.PettyCash.Id && c.PettyCash.BranchId == branch.Id 
                                              && c.PettyCash.TransactionDate.Month == model.PostingDate.Month
                                              group c by new 
                                              {
                                                AccountId = c.AccountId,
                                                BranchId = c.BranchId
                                              } into a
                                              select new
                                              {
                                                  AccountId = a.Key.AccountId,
                                                  Debet = a.Sum(c => c.Debet),
                                                  BranchId = a.Key.BranchId
                                              }).ToList();*/
                    
                    entities.SaveChanges();

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ViewData["Message"] = "Data tidak dapat diubah. Detail: " + ex.Message;
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GetAccountCodeSelectList(string text)
        {
            var branch = GetCurrentUserBranchId();

            var branchAccount = entities.GetAccountBranch(branch.Code, text);
            var accounts = (from a in branchAccount where a.AccountCode != null
                            select new
                            {
                                AccountCode = a.AccountCode
                            }).ToList();

            return new JsonResult { Data = new SelectList(accounts, "AccountCode", "AccountCode") };
        }

        [HttpGet]
        public ActionResult Create(string dhhd = "", string jbk = "")
        {
            Session.Remove("VoucherDetailSessionList");

            var model = new VoucherViewModel();
            model.TransactionDate = DateTime.Now;

            ViewData["mode"] = GridEditMode.InLine;
            ViewData["type"] = GridButtonType.Text;
            ViewData["dhhd"] = dhhd;
            ViewData["JBK"] = jbk;
            return View(model);
        }

        public ActionResult GetPartner()
        {
            var model = (from c in entities.Partners
                         orderby c.Account.Code
                         select c).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult CheckNumber(string DRDNumber, Guid Id)
        {
            string branch;
            int countCharacter;
            countCharacter = DRDNumber.Count();
            branch= DRDNumber.Substring(0,2);

            var checkBranch = (from d in entities.Branches
                               where d.Code == branch
                               select d.Code).FirstOrDefault();
            var value = "";

            if (checkBranch == null)
            {
                value = "notbranch";
            }
            else if (countCharacter < 8)
            {
                value = "invalidCount";
            }
            else
            {
                if (Id == null)
                {
                    var existingNumber = (from c in entities.AccountJournals
                                          where c.DRDNumber == DRDNumber
                                          select c.DRDNumber).FirstOrDefault();

                    if (existingNumber != null)
                    {
                        value = "invalid";
                    }
                }
                else
                {
                    var existingNumberEdit = (from c in entities.Vouchers
                                              where c.Number == DRDNumber && c.Id != Id
                                              select c.Number).FirstOrDefault();

                    if (existingNumberEdit != null)
                    {
                        value = "invalid";
                    }
                }
            }

            //var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 2).FirstOrDefault();

            //var value = getCodeStatus.AccountSide;
            return new JsonResult { Data = value };
        }

        [HttpPost]
        public ActionResult Create(VoucherViewModel model)
        {
            var VoucherSessionList = (List<VoucherDetailViewModel>)(Session[VoucherSessionName]);

            
            if (VoucherSessionList == null || VoucherSessionList.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail voucher.");

            var branch = GetCurrentUserBranchId();
            string transactionDateString = Request.Form["TransactionDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);

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
                    var partner = entities.Partners.Where(a => a.Name == model.PartnerName).FirstOrDefault();
                    var newDataVoucher = new Voucher();
                    newDataVoucher.Id = Guid.NewGuid();
                    if (model.Number == null)
                    {
                        var nextNumber = 0;
                        var recordCount = (from vrc in entities.VoucherRecordCounters
                                           where vrc.BranchId == branch.Id
                                           select vrc).FirstOrDefault();

                        if (recordCount != null)
                        {
                            nextNumber = recordCount.LastCounter;

                        }



                        //var vcNumber = (from v in entities.Vouchers
                        //                where v.BranchId == branch.Id
                        //                select v.Number).Max();


                        //int no;
                        //int lastCounter;
                        int VoucherNumber;
                        //if (vcNumber != null)
                        //{
                        //    no = int.Parse(vcNumber.Substring(2));
                        //}
                        //else
                        //{
                        //    no = 0;
                        //}

                        //if (no >= nextNumber)
                        //{
                        //    lastCounter = no;
                        //}
                        //else
                        //{
                        //    lastCounter = nextNumber;
                        //}

                        //if (lastCounter.Equals(999999))
                        //{
                        //    VoucherNumber = 0;

                        //}
                        //else
                        //{
                        //    VoucherNumber = lastCounter;
                        //}
                        VoucherNumber = nextNumber ;
                        string number = string.Format("{0:000000}", VoucherNumber + 1);
                        newDataVoucher.Number = string.Format("{0}{1}", branch.Code, number);

                        if (recordCount != null)
                            recordCount.LastCounter = VoucherNumber + 1;
                        else
                        {
                            var newCounter = new VoucherRecordCounter
                            {
                                Id = Guid.NewGuid(),
                                BranchId = branch.Id,
                                LastCounter = 1
                            };
                            entities.VoucherRecordCounters.AddObject(newCounter);
                        }
                        saveHistory(string.Format("{0:000000}", VoucherNumber + 1));
                    }
                  else
                    {
                        var checkVoucher = (from v in entities.Vouchers
                                            where v.Number == model.Number
                                            select v).FirstOrDefault();

                        if (checkVoucher == null)
                        {
                            newDataVoucher.Number = model.Number;
                        }
                        else
                        {
                            ModelState.AddModelError("Number", "Number Voucher sudah ada.");
                        }
                    }

                    
                    newDataVoucher.TransactionDate = model.TransactionDate;
                    newDataVoucher.PartnerId = partner.Id;
                    newDataVoucher.Description = model.Description;
                    newDataVoucher.Attachment = model.Attachment;
                    newDataVoucher.BranchId = branch.Id;
                    newDataVoucher.VoucherStatusId = 1;
                    newDataVoucher.PaymentStatus = 1;

                    entities.Vouchers.AddObject(newDataVoucher);
                    
                    foreach (var item in VoucherSessionList)
                    {
                        var detail = new VoucherDetail
                        {
                            AccountId = item.AccountId,
                            VoucherId = newDataVoucher.Id,
                            Debet = item.Debet,
                            Kredit = item.Kredit,
                            BranchId = item.BranchId,
                            MasterBranch = item.MasterBranch 
                        };
                        entities.VoucherDetails.AddObject(detail);
                    }

                    //var detailDHHD = new VoucherDetail();

                    //detailDHHD.AccountId = newDataVoucher.Partner.AccountId;
                    //detailDHHD.VoucherId = newDataVoucher.Id;
                    //detailDHHD.Kredit = model.AmountPaid;
                    //detailDHHD.BranchId = newDataVoucher.BranchId;
                    //detailDHHD.MasterBranch = branch.Id;

                    //entities.VoucherDetails.AddObject(detailDHHD);

                    
                    entities.SaveChanges();
                    
                    Session.Remove(VoucherSessionName);
                    //var newAdded = entities.Vouchers.Select(a => a.Id).Max();

                    return RedirectToAction("Confirmation", new { id = newDataVoucher.Id });
                    
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing VoucherKasController.Create[Post].", ex);
                    SetErrorMessageViewData("Data tidak dapat dibuat.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id, string dhhd = "")
        {
            Session.Remove(VoucherSessionName);
            var branch = GetCurrentUserBranchId();
            var model = (from m in entities.Vouchers
                         join s in entities.Partners on m.PartnerId equals s.Id 
                         where m.Id == id
                         select new VoucherViewModel
                         {
                             Number = m.Number,
                             TransactionDate = m.TransactionDate,
                             PartnerId = m.PartnerId,
                             PartnerName = s.Name,
                             Description = m.Description,
                             Attachment = m.Attachment,
                             BranchId = m.BranchId
                         }).FirstOrDefault();

            ViewData["dhhd"] = dhhd;
            return View(model);

        }

        [HttpPost]
        public ActionResult Edit(VoucherViewModel model, string dhhd = "")
        {
            var branch = GetCurrentUserBranchId();
            var VoucherDetailSessionList = (List<VoucherDetailViewModel>)(Session[VoucherSessionName]);

            if (VoucherDetailSessionList == null || VoucherDetailSessionList.Count <= 0)
                ModelState.AddModelError("", "Silahkan isi detail voucher.");

            string transactionDateString = Request.Form["TransactionDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);

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
                    var partner = entities.Partners.Where(a => a.Name == model.PartnerName).FirstOrDefault();
                    var newDetail = (from c in entities.Vouchers
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();
                    var duhd = (from a in entities.DHHDJournals
                                where a.VoucherId == model.Id
                                select a).FirstOrDefault();
                    
                    if (newDetail == null)
                    {
                        return RedirectToAction("Index");
                    }

                    if (duhd != null)
                    {
                        duhd.Description = model.Description;
                        duhd.DateVoucher = model.TransactionDate;
                    
                    }
                    if (model.Number == null)
                    {
                        {
                            var nextNumber = 0;
                            var recordCount = (from vrc in entities.VoucherRecordCounters
                                               where vrc.BranchId == branch.Id
                                               select vrc).FirstOrDefault();

                            if (recordCount != null)
                            {
                                nextNumber = recordCount.LastCounter;

                            }



                            //var vcNumber = (from v in entities.Vouchers
                            //                where v.BranchId == branch.Id
                            //                select v.Number).Max();

                            int VoucherNumber;
                            //int no;
                            //int lastCounter;
                            //int VoucherNumber;
                            //if (vcNumber != null)
                            //{
                            //    no = int.Parse(vcNumber.Substring(2));
                            //}
                            //else
                            //{
                            //    no = 0;
                            //}

                            //if (no >= nextNumber)
                            //{
                            //    lastCounter = no;
                            //}
                            //else
                            //{
                            //    lastCounter = nextNumber;
                            //}

                            //if (lastCounter.Equals(999999))
                            //{
                            //    VoucherNumber = 0;

                            //}
                            //else
                            //{
                            //    VoucherNumber = lastCounter;
                            //}
                            VoucherNumber = nextNumber;
                            string number = string.Format("{0:000000}", VoucherNumber + 1);
                            newDetail.Number = string.Format("{0}{1}", branch.Code, number);

                            if (recordCount != null)
                                recordCount.LastCounter = VoucherNumber + 1;
                            else
                            {
                                var newCounter = new VoucherRecordCounter
                                {
                                    Id = Guid.NewGuid(),
                                    BranchId = branch.Id,
                                    LastCounter = 1
                                };
                                entities.VoucherRecordCounters.AddObject(newCounter);
                            }
                        }
                    }
                    else
                    {
                        var checkVoucher = (from v in entities.Vouchers
                                            where v.Number == model.Number
                                            select v).FirstOrDefault();

                        if (checkVoucher == null)
                        {
                            newDetail.TransactionDate = model.TransactionDate;
                            newDetail.PartnerId = partner.Id;
                            newDetail.Number = model.Number;
                            newDetail.Description = model.Description;
                            newDetail.Attachment = model.Attachment;

                            foreach (var item in entities.VoucherDetails.Where(p => p.VoucherId == newDetail.Id))
                            {
                                entities.VoucherDetails.DeleteObject(item);
                            }

                            
                            // insert new detail
                            foreach (var item in VoucherDetailSessionList)
                            {
                                var detail = new VoucherDetail
                                {
                                    VoucherId = newDetail.Id,
                                    AccountId = item.AccountId,
                                    Debet = item.Debet,
                                    Kredit = item.Kredit,
                                    BranchId = item.BranchId,
                                    MasterBranch = branch.Id
                                };

                                entities.VoucherDetails.AddObject(detail);
                            }

                            //var detailDHHD = new VoucherDetail();

                            //detailDHHD.AccountId = newDetail.Partner.AccountId;
                            //detailDHHD.VoucherId = newDetail.Id;
                            //detailDHHD.Kredit = model.AmountPaid;
                            //detailDHHD.BranchId = newDetail.BranchId;
                            //detailDHHD.MasterBranch = branch.Id;
                            //entities.VoucherDetails.AddObject(detailDHHD);
                            
                            entities.SaveChanges();
                            saveHistory(newDetail.Number);

                            if (dhhd != "")
                            {
                                return RedirectToAction("../DHHDJournal/Index", new { Area = "SKA" });
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            if (newDetail.Number.Equals(model.Number))
                            {
                                newDetail.TransactionDate = model.TransactionDate;
                                newDetail.PartnerId = partner.Id;
                                newDetail.Number = model.Number;
                                newDetail.Description = model.Description;
                                newDetail.Attachment = model.Attachment;

                                foreach (var item in entities.VoucherDetails.Where(p => p.VoucherId == newDetail.Id))
                                {
                                    entities.VoucherDetails.DeleteObject(item);
                                }

                                
                                // insert new detail
                                foreach (var item in VoucherDetailSessionList)
                                {
                                    var detail = new VoucherDetail
                                    {
                                        VoucherId = newDetail.Id,
                                        AccountId = item.AccountId,
                                        Debet = item.Debet,
                                        Kredit = item.Kredit,
                                        BranchId = item.BranchId,
                                        MasterBranch = branch.Id
                                    };

                                    entities.VoucherDetails.AddObject(detail);
                                }

                                //var detailDHHD = new VoucherDetail();

                                //detailDHHD.AccountId = newDetail.Partner.AccountId;
                                //detailDHHD.VoucherId = newDetail.Id;
                                //detailDHHD.Kredit = model.AmountPaid;
                                //detailDHHD.BranchId = newDetail.BranchId;
                                //detailDHHD.MasterBranch = branch.Id;
                                //entities.VoucherDetails.AddObject(detailDHHD);

                                entities.SaveChanges();
                                saveHistory(newDetail.Number);

                                if (dhhd != "")
                                {
                                    return RedirectToAction("../DHHDJournal/Index", new { Area = "SKA" });
                                }
                                else
                                {
                                    return RedirectToAction("Index");
                                }

                            } else
                            {
                                ModelState.AddModelError("Number", "Number Voucher sudah ada.");
                            }
                        }
                       
                    }
                                  
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing VoucherKasController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data tidak dapat diubah.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var model = new VoucherViewModel();

            try
            {
                var deleteItem = (from d in entities.Vouchers
                                  where d.Id == id
                                  select d).FirstOrDefault();


                if (deleteItem == null)
                {
                    SetErrorMessageViewData("Data tidak dapat ditemukan");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Number = deleteItem.Number;
                    model.TransactionDate = deleteItem.TransactionDate;
                    model.PartnerId = deleteItem.PartnerId;
                    model.PartnerName = deleteItem.Partner.Name;
                    model.Description = deleteItem.Description;
                    model.Attachment = deleteItem.Attachment;
                    model.BranchId = deleteItem.BranchId;
                    model.ApproveDate = deleteItem.ApproveDate;
                    //model.VoucherStatusId = deleteItem.VoucherStatusId;
                    model.VoucherStatusName = deleteItem.SystemVoucherStatu.Description;
                    //model.Status = deleteItem.PaymentStatus;
                }

            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing VoucherKasController.Delete[Get].", ex);
                SetErrorMessageViewData("Data tidak dapat ditampilkan");

            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(VoucherViewModel model)
        {
            try
            {
                var deleteItem = (from c in entities.Vouchers
                                  where c.Id == model.Id
                                  select c).FirstOrDefault();

                if (deleteItem != null)
                {
                    foreach (var item in entities.VoucherDetails.Where(p => p.VoucherId == deleteItem.Id))
                    {
                        entities.VoucherDetails.DeleteObject(item);
                    }

                    entities.Vouchers.DeleteObject(deleteItem);
                    entities.SaveChanges();
                    saveHistory(deleteItem.Number);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing VoucherKasController.Delete [Post].", ex);
                SetErrorMessageViewData("Data tidak dapat dihapus.");
            }

            return View(model);
        }

        [GridAction]
        public ActionResult GetPartnerList()
        {
            var branch = GetCurrentUserBranchId();
            string AccountCodeforPartner = System.Configuration.ConfigurationManager.AppSettings["AccountCodeforPartner"];
            var model = from p in entities.Partners
                        join a in entities.Accounts on p.AccountId equals a.Id
                        where a.Code.StartsWith(AccountCodeforPartner)
                        select new PartnerViewModel 
                        {
                            Id = p.Id, 
                            Code = p.Code, 
                            Name = p.Name, 
                            AccountCode = a.Code + "." + branch.Code
                        };

            return View(new GridModel<PartnerViewModel>(model));
        }

        [HttpGet]
        public ActionResult Confirmation(Guid id)
        {
            ViewData["Id"] = id;
            try
            {
                var confirmation = (from c in entities.Vouchers
                                    where c.Id == id
                                    select new VoucherViewModel
                                    {
                                        Number = c.Number,
                                        TransactionDate = c.TransactionDate,
                                        PartnerName = c.Partner.Name,
                                        Description = c.Description,
                                        Attachment = c.Attachment
                                    }).FirstOrDefault();

                if (confirmation == null)
                {
                    return RedirectToAction("Index");
                }

                return View(confirmation);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing VoucherKasController.Confirmation [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Confirmation(VoucherViewModel model)
        {
            try
            {
                var deleteItem = (from c in entities.Vouchers
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
                Logger.Error("Error while executing VoucherKasController.Confirmation [Post].", ex);
                return RedirectToAction("Index");
            }
        }

        //[HttpGet]
        //public ActionResult IndexApprove()
        //{
        //    if (TempData["SearchValue"] != null)
        //    {
        //        ViewBag.SearchValue = TempData["SearchValue"].ToString();
        //    }
        //    else
        //    {
        //        ViewBag.SearchValue = string.Empty;
        //    }
        //    return View();
        //}

        

        [GridAction]
        public ActionResult GetApproveVoucherList(string searchValue)
        {
            var branch = GetCurrentUserBranchId();
            var model = from vk in entities.Vouchers
                        where vk.ApproveDate != null// && vk.BranchId == branch.Id 
                        select new VoucherViewModel
                        {
                            Id = vk.Id,
                            Number = vk.Number,
                            TransactionDate = vk.TransactionDate,
                            PartnerId = vk.PartnerId,
                            PartnerName = vk.Partner.Name,
                            Description = vk.Description,
                            Attachment = vk.Attachment,
                            BranchId = vk.BranchId,
                            ApproveDate = vk.ApproveDate

                        };

            //TODO: check bagaimana jika table closing masih benar-benar kosong, getClosingData bisa null -> tidak ada kondisi untuk ini
            var getClosingData = entities.Closings.FirstOrDefault();

            if (getClosingData.ClosingDate != null && branch.Code != "00")
            {
                model = model.Where(a => a.BranchId == branch.Id && a.TransactionDate >= getClosingData.ClosingDate && a.ApproveDate >= getClosingData.ClosingDate);
            }
            else if (getClosingData.ClosingDate != null)
            {
                model = model.Where(a => a.TransactionDate >= getClosingData.ClosingDate && a.ApproveDate >= getClosingData.ClosingDate);
            }
            else if (branch.Code != "00")
            {
                model = model.Where(a => a.BranchId == branch.Id);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Number.Contains(searchValue));
            }
            return View(new GridModel<VoucherViewModel>(model));
        }

        public ActionResult detail(Guid id)
        {
            Session.Remove(VoucherSessionName);

            var model = (from m in entities.Vouchers
                         where m.Id == id
                         select new VoucherViewModel
                         {
                             Number = m.Number,
                             TransactionDate = m.TransactionDate,
                             PartnerId = m.PartnerId,
                             PartnerName = m.Partner.Name,
                             Description = m.Description,
                             Attachment = m.Attachment,
                             BranchId = m.BranchId,
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult CheckTax(string AccountCode)
        {
            var value = "";
            var code = AccountCode.Substring(0, 8);

            if(code.StartsWith("50.06"))
            {
                value = "tax";
            }
            return new JsonResult { Data = value };
        }

       }
}




