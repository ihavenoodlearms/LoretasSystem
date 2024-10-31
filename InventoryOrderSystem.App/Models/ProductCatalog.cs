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
                {"Strawberry Cheesecake", new Product("Strawberry Cheesecake", 118.00m, 4)},
                {"Ube Cheesecake", new Product("Ube Cheesecake", 118.00m, 5)},
                {"Wintermelon Cheesecake", new Product("Wintermelon Cheesecake", 118.00m, 6)},

                {"Berry Blossom", new Product("Berry Blossom", 68.00m, 7)},
                {"Blue Lemonade", new Product("Blue Lemonade", 68.00m, 8)},
                {"Green Apple Lemonade", new Product("Green Apple Lemonade", 68.00m, 9)},
                {"Lychee", new Product("Lychee", 68.00m, 10)},
                {"Paradise", new Product("Paradise", 68.00m, 11)},
                {"Strawberry Lemonade", new Product("Strawberry Lemonade", 68.00m, 12)},
                {"Sunrise", new Product("Sunrise", 68.00m, 13)},

                {"Chocolate (Milk Tea)", new Product("Chocolate (Milk Tea)", 78.00m, 14)},
                {"Cookies and Cream (Milk Tea)", new Product("Cookies andd Cream (Milk Tea)", 78.00m, 15)},
                {"Dark Chocolate (Milk Tea)", new Product("Dark Chocolate (Milk Tea)", 78.00m, 16)},
                {"Hazelnut (Milk Tea)", new Product("Hazelnut (Milk Tea)", 78.00m, 17)},
                {"Matcha (Milk Tea)", new Product("Matcha (Milk Tea)", 78.00m, 18)},
                {"Mocha (Milk Tea)", new Product("Mocha (Milk Tea)", 78.00m, 19)},
                {"Okinawa (Milk Tea)", new Product("Okinawa (Milk Tea)", 78.00m, 20)},
                {"Taro (Milk Tea)", new Product("Taro (Milk Tea)", 78.00m, 21)},
                {"Ube (Milk Tea)", new Product("Ube (Milk Tea)", 78.00m, 22)},
                {"Wintermelon (Milk Tea)", new Product("Wintermelon (Milk Tea)", 78.00m, 23)},

                {"Blueberry Milk", new Product("Blueberry Milk", 98.00m, 24)},
                {"Mango Milk", new Product("Mango Milk", 98.00m, 25)},
                {"Strawberry Milk", new Product("Strawberry Milk", 98.00m, 26)},

                {"Nutellatte", new Product("Nutellatte", 118.00m, 27)},
                {"Tiger Boba Milk", new Product("Tiger Boba Milk", 108.00m, 28)},
                {"Tiger Boba Milktea", new Product("Tiger Boba Milktea", 138.00m, 29)},
                {"Tiger Oreo Cheesecake", new Product("Tiger Oreo Cheesecake", 128.00m, 30)},

                {"Americano", new Product("Americano", 68.00m, 31)},
                {"Cafe Latte (Iced Coffee)", new Product("Cafe Latte (Iced Coffee)", 78.00m, 32)},
                {"Cafe Mocha", new Product("Cafe Mocha", 78.00m, 33)},
                {"Caramel Macchiato", new Product("Caramel Macchiato", 78.00m, 34)},
                {"Cappucino (Iced Coffee)", new Product("Cappucino (Iced Coffee)", 78.00m, 35)},
                {"Dirty Matcha", new Product("Dirty Matcha", 138.00m, 36)},
                {"French Vanilla", new Product("French Vanilla", 78.00m, 37)},
                {"Matcha Latte", new Product("Matcha Latte", 98.00m, 38)},
                {"Spanish Latte", new Product("Spanish Latte", 78.00m, 39)},
                {"Triple Chocolate Mocha", new Product("Triple Chocolate Mocha", 78.00m, 40)},

                {"Black Forest (Coffee Frappe)", new Product("Black Forest (Coffee Frappe)", 98.00m, 41)},
                {"Cafe Latte (Coffee Frappe)", new Product("Cafe Latte (Coffee Frappe)", 98.00m, 42)},
                {"Cappuccino (Coffee Frappe)", new Product("Cappuccino (Coffee Frappe)", 98.00m, 43)},
                {"Caramel (Coffee Frappe)", new Product("Caramel (Coffee Frappe)", 98.00m, 44)},
                {"Choc Chip (Coffee Frappe)", new Product("Choc Chip (Coffee Frappe)", 98.00m, 45)},
                {"Cookies and Cream (Coffee Frappe)", new Product("Cookies and Cream (Coffee Frappe)", 98.00m, 46)},
                {"Dark Chocolate (Coffee Frappe)", new Product("Dark Chocolate (Coffee Frappe)", 98.00m, 47)},
                {"Double Dutch (Coffee Frappe)", new Product("Double Dutch (Coffee Frappe)", 98.00m, 48)},
                {"Mango Graham (Coffee Frappe)", new Product("Mango Graham (Coffee Frappe)", 98.00m, 49)},
                {"Matcha (Coffee Frappe)", new Product("Matcha (Coffee Frappe)", 98.00m, 50)},
                {"Mocha (Coffee Frappe)", new Product("Mocha (Coffee Frappe)", 98.00m, 51)},
                {"Strawberry (Coffee Frappe)", new Product("Strawberry (Coffee Frappe)", 98.00m, 52)},
                {"Vanilla (Coffee Frappe)", new Product("Vanilla (Coffee Frappe)", 98.00m, 53)},

                {"Garlic Parmesan 3 pcs", new Product("Garlic Parmesan 3 pcs", 109.00m, 54)},
                {"Buffalo Wings 3 pcs", new Product("Buffalo Wings 3 pcs", 109.00m, 55)},
                {"Garlic Parmesan 4 pcs", new Product("Garlic Parmesan 3 pcs", 109.00m, 56)},
                {"Buffalo Wings 4pcs", new Product("Buffalo Wings 3 pcs", 109.00m, 57)},
                {"Garlic Parmesan 3 pcs • rice", new Product("Garlic Parmesan 3 pcs • rice", 139.00m, 58)},
                {"Buffalo Wings 3 pcs • rice", new Product("Buffalo Wings 3 pcs • rice", 139.00m, 59)},
                {"Garlic Parmesan 4 pcs • rice", new Product("Garlic Parmesan 4 pcs • rice", 169.00m, 60)},
                {"Buffalo Wings 4 pcs • rice", new Product("Garlic Parmesan 4 pcs • rice", 169.00m, 61)},

                {"Chicken Burger", new Product("Chicken Burger", 169.00m, 62)},
                {"Black Mamba", new Product("Black Mamba", 209.00m, 63)},

                {"Churros Classic", new Product("Churros Classic", 80.00m, 64)},
                {"Churros Overload", new Product("Churros Overload", 120.00m, 65)},
                {"Mojos (Solo)", new Product("Mojos (Solo)", 80.00m, 66)},
                {"Mojos (Barkada)", new Product("Mojos (Barkada)", 140.00m, 67)},
                {"Classic Bites", new Product("Classic Bites", 75.00m, 68)},
                {"Regular Classic", new Product("Regular Classic", 90.00m, 69)},
                {"Full Mozza", new Product("Full Mozza", 90.00m, 70)},
                {"Frenchie", new Product("Frenchie", 90.00m, 71)},
                {"Mozzadog", new Product("Mozzadog", 90.00m, 72)},

                {"Cheesy Fries", new Product("Cheesy Fries", 50.00m, 73)},
                {"Cheesy Nachos", new Product("Cheesy Nachos", 70.00m, 74)},
                {"Cheddar Sticks", new Product("Cheddar Sticks", 70.00m, 75)},
                {"Waffles", new Product("Waffles", 50.00m, 76)},
                {"Nutella Waffles", new Product("Nutella Waffles", 60.00m, 77)},
                {"Brownies", new Product("Brownies", 50.00m, 78)},
                {"Cookies", new Product("Cookies", 50.00m, 79)},

                {"Chocolate (Classic)", new Product("Chocolate (Classic)", 80.00m, 80)},
                {"Maple w/ Butter(Classic)", new Product("Maple w/ Butter(Classic)", 80.00m, 81)},
                {"Nutella (Classic)", new Product("Nutella (Classic)", 90.00m, 82)},
                {"Matcha (Classic)", new Product("Matcha (Classic)", 90.00m, 83)},
                {"Peanut Butter (Classic)", new Product("CooPeanut Butter (Classic)kies", 90.00m, 84)},
                {"Biscoff Spread (Classic)", new Product("Biscoff Spread (Classic)", 90.00m, 85)},
                {"Nutella Alcapone (Premium)", new Product("Nutella Alcapone (Premium)", 110.00m, 86)},
                {"Cookies & Cream (Premium)", new Product("Cookies & Cream (Premium)", 120.00m, 87)},
                {"Matcha Alcapone (Premium)", new Product("Matcha Alcapone (Premium)", 110.00m, 88)},
                {"Strawberry Cream (Premium", new Product("Strawberry Cream (Premium", 120.00m, 89)},
                {"Biscoff Overload (Premium)", new Product("Biscoff Overload (Premium)", 130.00m, 90)},
                {"Classic (IN A BOX)", new Product("Classic (IN A BOX)", 360.00m, 91)},
                {"Premium (IN A BOX)", new Product("Premium (IN A BOX)", 480.00m, 92)},
                {"Classic-Premium (IN A BOX)", new Product("Classic-Premium (IN A BOX)", 430.00m, 93)},

                {"Hungarian Classic (Sandwich)", new Product("Hungarian Classic (Sandwich)", 130.00m, 94)},
                {"Bacon & Cheese (Sandwich)", new Product("Bacon & Cheese (Sandwich)", 150.00m, 95)},
                {"Spicy Beef (Sandwich)", new Product("Spicy Beef (Sandwich)", 160.00m, 96)},
                {"Classic (On-Stick)", new Product("Classic (On-Stick)", 100.00m, 97)},
                {"Sausage & Bacon (On-Stick)", new Product("Sausage & Bacon (On-Stick)", 120.00m, 98)},
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