using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Filters;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;

namespace SKA.Areas.Setting.Controllers
{
    [BranchCodeFilter]
    public class BankController : BaseController
    {
        private SKAEntities entities;

        public BankController()
        {
            entities = new SKAEntities();
            //ViewBag.Value = this.getSetupValueLabel();
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (TempData["SearchValue"] != null)
            {
                ViewBag.SearchValue =TempData["SearchValue"].ToString();
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
        public ActionResult GetListBank(string searchValue)
        {
            var model = from b in entities.Banks
                        select new VoucherKas.Models.ViewModel.BankViewModel
                        {
                            Id = b.Id,
                            BankName = b.Name,
                            ACNumber = b.ACNumber
                        };
            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.BankName.Contains(searchValue)
                    || a.ACNumber.Contains(searchValue));
            }
            return View(new GridModel<VoucherKas.Models.ViewModel.BankViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(VoucherKas.Models.ViewModel.BankViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldBank = (from a in entities.Banks
                                   where a.ACNumber == model.ACNumber
                                   select a).FirstOrDefault();

                    var newData = new Bank();

                    if (oldBank == null)
                    {
                        newData.Id = Guid.NewGuid();
                        newData.ACNumber = model.ACNumber;
                        newData.Name = model.BankName;

                        entities.Banks.AddObject(newData);
                        entities.SaveChanges();
                        saveHistory(model.BankName + "-" + model.ACNumber);
                        return RedirectToAction("Index");
                    }
                    else {
                        ModelState.AddModelError("ACNumber","AC Bank sudah ada, silahkan isi data Bank dengan nomor AC yang lain.");
                    }                        
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing BankController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var model = new VoucherKas.Models.ViewModel.BankViewModel();

            try
            {
                var result = (from r in entities.Banks
                              where r.Id == id
                              select r).FirstOrDefault();
                if (result == null)
                {
                    SetErrorMessageViewData("Data tidak dapat ditemukan.");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.BankName = result.Name;
                    model.ACNumber = result.ACNumber;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing BankController.Edit[Get].", ex);
                SetErrorMessageViewData("Data tidak dapat ditampilkan.");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(VoucherKas.Models.ViewModel.BankViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var bank = (from b in entities.Banks
                                where b.Id == model.Id
                                select b).FirstOrDefault();

                    var acBank = (from a in entities.Banks
                                  where a.ACNumber == model.ACNumber && a.Id != model.Id
                                  select a).FirstOrDefault();
                    
                    if (acBank == null)
                    {
                        bank.Name = model.BankName;
                        bank.ACNumber = model.ACNumber;

                        entities.SaveChanges();
                        saveHistory(model.BankName + "-" + model.ACNumber);
                        return RedirectToAction("Index");
                    }
                    else {
                        ModelState.AddModelError("ACNumber","AC Bank sudah ada, silahkan isi data Bank dengan nomor AC yang lain.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing BankController.Edit[Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited");
                }

            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var model = new VoucherKas.Models.ViewModel.BankViewModel();

            try
            {
                var result = (from r in entities.Banks
                              where r.Id == id
                              select r).FirstOrDefault();
                if (result == null)
                {
                    SetErrorMessageViewData("Data tidak dapat ditemukan.");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.BankName = result.Name;
                    model.ACNumber = result.ACNumber;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error where executing BankController.Delete[Get].", ex);
                SetErrorMessageViewData("Data tidak dapat ditampilkan.");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(VoucherKas.Models.ViewModel.BankViewModel model)
        {
            try
            {
                var result = (from r in entities.Banks
                              where r.Id == model.Id
                              select r).FirstOrDefault();
                if (result == null)
                {
                    SetErrorMessageViewData("Data tidak dapat ditemukan.");
                }
                else
                {
                    entities.Banks.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory(result.Name + "-" + result.ACNumber);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while execute BankController.Delete[Post].", ex);
                SetErrorMessageViewData("Data Bank tidak dapat dihapus karena sedang digunakan pada transaksi lain.");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CheckACNumber(string ACNumber, Guid Id)
        {
            var value = "";
            if (Id != null)
            {
                var existingNumber = (from c in entities.Banks
                                      where c.ACNumber == ACNumber
                                      select c.ACNumber).FirstOrDefault();



                if (existingNumber != null)
                {
                    value = "Nomor yang Anda masukkan sudah ada, silahkan isi dengan nomor yang lain.";
                }
            }
            else
            {
                var existingNumberEdit = (from c in entities.Banks
                                          where c.ACNumber == ACNumber && c.Id != Id
                                          select c.ACNumber).FirstOrDefault();

                if (existingNumberEdit != null)
                {
                    value = "Nomor yang Anda masukkan sudah ada, silahkan isi dengan nomor yang lain.";
                }
            }
           
            //var getCodeStatus = entities.JournalAccounts.Where(a => a.AccountId == getCode.Id && a.JournalTypeId == 2).FirstOrDefault();

            //var value = getCodeStatus.AccountSide;
            return new JsonResult { Data = value };
        }
    }
}
