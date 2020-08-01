using System;

namespace Fanda.Core.Models
{
    public class LedgerBalanceDto
    {
        public Guid LedgerId { get; set; }
        public Guid YearId { get; set; }
        public decimal OpeningBalance { get; set; }
        public string BalanceSign { get; set; }
    }
}
