using System;
using System.Collections.Generic;

namespace Fanda.Inventory.Domain
{
    public class Stock
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string PartyBatchNumber { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? MfgDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Guid UnitId { get; set; }
        public decimal QtyOnHand { get; set; }

        public virtual Product Product { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}