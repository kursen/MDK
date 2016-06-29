using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SKA.Models;
using SKA.Areas.VoucherKas.Models.ViewModel;

namespace SKA.Areas.VoucherKas.Models.Repositories
{
    public class VoucherRepository
    {
        private SKAEntities entities;

        public VoucherRepository()
        {
            entities = new SKAEntities();
        }

        public List<VoucherDetailViewModel> List(int Id)
        {
            var model = (
                from data in entities.VoucherDetails
                where data.VoucherId.Equals(Id)
                orderby data.Account.Code
                select new VoucherDetailViewModel
                {
                    VoucherId = data.VoucherId,
                    AccountId = data.AccountId,
                    Debet = data.Debet,
                    Kredit = data.Kredit,
                }).ToList();

            return (model);

        }

        public void Save(VoucherDetailViewModel model)
        {
            VoucherDetail insertModel;

            try
            {
                if (model.VoucherId != null)
                {

                    insertModel = new VoucherDetail
                    {
                        //VoucherId = model.VoucherId,
                        AccountId = model.AccountId,
                        Debet = model.Debet,
                        Kredit = model.Kredit,
                    };
                    entities.VoucherDetails.AddObject(insertModel);
                    entities.SaveChanges();
                }

                else
                {
                    var updateModel = (
                        from data in entities.VoucherDetails
                        where data.VoucherId.Equals(model.VoucherId) && data.AccountId.Equals(model.AccountId)
                        select data).FirstOrDefault();

                    if (updateModel != null)
                    {
                        //updateModel.VoucherId = model.VoucherId;
                        updateModel.AccountId = model.AccountId;
                        updateModel.Debet = model.Debet;
                        updateModel.Kredit = model.Kredit;
                        entities.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Delete(int Id)
        {
            try
            {
                var deletemodel = (
                    from data in entities.VoucherDetails 
                    where data.AccountId.Equals(Id)
                    select data).FirstOrDefault();

                if (deletemodel != null)
                {
                    entities.VoucherDetails.DeleteObject(deletemodel);
                    entities.SaveChanges();

                }

            }
            catch (Exception ex)
            {
               
            }
        }
    }
}

       