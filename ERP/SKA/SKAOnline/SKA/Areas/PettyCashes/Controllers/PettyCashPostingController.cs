using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Controllers;
using SKA.Areas.PettyCashes.Models.ViewModels;
using SKA.Models;
using System.Globalization;

namespace SKA.Areas.PettyCashes.Controllers
{
    public class PettyCashPostingController : BaseController
    {
        //
        // GET: /PettyCashes/PettyCashPosting/
        private SKAEntities entities;

        public PettyCashPostingController()
        {
            entities = new SKAEntities();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(PettyCashPostingViewModel model)
        {
            string postingDateString = Request.Form["PostingDate"];
            DateTime postingDate;
            DateTime.TryParse(postingDateString, new CultureInfo("id-ID"), DateTimeStyles.None, out postingDate);
            model.PostingDate = postingDate;
            var errorItem = ModelState.FirstOrDefault(m => m.Key == "PostingDate");

            if (errorItem.Value != null)
                ModelState.Remove(errorItem);

            if (ModelState.IsValid)
            {
                var branch = GetCurrentUserBranchId();
                int getYear = DateTime.Now.Year;
                int getMonth = DateTime.Now.Month;
                string month = string.Format("{0:00}", getMonth);
                string generalJournalNumber = string.Format("{0}/VKK/{1}{2}", branch.Code, month, getYear);

                try
                {
                    var newGeneralJournal = entities.GeneralJournals.Where(a => a.EvidenceNumber == generalJournalNumber).FirstOrDefault();

                    var getPettyCashDetail = (from c in entities.PettyCashDetails
                                              where c.PettyCashId == c.PettyCash.Id && c.PettyCash.BranchId == branch.Id 
                                              && c.PettyCash.TransactionDate.Month == model.PostingDate.Month
                                              group c by new 
                                              {
                                                AccountId = c.AccountId,
                                                BranchId = c.BranchId,
                                              } into a
                                              select new
                                              {
                                                  AccountId = a.Key.AccountId,
                                                  Debet = a.Sum(c => c.Debet),
                                                  BranchId = a.Key.BranchId
                                              }).ToList();


                    if (newGeneralJournal == null)
                    {
                        var newAddedGeneralJournal = new GeneralJournal();

                        newAddedGeneralJournal.Id = Guid.NewGuid();
                        newAddedGeneralJournal.EvidenceNumber = generalJournalNumber;
                        newAddedGeneralJournal.DocumentDate = model.PostingDate;

                        entities.GeneralJournals.AddObject(newAddedGeneralJournal);
                    }
                    else
                    {
                        newGeneralJournal.DocumentDate = model.PostingDate;
                    }
                    entities.SaveChanges();

                    var getAddedJournal = entities.GeneralJournals.Where(a => a.EvidenceNumber == generalJournalNumber).FirstOrDefault();
                    var getJournalDetail = entities.GeneralJournalDetails.Where(a => a.GeneralJournalId == getAddedJournal.Id).ToList();

                    if (getJournalDetail.Count < 1)
                    {
                        for (int i = 0; i < getPettyCashDetail.Count; i++)
                        {
                            var newGeneralJournalDetail = new GeneralJournalDetail();
                            newGeneralJournalDetail.GeneralJournalId = getAddedJournal.Id;
                            newGeneralJournalDetail.AccountId = getPettyCashDetail[i].AccountId;
                            newGeneralJournalDetail.Debit = getPettyCashDetail[i].Debet;
                            newGeneralJournalDetail.BranchId = getPettyCashDetail[i].BranchId;
                            newGeneralJournalDetail.MasterBranch = branch.Id;
                            entities.GeneralJournalDetails.AddObject(newGeneralJournalDetail);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < getJournalDetail.Count; j++)
                        {
                            entities.GeneralJournalDetails.DeleteObject(getJournalDetail[j]);
                            entities.SaveChanges();
                        }

                        for (int i = 0; i < getPettyCashDetail.Count; i++)
                        {
                            var newGeneralJournalDetail = new GeneralJournalDetail();
                            newGeneralJournalDetail.GeneralJournalId = getAddedJournal.Id;
                            newGeneralJournalDetail.AccountId = getPettyCashDetail[i].AccountId;
                            newGeneralJournalDetail.Debit = getPettyCashDetail[i].Debet;
                            newGeneralJournalDetail.BranchId = getPettyCashDetail[i].BranchId;
                            newGeneralJournalDetail.MasterBranch = branch.Id;
                            entities.GeneralJournalDetails.AddObject(newGeneralJournalDetail);
                        }
                    }
                    var getAccountId = (from c in entities.PettyCashes
                                        join a in entities.Accounts
                                        on c.AccountCode.Substring(0,8) equals a.Code
                                        select new {
                                            Code = a.Code,
                                            Id = a.Id
                                        }).FirstOrDefault();

                    var getPettyCashCode = (from c in entities.PettyCashes
                                              where c.AccountCode.Substring(0,8) == getAccountId.Code && c.BranchId == branch.Id
                                              && c.TransactionDate.Month == model.PostingDate.Month
                                              group c by new
                                              {
                                                  AccountId = getAccountId.Id,
                                                  BranchId = c.BranchId
                                              } into a
                                              select new
                                              {
                                                  AccountId = a.Key.AccountId,
                                                  Kredit = a.Sum(c => c.Credit),
                                                  BranchId = a.Key.BranchId
                                              }).FirstOrDefault();

                    var addNewDetail = new GeneralJournalDetail();
                    addNewDetail.GeneralJournalId = getAddedJournal.Id;
                    addNewDetail.AccountId = getPettyCashCode.AccountId;
                    addNewDetail.Credit = getPettyCashCode.Kredit;
                    addNewDetail.BranchId = getPettyCashCode.BranchId;
                    addNewDetail.MasterBranch = branch.Id;

                    entities.GeneralJournalDetails.AddObject(addNewDetail);
                    entities.SaveChanges();
                    saveHistory();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while executing PettyCashPostingController.Create[Post].", ex);
                    SetErrorMessageViewData("Data cannot be created.");
                }
            }
            return View(model);
        }
    }
}
