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
    public class SetupValueController : BaseController
    {
        //
        // GET: /Setting/SetupValue/

        private SKAEntities entities;

        public SetupValueController()
        {
            entities = new SKAEntities();
            //ViewBag.Value = this.getSetupValueLabel();            
        }
        [HttpGet]
        public ActionResult Index()
        {
            var model = GetSetupValue();

            return View(model);
        }

        private SystemVoucherSetup GetSetupValue()
        {
            var model = new SystemVoucherSetup();
            try
            {
                var result = (from c in entities.SystemVoucherSetups
                              select c).FirstOrDefault();

                if (result == null)
                {
                    var newValue = new SystemVoucherSetup();
                    newValue.Value = 0;

                    entities.SystemVoucherSetups.AddObject(newValue);
                    entities.SaveChanges();
                    return newValue;
                }
                else
                {
                    model.Value = result.Value;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing SetupValueController.Edit [Get]", ex);
                SetErrorMessageViewData("Data cannot be found");
            }
            return model;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = GetSetupValue();
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SetupValueViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    var updateSetup = (from at in entities.SystemVoucherSetups
                                       where at.Id == model.Id
                                       select at).FirstOrDefault();

                    if (updateSetup == null)
                        return RedirectToAction("Index");

                    updateSetup.Value = model.Value;

                    entities.SaveChanges();
                    saveHistory(model.Value.ToString());
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing SetupValueController.Edit [Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited.");
                }
            }

            return View(model);
        }
    }
}
