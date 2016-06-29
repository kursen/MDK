using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class ProfitsandLossBudgetViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FiscalYear { get; set; }
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Amount { get; set; }
        public string AccountCode { get; set; }
        public decimal? January { get; set; }
        public decimal? February { get; set; }
        public decimal? March { get; set; }
        public decimal? April { get; set; }
        public decimal? May { get; set; }
        public decimal? June { get; set; }
        public decimal? July { get; set; }
        public decimal? August { get; set; }
        public decimal? September { get; set; }
        public decimal? October { get; set; }
        public decimal? November { get; set; }
        public decimal? December { get; set; }
    }
}