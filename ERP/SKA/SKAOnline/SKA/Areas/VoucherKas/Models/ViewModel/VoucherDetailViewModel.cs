using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SKA.Models;
using System.Web.Mvc;

namespace SKA.Areas.VoucherKas.Models.ViewModel
{
    public class VoucherDetailViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public Guid VoucherId { get; set; }

        public int AccountId { get; set; }

        [DataType("AccountCode")]
        public string AccountCode { get; set; }
        //[DataType("AccountCode")]
        public string AccountName { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Debet { get; set; }

//        [DataType(DataType.Currency )]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Kredit { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Total { get; set; }

        public int BranchId { get; set; }

        public int MasterBranch { get; set; }
    }
}