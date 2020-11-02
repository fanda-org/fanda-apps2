using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanda.Accounting.Repository.Dto
{
    public class TransactionDto
    {
        public Guid DebitLedgerId { get; set; }
        public Guid CreditLedgerId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
