using System;
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
    public class BudgetFinancialPositionController : BaseController
    {
        //
        // GET: /SKA/BudgetFinancialPosition/
        private SKAEntities entities;
        private string budgetFinancialPositionSessionName = "BudgetFinancialPosition";
        private static int tempYear;
        public BudgetFinancialPositionController()
        {
            entities = new SKAEntities();
        }
        
        public ActionResult GetYear()
        {
            Dictionary<int, int> years = new Dictionary<int, int>();

            for (int i = DateTime.Now.Year + 1; i >= 1990 ; i--)
            {
                years.Add(i, i);
            }

            var newYear = from a in years
                          select a;

            ViewBag.Year = new SelectList(newYear.ToList(), "Key", "Value");

            return View();
        }

        public ActionResult Index(int year)
        {
            var model = new BudgetFinancialPositionViewModel();
            model.FiscalYear = year;
            tempYear = year;

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
            return RedirectToAction("Index", new { year = tempYear });
        }

        [GridAction]
        public ActionResult GetList(int year, string searchValue)
        {
            var getYear = entities.BudgetofFinancialPositions.Where(a => a.FiscalYear == year).FirstOrDefault();

            var template = (from c in entities.BudgetToFinancialPositionTemplates
                            select c).ToList();

            try
            {
                if (getYear == null)
                {

                    for (int i = 0; i < template.Count; i++)
                    {
                        var newPosition = new BudgetofFinancialPosition();
                        newPosition.Name = template[i].Name;
                        newPosition.FiscalYear = year;
                        entities.BudgetofFinancialPositions.AddObject(newPosition);

                    }
                    entities.SaveChanges();

                    var model = from c in entities.BudgetofFinancialPositions
                                where c.FiscalYear == year
                                select new BudgetFinancialPositionViewModel
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    FiscalYear = c.FiscalYear,
                                    Amount = c.Amount
                                };

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        model = model.Where(a => a.Name.Contains(searchValue));
                    }

                    return View(new GridModel<BudgetFinancialPositionViewModel>(model));

                }
                else
                {
                    var model = from c in entities.BudgetofFinancialPositions
                                where c.FiscalYear == year
                                select new BudgetFinancialPositionViewModel
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    FiscalYear = c.FiscalYear,
                                    Amount = c.Amount
                                };

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        model = model.Where(a => a.Name.Contains(searchValue));
                    }

                    return View(new GridModel<BudgetFinancialPositionViewModel>(model));
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error. Detail: ", ex);
                return RedirectToAction("GetYear");
            }
        }

        [GridAction]
        public ActionResult _SelectDetail(int? id)
        {
            var model = (List<BudgetFinancialPositionDetailViewModel>)Session[budgetFinancialPositionSessionName];

            if (model == null)
            {
                model = (from data in entities.BudgetofFinancialPositionDetails
                         where data.BudgetofFinancialPositionId.Equals(id.Value)
                         select new BudgetFinancialPositionDetailViewModel
                         {
                             AccountId1 = data.AccountId1,
                             AccountId2 = data.AccountId2,
                             AccountCode1 = (data.Account.Code.Length == 8) ? data.Account.Code + "." + data.Branch.Code + " - " + data.Account.Name + " " + data.Branch.Name : data.Account.Code + " - " + data.Account.Name,
                             AccountCode2 = (data.Account1.Code.Length == 8) ? data.Account1.Code + "." + data.Branch1.Code + " - " + data.Account1.Name + " " + data.Branch1.Name : data.Account1.Code + " - " + data.Account1.Name,
                             BranchId1 = data.BranchId1,
                             BranchId2 = data.BranchId2,
                             Description = data.Description,
                             Budget = data.Budget
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[budgetFinancialPositionSessionName] = model;
            }

            return View(new GridModel<BudgetFinancialPositionDetailViewModel>(model));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Session.Remove(budgetFinancialPositionSessionName);

            var existingId = entities.BudgetofFinancialPositionDetails.Where(a => a.BudgetofFinancialPositionId == id).FirstOrDefault();
            var getYear = entities.BudgetofFinancialPositions.Where(b => b.Id == id).FirstOrDefault();
            try
            {
                if (existingId == null)
                {
                    var newBudgetPositionDetail = new BudgetofFinancialPositionDetail();
                    newBudgetPositionDetail.BudgetofFinancialPositionId = id;
                    entities.BudgetofFinancialPositionDetails.AddObject(newBudgetPositionDetail);
                    entities.SaveChanges();
                    
                    var model = (from c in entities.BudgetofFinancialPositions
                                 where c.Id == id
                                 select new BudgetFinancialPositionViewModel
                                 {
                                     Name = c.Name,
                                     Amount = c.Amount,
                                     FiscalYear = c.FiscalYear
                                 }).FirstOrDefault();

                    ViewData["Year"] = model.FiscalYear;
                    return View(model);
                }
                else
                {
                    var model = (from c in entities.BudgetofFinancialPositions
                                 where c.Id == id
                                 select new BudgetFinancialPositionViewModel
                                 {
                                     Name = c.Name,
                                     Amount = c.Amount,
                                     FiscalYear = c.FiscalYear
                                 }).FirstOrDefault();

                    ViewData["Year"] = model.FiscalYear;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error. Detail: ", ex);
                return RedirectToAction("Index", new { year = getYear.FiscalYear });
            }
        }

        [HttpPost]
        public ActionResult Edit(BudgetFinancialPositionViewModel model)
        {
            var budgetDetailSessionlist = (List<BudgetFinancialPositionDetailViewModel>)(Session[budgetFinancialPositionSessionName]);

            //if (pettyCashDetailSessionlist == null || pettyCashDetailSessionlist.Count <= 0)
            //    ModelState.AddModelError("", "Silahkan isi detail pembayaran.");

            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.BudgetofFinancialPositions
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    if (newDetail == null)
                    {
                        return RedirectToAction("Index", new { year = newDetail.FiscalYear });
                    }

                    newDetail.Amount = model.Amount;

                    // delete old data first, before inserting new detail
                    foreach (var item in entities.BudgetofFinancialPositionDetails.Where(p => p.BudgetofFinancialPositionId == newDetail.Id))
                    {
                        entities.BudgetofFinancialPositionDetails.DeleteObject(item);
                    }

                    // insert new detail
                    foreach (var item in budgetDetailSessionlist)
                    {
                        var detail = new BudgetofFinancialPositionDetail
                        {
                            BudgetofFinancialPositionId = newDetail.Id,
                            AccountId1 = item.AccountId1,
                            AccountId2 = item.AccountId2,
                            BranchId1 = item.BranchId1,
                            BranchId2 = item.BranchId2,
                            Description = item.Description,
                            Budget = item.Budget
                        };

                        entities.BudgetofFinancialPositionDetails.AddObject(detail);
                    }

                    entities.SaveChanges();
                    saveHistory();
                    var getYear = entities.BudgetofFinancialPositions.Where(a => a.Id == model.Id).FirstOrDefault();
                    return RedirectToAction("Index", new { year = getYear.FiscalYear });
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing BudgetFinancialPositionController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }

        [GridAction()]
        public ActionResult _InsertDetail()
        {
            var model = (List<BudgetFinancialPositionDetailViewModel>)Session[budgetFinancialPositionSessionName];
            BudgetFinancialPositionDetailViewModel postmodel = new BudgetFinancialPositionDetailViewModel();

            if (model == null)
                model = new List<BudgetFinancialPositionDetailViewModel>();

            if (TryUpdateModel(postmodel))
            {
                //---------------------- Account Code 1---------------------//
                string[] words = postmodel.AccountCode1.Split('-');
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

                //------------------------- Account Code 2 ------------------//
                string[] words2 = postmodel.AccountCode2.Split('-');
                var code2 = words2[0];

                var getBranchCode2 = code2.Substring(9, 2);
                var accountCode2 = code2.Substring(0, 8);
                var account2 = from c in entities.Accounts
                              select c;

                var getBranches2 = entities.Branches.Where(a => a.Code == getBranchCode2).FirstOrDefault();


                if (getBranches2 != null)
                {
                    account2 = account2.Where(a => a.Code == accountCode2);
                }
                else
                {
                    account2 = account2.Where(a => a.Code == code2);
                }


                if ((account != null) && (account2 != null))
                {
                    if (account.FirstOrDefault().Code.Length.Equals(8) && account2.FirstOrDefault().Code.Length.Equals(8))
                    {
                        var getBranch1 = entities.Branches.Where(a => a.Code == getBranchCode).FirstOrDefault();
                        var getBranch2 = entities.Branches.Where(a => a.Code == getBranchCode2).FirstOrDefault();

                        postmodel.AccountId1 = account.FirstOrDefault().Id;
                        postmodel.AccountId2 = account2.FirstOrDefault().Id;
                        postmodel.AccountCode1 = account.FirstOrDefault().Code + "." + getBranch1.Code + " - " + account.FirstOrDefault().Name + " " + getBranch1.Name;
                        postmodel.AccountCode2 = account2.FirstOrDefault().Code + "." + getBranch2.Code + " - " + account2.FirstOrDefault().Name + " " + getBranch2.Name;
                        postmodel.BranchId1 = getBranch1.Id;
                        postmodel.BranchId2 = getBranch2.Id;
                    }
                    else {
                        postmodel.AccountId1 = account.FirstOrDefault().Id;
                        postmodel.AccountId2 = account2.FirstOrDefault().Id;
                        postmodel.AccountCode1 = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                        postmodel.AccountCode2 = account2.FirstOrDefault().Code +  " - " + account2.FirstOrDefault().Name;
                    
                    }
                    int maxId = 0;

                    if (model.Count > 0)
                        maxId = model.Max(m => m.Id);

                    postmodel.Id = maxId + 1;

                    model.Insert(0, postmodel);

                    Session[budgetFinancialPositionSessionName] = model;
                }
                else
                {
                    ModelState.AddModelError("AccountCode1", "Kode Perkiraan tidak ditemukan");
                    ModelState.AddModelError("AccountCode2", "Kode Perkiraan tidak ditemukan");
                }
            }

            return View(new GridModel<BudgetFinancialPositionDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<BudgetFinancialPositionDetailViewModel>)Session[budgetFinancialPositionSessionName];
            BudgetFinancialPositionDetailViewModel postmodel = new BudgetFinancialPositionDetailViewModel();

            if (TryUpdateModel(postmodel))
            {
                var item = model.Where(m => m.Id == postmodel.Id).FirstOrDefault();

                if (item != null)
                {
                    //---------------------- Account Code 1---------------------//
                    string[] words = postmodel.AccountCode1.Split('-');
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

                    //------------------------- Account Code 2 ------------------//
                    string[] words2 = postmodel.AccountCode2.Split('-');
                    var code2 = words2[0];

                    var getBranchCode2 = code2.Substring(9, 2);
                    var accountCode2 = code2.Substring(0, 8);
                    var account2 = from c in entities.Accounts
                                   select c;

                    var getBranches2 = entities.Branches.Where(a => a.Code == getBranchCode2).FirstOrDefault();


                    if (getBranches2 != null)
                    {
                        account2 = account2.Where(a => a.Code == accountCode2);
                    }
                    else
                    {
                        account2 = account2.Where(a => a.Code == code2);
                    }

                    if ((account != null) && (account2 != null))
                    {
                        if (account.FirstOrDefault().Code.Length.Equals(8) && account2.FirstOrDefault().Code.Length.Equals(8))
                        {
                            var getBranch1 = entities.Branches.Where(a => a.Code == getBranchCode).FirstOrDefault();
                            var getBranch2 = entities.Branches.Where(a => a.Code == getBranchCode2).FirstOrDefault();

                            item.AccountId1 = account.FirstOrDefault().Id;
                            item.AccountId2 = account2.FirstOrDefault().Id;
                            item.AccountCode1 = account.FirstOrDefault().Code + "." + getBranch1.Code + " - " + account.FirstOrDefault().Name + " " + getBranch1.Name;
                            item.AccountCode2 = account2.FirstOrDefault().Code + "." + getBranch2.Code + " - " + account2.FirstOrDefault().Name + " " + getBranch2.Name;
                            item.BranchId1 = getBranch1.Id;
                            item.BranchId2 = getBranch2.Id;
                            item.Budget = postmodel.Budget;
                            item.Description = postmodel.Description;
                        }
                        else
                        {
                            item.AccountId1 = account.FirstOrDefault().Id;
                            item.AccountId2 = account2.FirstOrDefault().Id;
                            item.AccountCode1 = account.FirstOrDefault().Code + " - " + account.FirstOrDefault().Name;
                            item.AccountCode2 = account2.FirstOrDefault().Code + " - " + account2.FirstOrDefault().Name;
                            item.Budget = postmodel.Budget;
                            item.Description = postmodel.Description;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("AccountCode1", "Kode Perkiraan tidak ditemukan");
                        ModelState.AddModelError("AccountCode2", "Kode Perkiraan tidak ditemukan");
                    }
                }
            }

            return View(new GridModel<BudgetFinancialPositionDetailViewModel>(model));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetail()
        {
            IList<BudgetFinancialPositionDetailViewModel> model = (IList<BudgetFinancialPositionDetailViewModel>)Session[budgetFinancialPositionSessionName];
            var detail = new BudgetFinancialPositionDetailViewModel();

            if (!TryUpdateModel<BudgetFinancialPositionDetailViewModel>(detail))
            {
                // check for member with [Required] attribute. 
                // If it found, clear the error, otherwise, the delete operation will not perform.

                var accountCodeError1 = ModelState.Keys.Where(e => e == "AccountCode1");
                var accountCodeError2 = ModelState.Keys.Where(e => e == "AccountCode2");
                var descriptionError = ModelState.Keys.Where(e => e == "Description");

                if (accountCodeError1 != null && accountCodeError2 != null)
                { 
                    ModelState["AccountCode1"].Errors.Clear();
                    ModelState["AccountCode2"].Errors.Clear();
                }
                if (descriptionError != null)
                {
                    ModelState["Description"].Errors.Clear();
                }
            }

            if (detail != null)
            {
                var item = model.Where(c => c.Id == detail.Id).FirstOrDefault();

                if (item != null)
                    model.Remove(item);
            }

            return View(new GridModel<BudgetFinancialPositionDetailViewModel>(model));
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
    }
}
