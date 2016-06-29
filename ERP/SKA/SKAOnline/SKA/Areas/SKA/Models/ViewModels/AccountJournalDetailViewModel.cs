using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class AccountJournalDetailViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public Guid AccountJournalId { get; set; }

        public int AccountId { get; set; }

        [Required (ErrorMessage="Perkiraan tidak boleh kosong")]
        [DataType("AccountCodeJournalAccount")]
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