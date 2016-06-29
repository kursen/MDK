using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.Setting.Models.ViewModels;
using SKA.Controllers;
using SKA.Filters;

namespace SKA.Areas.Setting.Controllers
{
    [BranchCodeFilter]
    public class BranchController : BaseController
    {
        private SKAEntities entities;

        public BranchController()
        {
            entities = new SKAEntities();
        }

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

        [GridAction]
        public ActionResult GetList(string searchValue)
        {
            var getBranch = GetCurrentUserBranchId();
            var model = from b in entities.Branches
                        select new BranchViewModel
                        { 
                            Id = b.Id, 
                            Name = b.Name, 
                            ShortName = b.ShortName, 
                            Code = b.Code,
                            Address = b.Address,
                            ApproverName = b.ApproverName,
                            ApproverPosition = b.ApproverPosition,
                            MakerName = b.MakerName,
                            MakerPosition = b.MakerPosition,
                            FirstExaminerName = b.FirstExaminerName,
                            FirstExaminerPosition = b.FirstExaminerPosition,
                            SecondExaminerName = b.SecondExaminerName,
                            SecondExaminerPosition = b.SecondExaminerPosition
                        };

            if (getBranch.Code != "00")
            {
                model = model.Where(a => a.Code == getBranch.Code);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Name.Contains(searchValue)
                    || a.Code.Contains(searchValue)
                    || a.ShortName.Contains(searchValue)
                    || a.Address.Contains(searchValue));
            }
            return View(new GridModel <BranchViewModel >(model));
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost ]
        public ActionResult Create(BranchViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dataBranch = entities.Branches.Where(a => a.Code == model.Code || a.Name == model.Name).FirstOrDefault();

                    if (dataBranch == null)
                    {
                        var newData = new Branch();

                        newData.Name = model.Name;
                        newData.ShortName = model.ShortName;
                        newData.Code = model.Code;
                        newData.Address = model.Address;
                        newData.ApproverName = model.ApproverName;
                        newData.ApproverPosition = model.ApproverPosition;
                        newData.MakerName = model.MakerName;
                        newData.MakerPosition = model.MakerPosition;
                        newData.FirstExaminerName = model.FirstExaminerName;
                        newData.FirstExaminerPosition = model.FirstExaminerPosition;
                        newData.SecondExaminerName = model.SecondExaminerName;
                        newData.SecondExaminerPosition = model.SecondExaminerPosition;

                        entities.Branches.AddObject(newData);
                        entities.SaveChanges();
                        saveHistory(model.Code + "-" + model.Name);

                        return RedirectToAction("Index");
                    }
                    else {
                        SetErrorMessageViewData("Data tidak bisa ditambah. Data cabang sudah ada");
                    }
                    
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing BranchController.Create [Post].",ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var model = new BranchViewModel();

            try
                {
                    var result = (from b in entities.Branches
                                  where b.Id == Id
                                  select b).FirstOrDefault();
                    if (result == null)
                    {
                        SetErrorMessageViewData("Data cannot be found.");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        model.Id = result.Id;
                        model.Code = result.Code;
                        model.ShortName = result.ShortName;
                        model.Name = result.Name;
                        model.Address = result.Address;
                        model.ApproverName = result.ApproverName;
                        model.ApproverPosition = result.ApproverPosition;
                        model.MakerName = result.MakerName;
                        model.MakerPosition = result.MakerPosition;
                        model.FirstExaminerName = result.FirstExaminerName;
                        model.FirstExaminerPosition = result.FirstExaminerPosition;
                        model.SecondExaminerName = result.SecondExaminerName;
                        model.SecondExaminerPosition = result.SecondExaminerPosition;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing BranchController.Edit[Get].",ex);
                    SetErrorMessageViewData("Data cannot be displayed");
                }
                return View(model);
            }

        [HttpPost]
        public ActionResult Edit(BranchViewModel model)
        {
            if(ModelState.IsValid )
            {
                try
                {
                    var getBranch = GetCurrentUserBranchId();
                    var branch = (from b in entities.Branches 
                                  where b.Id == model.Id
                                  select b).FirstOrDefault();

                    var lastBranch = (from b in entities.Branches
                                      where b.Id != model.Id && b.Code == model.Code
                                      select b).FirstOrDefault();
                    if (lastBranch == null)
                    {
                        branch.Code = model.Code;
                        branch.Name = model.Name;
                        branch.ShortName = model.ShortName;
                        branch.Address = model.Address;
                        branch.ApproverName = model.ApproverName;
                        branch.ApproverPosition = model.ApproverPosition;
                        branch.MakerName = model.MakerName;
                        branch.MakerPosition = model.MakerPosition;
                        branch.FirstExaminerName = model.FirstExaminerName;
                        branch.FirstExaminerPosition = model.FirstExaminerPosition;
                        branch.SecondExaminerName = model.SecondExaminerName;
                        branch.SecondExaminerPosition = model.SecondExaminerPosition;

                        entities.SaveChanges();
                        saveHistory(model.Code + "-" + model.Name);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Code","Kode cabang sudah tersedia, silahkan masukkan kode lainnya.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing BranchController.Edit[Post].",ex);
                    SetErrorMessageViewData("Data cannot be edited");
                }

            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model=new BranchViewModel ();

            try
            {
                var result = (from b in entities.Branches
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
                    model.Code = result.Code;
                    model.ShortName = result.ShortName;
                    model.Name = result.Name;
                    model.Address = result.Address;
                    model.ApproverName = result.ApproverName;
                    model.ApproverPosition = result.ApproverPosition;
                    model.MakerName = result.MakerName;
                    model.MakerPosition = result.MakerPosition;
                    model.FirstExaminerName = result.FirstExaminerName;
                    model.FirstExaminerPosition = result.FirstExaminerPosition;
                    model.SecondExaminerName = result.SecondExaminerName;
                    model.SecondExaminerPosition = result.SecondExaminerPosition;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error where executing BranchController.Delete[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(BranchViewModel model)
        {
            try
            {
                var result = (from b in entities.Branches
                              where b.Id == model.Id
                              select b).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                }
                else
                {
                    entities.Branches.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory(result.Code + "-" + result.Name);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while execute BranchController.Delete[Post].", ex);
                SetErrorMessageViewData("Data tidak dapat dihapus karena digunakan dalam transaksi.");
            }
            return View(model);
        }


    }
}
