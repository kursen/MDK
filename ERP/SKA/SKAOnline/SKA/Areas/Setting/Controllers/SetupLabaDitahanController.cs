using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.VoucherKas.Models.ViewModel;
using Telerik.Web.Mvc.UI;
using SKA.Areas.VoucherKas.Models.Repositories;
using System.Threading;
using SKA.Areas.Setting.Models.ViewModels;
using System.Globalization;
using SKA.Areas.SKA.Models.ViewModels;

namespace SKA.Areas.Setting.Controllers
{
    public class SetupLabaDitahanController : BaseController
    {
        private SKAEntities entities = new SKAEntities();
        private string LabaDitahanSessionName = "LabaDitahanSession";


        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult _SelectLabaDitahan()
        {
            var model = (List<SetupLabaDitahanViewModel>)Session[LabaDitahanSessionName];
            model = (from data in entities.SetupLabaDitahans
                        join s in entities.Branches on data.BranchCode equals s.Code
                        where data.AccountCode.Equals("70.07.00")
                        select new SetupLabaDitahanViewModel
                        {
                            Id = (int)data.Id,
                            AccountCode = data.AccountCode + "." + s.Code,
                            AccountName = "LABA DITAHAN/( AKUMULASI KERUGIAN ) " + s.Name,
                            BranchCode = s.Code,
                            Debet = (double)data.Debet,
                            Credit = (double)data.Credit,
                            Years = data.Years
                             
                        }).ToList();

            return View(new GridModel<SetupLabaDitahanViewModel>(model.OrderBy(p => p.BranchCode)));
        }

        [GridAction()]
        public ActionResult _UpdateLabaDitahan(int? id)
        {
            var model = (List<SetupLabaDitahanViewModel>)Session[LabaDitahanSessionName];
            SetupLabaDitahanViewModel postmodel = new SetupLabaDitahanViewModel();


            string YearsString = Request.Form["Years"];
            string accountCode = "70.07.00";
            string accountName = "LABA DITAHAN/(AKUMULASI KERUGIAN)";
            string branchCode = Request.Form["BranchCode"];
            string debetString = Request.Form["Debet"];
            string creditString = Request.Form["Credit"];
            double debet = 0;
            double credit = 0;
            DateTime years;
            DateTime.TryParse(YearsString, new CultureInfo("id-ID"), DateTimeStyles.None, out years);
            var errorItem = ModelState.FirstOrDefault(m => m.Key == "Years");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (years == null)
                ModelState.AddModelError("Years", "Tahun harus diisi!!");

            postmodel.Id = (int)(id);
            postmodel.Years = years;
            postmodel.AccountCode = accountCode;
            postmodel.AccountName = accountName;
            postmodel.BranchCode = branchCode;
            if (debetString != "") { debet = Convert.ToDouble(debetString); }
            if (creditString != "") { credit = Convert.ToDouble(creditString); }
            postmodel.Debet = debet;
            postmodel.Credit = credit; 
            
            //postmodel.ValueMoney = moneyString;

            if (TryUpdateModel(postmodel))
            {

                var item = entities.SetupLabaDitahans.Where(m => m.Id == postmodel.Id).FirstOrDefault();

                if (item != null)
                {
                    item.Debet = Convert.ToDecimal(postmodel.Debet);
                    item.Credit = Convert.ToDecimal(postmodel.Credit);
                    item.Years = postmodel.Years;
                    entities.SaveChanges();
                    
                }

            }

            if (model == null)
            {
                model = (from data in entities.SetupLabaDitahans
                         join s in entities.Branches on data.BranchCode equals s.Code
                         where data.AccountCode.Equals("70.07.00")
                         select new SetupLabaDitahanViewModel
                         {
                             Id = (int)data.Id,
                             AccountCode = data.AccountCode + "." + s.Code,
                             AccountName = "LABA DITAHAN/( AKUMULASI KERUGIAN ) " + s.Name,
                             BranchCode = s.Code,
                             Debet = (double)data.Debet,
                             Credit = (double)data.Credit,
                             Years = data.Years

                         }).ToList();
            }

               return View(new GridModel<SetupLabaDitahanViewModel>(model.OrderBy(p=>p.BranchCode)));
           
         }


        public ActionResult Create()
        {
            
            var model = (from c in entities.Branches 
                        select c);

            var branchCode = new SelectList(model.ToList(), "Code", "Name");
            ViewBag.Branch = branchCode;

            return View();
        }

        [HttpPost]
        public ActionResult Create(SetupLabaDitahanViewModel model)
        {
            
            var b = (from c in entities.Branches
                         select c);

            var branchCode = new SelectList(b.ToList(), "Code", "Name");
            ViewBag.Branch = branchCode;

            string YearsString = Request.Form["Years"];
            DateTime years;
            DateTime.TryParse(YearsString, new CultureInfo("id-ID"), DateTimeStyles.None, out years);
            model.Years = years;

            var errorItem = ModelState.FirstOrDefault(m => m.Key == "Years");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (model.Years == null)
                ModelState.AddModelError("Years", "Tahun harus diisi!!");

            var checkYear = (from c in entities.SetupLabaDitahans 
                             where c.Years.Year.Equals(model.Years.Year) && c.BranchCode == model.BranchCode
                             select c).FirstOrDefault();
            if (checkYear != null)
            {
                ModelState.AddModelError("Years", "Setup laba ditahan Tahun " + checkYear.Years.Year + " Sudah ada");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    decimal debet = 0;
                    decimal credit = 0;
                    if (model.Debet != null) { debet = Convert.ToDecimal(model.Debet); }
                    if (model.Credit != null) { credit = Convert.ToDecimal(model.Credit); }

                    var setuplabaditahan = new SetupLabaDitahan();
                    setuplabaditahan.BranchCode = model.BranchCode;
                    setuplabaditahan.AccountCode = "70.07.00";
                    setuplabaditahan.Debet = debet;
                    setuplabaditahan.Credit = credit;
                    setuplabaditahan.Years = model.Years;

                    entities.SetupLabaDitahans.AddObject(setuplabaditahan);
                    entities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("erro", ex);
                    SetErrorMessageViewData("Data gagal dibuat");
                }

            }

            return View(model);
        }

        [GridAction]
        public ActionResult _DeleteLabaDitahan(int id)
        {
            var model = (List<SetupLabaDitahanViewModel>)Session[LabaDitahanSessionName];
            try
            {
                var result = (from s in entities.SetupLabaDitahans where s.Id.Equals(id) select s).FirstOrDefault();
                entities.SetupLabaDitahans.DeleteObject(result);
                entities.SaveChanges();
                

            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing SetupLabaDitahanController.Delete [Post].", ex);
      
            }

            if (model == null)
            {
                model = (from data in entities.SetupLabaDitahans
                         join s in entities.Branches on data.BranchCode equals s.Code
                         where data.AccountCode.Equals("70.07.00")
                         select new SetupLabaDitahanViewModel
                         {
                             Id = (int)data.Id,
                             AccountCode = data.AccountCode + "." + s.Code,
                             AccountName = "LABA DITAHAN/( AKUMULASI KERUGIAN ) " + s.Name,
                             BranchCode = s.Code,
                             Debet = (double)data.Debet,
                             Credit = (double)data.Credit,
                             Years = data.Years

                         }).ToList();
            }

            return View(new GridModel<SetupLabaDitahanViewModel>(model.OrderBy(p => p.Id)));
        }

    }
}
