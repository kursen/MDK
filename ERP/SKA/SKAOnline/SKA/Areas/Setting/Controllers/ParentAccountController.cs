using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Areas.Setting.Models.ViewModels;
using Telerik.Web.Mvc;
using SKA.Models;

namespace SKA.Areas.Setting.Controllers
{
    public class ParentAccountController : BaseController
    {
        //
        // GET: /Setting/ParentAccount/
        private SKAEntities entities;

        public ParentAccountController()
        {
            entities = new SKAEntities();
        }
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
            var model = from at in entities.Parents
                        select new ParentAccountViewModel
                        {
                            Id = at.Id,
                            Code = at.Code,
                            Name = at.Name
                        };

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Name.Contains(searchValue)
                    || a.Code.Contains(searchValue));
            }
            return View(new GridModel<ParentAccountViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ParentAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newAccount = new Parent();
                    
                    var oldAccount = (from a in entities.Parents
                                      where a.Code == model.Code
                                      select a).FirstOrDefault();
                    if (oldAccount == null)
                    {
                        newAccount.Code = model.Code;
                        newAccount.Name = model.Name;

                        entities.Parents.AddObject(newAccount);
                        entities.SaveChanges();
                        saveHistory(model.Code);
                        return RedirectToAction("Index");
                    }
                    else {
                        ModelState.AddModelError("Code","Kode sudah ada sebelumnya, silahkan masukkan kembali kode untuk kepala perkiraan.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing ParentAccountController.Create [Post].", ex);
                    SetErrorMessageViewData("Data tidak dapat ditambah.");
                }
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new ParentAccountViewModel();

            try
            {

                var result = (from at in entities.Parents
                              where at.Id == id
                              select at).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.Code = result.Code;
                    model.Name = result.Name;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing ParentAccountController.Edit [Get].", ex);
                SetErrorMessageViewData("Data tidak dapat ditampilkan.");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ParentAccountViewModel model)
        {
            var updateAccount = (from at in entities.Parents
                                 where at.Id == model.Id
                                 select at).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    var oldAccount = (from a in entities.Parents
                                      where a.Code == model.Code && a.Id != model.Id
                                      select a).FirstOrDefault();

                    if (oldAccount == null)
                    {
                        updateAccount.Code = model.Code;
                        updateAccount.Name = model.Name;

                        entities.SaveChanges();
                        saveHistory(model.Code);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Code", "Kode sudah ada sebelumnya, silahkan masukkan kembali kode untuk kepala perkiraan.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing ParentAccountController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data tidak dapat diedit.");
                }
            }

            model.Code = updateAccount.Code;
            model.Name = updateAccount.Name;

            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = new ParentAccountViewModel();

            try
            {

                var result = (from at in entities.Parents
                              where at.Id == id
                              select at).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data tidak dapat ditemukan.");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.Code = result.Code;
                    model.Name = result.Name;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing ParentAccountController.Delete [Get].", ex);
                SetErrorMessageViewData("Data tidak dapat ditampilkan.");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(ParentAccountViewModel model)
        {
            var result = (from at in entities.Parents
                          where at.Id == model.Id
                          select at).FirstOrDefault();
            try
            {
                if (result == null)
                {
                    SetErrorMessageViewData("Data tidak dapat ditemukan.");
                }
                else
                {
                    entities.Parents.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory(result.Code);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing ParentAccountController.Delete [Post].", ex);
                SetErrorMessageViewData("Kode Perkiraan tidak bisa dihapus.");
            }

            model.Id = result.Id;
            model.Code = result.Code;
            model.Name = result.Name;
            return View(model);
        }
    }
}
