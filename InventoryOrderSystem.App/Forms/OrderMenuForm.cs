using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using InventoryOrderSystem.Forms;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;
using InventoryOrderSystem;
using InventoryOrderSystem.App.Forms;

namespace InventoryOrderingSystem
{
    public partial class OrderMenuForm : Form
    {
        private Dictionary<string, List<InventoryOrderSystem.Product>> categoryProducts;
        private List<OrderItem> orderItems;
        private User _currentUser;
        private Dictionary<string, GroupBox> productBoxes;
        private DatabaseManager _dbManager;
        private List<string> currentAddOns;
        private const string ADD_ONS_FILE = "add_ons.json";

        public event EventHandler<Order> OrderPlaced;

        private void LoadAddOns()
        {
            if (File.Exists(ADD_ONS_FILE))
            {
                string json = File.ReadAllText(ADD_ONS_FILE);
                currentAddOns = JsonConvert.DeserializeObject<List<string>>(json);
            }
            else
            {
                currentAddOns = new List<string>
                {
                    "Pearls", "Nata de Coco", "Rainbow Jelly", "Chia Seeds",
                    "Crushed Oreo", "Crushed Graham", "Cream Cheese", "Brown Sugar", "Espresso"
                };
                SaveAddOns();
            }
        }

        private void SaveAddOns()
        {
            string json = JsonConvert.SerializeObject(currentAddOns);
            File.WriteAllText(ADD_ONS_FILE, json);
        }

        private void EditAddOns_Click(object sender, EventArgs e)
        {
            using (var form = new AddOnEditForm(currentAddOns))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    currentAddOns = form.UpdatedAddOns;
                    SaveAddOns();
                    RefreshProductBoxes();
                }
            }
        }

        private void RefreshProductBoxes()
        {
            foreach (var productBox in productBoxes.Values)
            {
                UpdateProductBoxAddOns(productBox);
            }
        }

        private void UpdateProductBoxAddOns(GroupBox productBox)
        {
            Panel addOnsPanel = productBox.Controls.OfType<Panel>().FirstOrDefault();
            if (addOnsPanel != null)
            {
                addOnsPanel.Controls.Clear();

                Label addOnsLabel = new Label
                {
                    Text = "Add-ons:",
                    AutoSize = true,
                    Location = new Point(5, 5)
                };
                addOnsPanel.Controls.Add(addOnsLabel);

                int yPos = 25;
                foreach (var addOn in currentAddOns)
                {
                    CheckBox addOnCheckBox = new CheckBox
                    {
                        Text = addOn,
                        AutoSize = true,
                        Location = new Point(5, yPos),
                        Enabled = productBox.Controls.OfType<CheckBox>().First().Checked
                    };
                    addOnsPanel.Controls.Add(addOnCheckBox);
                    yPos += 25;
                }
            }
        }

        public OrderMenuForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _dbManager = new DatabaseManager();
            productBoxes = new Dictionary<string, GroupBox>();
            orderItems = new List<OrderItem>();
            LoadAddOns();
            InitializeCategories();
            buttonProceedToPayment.Click += ProceedToPaymentButton_Click;

            // Add event handlers for the shared buttons
            buttonAddToChecklist.Click += SharedAddToChecklist_Click;
            buttonRemoveFromChecklist.Click += SharedRemoveFromChecklist_Click;

            Button editAddOnsButton = new Button
            {
                Text = "Edit Add-ons",
                Location = new Point(10, flowLayoutPanelCategories.Bottom + 10),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            editAddOnsButton.Click += EditAddOns_Click;
            this.Controls.Add(editAddOnsButton);

            // Add to your OrderMenuForm constructor after initializing other components
            if (_currentUser.IsSuperAdmin)
            {
                MessageBox.Show("Adding admin button"); // Debug message
                Button btnManageProducts = new Button
                {
                    Text = "Manage Products",
                    Size = new Size(210, 40),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(74, 44, 42),
                    ForeColor = Color.White,
                    Margin = new Padding(0, 10, 0, 0)
                };
                btnManageProducts.FlatAppearance.BorderSize = 0;
                btnManageProducts.Font = new Font("Arial Rounded MT Bold", 9, FontStyle.Bold);
                btnManageProducts.Click += (s, e) =>
                {
                    using (var productManagement = new ProductManagementForm(_currentUser, categoryProducts))
                    {
                        if (productManagement.ShowDialog() == DialogResult.OK)
                        {
                            InitializeCategories();
                            if (flowLayoutPanelCategories.Controls.Count > 0)
                            {
                                var firstCategoryButton = flowLayoutPanelCategories.Controls
                                    .OfType<Button>()
                                    .FirstOrDefault(b => b.Tag != null);

                                if (firstCategoryButton != null)
                                {
                                    CategoryButton_Click(firstCategoryButton, EventArgs.Empty);
                                }
                            }
                        }
                    }
                };

                // Add at a specific position
                int insertIndex = flowLayoutPanelCategories.Controls.Count - 1; // Before Edit Add-ons button
                flowLayoutPanelCategories.Controls.Add(btnManageProducts);
            }

            Console.WriteLine($"User is admin: {_currentUser.IsSuperAdmin}");
        }


        private void InitializeCategories()
        {
            categoryProducts = new Dictionary<string, List<InventoryOrderSystem.Product>>
{
                {"Cheesecake Series", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Matcha Cheesecake", 118.00m, 1),
                        new InventoryOrderSystem.Product("Oreo Cheesecake", 118.00m, 2),
                        new InventoryOrderSystem.Product("Red Velvet Cheesecake", 118.00m, 3),
                        new InventoryOrderSystem.Product("Strawberry Cheesecake", 118.00m, 4),
                        new InventoryOrderSystem.Product("Ube Cheesecake", 118.00m, 5),
                        new InventoryOrderSystem.Product("Wintermelon Cheesecake", 118.00m, 6),
                    }
                },
                {"Fruit Tea & Lemonade", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Berry Blossom", 68.00m, 7),
                        new InventoryOrderSystem.Product("Blue Lemonade", 68.00m, 8),
                        new InventoryOrderSystem.Product("Green Apple Lemonade", 68.00m, 9),
                        new InventoryOrderSystem.Product("Lychee", 68.00m, 10),
                        new InventoryOrderSystem.Product("Paradise", 68.00m, 11),
                        new InventoryOrderSystem.Product("Strawberry Lemonade", 68.00m, 12),
                        new InventoryOrderSystem.Product("Sunrise", 68.00m, 13),
                    }
                },
                {"Milk Tea Classic", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Chocolate (Milk Tea)", 78.00m, 14),
                        new InventoryOrderSystem.Product("Cookies and Cream (Milk Tea)", 78.00m, 15),
                        new InventoryOrderSystem.Product("Dark Chocolate (Milk Tea)", 78.00m, 16),
                        new InventoryOrderSystem.Product("Hazelnut (Milk Tea)", 78.00m, 17),
                        new InventoryOrderSystem.Product("Matcha (Milk Tea)", 78.00m, 18),
                        new InventoryOrderSystem.Product("Mocha (Milk Tea)", 78.00m, 19),
                        new InventoryOrderSystem.Product("Okinawa (Milk Tea)", 78.00m, 20),
                        new InventoryOrderSystem.Product("Taro (Milk Tea)", 78.00m, 21),
                        new InventoryOrderSystem.Product("Ube (Milk Tea)", 78.00m, 22),
                        new InventoryOrderSystem.Product("Wintermelon (Milk Tea)", 78.00m, 23),
                    }
                },
                {"Fruit Milk", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Blueberry Milk", 98.00m, 24),
                        new InventoryOrderSystem.Product("Mango Milk", 98.00m, 25),
                        new InventoryOrderSystem.Product("Strawberry Milk", 98.00m, 26),
                    }
                },
                {"Loreta's Specials", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Nutellatte", 118.00m, 27),
                        new InventoryOrderSystem.Product("Tiger Boba Milk", 108.00m, 28),
                        new InventoryOrderSystem.Product("Tiger Boba Milktea", 138.00m, 29),
                        new InventoryOrderSystem.Product("Tiger Oreo Cheesecake", 128.00m, 30),
                    }
                },
                {"Iced Coffee", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Americano", 68.00m, 31),
                        new InventoryOrderSystem.Product("Cafe Latte (Iced Coffee)", 78.00m, 32),
                        new InventoryOrderSystem.Product("Cafe Mocha", 78.00m, 33),
                        new InventoryOrderSystem.Product("Caramel Macchiato", 78.00m, 34),
                        new InventoryOrderSystem.Product("Cappuccino (Iced Coffee)", 78.00m, 35),
                        new InventoryOrderSystem.Product("Dirty Matcha", 138.00m, 36),
                        new InventoryOrderSystem.Product("French Vanilla", 78.00m, 37),
                        new InventoryOrderSystem.Product("Matcha Latte", 98.00m, 38),
                        new InventoryOrderSystem.Product("Spanish Latte", 78.00m, 39),
                        new InventoryOrderSystem.Product("Triple Chocolate Mocha", 78.00m, 40),
                    }
                },
                {"Frappe/Coffee", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Black Forest (Coffee Frappe)", 98.00m, 41),
                        new InventoryOrderSystem.Product("Cafe Latte (Coffee Frappe)", 98.00m, 42),
                        new InventoryOrderSystem.Product("Cappuccino (Coffee Frappe)", 98.00m, 43),
                        new InventoryOrderSystem.Product("Caramel (Coffee Frappe)", 98.00m, 44),
                        new InventoryOrderSystem.Product("Choc Chip (Coffee Frappe)", 98.00m, 45),
                        new InventoryOrderSystem.Product("Cookies and Cream (Coffee Frappe)", 98.00m, 46),
                        new InventoryOrderSystem.Product("Dark Chocolate (Coffee Frappe)", 98.00m, 47),
                        new InventoryOrderSystem.Product("Double Dutch (Coffee Frappe)", 98.00m, 48),
                        new InventoryOrderSystem.Product("Mango Graham (Coffee Frappe)", 98.00m, 49),
                        new InventoryOrderSystem.Product("Matcha (Coffee Frappe)", 98.00m, 50),
                        new InventoryOrderSystem.Product("Mocha (Coffee Frappe)", 98.00m, 51),
                        new InventoryOrderSystem.Product("Strawberry (Coffee Frappe)", 98.00m, 52),
                        new InventoryOrderSystem.Product("Vanilla (Coffee Frappe)", 98.00m, 53),
                    }
                },
                {"Garlic Parmesan & Buffalo Wings", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Garlic Parmesan 3 pcs", 109.00m, 54),
                        new InventoryOrderSystem.Product("Buffalo Wings 3 pcs", 109.00m, 55),
                        new InventoryOrderSystem.Product("Garlic Parmesan 4 pcs", 139.00m, 56),
                        new InventoryOrderSystem.Product("Buffalo Wings 4 pcs", 139.00m, 57),
                        new InventoryOrderSystem.Product("Garlic Parmesan 3 pcs • rice", 139.00m, 58),
                        new InventoryOrderSystem.Product("Buffalo Wings 3 pcs • rice", 139.00m, 59),
                        new InventoryOrderSystem.Product("Garlic Parmesan 4 pcs • rice", 169.00m, 60),
                        new InventoryOrderSystem.Product("Buffalo Wings 4 pcs • rice", 169.00m, 61),
                    }
                },
                {"Chicken Burger", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Chicken Burger", 169.00m, 62),
                        new InventoryOrderSystem.Product("Black Mamba", 209.00m, 63),
                    }
                },
                {"Churros, Mojos, Corndogs", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Churros Classic", 80.00m, 64),
                        new InventoryOrderSystem.Product("Churros Overload", 120.00m, 65),
                        new InventoryOrderSystem.Product("Mojos (Solo)", 80.00m, 66),
                        new InventoryOrderSystem.Product("Mojos (Barkada)", 140.00m, 67),
                        new InventoryOrderSystem.Product("Classic Bites", 75.00m, 68),
                        new InventoryOrderSystem.Product("Regular Classic", 90.00m, 69),
                        new InventoryOrderSystem.Product("Full Mozza", 90.00m, 70),
                        new InventoryOrderSystem.Product("Frenchie", 90.00m, 71),
                        new InventoryOrderSystem.Product("Mozzadog", 90.00m, 72),
                    }
                },
                {"Loreta's Snacks", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Cheesy Fries", 50.00m, 73),
                        new InventoryOrderSystem.Product("Cheesy Nachos", 70.00m, 74),
                        new InventoryOrderSystem.Product("Cheddar Sticks", 70.00m, 75),
                        new InventoryOrderSystem.Product("Waffles", 50.00m, 76),
                        new InventoryOrderSystem.Product("Nutella Waffles", 60.00m, 77),
                        new InventoryOrderSystem.Product("Brownies", 50.00m, 78),
                        new InventoryOrderSystem.Product("Cookies", 50.00m, 79),
                    }
                },
                {"Croffle", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Chocolate (Classic)", 80.00m, 80),
                        new InventoryOrderSystem.Product("Maple w/ Butter(Classic)", 80.00m, 81),
                        new InventoryOrderSystem.Product("Nutella (Classic)", 90.00m, 82),
                        new InventoryOrderSystem.Product("Matcha (Classic)", 90.00m, 83),
                        new InventoryOrderSystem.Product("Peanut Butter (Classic)", 90.00m, 84),
                        new InventoryOrderSystem.Product("Biscoff Spread (Classic)", 90.00m, 85),
                        new InventoryOrderSystem.Product("Nutella Alcapone (Premium)", 110.00m, 86),
                        new InventoryOrderSystem.Product("Cookies & Cream (Premium)", 120.00m, 87),
                        new InventoryOrderSystem.Product("Matcha Alcapone (Premium)", 110.00m, 88),
                        new InventoryOrderSystem.Product("Strawberry Cream (Premium)", 120.00m, 89),
                        new InventoryOrderSystem.Product("Biscoff Overload (Premium)", 130.00m, 90),
                        new InventoryOrderSystem.Product("Classic (IN A BOX)", 360.00m, 91),
                        new InventoryOrderSystem.Product("Premium (IN A BOX)", 480.00m, 92),
                        new InventoryOrderSystem.Product("Classic-Premium (IN A BOX)", 430.00m, 93),
                    }
                },
                {"Hungarian Sausage", new List<InventoryOrderSystem.Product>
                    {
                        new InventoryOrderSystem.Product("Hungarian Classic (Sandwich)", 130.00m, 94),
                        new InventoryOrderSystem.Product("Bacon & Cheese (Sandwich)", 150.00m, 95),
                        new InventoryOrderSystem.Product("Spicy Beef (Sandwich)", 160.00m, 96),
                        new InventoryOrderSystem.Product("Classic (On-Stick)", 100.00m, 97),
                        new InventoryOrderSystem.Product("Sausage & Bacon (On-Stick)", 120.00m, 98),
                    }
                }

        };



            // Add "Drinks" label
            Label drinksLabel = new Label
            {
                Text = "Drinks",
                AutoSize = true,
                Font = new Font(this.Font.FontFamily, 16, FontStyle.Bold),
                Margin = new Padding(10),
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
                Font = new Font(this.Font.FontFamily, 16, FontStyle.Bold),
                ForeColor = Color.White,
                Margin = new Padding(10),
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

            Button editAddOnsButton = new Button
            {
                Text = "Edit Add-ons",
                Size = new Size(210, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White,
                Margin = new Padding(0, 40, 0, 0) 
            };
            editAddOnsButton.FlatAppearance.BorderSize = 0;
            editAddOnsButton.Click += EditAddOns_Click;
            editAddOnsButton.Font = new Font("Arial Rounded MT Bold", 9, FontStyle.Bold);
            flowLayoutPanelCategories.Controls.Add(editAddOnsButton);

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
                FlatStyle = FlatStyle.Flat,        
                BackColor = Color.FromArgb(74, 44, 42), 
                ForeColor = Color.White,
                Size = new Size(205, 40),
                Tag = category                   
            };

           
            categoryButton.FlatAppearance.BorderSize = 0;
            categoryButton.Font = new Font("Arial Rounded MT Bold", 9, FontStyle.Bold);
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

        private void OrderMenuForm_Resize(object sender, EventArgs e)
        {
           
            // Refresh the layout
            
        }

        private void ResizeProductBoxes()
        {
            foreach (GroupBox box in flowLayoutPanelProducts.Controls.OfType<GroupBox>())
            {
                int newWidth = (int)(flowLayoutPanelProducts.Width * 0.3);
                box.Width = newWidth;

                // Resize and reposition controls inside the box
                foreach (Control control in box.Controls)
                {
                    if (control is Button btn)
                    {
                        if (btn.Text == "Add to Checklist")
                        {
                            btn.Width = (newWidth - 30) / 2;
                            btn.Location = new Point(10, box.Height - 40);
                        }
                        else if (btn.Text == "Remove")
                        {
                            btn.Width = (newWidth - 30) / 2;
                            btn.Location = new Point(btn.Width + 20, box.Height - 40);
                        }
                    }
                    else if (control is Panel addOnsPanel)
                    {
                        addOnsPanel.Width = newWidth - 20;
                    }
                }
            }
        }


        private GroupBox CreateProductBox(InventoryOrderSystem.Product product)
        {
            // Calculate box size based on container width
            int boxWidth = (int)(flowLayoutPanelProducts.Width * 0.3);
            int boxHeight = 320;

            GroupBox productBox = new GroupBox
            {
                Text = product.Name,
                Size = new Size(boxWidth, boxHeight),
                Margin = new Padding(5),
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Tag = product  // Store the product in the Tag property
            };

            // Adjust control positions to be relative to box size
            int padding = 10;
            int controlHeight = 25;

            CheckBox checkBox = new CheckBox
            {
                Text = $"₱{product.Price:F2}",
                Tag = product,
                AutoSize = true,
                Location = new Point(padding, controlHeight)
            };
            checkBox.CheckedChanged += ProductCheckBox_CheckedChanged;

            NumericUpDown quantityUpDown = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 10,
                Value = 0,
                Location = new Point(boxWidth - 70 - padding, controlHeight),
                Size = new Size(70, controlHeight),
                Enabled = false
            };

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

                productBox.Controls.Add(sizeComboBox);

                if (categoryProducts["Frappe/Coffee"].Contains(product))
                {
                    CheckBox extraShotCheckBox = new CheckBox
                    {
                        Text = "Extra Shot",
                        AutoSize = true,
                        Location = new Point(100, 52),
                        Enabled = false
                    };
                    productBox.Controls.Add(extraShotCheckBox);
                }

                Panel addOnsPanel = new Panel
                {
                    Location = new Point(10, 80),
                    Size = new Size(230, 190),
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


                int yPos = 25;
                foreach (var addOn in currentAddOns)
                {
                    CheckBox addOnCheckBox = new CheckBox
                    {
                        Text = addOn,
                        AutoSize = true,
                        Location = new Point(5, yPos),
                        Enabled = false
                    };
                    addOnsPanel.Controls.Add(addOnCheckBox);
                    yPos += 25;
                }
            }

            return productBox;
        }

        private void SharedAddToChecklist_Click(object sender, EventArgs e)
        {
            // Find selected product
            var selectedProduct = GetSelectedProduct();
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product first.", "No Product Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GroupBox productBox = productBoxes[selectedProduct.Name];
            NumericUpDown quantityUpDown = productBox.Controls.OfType<NumericUpDown>().First();

            if (quantityUpDown.Value == 0)
            {
                MessageBox.Show("Please specify a quantity.", "Invalid Quantity",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get all the necessary information from the product box
            ComboBox sizeComboBox = productBox.Controls.OfType<ComboBox>().FirstOrDefault();
            CheckBox extraShotCheckBox = productBox.Controls.OfType<CheckBox>().ElementAtOrDefault(1);
            Panel addOnsPanel = productBox.Controls.OfType<Panel>().FirstOrDefault();

            string size = sizeComboBox?.Text ?? "N/A";
            bool extraShot = extraShotCheckBox?.Checked ?? false;
            List<string> selectedAddOns = new List<string>();

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

            OrderItem newItem = new OrderItem(selectedProduct, size, extraShot,
                (int)quantityUpDown.Value, selectedAddOns);
            orderItems.Add(newItem);

            string itemDescription = GetItemDescription(newItem);
            listBoxOrderSummary.Items.Add(itemDescription);

            // Update total
            decimal itemTotal = newItem.CalculatePrice();
            decimal currentTotal = decimal.Parse(labelTotal.Text.Replace("Total: ₱", ""));
            decimal newTotal = currentTotal + itemTotal;
            labelTotal.Text = $"Total: ₱{newTotal:F2}";

            // Reset the quantity and checkbox
            quantityUpDown.Value = 0;
            productBox.Controls.OfType<CheckBox>().First().Checked = false;

            MessageBox.Show("Item added to checklist successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SharedRemoveFromChecklist_Click(object sender, EventArgs e)
        {
            if (listBoxOrderSummary.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an item from the checklist to remove.",
                    "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedItem = listBoxOrderSummary.SelectedItem.ToString();
            OrderItem itemToRemove = orderItems.FirstOrDefault(item =>
                GetItemDescription(item) == selectedItem);

            if (itemToRemove != null)
            {
                orderItems.Remove(itemToRemove);
                listBoxOrderSummary.Items.RemoveAt(listBoxOrderSummary.SelectedIndex);

                decimal removedItemPrice = itemToRemove.CalculatePrice();
                decimal currentTotal = decimal.Parse(labelTotal.Text.Replace("Total: ₱", ""));
                decimal newTotal = currentTotal - removedItemPrice;
                labelTotal.Text = $"Total: ₱{newTotal:F2}";

                MessageBox.Show("Item removed from checklist successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private InventoryOrderSystem.Product GetSelectedProduct()
        {
            foreach (GroupBox productBox in flowLayoutPanelProducts.Controls.OfType<GroupBox>())
            {
                CheckBox checkBox = productBox.Controls.OfType<CheckBox>().First();
                if (checkBox.Checked)
                {
                    return (InventoryOrderSystem.Product)checkBox.Tag;
                }
            }
            return null;
        }

        private void AddToChecklist_Click(object sender, EventArgs e, InventoryOrderSystem.Product product)
        {
            Button button = (Button)sender;
            GroupBox productBox = (GroupBox)button.Parent;

            CheckBox mainCheckBox = productBox.Controls.OfType<CheckBox>().First();
            if (!mainCheckBox.Checked)
            {
                MessageBox.Show("Please select the product before adding to the checklist.", "Product Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NumericUpDown quantityUpDown = productBox.Controls.OfType<NumericUpDown>().First();
            if (quantityUpDown.Value == 0)
            {
                MessageBox.Show("Please specify a quantity before adding to the checklist.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ComboBox sizeComboBox = productBox.Controls.OfType<ComboBox>().FirstOrDefault();
            CheckBox extraShotCheckBox = productBox.Controls.OfType<CheckBox>().ElementAtOrDefault(1);
            Panel addOnsPanel = productBox.Controls.OfType<Panel>().FirstOrDefault();

            string size = sizeComboBox?.Text ?? "N/A";
            bool extraShot = extraShotCheckBox?.Checked ?? false;
            List<string> selectedAddOns = new List<string>();

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

            OrderItem newItem = new OrderItem(product, size, extraShot, (int)quantityUpDown.Value, selectedAddOns);
            orderItems.Add(newItem);

            string itemDescription = GetItemDescription(newItem);
            listBoxOrderSummary.Items.Add(itemDescription);

            decimal itemTotal = newItem.CalculatePrice();
            decimal currentTotal = decimal.Parse(labelTotal.Text.Replace("Total: ₱", ""));
            decimal newTotal = currentTotal + itemTotal;
            labelTotal.Text = $"Total: ₱{newTotal:F2}";

            MessageBox.Show("Item added to the checklist successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reset the quantity, but keep other selections intact
            quantityUpDown.Value = 0;
            mainCheckBox.Checked = false;
        }

        private void RemoveFromChecklist_Click(object sender, EventArgs e, InventoryOrderSystem.Product product)
        {
            if (listBoxOrderSummary.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an item from the checklist to remove.", "No Item Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedItem = listBoxOrderSummary.SelectedItem.ToString();
            OrderItem itemToRemove = orderItems.FirstOrDefault(item => GetItemDescription(item) == selectedItem);

            if (itemToRemove != null)
            {
                orderItems.Remove(itemToRemove);
                listBoxOrderSummary.Items.RemoveAt(listBoxOrderSummary.SelectedIndex);

                decimal removedItemPrice = itemToRemove.CalculatePrice();
                decimal currentTotal = decimal.Parse(labelTotal.Text.Replace("Total: ₱", ""));
                decimal newTotal = currentTotal - removedItemPrice;
                labelTotal.Text = $"Total: ₱{newTotal:F2}";

                MessageBox.Show("Item removed from the checklist successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private string GetItemDescription(OrderItem item)
        {
            string description = $"{item.Product.Name} ({item.Size})";
            if (item.ExtraShot) description += " +Shot";
            if (item.AddOns.Any()) description += $" +{string.Join(", ", item.AddOns)}";
            description += $" x{item.Quantity} - ₱{item.CalculatePrice():F2}";
            return description;
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

            InventoryOrderSystem.Product product = (InventoryOrderSystem.Product)checkBox.Tag;
            if (categoryProducts["Frappe/Coffee"].Contains(product))
            {
                CheckBox extraShotCheckBox = productBox.Controls.OfType<CheckBox>().ElementAtOrDefault(1);
                if (extraShotCheckBox != null)
                {
                    extraShotCheckBox.Enabled = checkBox.Checked;
                }
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
                MessageBox.Show("Please add items to your order before proceeding to payment.",
                    "Empty Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Show payment method selection form
            using (var paymentMethodForm = new PaymentMethodForm())
            {
                if (paymentMethodForm.ShowDialog() == DialogResult.OK)
                {
                    string selectedPaymentMethod = paymentMethodForm.SelectedPaymentMethod;
                    string orderSummary = GetOrderSummary();

                    // Add payment method to order summary
                    orderSummary += $"\nPayment Method: {selectedPaymentMethod}";

                    DialogResult result = MessageBox.Show(
                        $"Please confirm your order:\n\n{orderSummary}\n\nProceed to payment?",
                        "Confirm Order",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            decimal totalAmount = decimal.Parse(labelTotal.Text.Replace("Total: ₱", ""));
                            List<InventoryOrderSystem.Models.OrderItem> convertedItems = ConvertToOrderItems(orderItems);

                            Order newOrder = new Order
                            {
                                UserId = _currentUser.UserId,
                                OrderDate = DateTime.Now,
                                TotalAmount = totalAmount,
                                PaymentMethod = selectedPaymentMethod,
                                OrderItems = convertedItems,
                                Status = "Active"
                            };

                            // Raise the OrderPlaced event
                            OrderPlaced?.Invoke(this, newOrder);

                            // Clear the order form
                            ResetOrderForm();

                            // Close OrderMenuForm and return to previous form
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error processing order: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void ResetOrderForm()
        {
            // Clear the order items list
            orderItems.Clear();

            // Clear the listbox
            listBoxOrderSummary.Items.Clear();

            // Reset the total
            labelTotal.Text = "Total: ₱0.00";

            // Reset all product boxes
            foreach (var productBox in productBoxes.Values)
            {
                var mainCheckBox = productBox.Controls.OfType<CheckBox>().FirstOrDefault();
                if (mainCheckBox != null)
                {
                    mainCheckBox.Checked = false;
                }

                var quantityUpDown = productBox.Controls.OfType<NumericUpDown>().FirstOrDefault();
                if (quantityUpDown != null)
                {
                    quantityUpDown.Value = 0;
                }

                var sizeComboBox = productBox.Controls.OfType<ComboBox>().FirstOrDefault();
                if (sizeComboBox != null)
                {
                    sizeComboBox.SelectedIndex = 0;
                }

                var extraShotCheckBox = productBox.Controls.OfType<CheckBox>().ElementAtOrDefault(1);
                if (extraShotCheckBox != null)
                {
                    extraShotCheckBox.Checked = false;
                }

                var addOnsPanel = productBox.Controls.OfType<Panel>().FirstOrDefault();
                if (addOnsPanel != null)
                {
                    foreach (var addOnCheckBox in addOnsPanel.Controls.OfType<CheckBox>())
                    {
                        addOnCheckBox.Checked = false;
                    }
                }
            }

            // Optionally, reset the category view to the first category
            if (flowLayoutPanelCategories.Controls.Count > 0)
            {
                var firstCategoryButton = flowLayoutPanelCategories.Controls.OfType<Button>().FirstOrDefault();
                if (firstCategoryButton != null)
                {
                    CategoryButton_Click(firstCategoryButton, EventArgs.Empty);
                }
            }
        }

        private List<InventoryOrderSystem.Models.OrderItem> ConvertToOrderItems(List<InventoryOrderingSystem.OrderItem> menuOrderItems)
        {
            List<InventoryOrderSystem.Models.OrderItem> dbOrderItems = new List<InventoryOrderSystem.Models.OrderItem>();
            foreach (var item in menuOrderItems)
            {
                int itemId = ProductCatalog.GetProductId(item.Product.Name);
                if (itemId != -1)
                {
                    dbOrderItems.Add(new InventoryOrderSystem.Models.OrderItem
                    {
                        ItemId = itemId,
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        Price = item.CalculatePrice(),
                        Size = item.Size,
                        ExtraShot = item.ExtraShot,
                        AddOns = item.AddOns
                    });
                }
                else
                {
                    MessageBox.Show($"Warning: Item '{item.Product.Name}' not found in the catalog. It will be skipped.", "Item Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return dbOrderItems;
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

        private void buttonBackToDashboard_Click(object sender, EventArgs e)
        {
            // Check the role of the current user
            if (_currentUser.Role == "Admin")
            {
                this.Hide();
                DashboardForm adminDashboard = new DashboardForm(_currentUser);  
                adminDashboard.Show();
            }
            else if (_currentUser.Role == "User")
            {
                
                this.Hide();
                UserForm userDashboard = new UserForm(_currentUser);  
                userDashboard.Show();
            }
            else
            {
                MessageBox.Show("Unknown role. Please contact the administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string ProductName { get; set; }
        public InventoryOrderSystem.Product Product { get; set; }
        public string Size { get; set; }
        public bool ExtraShot { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> AddOns { get; set; }

        public OrderItem() { }

        public OrderItem(InventoryOrderSystem.Product product, string size, bool extraShot, int quantity, List<string> addOns)
        {
            Product = product;
            ProductName = product.Name;
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