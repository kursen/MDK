using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class BudgetFinancialPositionDetailViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        public int BudgetToFinancialId { get; set; }
        
        public int? AccountId1 { get; set; }
        public int? AccountId2 { get; set; }

        [Required]
        [DataType("AccountCode1")]
        public string AccountCode1 { get; set; }
        [Required]
        [DataType("AccountCode2")]
        public string AccountCode2 { get; set; }
        
        [Required]
        public string Description { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Budget { get; set; }

        public int? BranchId1 { get; set; }
        public int? BranchId2 { get; set; }
    }
}