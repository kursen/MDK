using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SKA.Areas.PettyCashes.Models.ViewModels;
using SKA.Models;
using System.Web.Mvc;

namespace SKA.Areas.PettyCashes.Models.Repositories
{
    public class PettyCashesRepository
    {
        private SKAEntities entities;

        public PettyCashesRepository()
        {
            entities = new SKAEntities();
        }

        public List<PettyCashesDetailViewModel> List(int Id)
        {
            var model = (
                from data in entities.PettyCashDetails
                where data.PettyCashId.Equals(Id)
                select new PettyCashesDetailViewModel
                {
                    PettyCashId = data.PettyCashId,
                    AccountId = data.AccountId,
                    AccountCode = data.Account.Code,
                    AccountName = data.Account.Name,
                    Debet = data.Debet
                }).ToList();

            return (model);
        }

        public void Save(PettyCashesDetailViewModel model)
        {
            PettyCashDetail insertmodel;
            try
            {
                if (model.PettyCashId != null)
                {
                    insertmodel = new PettyCashDetail
                    {
                        PettyCashId = model.PettyCashId,
                        AccountId = model.AccountId,
                        Debet = model.Debet
                    };

                    entities.PettyCashDetails.AddObject(insertmodel);
                    entities.SaveChanges();
                }
                else
                {
                    var updatemodel = (
                        from data in entities.PettyCashDetails
                        where data.PettyCashId.Equals(model.PettyCashId) && data.AccountId.Equals(model.AccountId)
                        select data).FirstOrDefault();

                    if (updatemodel != null)
                    {
                        updatemodel.PettyCashId = model.PettyCashId;
                        updatemodel.AccountId = model.AccountId;
                        updatemodel.Debet = model.Debet;
                        entities.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Delete(int id)
        {
            try
            {
                var deletemodel = (
                    from data in entities.PettyCashDetails
                    where data.PettyCashId.Equals(id)
                    select data).FirstOrDefault();

                if (deletemodel != null)
                {
                    entities.PettyCashDetails.DeleteObject(deletemodel);
                    entities.SaveChanges();

                }

            }
            catch (Exception ex)
            {

            }
        }

        public SelectList GetChartOfAccount()
        {
            var account = (from c in entities.Accounts
                           select c).ToList();
            var accountSelected = new SelectList(account, "Id", "Code");
            return accountSelected;
        }
    }

}