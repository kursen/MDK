using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class MonthlyReportsSetupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CheckerName { get; set; }
        public string CheckerPosition { get; set; }
        public string MakerName { get; set; }
        public string MakerPosition { get; set; }
    }
}