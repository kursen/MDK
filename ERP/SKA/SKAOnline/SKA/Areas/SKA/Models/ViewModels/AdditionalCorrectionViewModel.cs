using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class AdditionalCorrectionViewModel
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; } 
    }
}