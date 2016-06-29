using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class AdditionalCorrectionDetailViewModel
    {
        public int Id { get; set; }
        public int CorrectionandBackupId { get; set; }
        public int AccountId { get; set; }

        [DataType("AccountCode")]
        public string AccountCode { get; set; }
        public string AccountName { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Amount { get; set; }
        public int BranchId { get; set; }
    }
}