using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SKA.Models;
using System.Web.Mvc;

namespace SKA.Areas.PettyCashes.Models.ViewModels
{
    public class PettyCashesDetailViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public Guid PettyCashId { get; set; }

        public int AccountId { get; set; }

        [Required(ErrorMessage = "Kode Perkiraan Harus diisi")]
        [DataType("AccountCode")]
        public string AccountCode { get; set; }
        //[DataType("AccountName")]
        public string AccountName { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Amount { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Debet { get; set; }

        public int BranchId { get; set; }
        public int MasterBranch { get; set; }

    }
}
