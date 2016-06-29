using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SKA.Areas.SKA.Models.ViewModels
{
    public class VelocityMoneyBudgetDetailViewModel
    {
        public int Id { get; set; }
        public int VelocityMoneyBudgetId { get; set; }
        public int TemplateId { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? OperationalAcceptance { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? NonOperationalAcceptance { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? AcceptanceAmount { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? OperationalExpenses { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? NonOperationalExpenses { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? ExpensesAmount { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? CashIncrementDecrement { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? FirstBalance { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public decimal? LasBalance { get; set; }
    }
}