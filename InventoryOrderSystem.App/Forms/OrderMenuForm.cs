using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using InventoryOrderSystem.Forms;
using InventoryOrderSystem.Models;

namespace InventoryOrderingSystem
{
    public partial class OrderMenuForm : Form
    {
        private Dictionary<string, List<Product>> categoryProducts;
        private List<OrderItem> orderItems;
        private User _currentUser;
        private Dictionary<string, GroupBox> productBoxes;

        public OrderMenuForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            productBoxes = new Dictionary<string, GroupBox>();
            orderItems = new List<OrderItem>();
            InitializeCategories();
            buttonBackToDashboard.Click += BackButton_Click;
            buttonProceedToPayment.Click += ProceedToPaymentButton_Click;
        }

        private void InitializeCategories()
        {
            categoryProducts = new Dictionary<string, List<Product>>
            {
                {"Cheesecake Series", new List<Product>
                    {
                        new Product("Matcha Cheesecake", 118.00m),
                        new Product("Oreo Cheesecake", 118.00m),
                        new Product("Red Velvet Cheesecake", 118.00m),
                        new Product("Strawberry Cheesecake", 118.00m),
                        new Product("Ube Cheesecake", 118.00m),
                        new Product("Wintermelon Cheesecake", 118.00m),
                    }
                },
                                {"Fruit Tea & Lemonade", new List<Product>
                    {
                        new Product("Berry Blossom", 68.00m),
                        new Product("Blue Lemonade", 68.00m),
                        new Product("Green Apple Lemonade", 68.00m),
                        new Product("Lychee", 68.00m),
                        new Product("Paradise", 68.00m),
                        new Product("Strawberry Lemonade", 68.00m),
                        new Product("Sunrise", 68.00m),
                    }
                },
                {"Milk Tea Classic", new List<Product>
                    {
                        new Product("Chocolate", 78.00m),
                        new Product("Cookies and Cream", 78.00m),
                        new Product("Dark Chocolate", 78.00m),
                        new Product("Hazelnut", 78.00m),
                        new Product("Matcha", 78.00m),
                        new Product("Mocha", 78.00m),
                        new Product("Okinawa", 78.00m),
                        new Product("Taro", 78.00m),
                        new Product("Ube", 78.00m),
                        new Product("Wintermelon", 78.00m),
                    }
                },
                {"Fruit Milk", new List<Product>
                    {
                        new Product("Blueberry Milk", 98.00m),
                        new Product("Mango Milk", 98.00m),
                        new Product("Strawberry Milk", 98.00m),
                    }
                },
                {"Loreta's Specials", new List<Product>
                    {
                        new Product("Nutellatte", 118.00m),
                        new Product("Tiger Boba Milk", 108.00m),
                        new Product("Tiger Boba Milktea", 138.00m),
                        new Product("Tiger Oreo Cheesecake", 128.00m),
                    }
                },
                {"Iced Coffee", new List<Product>
                    {
                        new Product("Americano", 68.00m),
                        new Product("Cafe Latte", 78.00m),
                        new Product("Cafe Mocha", 78.00m),
                        new Product("Caramel Macchiato", 78.00m),
                        new Product("Cappuccino", 78.00m),
                        new Product("Dirty Matcha", 138.00m),
                        new Product("French Vanilla", 78.00m),
                        new Product("Matcha Latte", 98.00m),
                        new Product("Spanish Latte", 78.00m),
                        new Product("Triple Chocolate Mocha", 78.00m),
                    }
                },
                {"Frappe/Coffee", new List<Product>
                    {
                        new Product("Black Forest", 98.00m),
                        new Product("Cafe Latte", 98.00m),
                        new Product("Cappuccino", 98.00m),
                        new Product("Caramel", 98.00m),
                        new Product("Choc Chip", 98.00m),
                        new Product("Cookies and Cream", 98.00m),
                        new Product("Dark Chocolate", 98.00m),
                        new Product("Double Dutch", 98.00m),
                        new Product("Mango Graham", 98.00m),
                        new Product("Matcha", 98.00m),
                        new Product("Mocha", 98.00m),
                        new Product("Strawberry", 98.00m),
                        new Product("Vanilla", 98.00m),
                    }
                },
                {"Garlic Parmesan & Buffalo Wings", new List<Product>
                    {
                        new Product("3 pcs", 109.00m),
                        new Product("4 pcs", 139.00m),
                        new Product("3 pcs • rice", 139.00m),
                        new Product("4 pcs • rice", 169.00m),
                    }
                },
                {"Chicken Burger", new List<Product>
                    {
                        new Product("Chicken Burger", 169.00m),
                        new Product("Black Mamba", 209.00m),
                    }
                },
                {"Churros, Mojos, Corndogs", new List<Product>
                    {
                        new Product("Churros Classic", 80.00m),
                        new Product("Churros Overload", 120.00m),
                        new Product("Mojos (Solo)", 80.00m),
                        new Product("Mojos (Barkada)", 140.00m),
                        new Product("Classic Bites", 75.00m),
                        new Product("Regular Classic", 90.00m),
                        new Product("Full Mozza", 90.00m),
                        new Product("Frenchie", 90.00m),
                        new Product("Mozzadog", 90.00m),
                    }
                },
                {"Loreta's Snacks", new List<Product>
                    {
                        new Product("Cheesy Fries", 50.00m),
                        new Product("Cheesy Nachos", 70.00m),
                        new Product("Cheddar Sticks", 70.00m),
                        new Product("Waffles", 50.00m),
                        new Product("Nutella Waffles", 60.00m),
                        new Product("Brownies", 50.00m),
                        new Product("Cookies", 50.00m),
                    }
                },
                {"Croffle", new List<Product>
                    {
                        new Product("Chocolate (Classic)", 80.00m),
                        new Product("Maple w/ Butter(Classic)", 80.00m),
                        new Product("Nutella (Classic)", 90.00m),
                        new Product("Matcha (Classic)", 90.00m),
                        new Product("Peanut Butter (Classic)", 90.00m),
                        new Product("Biscoff Spread (Classic)", 90.00m),
                        new Product("Nutella Alcapone (Premium)", 110.00m),
                        new Product("Cookies & Cream (Premium)", 120.00m),
                        new Product("Matcha Alcapone (Premium)", 110.00m),
                        new Product("Strawberry Cream (Premium)", 120.00m),
                        new Product("Biscoff Overload (Premium)", 130.00m),
                        new Product("Classic (IN A BOX)", 360.00m),
                        new Product("Premium (IN A BOX)", 480.00m),
                        new Product("Classic-Premium (IN A BOX)", 430.00m),
                    }
                },
                {"Hungarian Sausage", new List<Product>
                    {
                        new Product("Hungarian Classic (Sandwich)", 130.00m),
                        new Product("Bacon & Cheese (Sandwich)", 150.00m),
                        new Product("Spicy Beef (Sandwich)", 160.00m),
                        new Product("Classic (On-Stick)", 100.00m),
                        new Product("Sausage & Bacon (On-Stick)", 120.00m),
                    }
                },
        };

            // Add "Drinks" label
            Label drinksLabel = new Label
            {
                Text = "Drinks",
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 10)
            };
            flowLayoutPanelCategories.Controls.Add(drinksLabel);

            // Add drink categories
            string[] drinkCategories = new string[]
            {
                "Cheesecake Series", "Fruit Tea & Lemonade", "Milk Tea Classic",
                "Fruit Milk", "Loreta's Specials", "Iced Coffee", "Frappe/Coffee"
            };

            foreach (var category in drinkCategories)
            {
                AddCategoryButton(category);
            }

            // Add "Snacks" label
            Label snacksLabel = new Label
            {
                Text = "Snacks",
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, flowLayoutPanelCategories.Controls[flowLayoutPanelCategories.Controls.Count - 1].Bottom + 20)
            };
            flowLayoutPanelCategories.Controls.Add(snacksLabel);

            // Add snack categories
            string[] snackCategories = new string[]
            {
                "Garlic Parmesan & Buffalo Wings", "Chicken Burger", "Churros, Mojos, Corndogs",
                "Loreta's Snacks", "Croffle", "Hungarian Sausage"
            };

            foreach (var category in snackCategories)
            {
                AddCategoryButton(category);
            }

            if (flowLayoutPanelCategories.Controls.Count > 0)
            {
                CategoryButton_Click(flowLayoutPanelCategories.Controls.OfType<Button>().First(), EventArgs.Empty);
            }
        }

        private void AddCategoryButton(string category)
        {
            Button categoryButton = new Button
            {
                Text = category,
                Size = new Size(180, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White,
                Tag = category
            };
            categoryButton.Click += CategoryButton_Click;
            flowLayoutPanelCategories.Controls.Add(categoryButton);
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            string category = (string)clickedButton.Tag;
            DisplayProducts(category);
        }

        private void DisplayProducts(string category)
        {
            flowLayoutPanelProducts.Controls.Clear();

            foreach (var product in categoryProducts[category])
            {
                GroupBox productBox;
                if (!productBoxes.TryGetValue(product.Name, out productBox))
                {
                    productBox = CreateProductBox(product);
                    productBoxes[product.Name] = productBox;
                }
                flowLayoutPanelProducts.Controls.Add(productBox);
            }
        }

        private GroupBox CreateProductBox(Product product)
        {
            GroupBox productBox = new GroupBox
            {
                Text = product.Name,
                Size = new Size(250, 300),
                Margin = new Padding(5)
            };

            CheckBox checkBox = new CheckBox
            {
                Text = $"₱{product.Price:F2}",
                Tag = product,
                AutoSize = true,
                Location = new Point(10, 20)
            };
            checkBox.CheckedChanged += ProductCheckBox_CheckedChanged;

            NumericUpDown quantityUpDown = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 10,
                Value = 0,
                Location = new Point(180, 20),
                Size = new Size(50, 25),
                Enabled = false
            };
            quantityUpDown.ValueChanged += (sender, e) => QuantityUpDown_ValueChanged(sender, e, product);

            productBox.Controls.Add(checkBox);
            productBox.Controls.Add(quantityUpDown);

            string[] categoriesWithOptions = new string[]
            {
                "Cheesecake Series", "Fruit Tea & Lemonade", "Milk Tea Classic",
                "Fruit Milk", "Loreta's Specials", "Iced Coffee", "Frappe/Coffee"
            };

            if (categoriesWithOptions.Any(category => categoryProducts[category].Contains(product)))
            {
                ComboBox sizeComboBox = new ComboBox
                {
                    Items = { "8oz", "16oz" },
                    SelectedIndex = 0,
                    Location = new Point(10, 50),
                    Size = new Size(80, 25),
                    Enabled = false
                };
                sizeComboBox.SelectedIndexChanged += (s, e) => UpdateOrderSummary();

                CheckBox extraShotCheckBox = new CheckBox
                {
                    Text = "Extra Shot",
                    AutoSize = true,
                    Location = new Point(100, 52),
                    Enabled = false
                };
                extraShotCheckBox.CheckedChanged += (s, e) => UpdateOrderSummary();

                productBox.Controls.Add(sizeComboBox);
                productBox.Controls.Add(extraShotCheckBox);

                Panel addOnsPanel = new Panel
                {
                    Location = new Point(10, 80),
                    Size = new Size(230, 210),
                    AutoScroll = true
                };
                productBox.Controls.Add(addOnsPanel);

                Label addOnsLabel = new Label
                {
                    Text = "Add-ons:",
                    AutoSize = true,
                    Location = new Point(5, 5)
                };
                addOnsPanel.Controls.Add(addOnsLabel);

                string[] addOns = new string[]
                {
                    "Pearls", "Nata de Coco", "Rainbow Jelly", "Chia Seeds",
                    "Crushed Oreo", "Crushed Graham", "Cream Cheese", "Brown Sugar", "Espresso"
                };

                int yPos = 25;
                foreach (var addOn in addOns)
                {
                    CheckBox addOnCheckBox = new CheckBox
                    {
                        Text = addOn,
                        AutoSize = true,
                        Location = new Point(5, yPos),
                        Enabled = false
                    };
                    addOnCheckBox.CheckedChanged += (s, e) => UpdateOrderSummary();
                    addOnsPanel.Controls.Add(addOnCheckBox);
                    yPos += 25;
                }
            }

            return productBox;
        }

        private void ProductCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            GroupBox productBox = (GroupBox)checkBox.Parent;

            NumericUpDown quantityUpDown = productBox.Controls.OfType<NumericUpDown>().First();
            quantityUpDown.Enabled = checkBox.Checked;

            ComboBox sizeComboBox = productBox.Controls.OfType<ComboBox>().FirstOrDefault();
            if (sizeComboBox != null)
            {
                sizeComboBox.Enabled = checkBox.Checked;
            }

            CheckBox extraShotCheckBox = productBox.Controls.OfType<CheckBox>().ElementAtOrDefault(1);
            if (extraShotCheckBox != null)
            {
                extraShotCheckBox.Enabled = checkBox.Checked;
            }

            Panel addOnsPanel = productBox.Controls.OfType<Panel>().FirstOrDefault();
            if (addOnsPanel != null)
            {
                foreach (CheckBox addOnCheckBox in addOnsPanel.Controls.OfType<CheckBox>())
                {
                    addOnCheckBox.Enabled = checkBox.Checked;
                }
            }

            if (checkBox.Checked && quantityUpDown.Value == 0)
            {
                quantityUpDown.Value = 1;
            }
            else if (!checkBox.Checked)
            {
                quantityUpDown.Value = 0;
            }

            UpdateOrderSummary();
        }

        private void QuantityUpDown_ValueChanged(object sender, EventArgs e, Product product)
        {
            NumericUpDown quantityUpDown = (NumericUpDown)sender;
            GroupBox productBox = (GroupBox)quantityUpDown.Parent;
            CheckBox checkBox = productBox.Controls.OfType<CheckBox>().First();

            if (quantityUpDown.Value > 0 && !checkBox.Checked)
            {
                checkBox.Checked = true;
            }
            else if (quantityUpDown.Value == 0 && checkBox.Checked)
            {
                checkBox.Checked = false;
            }

            UpdateOrderSummary();
        }

        private void UpdateOrderSummary()
        {
            orderItems.Clear();
            listBoxOrderSummary.Items.Clear();
            decimal total = 0;

            foreach (var productBox in productBoxes.Values)
            {
                CheckBox checkBox = productBox.Controls.OfType<CheckBox>().First();
                if (checkBox.Checked)
                {
                    Product product = (Product)checkBox.Tag;
                    NumericUpDown quantityUpDown = productBox.Controls.OfType<NumericUpDown>().First();
                    ComboBox sizeComboBox = productBox.Controls.OfType<ComboBox>().FirstOrDefault();
                    CheckBox extraShotCheckBox = productBox.Controls.OfType<CheckBox>().ElementAtOrDefault(1);

                    string size = sizeComboBox?.Text ?? "N/A";
                    bool extraShot = extraShotCheckBox?.Checked ?? false;

                    List<string> selectedAddOns = new List<string>();
                    Panel addOnsPanel = productBox.Controls.OfType<Panel>().FirstOrDefault();
                    if (addOnsPanel != null)
                    {
                        foreach (CheckBox addOnCheckBox in addOnsPanel.Controls.OfType<CheckBox>())
                        {
                            if (addOnCheckBox.Checked)
                            {
                                selectedAddOns.Add(addOnCheckBox.Text);
                            }
                        }
                    }

                    OrderItem item = new OrderItem(product, size, extraShot, (int)quantityUpDown.Value, selectedAddOns);
                    orderItems.Add(item);

                    decimal itemPrice = item.Product.Price;
                    if (size == "16oz") itemPrice += 20;
                    if (extraShot) itemPrice += 20;
                    itemPrice += selectedAddOns.Count * 15; // Assuming each add-on costs ₱15

                    string itemDescription = $"{item.Product.Name}";
                    if (size != "N/A") itemDescription += $" ({size})";
                    if (extraShot) itemDescription += " +Shot";
                    if (selectedAddOns.Any())
                    {
                        itemDescription += $" +{string.Join(", ", selectedAddOns)}";
                    }

                    decimal totalItemPrice = itemPrice * item.Quantity;
                    listBoxOrderSummary.Items.Add($"{itemDescription} x{item.Quantity} - ₱{totalItemPrice:F2}");
                    total += totalItemPrice;
                }
            }

            labelTotal.Text = $"Total: ₱{total:F2}";
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DashboardForm(_currentUser).Show();
        }

        private void ProceedToPaymentButton_Click(object sender, EventArgs e)
        {
            if (orderItems.Count == 0)
            {
                MessageBox.Show("Please add items to your order before proceeding to payment.", "Empty Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string orderSummary = GetOrderSummary();
            DialogResult result = MessageBox.Show(
                $"Please confirm your order:\n\n{orderSummary}\n\nProceed to payment?",
                "Confirm Order",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Hide();
                new PaymentForm(_currentUser, orderItems, decimal.Parse(labelTotal.Text.Replace("Total: ₱", ""))).Show();
            }
        }

        private string GetOrderSummary()
        {
            System.Text.StringBuilder summary = new System.Text.StringBuilder();
            foreach (var item in orderItems)
            {
                summary.AppendLine($"{item.Product.Name} ({item.Size}){(item.ExtraShot ? " +Shot" : "")} x{item.Quantity}");
                if (item.AddOns.Any())
                {
                    summary.AppendLine($"  Add-ons: {string.Join(", ", item.AddOns)}");
                }
            }
            summary.AppendLine($"\n{labelTotal.Text}");
            return summary.ToString();
        }
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    public class OrderItem
    {
        public Product Product { get; set; }
        public string Size { get; set; }
        public bool ExtraShot { get; set; }
        public int Quantity { get; set; }
        public List<string> AddOns { get; set; }

        public OrderItem(Product product, string size, bool extraShot, int quantity, List<string> addOns)
        {
            Product = product;
            Size = size;
            ExtraShot = extraShot;
            Quantity = quantity;
            AddOns = addOns;
        }

        public decimal CalculatePrice()
        {
            decimal price = Product.Price * Quantity;

            if (Size == "16oz")
                price += 20 * Quantity;

            if (ExtraShot)
                price += 20 * Quantity;

            price += AddOns.Count * 15 * Quantity; // Assuming each add-on costs ₱15

            return price;
        }

        public override string ToString()
        {
            string description = $"{Product.Name} ({Size})";
            if (ExtraShot)
                description += " +Shot";
            if (AddOns.Any())
                description += $" +{string.Join(", ", AddOns)}";
            return $"{description} x{Quantity}";
        }
    }
}