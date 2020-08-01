using Fanda.Core.Base;
using Fanda.Shared;
using System;
using System.Collections.Generic;

namespace Fanda.Core.Models
{
    public class InvoiceDto : BaseYearDto
    {
        public InvoiceDto()
        {
            InvoiceItems = new HashSet<InvoiceItemDto>();
        }
        //public Guid Id { get; set; }
        //public string InvoiceNumber { get; set; }
        //public DateTime InvoiceDate { get; set; }
        public Guid CategoryId { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public StockInvoiceType StockInvoiceType { get; set; }
        public Guid PartyId { get; set; }
        public string PartyRefNum { get; set; }
        public DateTime? PartyRefDate { get; set; }
        public Guid? BuyerId { get; set; }
        public string Notes { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountPct { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal TaxPct { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal MiscAddDesc { get; set; }
        public decimal MiscAddAmt { get; set; }
        public decimal GrandTotal { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }

        public ICollection<InvoiceItemDto> InvoiceItems { get; set; }
    }
}