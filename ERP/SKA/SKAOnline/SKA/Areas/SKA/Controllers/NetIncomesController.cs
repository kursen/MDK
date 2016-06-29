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
    public class NetIncomesController : BaseController
    {
        //
        // GET: /SKA/NetIncome/
        private SKAEntities entities;

        public NetIncomesController()
        {
            entities = new SKAEntities();
        }

        [GridAction]
        public ActionResult GetList()
        {
            var model = from c in entities.NetIncomes
                        select new NetIncomesViewModel 
                        {
                            Id = c.Id,
                            Amount = c.Amount,
                            Year = c.Year
                        };
            return View(new GridModel<NetIncomesViewModel>(model));
        }

        public ActionResult GetYear()
        {
            Dictionary<int, int> years = new Dictionary<int, int>();

            for (int i = DateTime.Now.Year + 1; i >= 1990; i--)
            {
                years.Add(i, i);
            }

            var newYear = from a in years
                          select a;

            ViewBag.Year = new SelectList(newYear.ToList(), "Key", "Value");

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new NetIncomesViewModel();
            model.Year = DateTime.Now.Year;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(NetIncomesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existItem = (from c in entities.NetIncomes
                                     where c.Year == model.Year
                                     select c).FirstOrDefault();

                    var newItem = new NetIncome();

                    if (existItem == null)
                    {
                        newItem.Id = model.Id;
                        newItem.Amount = model.Amount;
                        newItem.Year = model.Year;

                        entities.NetIncomes.AddObject(newItem);
                        entities.SaveChanges();
                        saveHistory("Tahun " + model.Year.ToString());
                        return RedirectToAction("Index");
                    }
                    else 
                    {
                        SetErrorMessageViewData("Data has existed");
                    }
                }
                catch(Exception ex)
                {
                    Logger.Error("Error while executing NetIncomesController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = (from c in entities.NetIncomes
                        where c.Id == id
                        select new NetIncomesViewModel 
                        {
                            Amount = c.Amount,
                            Year = c.Year
                        }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(NetIncomesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateItem = (from c in entities.NetIncomes
                                      where c.Id == model.Id
                                      select c).FirstOrDefault();

                    if (updateItem == null)
                    {
                        return RedirectToAction("Index");
                    }

                    updateItem.Amount = model.Amount;
                    updateItem.Year = model.Year;

                    entities.SaveChanges();
                    saveHistory("Tahun " + model.Year.ToString());
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing NetIncomesController.Edit [Post].", ex);
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
                var model = (from c in entities.NetIncomes
                             where c.Id == id
                             select new NetIncomesViewModel
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
                Logger.Error("Error while executing NetIncomesController.Delete [Get].", ex);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(NetIncomesViewModel model)
        {
            try
            {
                var deleteItem = (from c in entities.NetIncomes
                                  where c.Id == model.Id
                                  select c).FirstOrDefault();

                if (deleteItem != null)
                {
                    entities.NetIncomes.DeleteObject(deleteItem);
                    entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing NetIncomesController.Delete [Post].", ex);
                SetErrorMessageViewData("Data cannot be deleted.");
            }
            return View("Index");
        }
    }
}
