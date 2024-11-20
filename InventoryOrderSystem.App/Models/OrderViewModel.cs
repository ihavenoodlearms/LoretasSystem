using System;

namespace InventoryOrderSystem.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string transaction_id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public int OrderItemsCount { get; set; }
        public string Status { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? ChangeAmount { get; set; }
    }
}