using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using SKA.Models;
using System.Web.Mvc;

namespace SKA.Areas.VoucherKas.Models.ViewModel
{
    public class VoucherViewModel
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        [Required(ErrorMessage = "Tanggal transaksi masih kosong")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        //[DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        //[Required(ErrorMessage="Rekanan masih kosong")]
        public Guid? PartnerId { get; set; }
        //[DataType ("PartnerAutoComplete")]
        [Required(ErrorMessage="Rekanan harus diisi")]
        public string PartnerName { get; set; }

        [Required(ErrorMessage = "Keterangan harus diisi")]
        public string Description { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType("DatePicker")]
        public DateTime? ApproveDate { get; set; }

        [HiddenInput]
        public int? Status { get; set; }
        public int? VoucherStatusId { get; set; }
        public string VoucherStatusName { get; set; }

        [Required(ErrorMessage = "Error")]
        public int BranchId { get; set; }

        [Required(ErrorMessage= "Lampiran harus diisi")]
        public string Attachment { get; set; }

        public int AccountId { get; set; }
        public string AccountCode { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? PaymentDate { get; set; }

        public string CheckNumber { get; set; }

        public string Bank { get; set; }
        public int BankId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime JBKDate { get; set; }

        public string code { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Debet { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Kredit { get; set; }

        public decimal AmountPaid { get; set; }
        public List<VoucherDetailViewModel> Detail { get; set; }

        public VoucherViewModel()
        {
            Id = Guid.NewGuid();
        }
    }

        
}