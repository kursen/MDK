using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class AccountJournalViewModel
    {
        public Guid Id { get; set; }

        [Required (ErrorMessage="Nomor DRD tidak boleh kosong")]
        public string DRDNumber { get; set; }

        [Required(ErrorMessage = "Tanggal tidak boleh kosong")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DocumentDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Description { get; set; }

        public int? WaterBill { get; set; }

        public int? NonWaterBill { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal AmountDebet { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal AmountKredit { get; set; }

        public AccountJournalViewModel()
        {
            Id = Guid.NewGuid();
        }
    }
}