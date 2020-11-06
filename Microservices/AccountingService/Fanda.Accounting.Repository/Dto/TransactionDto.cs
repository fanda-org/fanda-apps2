using System;

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