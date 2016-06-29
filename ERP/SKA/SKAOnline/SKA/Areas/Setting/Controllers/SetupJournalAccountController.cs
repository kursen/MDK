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

namespace SKA.Areas.Setting.Controllers
{
    [BranchCodeFilter]
    public class SetupJournalAccountController : BaseController
    {
        //
        // GET: /Setting/SetupJournalAccount/
        private SKAEntities entities;
        private string setupAccountJournalName = "SetupAccountJournal";

        public SetupJournalAccountController()
        {
            entities = new SKAEntities();
            //ViewBag.Value = this.getSetupValueLabel();
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult GetListJournal()
        {
            var model = (from c in entities.JournalTypes
                        select new SetupJournalAccountViewModel
                        {
                            Id = c.Id,
                            JournalName = c.JournalName
                        }).ToList();
            return View(new GridModel<SetupJournalAccountViewModel>(model));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Session.Remove(setupAccountJournalName);

            var model = (from d in entities.JournalTypes
                         where d.Id == id
                         select new SetupJournalAccountViewModel
                         {
                             JournalName = d.JournalName
                         }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SetupJournalAccountViewModel model)
        {
            var setupAccountJournalDetailSessionlist = (List<SetupJournalAccountDetailViewModel>)(Session[setupAccountJournalName]);

            //if (setupAccountJournalDetailSessionlist == null || setupAccountJournalDetailSessionlist.Count <= 0)
            //    ModelState.AddModelError("", "Silahkan isi detail jurnal.");

            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.JournalTypes
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    
                    if (newDetail == null)
                    {
                        return RedirectToAction("Index");
                    }
                    // delete old data first, before inserting new detail
                    foreach (var item in entities.JournalAccounts.Where(p => p.JournalTypeId == newDetail.Id))
                    {
                        entities.JournalAccounts.DeleteObject(item);
                    }

                    
                    // insert new detail
                    foreach (var item in setupAccountJournalDetailSessionlist)
                    {
                        var detail = new JournalAccount
                        {
                            JournalTypeId = newDetail.Id,
                            AccountId = item.AccountId,
                            AccountSide = item.AccountSide
                        };

                        
                        entities.JournalAccounts.AddObject(detail);
                    }

                    entities.SaveChanges();
                    saveHistory(model.JournalName);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing SetupAccountJournalController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }
        
         [GridAction]
        public ActionResult _SelectDetail(int? id)
        {
            var model = (List<SetupJournalAccountDetailViewModel>)Session[setupAccountJournalName];

            if (model == null)
            {
               
                model = (from data in entities.JournalAccounts
                         where data.JournalTypeId.Equals(id.Value)
                         select new SetupJournalAccountDetailViewModel
                         {
                             JournalTypeId = data.JournalTypeId,
                             AccountId = data.AccountId,
                             AccountCode = data.Account.Code + " - " + data.Account.Name,
                             AccountSide = data.AccountSide
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[setupAccountJournalName] = model;
            }

            return View(new GridModel<SetupJournalAccountDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _InsertDetail()
        {
            var model = (List<SetupJournalAccountDetailViewModel>)Session[setupAccountJournalName];
            SetupJournalAccountDetailViewModel postmodel = new SetupJournalAccountDetailViewModel();

            if (model == null)
                model = new List<SetupJournalAccountDetailViewModel>();

            if (TryUpdateModel(postmodel))
            {
                string[] words = postmodel.AccountCode.Split('-');
                var code = words[0];
                
                var account = entities.Accounts.Where(a => a.Code == code).FirstOrDefault();

                if (account != null)
                {
                    postmodel.AccountCode = account.Code + " - " + account.Name;
                    postmodel.AccountId = account.Id;
                    
                    int maxId = 0;

                    if (model.Count > 0)
                        maxId = model.Max(m => m.Id);

                    postmodel.Id = maxId + 1;

                    model.Insert(0, postmodel);

                    Session[setupAccountJournalName] = model;
                }
                else
                {
                    ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                }
            }

            return View(new GridModel<SetupJournalAccountDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _UpdateDetail()
        {
            var model = (List<SetupJournalAccountDetailViewModel>)Session[setupAccountJournalName];
            SetupJournalAccountDetailViewModel postmodel = new SetupJournalAccountDetailViewModel();

            if (TryUpdateModel(postmodel))
            {
                var item = model.Where(m => m.Id == postmodel.Id).FirstOrDefault();

                if (item != null)
                {
                    string[] words = postmodel.AccountCode.Split('-');
                    var code = words[0];
                    var account = entities.Accounts.Where(a => a.Code == code).FirstOrDefault();

                    if (account != null)
                    {
                        item.AccountId = account.Id;
                        item.AccountCode = account.Code + " - " + account.Name;
                        item.AccountSide = postmodel.AccountSide;
                    }
                    else
                    {
                        ModelState.AddModelError("AccountCode", "Kode Perkiraan tidak ditemukan");
                    }
                }
            }

            return View(new GridModel<SetupJournalAccountDetailViewModel>(model));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetail()
        {
            IList<SetupJournalAccountDetailViewModel> model = (IList<SetupJournalAccountDetailViewModel>)Session[setupAccountJournalName];
            var detail = new SetupJournalAccountDetailViewModel();

            if (!TryUpdateModel<SetupJournalAccountDetailViewModel>(detail))
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

            return View(new GridModel<SetupJournalAccountDetailViewModel>(model));
        }

        [HttpPost]
        public ActionResult GetAccountCodeSelectList(string text)
        {
            var accounts = from c in entities.Accounts
                           select new
                           {
                               Id = c.Id,
                               Name = c.Name,
                               Code = c.Code,
                               Fullname = c.Code + " - " + c.Name
                           };

            if (text != null)
            {
                accounts = accounts.Where(a => a.Name.Contains(text) || a.Code.Contains(text));
            }

            return new JsonResult { Data = new SelectList(accounts.ToList(), "Id", "Fullname") };
        }

        [HttpPost]
        public ActionResult GetAccountSide()
        {
            Dictionary<string, string> accountSide = new Dictionary<string, string>();

            accountSide.Add("D", "Debet");
            accountSide.Add("K", "Kredit");

            var accountSides = from a in accountSide
                               select a;
            return new JsonResult { Data = new SelectList(accountSides.ToList(), "Key", "Value") };
        }
    }
}
