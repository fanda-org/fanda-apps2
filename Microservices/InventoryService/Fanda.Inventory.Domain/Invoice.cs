using System;
using System.Collections.Generic;
using Fanda.Core;
using Fanda.Inventory.Domain.Base;

namespace Fanda.Inventory.Domain
{
    public class Invoice : YearInvtEntity
    {
        //public Guid Id { get; set; }
        //public string InvoiceNumber { get; set; }
        //public DateTime InvoiceDate { get; set; }
        public Guid CategoryId { get; set; }

        public InvoiceType InvoiceType { get; set; }

        public string InvoiceTypeString
        {
            get => InvoiceType.ToString();
            set => InvoiceType = (InvoiceType)Enum.Parse(typeof(InvoiceType), value, true);
        }

        public StockInvoiceType StockInvoiceType { get; set; }

        public string StockInvoiceTypeString
        {
            get => StockInvoiceType.ToString();
            set => StockInvoiceType = (StockInvoiceType)Enum.Parse(typeof(StockInvoiceType), value, true);
        }

        public GstTreatment GstTreatment { get; set; }

        public string GstTreatmentString
        {
            get => GstTreatment.ToString();
            set => GstTreatment = (GstTreatment)Enum.Parse(typeof(GstTreatment), value, true);
        }

        public InvoiceTaxPreference TaxPreference { get; set; }

        public string TaxPreferenceString
        {
            get => TaxPreference.ToString();
            set => TaxPreference = (InvoiceTaxPreference)Enum.Parse(typeof(InvoiceTaxPreference), value, true);
        }

        public string Notes { get; set; }
        public Guid PartyId { get; set; }
        public string PartyRefNum { get; set; }
        public DateTime? PartyRefDate { get; set; }
        public Guid? BuyerId { get; set; }

        // Trailer
        public decimal Subtotal { get; set; }

        public decimal DiscountPct { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal TaxPct { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal MiscAddDesc { get; set; }
        public decimal MiscAddAmt { get; set; }

        public decimal GrandTotal { get; set; }
        //public Guid YearId { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime? DateModified { get; set; }

        // Virtual members
        public virtual InvoiceCategory Category { get; set; }

        //public virtual Party Party { get; set; }
        public virtual Buyer Buyer { get; set; }

        //public virtual AccountYear AccountYear { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}