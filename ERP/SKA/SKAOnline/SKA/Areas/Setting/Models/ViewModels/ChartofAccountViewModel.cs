using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.Setting.Models.ViewModels
{
    public class ChartofAccountViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Kode Perkiraan harus diisi")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Nama Perkiraan harus diisi")]
        public string Name { get; set; }

        public int Count { get; set; }
        //public decimal? DebitBeginningBalance { get; set; }
        //public decimal? CreditBeginningBalance { get; set; }
    }
}