using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class RevenueExpenditureViewModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        
        [DataType("AccountCodeExpenditure")]
        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        public string Tipe { get; set; }
    }
}