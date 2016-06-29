using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class PBIKJournalViewModel
    {
        public Guid Id { get; set; }

        [Required (ErrorMessage ="Nomor Bukti tidak boleh kosong")]
        public string EvidenceNumber { get; set; }

        [Required(ErrorMessage="Tanggal tidak boleh kosong")]
        [DataType (DataType.DateTime )]
        public DateTime DocumentDate { get; set; }

        public string Description { get; set; }

        //property untuk mengecek total kredit dan total kredit adalah sama
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal AmountDebet { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal AmountKredit { get; set; }

        public DateTime ClosingDate { get; set; }

        public int? BranchId { get; set; } 
    }
}