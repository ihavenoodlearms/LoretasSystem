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

namespace InventoryOrderingSystem
{
    public partial class OrderMenuForm : Form
    {
        private Dictionary<string, List<Product>> categoryProducts;
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
            buttonBackToDashboard.Click += BackButton_Click;
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
                        new Product("Chocolate (Milk Tea)", 78.00m),
                        new Product("Cookies and Cream (Milk Tea)", 78.00m),
                        new Product("Dark Chocolate (Milk Tea)", 78.00m),
                        new Product("Hazelnut (Milk Tea)", 78.00m),
                        new Product("Matcha (Milk Tea)", 78.00m),
                        new Product("Mocha (Milk Tea)", 78.00m),
                        new Product("Okinawa (Milk Tea)", 78.00m),
                        new Product("Taro (Milk Tea)", 78.00m),
                        new Product("Ube (Milk Tea)", 78.00m),
                        new Product("Wintermelon (Milk Tea)", 78.00m),
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
                        new Product("Cafe Latte (Iced Coffee)", 78.00m),
                        new Product("Cafe Mocha", 78.00m),
                        new Product("Caramel Macchiato", 78.00m),
                        new Product("Cappuccino (Iced Coffee)", 78.00m),
                        new Product("Dirty Matcha", 138.00m),
                        new Product("French Vanilla", 78.00m),
                        new Product("Matcha Latte", 98.00m),
                        new Product("Spanish Latte", 78.00m),
                        new Product("Triple Chocolate Mocha", 78.00m),
                    }
                },
                {"Frappe/Coffee", new List<Product>
                    {
                        new Product("Black Forest (Coffee Frappe)", 98.00m),
                        new Product("Cafe Latte (Coffee Frappe)", 98.00m),
                        new Product("Cappuccino (Coffee Frappe)", 98.00m),
                        new Product("Caramel (Coffee Frappe)", 98.00m),
                        new Product("Choc Chip (Coffee Frappe)", 98.00m),
                        new Product("Cookies and Cream (Coffee Frappe)", 98.00m),
                        new Product("Dark Chocolate (Coffee Frappe)", 98.00m),
                        new Product("Double Dutch (Coffee Frappe)", 98.00m),
                        new Product("Mango Graham (Coffee Frappe)", 98.00m),
                        new Product("Matcha (Coffee Frappe)", 98.00m),
                        new Product("Mocha (Coffee Frappe)", 98.00m),
                        new Product("Strawberry (Coffee Frappe)", 98.00m),
                        new Product("Vanilla (Coffee Frappe)", 98.00m),
                    }
                },
                {"Garlic Parmesan & Buffalo Wings", new List<Product>
                    {
                        new Product("Garlic Parmesan 3 pcs", 109.00m),
                        new Product("Buffalo Wings 3 pcs", 109.00m),
                        new Product("Garlic Parmesan 4 pcs", 139.00m),
                        new Product("Buffalo Wings 4 pcs", 139.00m),
                        new Product("Garlic Parmesan 3 pcs • rice", 139.00m),
                        new Product("Buffalo Wings 3 pcs • rice", 139.00m),
                        new Product("Garlic Parmesan 4 pcs • rice", 169.00m),
                        new Product("Buffalo Wings 4 pcs • rice", 169.00m),
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

            Button editAddOnsButton = new Button
            {
                Text = "Edit Add-ons",
                Size = new Size(180, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White,
                Margin = new Padding(0, 90, 0, 0) // Add some top margin to separate it from other buttons
            };
            editAddOnsButton.Click += EditAddOns_Click;
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

        private void OrderMenuForm_Resize(object sender, EventArgs e)
        {
            // Recalculate panel sizes
            panelSidebar.Width = (int)(this.ClientSize.Width * 0.17);

            // Adjust products panel
            panelProducts.Width = this.ClientSize.Width - panelSidebar.Width;
            panelProducts.Height = (int)(this.ClientSize.Height * 0.75);
            panelProducts.Location = new Point(panelSidebar.Width, 0);

            // Adjust order summary panel
            panelOrderSummary.Width = this.ClientSize.Width - panelSidebar.Width;
            panelOrderSummary.Location = new Point(panelSidebar.Width, panelProducts.Height);

            // Adjust button positions in order summary panel
            int buttonMargin = 10;
            buttonProceedToPayment.Location = new Point(
                panelOrderSummary.Width - buttonProceedToPayment.Width - buttonMargin,
                panelOrderSummary.Height - buttonProceedToPayment.Height - buttonMargin
            );

            buttonBackToDashboard.Location = new Point(
                buttonProceedToPayment.Left - buttonBackToDashboard.Width - buttonMargin,
                buttonProceedToPayment.Top
            );

            // Adjust list box size
            listBoxOrderSummary.Width = panelOrderSummary.Width - (buttonMargin * 2);
            listBoxOrderSummary.Height = panelOrderSummary.Height - labelTotal.Height - (buttonMargin * 4);

            // Adjust total label position
            labelTotal.Location = new Point(
                buttonMargin,
                listBoxOrderSummary.Bottom + buttonMargin
            );

            // Adjust product flow layout
            flowLayoutPanelProducts.Width = panelProducts.Width;
            flowLayoutPanelProducts.Height = panelProducts.Height;

            // Adjust categories flow layout
            flowLayoutPanelCategories.Width = panelSidebar.Width;
            flowLayoutPanelCategories.Height = panelSidebar.Height;

            // Resize product boxes
            ResizeProductBoxes();

            // Refresh the layout
            this.PerformLayout();
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


        private GroupBox CreateProductBox(Product product)
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

        private Product GetSelectedProduct()
        {
            foreach (GroupBox productBox in flowLayoutPanelProducts.Controls.OfType<GroupBox>())
            {
                CheckBox checkBox = productBox.Controls.OfType<CheckBox>().First();
                if (checkBox.Checked)
                {
                    return (Product)checkBox.Tag;
                }
            }
            return null;
        }

        private void AddToChecklist_Click(object sender, EventArgs e, Product product)
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

        private void RemoveFromChecklist_Click(object sender, EventArgs e, Product product)
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

            Product product = (Product)checkBox.Tag;
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
                if (_currentUser == null)
                {
                    MessageBox.Show("Error: No user is currently logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal totalAmount = decimal.Parse(labelTotal.Text.Replace("Total: ₱", ""));
                List<InventoryOrderSystem.Models.OrderItem> convertedItems = ConvertToOrderItems(orderItems);

                if (convertedItems.Count == 0)
                {
                    MessageBox.Show("Error: Unable to process order items.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Order newOrder = new Order
                {
                    UserId = _currentUser.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    PaymentMethod = "Cash", // You can add a payment method selection if needed
                    OrderItems = convertedItems,
                    Status = "Active"
                };

                OrderPlaced?.Invoke(this, newOrder);
                ResetOrderForm();
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
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string ProductName { get; set; }
        public Product Product { get; set; }
        public string Size { get; set; }
        public bool ExtraShot { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> AddOns { get; set; }

        public OrderItem() { }

        public OrderItem(Product product, string size, bool extraShot, int quantity, List<string> addOns)
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