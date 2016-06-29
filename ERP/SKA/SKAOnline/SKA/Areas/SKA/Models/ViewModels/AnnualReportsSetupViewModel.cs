using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class AnnualReportsSetupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApproverName { get; set; }
        public string ApproverPosition { get; set; }
        public string KnownByName { get; set; }
        public string KnownByPosition { get; set; }
    }
}