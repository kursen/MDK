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
    public class OverviewLongTermDebtController : BaseController
    {
        //
        // GET: /SKA/OverviewLongTermDebt/
        private SKAEntities entities;
        private int thisMonth = DateTime.Now.Month;
        public OverviewLongTermDebtController()
        {
            entities = new SKAEntities();
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

        public ActionResult Index(int year)
        {
            var model = new OverviewLongTermDebtViewModel();
            model.Year = year;
            return View();
        }

        [HttpPost]
        public ActionResult Index(OverviewLongTermDebtViewModel model)
        {
            return View(model);
        }

        [GridAction]
        public ActionResult GetList(int year)
        {
            var getYear = entities.LongTermReports.Where(a => a.Year == year && a.Month == thisMonth).FirstOrDefault();
            try
            {
                var template = (from c in entities.LongTermReportTemplates
                                select c).ToList();
                if (getYear == null)
                {

                    for (int i = 0; i < template.Count; i++)
                    {
                        var newPosition = new LongTermReport();
                        newPosition.Name = template[i].Name;
                        newPosition.Year = year;
                        newPosition.Month = thisMonth;
                        entities.LongTermReports.AddObject(newPosition);

                    }
                    entities.SaveChanges();

                    var model = from c in entities.LongTermReports
                                where c.Year == year && c.Month == thisMonth
                                select new OverviewLongTermDebtViewModel
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    Year = c.Year,
                                    Month = c.Month,
                                    NextOverdue = c.NextOverdue,
                                    Residual = c.Residual,
                                    Amount = c.Amount,
                                    ThisYearorMonth = c.ThisYearorMonth
                                };

                    return View(new GridModel<OverviewLongTermDebtViewModel>(model));

                }
                else
                {
                    var model = from c in entities.LongTermReports
                                where c.Year == year
                                select new OverviewLongTermDebtViewModel
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    Year = c.Year,
                                    Month = c.Month,
                                    NextOverdue = c.NextOverdue,
                                    Residual = c.Residual,
                                    Amount = c.Amount,
                                    ThisYearorMonth = c.ThisYearorMonth
                                };

                    return View(new GridModel<OverviewLongTermDebtViewModel>(model));
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error. Detail: ", ex);
                return RedirectToAction("GetYear");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var getYear = entities.LongTermReports.Where(b => b.Id == id).FirstOrDefault();
            try
            {
                var model = (from c in entities.LongTermReports
                                where c.Id == id
                                select new OverviewLongTermDebtViewModel
                                {
                                    Name = c.Name,
                                    Year = c.Year,
                                    Month = c.Month,
                                    ThisYearorMonth = c.ThisYearorMonth,
                                    Residual = c.Residual,
                                    NextOverdue = c.NextOverdue,
                                    Amount = c.Amount
                                }).FirstOrDefault();

                ViewData["Year"] = model.Year;
                return View(model);

            }
            catch (Exception ex)
            {
                Logger.Error("Error. Detail: ", ex);
                return RedirectToAction("Index", new { year = getYear.Year });
            }
        }

        [HttpPost]
        public ActionResult Edit(OverviewLongTermDebtViewModel model)
        {
           if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.LongTermReports
                                     where c.Id == model.Id
                                     select c).FirstOrDefault();

                    if (newDetail == null)
                    {
                        return RedirectToAction("Index", new { year = newDetail.Year });
                    }

                    newDetail.ThisYearorMonth = model.ThisYearorMonth;
                    newDetail.Residual = model.Residual;
                    newDetail.NextOverdue = model.NextOverdue;
                    newDetail.Amount = model.Amount;

                    entities.SaveChanges();
                    saveHistory("Tahun " + newDetail.Year.ToString());
                    var getYear = entities.LongTermReports.Where(a => a.Id == model.Id).FirstOrDefault();
                    return RedirectToAction("Index", new { year = getYear.Year });
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing OverviewLongTermDebtController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }

        
    }
}
