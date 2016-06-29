using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class CashPaymentJournalViewModel
    {
        public Guid Id { get; set; }

        public int VoucherId { get; set; }

        [Required (ErrorMessage="Tanggal Cek harus diisi")]
        [DataType (DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? PaymentDate { get; set; }


        public string Description { get; set; }

        //[Required(ErrorMessage="Nomor Voucher harus diisi")]
        public string VoucherNumber { get; set; }

        [Required(ErrorMessage = "Tanggal Pembayaran harus diisi")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? JBKDate { get; set; }
        public DateTime JBKDateVoucher { get; set; }

        [Required(ErrorMessage = "Nomor Cek harus diisi")]
        public string CheckNumber { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal AmountDebet { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal AmountKredit { get; set; }

        public DateTime ClosingDate { get; set; }
    }
}