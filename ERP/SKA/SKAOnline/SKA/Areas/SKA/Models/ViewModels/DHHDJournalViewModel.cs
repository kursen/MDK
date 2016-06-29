using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class DHHDJournalViewModel
    {
        public Guid Id { get; set; }
        public int VoucherId { get; set; }

        public string VoucherNumber { get; set; }

        [Required(ErrorMessage = "Tanggal harus diisi")]
        [DataType(DataType.Date)]
        public DateTime DateVoucher { get; set; }

        public string Description { get; set; }
        
        //[Required (ErrorMessage="Tanggal Pembayaran harus diisi")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? PaymentDate { get; set; }

        public string CheckNumber { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal AmountDebet { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal AmountKredit { get; set; }
        public DateTime ClosingDate { get; set; }
    }
}