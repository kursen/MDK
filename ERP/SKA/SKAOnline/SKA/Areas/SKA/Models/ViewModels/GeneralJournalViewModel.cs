using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class GeneralJournalViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nomor Bukti tidak boleh kosong")]
        public string EvidenceNumber { get; set; }

        [Required(ErrorMessage="Tanggal tidak boleh kosong")]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType (DataType.Date)]
        public DateTime DocumentDate { get; set; }

        public string Description { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal AmountDebet { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal AmountKredit { get; set; }

        public DateTime ClosingDate { get; set; }
        public int? BranchCode { get; set; }
        public int? Month { get; set; }
    }
}