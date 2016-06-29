using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class DHHDJournalFormatsViewModel
    {
        public int Id { get; set; }

        [DataType("AccountCodeJournalFormat")]
        public string AccountCode { get; set; }

        public string Status { get; set; }
        public byte TurnNumber { get; set; }

        public string Name { get; set; }
    }
}