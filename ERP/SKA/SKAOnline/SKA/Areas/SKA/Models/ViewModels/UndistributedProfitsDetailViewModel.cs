using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class UndistributedProfitsDetailViewModel
    {
        public int Id { get; set; }
        //public int UndistributedProfitId { get; set; }
        public string Description { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Amount { get; set; }

        public string Status { get; set; }

        [DataType("DatePicker")]
        public DateTime? Date { get; set; }
    }
}