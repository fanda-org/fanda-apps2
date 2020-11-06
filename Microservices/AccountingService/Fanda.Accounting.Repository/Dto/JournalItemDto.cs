using System;

namespace Fanda.Accounting.Repository.Dto
{
    public class JournalItemDto
    {
        public Guid JournalItemId { get; set; }
        public Guid LedgerId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime ReferenceDate { get; set; }
    }
}