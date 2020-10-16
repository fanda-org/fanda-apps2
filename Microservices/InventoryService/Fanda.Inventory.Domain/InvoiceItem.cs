using System;

namespace Fanda.Inventory.Domain
{
    public class InvoiceItem
    {
        public Guid InvoiceItemId { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid StockId { get; set; }
        public string Description { get; set; }
        public Guid UnitId { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPct { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal CentralGstPct { get; set; }
        public decimal CentralGstAmt { get; set; }
        public decimal StateGstPct { get; set; }
        public decimal StateGstAmt { get; set; }
        public decimal InterGstPct { get; set; }
        public decimal InterGstAmt { get; set; }

        //public decimal TaxPct { get; set; }
        //public decimal TaxAmt { get; set; }
        public decimal LineTotal { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Stock Stock { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
