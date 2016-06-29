using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.VoucherKas.Models.ViewModel
{
    public class VoucherPaymentViewModel
    {
        public Guid Id { get; set; }

        public int VoucherId { get; set; }
        public string VoucherNumber { get; set; }

        [Required (ErrorMessage ="Nomor cek tidak boleh kosong")]
        public string CheckNumber { get; set; }

        [Required(ErrorMessage = "Tanggal pembayaran tidak boleh kosong")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage = "Nama Bank tidak boleh kosong")]
        public Guid BankId { get; set; }
        public string Bank { get; set; }

        [Required(ErrorMessage = "Tanggal JBK")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        //[DataType(DataType.Date)]
        public DateTime JBKDate { get; set; }

        //public string Number { get; set; }

        [DataType (DataType.Date )]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? TransactionDate { get; set; }

        public Guid? PartnerId { get; set; }
        public string PartnerName { get; set; }

        public string Description { get; set; }

        [DataType (DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ApproveDate { get; set; }

        public int? Status { get; set; }

        public int BranchId { get; set; }

        public int? VoucherStatusId { get; set; }
        public string VoucherStatusDescription { get; set; }

        public string VoucherStatusName { get; set; }

        public int AccountId { get; set; }
        public string AccountCode { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? AmountPaid { get; set; }
    }
}