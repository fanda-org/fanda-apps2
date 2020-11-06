using System;

namespace Fanda.Inventory.Repository.Dto
{
    public class StockDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? MfgDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Guid UnitId { get; set; }
        public decimal QtyOnHand { get; set; }
    }
}