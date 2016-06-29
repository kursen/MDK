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
    public class DHHDJournalFormatsController : BaseController
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

        public DHHDJournalFormatsController()
        {
            entities = new SKAEntities();
        }

        [GridAction]
        public ActionResult _SelectDetail(string searchValue)
        {
            var model = from data in entities.DHHDJournalFormats
                         select new DHHDJournalFormatsViewModel
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

            return View(new GridModel<DHHDJournalFormatsViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(DHHDJournalFormatsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newDHHDJournalFormat = new DHHDJournalFormat();

                    newDHHDJournalFormat.AccountCode = model.AccountCode;
                    newDHHDJournalFormat.Status = model.Status;
                    newDHHDJournalFormat.TurnNumber = model.TurnNumber;
                    newDHHDJournalFormat.Name = model.Name;

                    entities.DHHDJournalFormats.AddObject(newDHHDJournalFormat);
                    entities.SaveChanges();
                    saveHistory(model.AccountCode);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing DHHDJournalFormatsController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = (from d in entities.DHHDJournalFormats
                         where d.Id == id
                         select new DHHDJournalFormatsViewModel
                         {
                             AccountCode = d.AccountCode,
                             Status = d.Status,
                             TurnNumber = d.TurnNumber,
                             Name = d.Name
                         }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DHHDJournalFormatsViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.DHHDJournalFormats
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
                    Logger.Error("Error while executing DHHDJournalFormatsController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = new DHHDJournalFormatsViewModel();

            try
            {
                var result = (from b in entities.DHHDJournalFormats
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
                Logger.Error("Error where executing DHHDJournalFormatsController.Delete[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(DHHDJournalFormatsViewModel model)
        {
            try
            {
                var result = (from b in entities.DHHDJournalFormats
                              where b.Id == model.Id
                              select b).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                }
                else
                {
                    entities.DHHDJournalFormats.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory(result.AccountCode);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while execute DHHDJournalFormatsController.Delete[Post].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
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
    }
}
