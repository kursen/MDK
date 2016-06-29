using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class DHHDJournalDetailViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public Guid DHHDJournalId { get; set; }

        public int AccountId { get; set; }

        [Required]
        [DataType("AccountCodeJournalDHHD")]
        public string AccountCode { get; set; }
        public string AccountName { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Debet { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Kredit { get; set; }

        public int BranchId { get; set; }
        public int? MasterBranch { get; set; }
    }
}