using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKA.Areas.VoucherKas.Models.ViewModel
{
    public class BankViewModel
    {
        public Guid Id { get; set; }

        public string BankName { get; set; }

        public string ACNumber { get; set; }
    }
}