using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class EndofPeriodViewModel
    {
        //Closing Book
        public DateTime ClosingDate { get; set; }
        public string EvidenceNumber { get; set; }
        public string Information { get; set; }
        
        //BalanceMoving
        public DateTime NewPeriodDate { get; set; }
    }
}