using InventoryOrderingSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryOrderSystem.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; } // Added Status property
        public List<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Added Price property
        public string Size { get; set; }
        public bool ExtraShot { get; set; }
        public List<string> AddOns { get; set; }
        public Product Product { get; set; }

        // You may want to keep this constructor for creating menu items
        public OrderItem(Product product, string size, bool extraShot, int quantity, List<string> addOns)
        {
            Product = product;
            Size = size;
            ExtraShot = extraShot;
            Quantity = quantity;
            AddOns = addOns;
            CalculatePrice(); // Set the initial price
        }

        // Parameterless constructor
        public OrderItem() { }

        public decimal CalculatePrice()
        {
            if (Product == null) return 0;

            decimal basePrice = Product.Price * Quantity;

            if (Size == "16oz")
                basePrice += 20 * Quantity;

            if (ExtraShot)
                basePrice += 20 * Quantity;

            basePrice += AddOns?.Count * 15 * Quantity ?? 0; // Assuming each add-on costs ₱15

            Price = basePrice; // Update the Price property
            return Price;
        }

        public override string ToString()
        {
            string description = $"{Product?.Name} ({Size})";
            if (ExtraShot)
                description += " +Shot";
            if (AddOns?.Any() == true)
                description += $" +{string.Join(", ", AddOns)}";
            return $"{description} x{Quantity}";
        }
    }
}