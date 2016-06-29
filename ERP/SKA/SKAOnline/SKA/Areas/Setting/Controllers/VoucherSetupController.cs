using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.Setting.Models.ViewModels;
using SKA.Filters;

namespace SKA.Areas.Setting.Controllers
{
    [BranchCodeFilter]
    public class VoucherSetupController : BaseController
    {
        //
        // GET: /SKA/VoucherSetup/
        private SKAEntities entities;
        public VoucherSetupController()
        {
            entities = new SKAEntities();
            //ViewBag.Value = this.getSetupValueLabel();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //---------SETUP VOUCHER > 300------------//
        public ActionResult IndexVoucherUp()
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
        public ActionResult IndexVoucherUp(string searchValue)
        {
            TempData["SearchValue"] = searchValue;
            return RedirectToAction("IndexVoucherUp");
        }

        [GridAction]
        public ActionResult GetListVoucherUp(string searchValue)
        {
            var branch = GetCurrentUserBranchId();
            var model = from c in entities.VoucherSetups
                         where c.Status == "1"
                         select new VoucherSetupViewModel
                         {
                             Id = c.Id,
                             MakerName = c.MakerName,
                             MakerPosition = c.MakerPosition,
                             ExaminerName = c.ExaminerName,
                             ExaminerPosition = c.ExaminerPosition,
                             InvestigatorName = c.InvestigatorName,
                             InvestigatorPosition = c.InvestigatorPosition,
                             FirstAdministrationName = c.FirstAdministrationName,
                             SecondAdministrationName = c.SecondAdministrationName,
                             BranchId = c.BranchId,
                             BranchCode = c.Branch.Code,
                             Status = c.Status
                         };

            if (branch.Code != "00")
            {
                model = model.Where(a => a.BranchId == branch.Id);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.MakerName.Contains(searchValue)
                    || a.MakerPosition.Contains(searchValue)
                    || a.ExaminerName.Contains(searchValue)
                    || a.ExaminerPosition.Contains(searchValue)
                    || a.InvestigatorName.Contains(searchValue)
                    || a.InvestigatorPosition.Contains(searchValue)
                    || a.FirstAdministrationName.Contains(searchValue)
                    || a.SecondAdministrationName.Contains(searchValue)
                    || a.BranchCode.Contains(searchValue));
            }

            return View(new GridModel<VoucherSetupViewModel>(model));
        }

        [HttpGet]
        public ActionResult CreateVoucherUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateVoucherUp(VoucherSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var branch = GetCurrentUserBranchId();
                    

                    var getDirector = entities.VoucherSetups.Where(a => a.Status == "1" && a.BranchId == branch.Id).FirstOrDefault();

                    if (getDirector == null)
                    {
                        var newSetupUp = new VoucherSetup();
                        newSetupUp.MakerName = model.MakerName;
                        newSetupUp.MakerPosition = model.MakerPosition;
                        newSetupUp.ExaminerName = model.ExaminerName;
                        newSetupUp.ExaminerPosition = model.ExaminerPosition;
                        newSetupUp.InvestigatorName = model.InvestigatorName;
                        newSetupUp.InvestigatorPosition = model.InvestigatorPosition;
                        newSetupUp.BranchId = branch.Id;
                        newSetupUp.FirstAdministrationName = model.FirstAdministrationName;
                        newSetupUp.SecondAdministrationName = model.SecondAdministrationName;
                        newSetupUp.Status = "1";

                        entities.VoucherSetups.AddObject(newSetupUp);
                        entities.SaveChanges();
                        saveHistory();
                        return RedirectToAction("IndexVoucherUp");
                    }
                    else {
                        SetErrorMessageViewData("Data tidak dapat ditambah, setting pejabat untuk cabang ini sudah ada.");
                    }
                    
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing VoucherSetupController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult EditVoucherUp(Guid id)
        {
            var model = new VoucherSetupViewModel();
            var branch = GetCurrentUserBranchId();
            try
            {
                var getResult = (from c in entities.VoucherSetups
                                 where c.Id == id
                                 select c).FirstOrDefault();

                if (getResult == null)
                {
                    SetErrorMessageViewData("Data tidak ditemukan.");
                    return RedirectToAction("IndexVoucherUp");
                }
                else
                {
                    model.MakerName = getResult.MakerName;
                    model.MakerPosition = getResult.MakerPosition;
                    model.ExaminerName = getResult.ExaminerName;
                    model.ExaminerPosition = getResult.ExaminerPosition;
                    model.InvestigatorName = getResult.InvestigatorName;
                    model.InvestigatorPosition = getResult.InvestigatorPosition;
                    model.FirstAdministrationName = getResult.FirstAdministrationName;
                    model.SecondAdministrationName = getResult.SecondAdministrationName;
                    model.BranchId = branch.Id;
                    model.Status = getResult.Status;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing VoucherSetupController.Edit[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditVoucherUp(VoucherSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var setupUp = (from b in entities.VoucherSetups
                                   where b.Id == model.Id
                                   select b).FirstOrDefault();

                    setupUp.ExaminerName = model.ExaminerName;
                    setupUp.ExaminerPosition = model.ExaminerPosition;
                    setupUp.MakerName = model.MakerName;
                    setupUp.MakerPosition = model.MakerPosition;
                    setupUp.InvestigatorName = model.InvestigatorName;
                    setupUp.InvestigatorPosition = model.InvestigatorPosition;
                    setupUp.FirstAdministrationName = model.FirstAdministrationName;
                    setupUp.SecondAdministrationName = model.SecondAdministrationName;

                    entities.SaveChanges();
                    saveHistory();
                    return RedirectToAction("IndexVoucherUp");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing VoucherSetupController.Edit[Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited");
                }

            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteVoucherUp(Guid id)
        {
            
            var model = new VoucherSetupViewModel();
            var branch = GetCurrentUserBranchId();
            try
            {
                var result = (from b in entities.VoucherSetups
                              where b.Id == id
                              select b).FirstOrDefault();
                

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found");
                    return RedirectToAction("IndexVoucherUp");
                }
                else
                {

                    model.Id = result.Id;
                    model.ExaminerName = result.ExaminerName;
                    model.ExaminerPosition = result.ExaminerPosition;
                    model.InvestigatorName = result.InvestigatorName;
                    model.InvestigatorPosition = result.InvestigatorPosition;
                    model.MakerName = result.MakerName;
                    model.MakerPosition = result.MakerPosition;
                    model.FirstAdministrationName = result.FirstAdministrationName;
                    model.SecondAdministrationName = result.SecondAdministrationName;
                    model.BranchCode = branch.Code;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error where executing VoucherSetupController.Delete[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteVoucherUp(VoucherSetupViewModel model)
        {
            try
            {
                var result = (from b in entities.VoucherSetups
                              where b.Id == model.Id
                              select b).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                }
                else
                {
                    entities.VoucherSetups.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory();
                }
                return RedirectToAction("IndexVoucherUp");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while execute VoucherSetupController.Delete[Post].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        //---------SETUP VOUCHER < 300-----------//

        public ActionResult IndexVoucherDown()
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
        public ActionResult IndexVoucherDown(string searchValue)
        {
            TempData["SearchValue"] = searchValue;
            return RedirectToAction("IndexVoucherDown");
        }

        [GridAction]
        public ActionResult GetListVoucherDown(string searchValue)
        {
            var branch = GetCurrentUserBranchId();
            var model = from c in entities.VoucherSetups
                        where c.Status == "2"
                        select new VoucherSetupViewModel
                        {
                            Id = c.Id,
                            MakerName = c.MakerName,
                            MakerPosition = c.MakerPosition,
                            ExaminerName = c.ExaminerName,
                            ExaminerPosition = c.ExaminerPosition,
                            SecondExaminerName = c.SecondExaminerName,
                            SecondExaminerPosition = c.SecondExaminerPosition,
                            ApproverName = c.ApproverName,
                            ApproverPostion = c.ApproverPosition,
                            BranchId = c.BranchId,
                            BranchCode = c.Branch.Code,
                            Status = c.Status
                        };
            if (branch.Code != "00")
            {
                model = model.Where(a => a.BranchId == branch.Id);
            }
            
            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.MakerName.Contains(searchValue)
                    || a.MakerPosition.Contains(searchValue)
                    || a.ExaminerName.Contains(searchValue)
                    || a.ExaminerPosition.Contains(searchValue)
                    || a.SecondExaminerName.Contains(searchValue)
                    || a.SecondExaminerPosition.Contains(searchValue)
                    || a.ApproverName.Contains(searchValue)
                    || a.ApproverPostion.Contains(searchValue)
                    || a.BranchCode.Contains(searchValue));
            }
            return View(new GridModel<VoucherSetupViewModel>(model));
        }

        [HttpGet]
        public ActionResult CreateVoucherDown()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateVoucherDown(VoucherSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var branch = GetCurrentUserBranchId();
                    var getDirector = entities.VoucherSetups.Where(a => a.Status == "2" && a.BranchId == branch.Id).FirstOrDefault();

                    if (getDirector == null)
                    {
                        var newSetupDown = new VoucherSetup();
                        newSetupDown.Id = Guid.NewGuid();
                        newSetupDown.MakerName = model.MakerName;
                        newSetupDown.MakerPosition = model.MakerPosition;
                        newSetupDown.ExaminerName = model.ExaminerName;
                        newSetupDown.ExaminerPosition = model.ExaminerPosition;
                        newSetupDown.SecondExaminerName = model.SecondExaminerName;
                        newSetupDown.SecondExaminerPosition = model.SecondExaminerPosition;
                        newSetupDown.ApproverName = model.ApproverName;
                        newSetupDown.ApproverPosition = model.ApproverPostion;
                        newSetupDown.BranchId = branch.Id;
                        newSetupDown.Status = "2";

                        entities.VoucherSetups.AddObject(newSetupDown);
                        entities.SaveChanges();
                        saveHistory();
                        return RedirectToAction("IndexVoucherDown");
                    }
                    else {
                        SetErrorMessageViewData("Data tidak dapat ditambah, setting pejabat untuk cabang ini sudah ada.");
                    }
                    
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing VoucherSetupController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult EditVoucherDown(Guid id)
        {
            var model = new VoucherSetupViewModel();

            try
            {
                var getResult = (from c in entities.VoucherSetups
                                 where c.Id == id
                                 select c).FirstOrDefault();

                if (getResult == null)
                {
                    SetErrorMessageViewData("Data cannot be found.");
                    return RedirectToAction("IndexVoucherDown");
                }
                else
                {
                    model.MakerName = getResult.MakerName;
                    model.MakerPosition = getResult.MakerPosition;
                    model.ExaminerName = getResult.ExaminerName;
                    model.ExaminerPosition = getResult.ExaminerPosition;
                    model.SecondExaminerName = getResult.SecondExaminerName;
                    model.SecondExaminerPosition = getResult.SecondExaminerPosition;
                    model.ApproverName = getResult.ApproverName;
                    model.ApproverPostion = getResult.ApproverPosition;
                    model.Status = getResult.Status;
                    model.BranchId = getResult.BranchId;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing VoucherSetupController.Edit[Get].", ex);
                SetErrorMessageViewData("Data tidak dapat ditampilkan.");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditVoucherDown(VoucherSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var setupUp = (from b in entities.VoucherSetups
                                   where b.Id == model.Id
                                   select b).FirstOrDefault();

                    setupUp.ExaminerName = model.ExaminerName;
                    setupUp.ExaminerPosition = model.ExaminerPosition;
                    setupUp.SecondExaminerName = model.SecondExaminerName;
                    setupUp.SecondExaminerPosition = model.SecondExaminerPosition;
                    setupUp.MakerName = model.MakerName;
                    setupUp.MakerPosition = model.MakerPosition;
                    setupUp.ApproverName = model.ApproverName;
                    setupUp.ApproverPosition = model.ApproverPostion;

                    entities.SaveChanges();
                    saveHistory();
                    return RedirectToAction("IndexVoucherDown");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing VoucherSetupController.Edit[Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited");
                }

            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteVoucherDown(Guid id)
        {
            var model = new VoucherSetupViewModel();

            try
            {
                var result = (from b in entities.VoucherSetups
                              where b.Id == id
                              select b).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data tidak ditemukan.");
                    return RedirectToAction("IndexVoucherDown");
                }
                else
                {

                    model.Id = result.Id;
                    model.ExaminerName = result.ExaminerName;
                    model.ExaminerPosition = result.ExaminerPosition;
                    model.SecondExaminerName = result.SecondExaminerName;
                    model.SecondExaminerPosition = result.SecondExaminerPosition;
                    model.ApproverName = result.ApproverName;
                    model.ApproverPostion = result.ApproverPosition;
                    model.MakerName = result.MakerName;
                    model.MakerPosition = result.MakerPosition;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error where executing VoucherSetupController.Delete[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteVoucherDown(VoucherSetupViewModel model)
        {
            try
            {
                var result = (from b in entities.VoucherSetups
                              where b.Id == model.Id
                              select b).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data tidak dapat ditemukan.");
                }
                else
                {
                    entities.VoucherSetups.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory();
                }
                return RedirectToAction("IndexVoucherDown");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while execute VoucherSetupController.Delete[Post].", ex);
                SetErrorMessageViewData("Data tidak dapat ditampilkan.");
            }
            return View(model);
        }
    }
}
