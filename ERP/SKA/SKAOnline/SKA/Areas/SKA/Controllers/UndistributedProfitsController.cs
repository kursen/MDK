using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using SKA.Areas.SKA.Models.ViewModels;
using Telerik.Web.Mvc;
using System.Globalization;

namespace SKA.Areas.SKA.Controllers
{
    public class UndistributedProfitsController : BaseController
    {
        //
        // GET: /SKA/UndistributedProfits/
        private SKAEntities entities;
        private string undistributedProfits = "UndistributedProfitsSession";//session grid 1
        private string undistributedProfitsDecrement = "UndistributedProfitsDecrementSession";//session grid 2
        private string undistributedProfitsIncrement = "UndistributedProfitsIncrementSession";//session grid 3

        public UndistributedProfitsController()
        {
            entities = new SKAEntities();
        }
        public ActionResult Index()
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

        [HttpGet]
        public ActionResult Edit(int year)
        {
            Session.Remove(undistributedProfits);
            Session.Remove(undistributedProfitsDecrement);
            Session.Remove(undistributedProfitsIncrement);

            var getYear = (from c in entities.UndistributedProfitYears
                           where c.Year == year
                           select c).FirstOrDefault();

            if (getYear == null)
            {
                var newYear = new UndistributedProfitYear();
                newYear.Year = year;
                entities.UndistributedProfitYears.AddObject(newYear);
                entities.SaveChanges();
            }

            var getDetailProfit = entities.UndistributedProfitDatas.Where(c => c.UndistributedProfitYear.Year == year && c.Status == "Laba Rugi").FirstOrDefault();
            var getDetailBalance = entities.UndistributedProfitDatas.Where(c => c.UndistributedProfitYear.Year == year && c.Status == "Saldo Laba").FirstOrDefault();
            
            var newDetail = new UndistributedProfitData();
            
            if (getDetailProfit == null)
            {
                newDetail.UndistributedProfitId = getYear.Id;
                newDetail.Status = "Laba Rugi";
                newDetail.Amount = 0;
                entities.UndistributedProfitDatas.AddObject(newDetail);
                entities.SaveChanges();
            }
            var newBalance = new UndistributedProfitData();
            if (getDetailBalance == null)
            {
                newBalance.UndistributedProfitId = getYear.Id;
                newBalance.Status = "Saldo Laba";
                newBalance.Amount = 0;
                entities.UndistributedProfitDatas.AddObject(newBalance);
                entities.SaveChanges();
            }
            
            var model = new UndistributedProfitsViewModel();
            
            model.Year = getYear.Year;
            model.Id = getYear.Id;
            model.ProfitAmountTax = getDetailProfit.Amount == null ? 0 : getDetailProfit.Amount;
            model.ProfitAmount = getDetailBalance.Amount == null ? 0 : getDetailBalance.Amount;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UndistributedProfitsViewModel model)
        {
            var undistributeProfitslSessionlist = (List<UndistributedProfitsDetailViewModel>)(Session[undistributedProfits]);
            var undistributeProfitsDecrementlSessionlist = (List<UndistributedProfitsDetailViewModel>)(Session[undistributedProfitsDecrement]);
            var undistributeProfitslSecondIncrementSessionlist = (List<UndistributedProfitsDetailViewModel>)(Session[undistributedProfitsIncrement]);
            if (ModelState.IsValid)
            {
                try
                {
                    var newDetail = (from c in entities.UndistributedProfitYears
                                     where c.Year == model.Year
                                     select c).FirstOrDefault();


                    if (newDetail == null)
                    {
                        return RedirectToAction("Index");
                    }


                    // delete old data first, before inserting new detail
                    foreach (var item in entities.UndistributedProfitDatas.Where(p => p.UndistributedProfitId== newDetail.Id))
                    {
                        entities.UndistributedProfitDatas.DeleteObject(item);
                    }

                    var addProfit = new UndistributedProfitData();

                    addProfit.UndistributedProfitId = newDetail.Id;
                    addProfit.Status = "Laba Rugi";
                    addProfit.Amount = model.ProfitAmountTax;
                    addProfit.Description = "Laba/(Rugi) Setelah Pajak";

                    entities.UndistributedProfitDatas.AddObject(addProfit);
                    // insert new detail
                    foreach (var item in undistributeProfitslSessionlist)
                    {
                        var detail = new UndistributedProfitData
                        {
                            UndistributedProfitId = newDetail.Id,
                            Description = item.Description,
                            Amount = item.Amount,
                            Tanggal = item.Date,
                            Status = "Dikurangi I"
                        };
                        entities.UndistributedProfitDatas.AddObject(detail);
                    }

                    foreach (var itemDecrement in undistributeProfitsDecrementlSessionlist)
                    {
                        var detail = new UndistributedProfitData
                        {
                            UndistributedProfitId = newDetail.Id,
                            Description = itemDecrement.Description,
                            Amount = itemDecrement.Amount,
                            Tanggal = itemDecrement.Date,
                            Status = "Ditambah"
                        };
                        entities.UndistributedProfitDatas.AddObject(detail);
                    }

                    foreach (var itemIncrement in undistributeProfitslSecondIncrementSessionlist)
                    {
                        var detail = new UndistributedProfitData
                        {
                            UndistributedProfitId = newDetail.Id,
                            Description = itemIncrement.Description,
                            Amount = itemIncrement.Amount,
                            Tanggal = itemIncrement.Date,
                            Status = "Dikurangi II"
                        };
                        entities.UndistributedProfitDatas.AddObject(detail);
                    }

                    var addBalance = new UndistributedProfitData();

                    addBalance.UndistributedProfitId = newDetail.Id;
                    addBalance.Status = "Saldo Laba";
                    addBalance.Amount = model.ProfitAmount;
                    addBalance.Description = "Saldo Laba Yang Belum Dibagi";

                    entities.UndistributedProfitDatas.AddObject(addBalance);

                    entities.SaveChanges();
                    saveHistory("Tahun " + model.Year.ToString());
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing UndistributedProfitsController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }
            return View(model);
        }

        [GridAction]
        public ActionResult _SelectDetailIncrease(int? year)
        {
            var model = (List<UndistributedProfitsDetailViewModel>)Session[undistributedProfits];

            if (model == null)
            {
                model = (from data in entities.UndistributedProfitDatas
                         where data.UndistributedProfitYear.Year.Equals(year.Value) && data.Status.Equals("Dikurangi I")
                         select new UndistributedProfitsDetailViewModel
                         {
                             Id = data.UndistributedProfitId,
                             Description = data.Description,
                             Amount = data.Amount,
                             Date = data.Tanggal
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[undistributedProfits] = model;
            }

            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [GridAction]
        public ActionResult _SelectDetailDecrease(int? year)
        {
            var model = (List<UndistributedProfitsDetailViewModel>)Session[undistributedProfitsDecrement];

            if (model == null)
            {
                model = (from data in entities.UndistributedProfitDatas
                         where data.UndistributedProfitYear.Year.Equals(year.Value) && data.Status.Equals("Ditambah")
                         select new UndistributedProfitsDetailViewModel
                         {
                             //UndistributedProfitId = data.UndistributedProfitId,
                             Id = data.UndistributedProfitId,
                             Description = data.Description,
                             Amount = data.Amount,
                             Date = data.Tanggal
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[undistributedProfitsDecrement] = model;
            }

            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [GridAction]
        public ActionResult _SelectDetailSecondIncrease(int? year)
        {
            var model = (List<UndistributedProfitsDetailViewModel>)Session[undistributedProfitsIncrement];

            if (model == null)
            {
                model = (from data in entities.UndistributedProfitDatas
                         where data.UndistributedProfitYear.Year.Equals(year.Value) && data.Status.Equals("Dikurangi II")
                         select new UndistributedProfitsDetailViewModel
                         {
                             //UndistributedProfitId = data.UndistributedProfitId,
                             Id = data.UndistributedProfitId,
                             Description = data.Description,
                             Amount = data.Amount,
                             Date = data.Tanggal
                         }).ToList();

                int counter = 0;

                foreach (var item in model)
                {
                    counter += 1;
                    item.Id = counter;
                }

                Session[undistributedProfitsIncrement] = model;
            }

            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _InsertDetailIncrement()
        {
            var model = (List<UndistributedProfitsDetailViewModel>)Session[undistributedProfits];
            UndistributedProfitsDetailViewModel postmodel = new UndistributedProfitsDetailViewModel();

            if (model == null)
                model = new List<UndistributedProfitsDetailViewModel>();

            if (TryUpdateModel(postmodel))
            {
                
                    int maxId = 0;

                    if (model.Count > 0)
                        maxId = model.Max(m => m.Id);

                    postmodel.Id = maxId + 1;

                    model.Insert(0, postmodel);

                    Session[undistributedProfits] = model;
                
            }

            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _InsertDetailDecrement()
        {
            var model = (List<UndistributedProfitsDetailViewModel>)Session[undistributedProfitsDecrement];
            UndistributedProfitsDetailViewModel postmodel = new UndistributedProfitsDetailViewModel();

            if (model == null)
                model = new List<UndistributedProfitsDetailViewModel>();

            if (TryUpdateModel(postmodel))
            {

                int maxId = 0;

                if (model.Count > 0)
                    maxId = model.Max(m => m.Id);

                postmodel.Id = maxId + 1;

                model.Insert(0, postmodel);

                Session[undistributedProfitsDecrement] = model;

            }

            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _InsertDetailSecondIncrement()
        {
            var model = (List<UndistributedProfitsDetailViewModel>)Session[undistributedProfitsIncrement];
            UndistributedProfitsDetailViewModel postmodel = new UndistributedProfitsDetailViewModel();

            if (model == null)
                model = new List<UndistributedProfitsDetailViewModel>();

            if (TryUpdateModel(postmodel))
            {

                int maxId = 0;

                if (model.Count > 0)
                    maxId = model.Max(m => m.Id);

                postmodel.Id = maxId + 1;

                model.Insert(0, postmodel);

                Session[undistributedProfitsIncrement] = model;

            }

            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _UpdateDetailIncrement()
        {
            var model = (List<UndistributedProfitsDetailViewModel>)Session[undistributedProfits];
            UndistributedProfitsDetailViewModel postmodel = new UndistributedProfitsDetailViewModel();

            //string transactionDateString = postmodel.Date.ToString();
            //DateTime transactionDate;
            //DateTime.TryParse(transactionDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out transactionDate);

            //postmodel.Date = transactionDate;
            if (TryUpdateModel(postmodel))
            {
                var item = model.Where(m => m.Id == postmodel.Id).FirstOrDefault();

                if (item != null)
                {
                    item.Description = postmodel.Description;
                    item.Date = postmodel.Date;
                    item.Amount = postmodel.Amount;
                }
            }
            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _UpdateDetailDecrement()
        {
            var model = (List<UndistributedProfitsDetailViewModel>)Session[undistributedProfitsDecrement];
            UndistributedProfitsDetailViewModel postmodel = new UndistributedProfitsDetailViewModel();

            if (TryUpdateModel(postmodel))
            {
                var item = model.Where(m => m.Id == postmodel.Id).FirstOrDefault();

                if (item != null)
                {
                    item.Description = postmodel.Description;
                    item.Date = postmodel.Date;
                    item.Amount = postmodel.Amount;
                }
            }
            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [GridAction()]
        public ActionResult _UpdateDetailSecondIncrement()
        {
            var model = (List<UndistributedProfitsDetailViewModel>)Session[undistributedProfitsIncrement];
            UndistributedProfitsDetailViewModel postmodel = new UndistributedProfitsDetailViewModel();

            if (TryUpdateModel(postmodel))
            {
                var item = model.Where(m => m.Id == postmodel.Id).FirstOrDefault();

                if (item != null)
                {
                    item.Description = postmodel.Description;
                    item.Date = postmodel.Date;
                    item.Amount = postmodel.Amount;
                }
            }
            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetailIncrement()
        {
            IList<UndistributedProfitsDetailViewModel> model = (IList<UndistributedProfitsDetailViewModel>)Session[undistributedProfits];
            //var model = (List<UndistributedProfitsDetailViewModel>)Session[undistributedProfits];
            var detail = new UndistributedProfitsDetailViewModel();


            if (!TryUpdateModel<UndistributedProfitsDetailViewModel>(detail))
            {
                // check for member with [Required] attribute. 
                // If it found, clear the error, otherwise, the delete operation will not perform.

                var accountCodeError = ModelState.Keys.Where(e => e == "Date");

                if (accountCodeError != null)
                    ModelState["Date"].Errors.Clear();
            }

            if (detail != null)
            {
                var item = model.Where(a => a.Id == detail.Id).FirstOrDefault();

                if (item != null)
                    model.Remove(item);
            }

            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetailDecrement()
        {
            IList<UndistributedProfitsDetailViewModel> model = (IList<UndistributedProfitsDetailViewModel>)Session[undistributedProfitsDecrement];
            var detail = new UndistributedProfitsDetailViewModel();
            if (!TryUpdateModel<UndistributedProfitsDetailViewModel>(detail))
            {
                // check for member with [Required] attribute. 
                // If it found, clear the error, otherwise, the delete operation will not perform.

                var accountCodeError = ModelState.Keys.Where(e => e == "Date");

                if (accountCodeError != null)
                    ModelState["Date"].Errors.Clear();
            }

            if (detail != null)
            {
                var item = model.Where(c => c.Id == detail.Id).FirstOrDefault();

                if (item != null)
                    model.Remove(item);
            }

            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }

        [HttpPost]
        [GridAction]
        public ActionResult _DeleteDetailSecondIncrement()
        {
            IList<UndistributedProfitsDetailViewModel> model = (IList<UndistributedProfitsDetailViewModel>)Session[undistributedProfitsIncrement];
            var detail = new UndistributedProfitsDetailViewModel();

            if (!TryUpdateModel<UndistributedProfitsDetailViewModel>(detail))
            {
                // check for member with [Required] attribute. 
                // If it found, clear the error, otherwise, the delete operation will not perform.

                var accountCodeError = ModelState.Keys.Where(e => e == "Date");

                if (accountCodeError != null)
                    ModelState["Date"].Errors.Clear();
            }
            if (detail != null)
            {
                var item = model.Where(c => c.Id == detail.Id).FirstOrDefault();

                if (item != null)
                    model.Remove(item);
            }

            return View(new GridModel<UndistributedProfitsDetailViewModel>(model));
        }
    }
}
