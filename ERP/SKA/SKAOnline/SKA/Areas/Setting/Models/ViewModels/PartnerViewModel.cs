using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.Setting.Models.ViewModels
{
    public class PartnerViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Kode Rekanan tidak boleh kosong")]
        public string Code { get; set; }

        [Required (ErrorMessage ="Nama Rekanan tidak boleh kosong")]
        public string Name { get; set; }

        public string Address { get; set; }

        //[Required(ErrorMessage = "NPWP tidak boleh kosong")]
        public string NPWP { get; set; }

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Kode Perkiraan tidak boleh kosong")]
        public int AccountId { get; set; }
        public string AccountCode { get; set; }

        public string Remarks { get; set; }

        public PartnerViewModel()
        {
            Id = Guid.NewGuid();
        }
    }
}