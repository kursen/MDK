using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.Setting.Models.ViewModels
{
    public class SetupJournalAccountDetailViewModel
    {
        public int Id { get; set; }
        public int JournalTypeId { get; set; }
        public int AccountId { get; set; }
        [DataType("AccountCode")]
        public string AccountCode { get; set; }
        [DataType("AccountSide")]
        public string AccountSide { get; set; }
    }
}