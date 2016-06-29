using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.Setting.Models.ViewModels
{
    public class SetupLabaDitahanViewModel
    {
     
        public int Id { get; set; }
        public string BranchCode { get; set;}

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Required(ErrorMessage = "Tahun harus diisi!")]
        public DateTime Years { get; set;}
        public String AccountCode { get; set;}
        public String AccountName { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public Double? Debet{ get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public Double? Credit{ get; set; }

    }
}