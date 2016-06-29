using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Models;
using SKA.Controllers;
using Telerik.Web.Mvc;
using SKA.Areas.SKA.Models.ViewModels;

namespace SKA.Areas.SKA.Controllers
{
    public class BeginningBalancesController : BaseController
    {
        //
        // GET: /SKA/BeginningBalances/

        private SKAEntities entities;

        public BeginningBalancesController()
        {
            entities = new SKAEntities();
        }

        [GridAction]
        public ActionResult GetList()
        {
            var model = from c in entities.BeginningBalances
                        select new BeginningBalancesViewModel 
                        {
                            Id = c.Id,
                            Amount = c.Amount,
                            Year = c.Year
                        };
            return View(new GridModel<BeginningBalancesViewModel>(model));
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new BeginningBalancesViewModel();
            model.Year = DateTime.Now.Year;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(BeginningBalancesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existItem = (from c in entities.BeginningBalances
                                     where c.Year == model.Year
                                     select c).FirstOrDefault();

                    var newItem = new BeginningBalance();

                    if (existItem == null)
                    {
                        newItem.Id = model.Id;
                        newItem.Amount = model.Amount;
                        newItem.Year = model.Year;

                        entities.BeginningBalances.AddObject(newItem);
                        entities.SaveChanges();
                        saveHistory();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetErrorMessageViewData("Data has existed");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing BeginningBalancesController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = (from c in entities.BeginningBalances
                        where c.Id == id
                        select new BeginningBalancesViewModel 
                        {
                            Amount = c.Amount,
                            Year = c.Year
                        }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(BeginningBalancesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateItem = (from c in entities.BeginningBalances
                                      where c.Id == model.Id
                                      select c).FirstOrDefault();

                    if (updateItem == null)
                    {
                        return RedirectToAction("Index");
                    }

                    updateItem.Amount = model.Amount;
                    updateItem.Year = model.Year;

                    entities.SaveChanges();
                    saveHistory();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing BeginningBalancesController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var model = (from c in entities.BeginningBalances
                             where c.Id == id
                             select new BeginningBalancesViewModel
                             {
                                 Amount = c.Amount,
                                 Year = c.Year
                             }).FirstOrDefault();
                if (model == null)
                {
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing BeginningBalancesController.Delete [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(BeginningBalancesViewModel model)
        {
            try
            {
                var deleteItem = (from c in entities.BeginningBalances
                                  where c.Id == model.Id
                                  select c).FirstOrDefault();

                if (deleteItem != null)
                {
                    entities.BeginningBalances.DeleteObject(deleteItem);
                    entities.SaveChanges();
                    saveHistory();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing BeginningBalancesController.Delete [Post].", ex);
                SetErrorMessageViewData("Data cannot be deleted.");
            }
            return View("Index");
        }

    }
}
