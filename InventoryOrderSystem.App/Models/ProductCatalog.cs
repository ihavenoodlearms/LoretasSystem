using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryOrderSystem
{
    public static class ProductCatalog
    {
        public static Dictionary<string, Product> Products { get; private set; }

        static ProductCatalog()
        {
            InitializeProducts();
        }

        private static void InitializeProducts()
        {
            Products = new Dictionary<string, Product>
            {
                {"Matcha Cheesecake", new Product("Matcha Cheesecake", 118.00m, 1)},
                {"Oreo Cheesecake", new Product("Oreo Cheesecake", 118.00m, 2)},
                {"Red Velvet Cheesecake", new Product("Red Velvet Cheesecake", 118.00m, 3)},
                // Add all other products here...
            };
        }

        public static Product GetProduct(string name)
        {
            if (Products.TryGetValue(name, out Product product))
            {
                return product;
            }
            return null;
        }

        public static int GetProductId(string name)
        {
            if (Products.TryGetValue(name, out Product product))
            {
                return product.ItemId;
            }
            return -1;
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ItemId { get; set; }

        public Product(string name, decimal price, int itemId)
        {
            Name = name;
            Price = price;
            ItemId = itemId;
        }
    }
}