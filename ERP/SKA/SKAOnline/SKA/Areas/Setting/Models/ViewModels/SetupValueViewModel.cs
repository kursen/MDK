using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.Setting.Models.ViewModels
{
    public class SetupValueViewModel
    {
        public int Id { get; set;}

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal Value { get; set; }

    }
}