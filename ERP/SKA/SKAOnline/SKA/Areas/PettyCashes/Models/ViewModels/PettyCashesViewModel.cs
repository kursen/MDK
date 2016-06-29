using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SKA.Models;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.PettyCashes.Models.ViewModels
{
    public class PettyCashesViewModel
    {
        public Guid Id { get; set; }
        public string Number { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage="Isi Tanggal Transaksi")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage="Kolom Dibayar Kepada tidak boleh dikosongkan")]
        public string PaidTo { get; set; }

        [Required(ErrorMessage = "Kolom Untuk Kepentingan tidak boleh dikosongkan")]
        public string Necessity { get; set; }

        [Required(ErrorMessage = "Kolom Diterima Oleh tidak boleh dikosongkan")]
        public string ReceiverName { get; set; }

        public int BranchId { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? Amount { get; set; }
        public string AccountCodePettyCash { get; set; }
    }
}