using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKA.Areas.PettyCashes.Models.ViewModels
{
    public class PettyCashPostingViewModel
    {
        public int Id { get; set; }
        public DateTime PostingDate { get; set; }
        public int BranchId { get; set; }
    }
}