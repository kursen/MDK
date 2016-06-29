using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKA.Areas.Setting.Models.ViewModels
{
    public class VoucherSetupViewModel
    {
        public Guid Id { get; set; }
        public string MakerName { get; set; }
        public string MakerPosition { get; set; }
        public string ExaminerName { get; set; }
        public string ExaminerPosition { get; set; }
        //public string ExaminerName2 { get; set; }
        //public string ExaminerPosition2 { get; set; } 
        public string InvestigatorName { get; set; }
        public string InvestigatorPosition { get; set; }
        public string ApproverName { get; set; }
        public string ApproverPostion { get; set; }
        public int? BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string FirstAdministrationName { get; set; }
        public string SecondAdministrationName { get; set; }
        public string SecondExaminerName { get; set; }
        public string SecondExaminerPosition { get; set; }
        public string Status { get; set; }
    }
}