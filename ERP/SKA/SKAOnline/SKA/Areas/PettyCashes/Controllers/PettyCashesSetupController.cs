using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.PettyCashes.Models.ViewModels;

namespace SKA.Areas.PettyCashes.Controllers
{
    public class PettyCashesSetupController : BaseController
    {
        //
        // GET: /PettyCashes/PettyCashesSetup/
        private SKAEntities entities;

        public PettyCashesSetupController()
        {
            entities = new SKAEntities();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = GetPettyCashSetup();
            return View(model);
        }

        private PettyCashSetup GetPettyCashSetup()
        {
            var model = new PettyCashSetup();
            var branch = GetCurrentUserBranchId();
            try
            {
                var result = (from c in entities.PettyCashSetups
                              where c.BranchId == branch.Id
                              select c).FirstOrDefault();

                if (result == null)
                {
                    var newSetup = new PettyCashSetup();
                    newSetup.Id = Guid.NewGuid();
                    newSetup.ApproverName = "-";
                    newSetup.ApproverPosition = "-";
                    newSetup.BranchId = branch.Id;
                    newSetup.MakerName = "-";
                    newSetup.MakerPosition = "-";
                    newSetup.RecapitulationMaker = "-";
                    newSetup.RecapitulationMakerPosition = "-";

                    entities.PettyCashSetups.AddObject(newSetup);
                    entities.SaveChanges();
                    return newSetup;
                }
                else
                {
                    model.Id = result.Id;
                    model.ApproverName = result.ApproverName;
                    model.ApproverPosition = result.ApproverPosition;
                    model.MakerName = result.MakerName;
                    model.MakerPosition = result.MakerPosition;
                    model.RecapitulationMaker = result.RecapitulationMaker;
                    model.RecapitulationMakerPosition = result.RecapitulationMakerPosition;
                    model.BranchId = branch.Id;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing PettyCashesSetupController.Edit [Get]", ex);
                SetErrorMessageViewData("Data cannot be found");
            }
            return model;
        }

        [GridAction]
        public ActionResult GetList()
        {
            var branch = GetCurrentUserBranchId();
            var model = from c in entities.PettyCashSetups
                        where c.BranchId == branch.Id
                         select new PettyCashesSetupViewModel
                         {
                            Id = c.Id,
                            ApproverName = c.ApproverName,
                            ApproverPosition = c.ApproverPosition,
                            MakerName = c.MakerName,
                            MakerPosition = c.MakerPosition,
                            RecapitulationMaker = c.RecapitulationMaker,
                            RecapitulationMakerPosition = c.RecapitulationMakerPosition
                         };
            return View(new GridModel<PettyCashesSetupViewModel>(model));
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var model = GetPettyCashSetup();
            if (model == null)
            {
                SetErrorMessageViewData("Data cannot be found.");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PettyCashesSetupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var branch = GetCurrentUserBranchId();
                try
                {
                    var updateSetup = (from at in entities.PettyCashSetups
                                       where at.Id == model.Id
                                       select at).FirstOrDefault();

                    if (updateSetup == null)
                        return RedirectToAction("Index");

                    updateSetup.ApproverName = model.ApproverName;
                    updateSetup.ApproverPosition = model.ApproverPosition;
                    updateSetup.MakerName = model.MakerName;
                    updateSetup.MakerPosition = model.MakerPosition;
                    updateSetup.RecapitulationMaker = model.RecapitulationMaker;
                    updateSetup.RecapitulationMakerPosition = model.RecapitulationMakerPosition;
                    updateSetup.BranchId = branch.Id; //kode cabang sementara

                    entities.SaveChanges();
                    saveHistory();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing PettyCashSetupController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }

            return View(model);
        }
    }
}
