using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class NetIncomesViewModel
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "Rp. {0:N}")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Amount { get; set; }
        public int Year { get; set; }
    }
}