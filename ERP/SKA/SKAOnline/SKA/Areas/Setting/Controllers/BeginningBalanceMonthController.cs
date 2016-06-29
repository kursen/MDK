using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.Setting.Models.ViewModels;
using SKA.Filters;
using System.Globalization;
using System.Collections;

namespace SKA.Areas.Setting.Controllers
{
    [BranchCodeFilter]
    public class BeginningBalanceMonthController : BaseController
    {
        private SKAEntities entities;
        private string BeginningBalanceMonthSessionName = "BeginningBalanceMonthSession";
        public BeginningBalanceMonthController()
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
            var model = from at in entities.Accounts
                        select new ChartofAccountViewModel
                        {
                            Id = at.Id,
                            Code = at.Code,
                            Name = at.Name
                        };

            var countBranch = from c in entities.Branches
                              select new BranchViewModel
                              {
                                  Id = c.Id
                              };

            var accountBalance = from c in entities.BeginningBalanceMonthBranches
                                 select new BeginningBalanceViewModel
                                 {
                                     Id = c.Id,
                                     AccountId = c.AccountId,
                                     BranchId = c.BranchId
                                 };

            var aa = countBranch.Select(b => b.Id);

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Name.Contains(searchValue)
                    || a.Code.Contains(searchValue));
            }
            return View(new GridModel<ChartofAccountViewModel>(model));
        }

        public ActionResult Create(int id)
        {

            var b = (from c in entities.Branches
                         select c);

            var branchid = new SelectList(b.ToList(), "Id", "Name");
            ViewBag.Branch = branchid;

            var model = (from c in entities.BeginningBalanceMonthBranches
                         where c.AccountId == id
                         select new BeginningBalanceViewModel
                         {
                             AccountId = c.AccountId,
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(BeginningBalanceViewModel model)
        {

            var b = (from c in entities.Branches
                     select c);

            var branchid = new SelectList(b.ToList(), "Id", "Name");
            ViewBag.Branch = branchid;

            string TransactionDateString = Request.Form["TransactionDate"];
            DateTime transactiondate;
            DateTime.TryParse(TransactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactiondate);
            model.TransactionDate = transactiondate;

            var errorItem = ModelState.FirstOrDefault(m => m.Key == "TransactionDate");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (model.TransactionDate == null)
                ModelState.AddModelError("TransactionDate", "Tahun harus diisi!!");

            var checkMonth = (from c in entities.BeginningBalanceMonthBranches
                             where c.TransactionDate.Value.Month.Equals(model.TransactionDate.Value.Month) && c.BranchId == model.BranchId
                             select c).FirstOrDefault();
            if (checkMonth != null)
            {
                ModelState.AddModelError("TransactionDate", "Saldo Awal Bulan " + checkMonth.TransactionDate.Value.ToString("MMMM yyyy") + " Sudah ada!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    decimal debet = 0;
                    decimal credit = 0;
                    if (model.Debet != null) { debet = Convert.ToDecimal(model.Debet); }
                    if (model.Kredit != null) { credit = Convert.ToDecimal(model.Kredit); }

                    var item = new BeginningBalanceMonthBranch();
                    item.AccountId = model.AccountId;
                    item.BranchId = model.BranchId;
                    item.Debet = model.Debet;
                    item.Kredit = model.Kredit;
                    item.TransactionDate = model.TransactionDate;

                    entities.BeginningBalanceMonthBranches.AddObject(item);
                    entities.SaveChanges();
                    return RedirectToAction("BeginningBalanceMonth", "BeginningBalanceMonth", new { id = model.AccountId});
                }
                catch (Exception ex)
                {
                    Logger.Error("erro", ex);
                    SetErrorMessageViewData("Data gagal dibuat");
                }

            }

            return View(model);
        }

        [HttpGet]
        public ActionResult BeginningBalanceMonth(int id)
        {
            Session.Remove(BeginningBalanceMonthSessionName);
            DateTime getDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var beginningBalance = entities.SetFirstBalanceMonth(id);

            var model = (from c in entities.BeginningBalanceMonthBranches
                         where c.AccountId == id
                         select new BeginningBalanceViewModel
                         {
                             AccountId = c.AccountId,
                             AccountName = c.Account.Name,
                             AccountCode = c.Account.Code,
                             Debet = c.Debet,
                             Kredit = c.Kredit,
                             BranchId = c.BranchId,
                             TransactionDate = c.TransactionDate,
                             BeginningBalanceBranchId = c.Id
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult BeginningBalanceMonth(BeginningBalanceViewModel model)
        {
            var beginningBalanceSessionlist = (List<BeginningBalanceViewModel>)(Session[BeginningBalanceMonthSessionName]);


            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.Accounts
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    if (newDetail == null)
                    {
                        return RedirectToAction("Index");
                    }


                    // delete old data first, before inserting new detail
                    //DateTime getDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    foreach (var item in entities.BeginningBalanceMonthBranches.Where(p => p.AccountId == newDetail.Id))
                    {
                        entities.BeginningBalanceMonthBranches.DeleteObject(item);
                    }

                    // insert new detail
                    foreach (var item in beginningBalanceSessionlist)
                    {
                        var detail = new BeginningBalanceMonthBranch
                        {
                            AccountId = item.AccountId,
                            BranchId = item.BranchId,
                            Debet = item.Debet,
                            Kredit = item.Kredit,
                            TransactionDate = item.TransactionDate
                        };
                        
                     entities.BeginningBalanceMonthBranches.AddObject(detail);
                    }

                    entities.SaveChanges();
                    return RedirectToAction("BeginningBalanceMonth", "BeginningBalanceMonth", new { id = model.Id });
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing BeginningBalanceMonthController.BeginningBalanceMonthBranch [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }

        [GridAction]
        public ActionResult _SelectBeginningBalanceMonth(int? id)
        {
            var model = (List<BeginningBalanceViewModel>)Session[BeginningBalanceMonthSessionName];
            if (model == null)
            {
                model = (from data in entities.BeginningBalanceMonthBranches
                         where data.AccountId.Equals(id.Value)
                         select new BeginningBalanceViewModel
                         {
                             Id = data.Id,
                             AccountId = data.AccountId,
                             AccountCode = data.Account.Code + "." + data.Branch.Code,
                             AccountName = data.Account.Name + " " + data.Branch.Name,
                             BranchId = data.BranchId,
                             Debet = data.Debet,
                             Kredit = data.Kredit,
                             TransactionDate = data.TransactionDate,
                             BeginningBalanceBranchId = data.Id
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[BeginningBalanceMonthSessionName] = model;
            }

            return View(new GridModel<BeginningBalanceViewModel>(model));
        }

        [GridAction()]
        public ActionResult _UpdateDetail(int? id)
        {
            var model = (List<BeginningBalanceViewModel>)Session[BeginningBalanceMonthSessionName];
            BeginningBalanceViewModel postmodel = new BeginningBalanceViewModel();

            string transactionDateString = Request.Form["TransactionDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);

            int getThisYear = DateTime.Now.Year;
            postmodel.TransactionDate = transactionDate;

            //var errorItem = ModelState.Keys.Where(m => m == "TransactionDate");

            //if (errorItem != null)
            //    ModelState["TransactionDate"].Errors.Clear();

            if (TryUpdateModel(postmodel, new string[] { "Debet", "Kredit", "AccountId", "BranchId", "BeginningBalanceBranchId" }))
            {
                var item = model.Where(m => m.BeginningBalanceBranchId == postmodel.BeginningBalanceBranchId).FirstOrDefault();

                if (item != null)
                {
                    item.Debet = postmodel.Debet;
                    item.Kredit = postmodel.Kredit;
                    item.TransactionDate = postmodel.TransactionDate;
                }
            }

            return View(new GridModel<BeginningBalanceViewModel>(model));
        }
    }
}
