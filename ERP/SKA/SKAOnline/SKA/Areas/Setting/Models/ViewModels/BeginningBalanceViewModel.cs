using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.Setting.Models.ViewModels
{
    public class BeginningBalanceViewModel
    {
        public int Id { get; set; }

        public int BeginningBalanceBranchId { get; set; }

        [Required(ErrorMessage = "Kode Perkiraan tidak boleh kosong")]
        public int AccountId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Kode cabang tidak boleh diisi")]
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Debet { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Kredit { get; set; }

        public DateTime? TransactionDate { get; set; }
    }
}