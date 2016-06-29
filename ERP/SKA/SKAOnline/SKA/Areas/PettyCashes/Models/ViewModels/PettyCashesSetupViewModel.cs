using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.PettyCashes.Models.ViewModels
{
    public class PettyCashesSetupViewModel
    {
        public Guid Id { get; set; }

        [Required (ErrorMessage = "Disetujui Oleh harus diisi")]
        public string ApproverName { get; set; }

        [Required(ErrorMessage = "Jabatan Disetujui Oleh harus diisi")]
        public string ApproverPosition { get; set; }

        [Required(ErrorMessage = "Dibuat Oleh harus diisi")]
        public string MakerName { get; set; }

        [Required(ErrorMessage = "Jabatan Dibuat Oleh harus diisi")]
        public string MakerPosition { get; set; }

        [Required(ErrorMessage = "Pembuat Rekap harus diisi")]
        public string RecapitulationMaker { get; set; }

        [Required(ErrorMessage = "Jabatan Pembuat Rekap harus diisi")]
        public string RecapitulationMakerPosition { get; set; }
        public byte BranchId { get; set; }

    }
}