using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Areas.SKA.Models.ViewModels;
using SKA.Models;
using System.Globalization;

namespace SKA.Areas.SKA.Controllers
{
    public class EndofPeriodController : BaseController
    {
        //
        // GET: /SKA/EndofPeriod/
        private SKAEntities entities;

        public EndofPeriodController()
        {
            entities = new SKAEntities();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ClosingBook()
        {
            var model = new EndofPeriodViewModel();
            model.ClosingDate = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public ActionResult ClosingBook(EndofPeriodViewModel model)
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDate = date.AddMonths(1).AddDays(-1);

            string transactionDateString = Request.Form["ClosingDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);

            model.ClosingDate = transactionDate;

            var errorItem = ModelState.FirstOrDefault(m => m.Key == "ClosingDate");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (ModelState.IsValid)
            {
                //if (model.ClosingDate == lastDate)
                //{
                    var getClosingDate = entities.Closings.FirstOrDefault();

                    if (getClosingDate == null)
                    {
                        var newTransaction = new Closing();

                        newTransaction.ClosingDate = model.ClosingDate;
                        newTransaction.Description = model.Information;
                        newTransaction.RefrenceNumber = model.EvidenceNumber;

                        entities.Closings.AddObject(newTransaction);
                    }
                    else {
                        getClosingDate.ClosingDate = model.ClosingDate;
                        getClosingDate.Description = model.Information;
                        getClosingDate.RefrenceNumber = model.EvidenceNumber;
                    }
                    entities.SaveChanges();
                    saveHistory(model.ClosingDate.ToString());
                //}
                //else {
                //    ModelState.AddModelError("ClosingDate", "Maaf, belum waktunya tutup buku. Proses tutup buku dilakukan setiap akhir bulan dari bulan yang sedang berjalan.");
                //}
            }
            return View(model);
        }

        public ActionResult BalanceMoving()
        {
            var model = new EndofPeriodViewModel();
            model.NewPeriodDate = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public ActionResult BalanceMoving(EndofPeriodViewModel model)
        {
            string transactionDateString = Request.Form["NewPeriodDate"];
            DateTime transactionDate;
            DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);

            model.NewPeriodDate = transactionDate;
            var errorItem = ModelState.FirstOrDefault(m => m.Key == "NewPeriodDate");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            var balance = entities.ProcSetLastBalance(model.NewPeriodDate);
            saveHistory(model.NewPeriodDate.ToString());
            return View(model);
        }
        
    }
}
