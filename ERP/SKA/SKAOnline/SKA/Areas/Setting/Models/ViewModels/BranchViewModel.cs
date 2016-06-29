using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.Setting.Models.ViewModels
{
    public class BranchViewModel
    {
        [Required(ErrorMessage = "Error")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kolom Nama Cabang tidak boleh kosong")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kolom Inisial Cabang tidak boleh kosong")]
        public string ShortName { get; set; }

        [Required(ErrorMessage = "Kolom Kode Cabang tidak boleh kosong")]
        public string Code { get; set; }
        public string Address { get; set; }
        public string ApproverName { get; set; }
        public string ApproverPosition { get; set; }
        public string MakerName { get; set; }
        public string MakerPosition { get; set; }
        public string FirstExaminerName { get; set; }
        public string FirstExaminerPosition { get; set; }
        public string SecondExaminerName { get; set; }
        public string SecondExaminerPosition { get; set; }

        public string Status { get; set; }

        public int? StatusId { get; set; }
    }
}