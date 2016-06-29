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
using System.Data.SqlClient;
namespace SKA.Areas.Setting.Controllers
{
    [BranchCodeFilter]
    public class PartnerController : BaseController
    {
        private SKAEntities entities;

        public PartnerController()
        {
            entities = new SKAEntities();
            //ViewBag.Value = this.getSetupValueLabel();
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
            var model = from p in entities.Partners
                        select new PartnerViewModel 
                        {
                            Id = p.Id,
                            Code = p.Code,
                            Name = p.Name,
                            Address = p.Address,
                            NPWP = p.NPWP,
                            AccountId =p.AccountId,
                            PhoneNumber = p.PhoneNumber, 
                            Remarks = p.Remarks,
                            AccountCode = p.Account.Code
                        };
            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(a => a.Name.Contains(searchValue)
                    || a.Code.Contains(searchValue)
                    || a.Address.Contains(searchValue));
            }
            return View(new GridModel<PartnerViewModel>(model));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PartnerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var getBranch = GetCurrentUserBranchId();
                try
                {
                   // if (model.NPWP == null)
                    //    model.NPWP = "-";

                    var getPartner = (from a in entities.Partners
                               where a.Code == model.Code || a.NPWP == model.NPWP
                             select a).FirstOrDefault();
                
                    //var getPartner = entities.Partners.Where(a => a.Code == model.Code && a.NPWP == model.NPWP).FirstOrDefault();
                    string code = model.AccountCode; //model.AccountCode.Substring(0, 8);
                    //var branch = model.AccountCode.Substring(9, 2);
                    var accountCode = entities.Accounts.Where(a => a.Code == code).FirstOrDefault();
                    //var branchCode = entities.Branches.Where(a => a.Code == branch).FirstOrDefault();
                    model.AccountId = accountCode.Id;

                   if (getPartner == null)
                   {
                        var newData = new Partner();
                        newData.Id = Guid.NewGuid();
                        newData.Code = model.Code;
                        newData.Name = model.Name;
                        newData.NPWP = model.NPWP;
                        newData.PhoneNumber = model.PhoneNumber;
                        newData.Remarks = model.Remarks;
                        newData.Address = model.Address;
                        newData.AccountId = model.AccountId;
                        //newData.BranchId = branchCode.Id;

                        entities.Partners.AddObject(newData);
                        entities.SaveChanges();
                        saveHistory(model.Code + "-" + model.Name);
                        return RedirectToAction("Index");
                    }
                   else
                    {
                       //    ModelState.AddModelError("Code", "Kode rekanan sudah ada.");
                       //    ModelState.AddModelError("NPWP", "NPWP sudah ada.");

                       if (model.Code != null || model.NPWP != null)
                           SetErrorMessageViewData("Kode atau NPWP rekanan sudah ada");
                       
                    }

                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing PartnerController.Create [Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var model = new PartnerViewModel();
            //var branch = GetCurrentUserBranchId();
            try
            {
                var result = (from p in entities.Partners
                              where p.Id == id
                              select p).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.Code = result.Code;
                    model.Name = result.Name;
                    model.Address = result.Address;
                    model.NPWP = result.NPWP;
                    model.PhoneNumber = result.PhoneNumber;
                    model.Remarks = result.Remarks;
                    model.AccountId = result.AccountId;
                    model.AccountCode = result.Account.Code;
                   
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing PartnerController.Edit[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PartnerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var sql = new System.Text.StringBuilder();
                    var sqlDuhd = new System.Text.StringBuilder();
                    var sqlJBK = new System.Text.StringBuilder();

                    var getPartner = (from a in entities.Partners
                                      where a.Code == model.Code || a.NPWP == model.NPWP
                                      select a).FirstOrDefault();

                    var partner = (from p in entities.Partners
                                   where p.Id == model.Id
                                   select p).FirstOrDefault();

                    var accountPartner = partner.AccountId;

                    //update voucher dengan rekanan terbaru
                    sql.Append("UPDATE VD SET VD.AccountId = @newAccId ");
                    sql.Append("FROM Voucher V ");
                    sql.Append("    INNER JOIN VoucherDetail VD ");
                    sql.Append("    ON V.Id = VD.VoucherId ");
                    sql.Append("WHERE V.PartnerId = @partnerId AND VD.AccountId = @accId");

                    entities.ExecuteStoreCommand(sql.ToString(), new SqlParameter("@newAccId", model.AccountId),
                                                                new SqlParameter("@partnerId", partner.Id),
                                                                new SqlParameter("@accId", accountPartner));
                    
                    //update duhd dengan rekanan terbaru
                    sqlDuhd.Append("UPDATE DD SET DD.AccountId = @newAccId ");
                    sqlDuhd.Append("FROM DHHDJournal D ");
                    sqlDuhd.Append("    INNER JOIN DHHDJorurnalDetail DD ");
                    sqlDuhd.Append("    ON D.Id = DD.DHHDJournalId ");
                    sqlDuhd.Append("    INNER JOIN Voucher V ");
                    sqlDuhd.Append("    ON D.Number = V.Number ");
                    sqlDuhd.Append("WHERE V.PartnerId = @partnerId AND DD.AccountId = @accId ");

                    entities.ExecuteStoreCommand(sqlDuhd.ToString(), new SqlParameter("@newAccId", model.AccountId),
                                                                new SqlParameter("@partnerId", partner.Id),
                                                                new SqlParameter("@accId", accountPartner));

                    //update jbk dengan rekanan terbaru
                    sqlJBK.Append("UPDATE CP SET CP.AccountId = @newAccId ");
                    sqlJBK.Append("FROM CashPaymentJournal C ");
                    sqlJBK.Append("    INNER JOIN CashPaymentJournalDetail CP ");
                    sqlJBK.Append("    ON C.Id = CP.CashPaymentJournalId ");
                    sqlJBK.Append("    INNER JOIN Voucher V ");
                    sqlJBK.Append("    ON C.VoucherId = V.Id ");
                    sqlJBK.Append("WHERE V.PartnerId = @partnerId AND CP.AccountId = @accId ");

                    entities.ExecuteStoreCommand(sqlJBK.ToString(), new SqlParameter("@newAccId", model.AccountId),
                                                                new SqlParameter("@partnerId", partner.Id),
                                                                new SqlParameter("@accId", accountPartner));
                    //entities.ExecuteStoreCommand(sql.ToString(), 
                    var code = model.AccountCode.Substring(0, 8);
                    //var branch = model.AccountCode.Substring(9, 2);
                    var accountCode = entities.Accounts.Where(a => a.Code == code).FirstOrDefault();
                    //var branchCode = entities.Branches.Where(a => a.Code == branch).FirstOrDefault();
                    model.AccountId = accountCode.Id;

                    if (getPartner == null)
                    {
                        partner.Id = model.Id;
                        partner.Code = model.Code;
                        partner.Name = model.Name;
                        partner.Address = model.Address;
                        partner.NPWP = model.NPWP;
                        partner.PhoneNumber = model.PhoneNumber;
                        partner.Remarks = model.Remarks;
                        partner.AccountId = model.AccountId;
                        //partner.BranchId = branchCode.Id;

                        entities.SaveChanges();
                        saveHistory(model.Code + "-" + model.Name);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (partner.Code.Equals(model.Code))
                        {
                            partner.Id = model.Id;
                            partner.Code = model.Code;
                            partner.Name = model.Name;
                            partner.Address = model.Address;
                            partner.NPWP = model.NPWP;
                            partner.PhoneNumber = model.PhoneNumber;
                            partner.Remarks = model.Remarks;
                            partner.AccountId = model.AccountId;
                            //partner.BranchId = branchCode.Id;

                            entities.SaveChanges();
                            saveHistory(model.Code + "-" + model.Name);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            if (model.Code != null || model.NPWP != null)
                                SetErrorMessageViewData("Kode atau NPWP rekanan sudah ada");
                        }
                       }

                    }

                catch (Exception ex)
                {
                    Logger.Error("Error while executing PartnerController.Edit[Post].", ex);
                    SetErrorMessageViewData("Data cannot be edited");
                }

            }
           
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            var branch = GetCurrentUserBranchId();
            var model = new PartnerViewModel();

            try
            {
                var result = (from p in entities.Partners
                              where p.Id == id
                              select p).FirstOrDefault();
                if (result == null)
                {
                    SetErrorMessageViewData("Data cannot be found");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.Id = result.Id;
                    model.Code = result.Code;
                    model.Name = result.Name;
                    model.Address = result.Address;
                    model.NPWP = result.NPWP;
                    model.PhoneNumber = result.PhoneNumber;
                    model.Remarks = result.Remarks;
                    model.AccountCode = result.Account.Code + "." + branch.Code;
                    model.AccountId = result.AccountId;
                }

            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing PartnerController.Delete[Get].", ex);
                SetErrorMessageViewData("Data cannot be displayed");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(PartnerViewModel model)
        {
            try
            {
                var result = (from p in entities.Partners
                              where p.Id == model.Id
                              select p).FirstOrDefault();

                if (result == null)
                {
                    SetErrorMessageViewData("Data tidak dapat ditemukan.");
                }
                else
                {
                    entities.Partners.DeleteObject(result);
                    entities.SaveChanges();
                    saveHistory(result.Code + "-" + result.Name);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing PartnerController.Delete[Post].", ex);
                SetErrorMessageViewData("Data tidak dapat dihapus karena digunakan dalam transaksi.");
            }

            return View(model);
        }

        [GridAction]
        public ActionResult GetAccountCodeList()
        {
            var branch = GetCurrentUserBranchId();

            string AccountCodeforPartner = System.Configuration.ConfigurationManager.AppSettings["AccountCodeforPartner"];

            var model = from m in entities.Accounts
                        where m.Code.StartsWith(AccountCodeforPartner)
                        select new ChartofAccountViewModel
                        {
                            Id = m.Id,
                            Code = m.Code,
                            Name = m.Name
                        };
            return View(new GridModel<ChartofAccountViewModel>(model));
        }
    }
}
