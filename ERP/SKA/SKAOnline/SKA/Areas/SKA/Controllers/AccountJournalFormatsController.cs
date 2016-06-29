using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using Telerik.Web.Mvc;
using SKA.Models;
using SKA.Areas.SKA.Models.ViewModels;

namespace SKA.Areas.SKA.Controllers
{
    public class AccountJournalFormatsController : BaseController
    {
        //
        // GET: /SKA/AccountJournalFormats/
        private SKAEntities entities;

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

        public AccountJournalFormatsController()
        {
            entities = new SKAEntities();
        }

        [HttpPost]
        public ActionResult GetAccountCodeSelectList(string text)
        {
            var accounts = (from a in entities.Accounts
                           select new { Code = a.Code.Substring(0, 5) });

            if (text != null)
            {
                accounts = accounts.Where(a => a.Code.StartsWith(text));
            }

            return new JsonResult { Data = new SelectList(accounts.Distinct().ToList(), "Code", "Code") };
        }
        
        [GridAction]
        public ActionResult _SelectDetail(string searchValue)
        {
            var model = from data in entities.AccountJournalFormats
                         select new AccountJournalFormatsViewModel
                         {
                             Id = data.Id,
                             Status = data.Status,
                             AccountCode = data.AccountCode,
                             TurnNumber = data.TurnNumber,
                             Name = data.Name
                         };

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.AccountCode.Contains(searchValue));
            }
            return View(new GridModel<AccountJournalFormatsViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(AccountJournalFormatsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newAccountJournalFormat = new AccountJournalFormat();

                    newAccountJournalFormat.AccountCode = model.AccountCode;
                    newAccountJournalFormat.Status = model.Status;
                    newAccountJournalFormat.TurnNumber = model.TurnNumber;
                    newAccountJournalFormat.Name = model.Name;

                    entities.AccountJournalFormats.AddObject(newAccountJournalFormat);
                    entities.SaveChanges();
                    saveHistory(model.AccountCode);

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    Logger.Error("Error while executing AccountJournalController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = (from d in entities.AccountJournalFormats
                         where d.Id == id
                         select new AccountJournalFormatsViewModel
                         {
                             AccountCode = d.AccountCode,
                             Status = d.Status,
                             TurnNumber = d.TurnNumber,
                             Name = d.Name
                         }).FirstOrDefault();

            return View(model);
        }
        
        [HttpPost]
        public ActionResult Edit(AccountJournalFormatsViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.AccountJournalFormats
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    if (newDetail == null)
                    {
                        return RedirectToAction("Index");
                    }
                    
                    newDetail.AccountCode = model.AccountCode;
                    newDetail.Status = model.Status;
                    newDetail.TurnNumber = model.TurnNumber;
                    newDetail.Name = model.Name;

                    entities.SaveChanges();
                    saveHistory(model.AccountCode);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing AccountJournalFormatsController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = new AccountJournalFormatsViewModel();

            try
            {
                var result = (from b in entities.AccountJournalFormats
                              where b.Id == id
                              select b).FirstOrDefault();

               
                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.AccountCode = result.AccountCode;
                    model.Status = result.Status;
                    model.TurnNumber = result.TurnNumber;
                    model.Name = result.Name;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error where executing AccountJournalFormatsController.Delete[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(AccountJournalFormatsViewModel model)
        {
            try
            {
                var result = (from b in entities.AccountJournalFormats
                              where b.Id == model.Id
                              select b).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                }
                else
                {
                    entities.AccountJournalFormats.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory(result.AccountCode);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while execute AccountJournalFormatsController.Delete[Post].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }
    }
}
