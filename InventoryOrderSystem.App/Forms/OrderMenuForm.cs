using System;
using System.Collections.Generic;
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

        public OrderMenuForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            InitializeCategories();
            orderItems = new List<OrderItem>();
            AddBackButton();
        }

        private void AddBackButton()
        {
            Button backButton = new Button
            {
                Text = "Back to Dashboard",
                Location = new System.Drawing.Point(12, this.ClientSize.Height - 40),
                Size = new System.Drawing.Size(120, 30)
            };
            backButton.Click += BackButton_Click;
            this.Controls.Add(backButton);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DashboardForm(_currentUser).Show();
        }

        private void InitializeCategories()
        {
            categoryProducts = new Dictionary<string, List<Product>>
            {
                {"COFFEES & TEAS", new List<Product>
                    {
                        new Product("Americano", 98.00m),
                        new Product("Cappuccino", 98.00m),
                        new Product("Cafe Mocha", 98.00m),
                        new Product("Caramel Macchiato", 98.00m)
                    }
                },
                {"SNACKS", new List<Product>()},
                {"FRAPPES, FRUIT MILKS, & MORE", new List<Product>()}
            };

            foreach (var category in categoryProducts.Keys)
            {
                TabPage tabPage = new TabPage(category);
                tabControl1.TabPages.Add(tabPage);
                DisplayProducts(category, tabPage);
            }
        }

        private void DisplayProducts(string category, TabPage tabPage)
        {
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            foreach (var product in categoryProducts[category])
            {
                GroupBox productBox = new GroupBox
                {
                    Text = product.Name,
                    Size = new System.Drawing.Size(250, 130),
                    Margin = new Padding(5)
                };

                CheckBox checkBox = new CheckBox
                {
                    Text = $"{product.Name} - ₱{product.Price:F2}",
                    Tag = product,
                    AutoSize = true,
                    Location = new System.Drawing.Point(10, 20)
                };
                checkBox.CheckedChanged += ProductCheckBox_CheckedChanged;

                ComboBox sizeComboBox = new ComboBox
                {
                    Items = { "8oz", "16oz" },
                    SelectedIndex = 0,
                    Location = new System.Drawing.Point(10, 50),
                    Size = new System.Drawing.Size(80, 25)
                };
                sizeComboBox.SelectedIndexChanged += (s, e) => UpdateOrderSummary();

                CheckBox extraShotCheckBox = new CheckBox
                {
                    Text = "Extra Shot",
                    AutoSize = true,
                    Location = new System.Drawing.Point(10, 80)
                };
                extraShotCheckBox.CheckedChanged += (s, e) => UpdateOrderSummary();

                NumericUpDown quantityUpDown = new NumericUpDown
                {
                    Minimum = 0,
                    Maximum = 10,
                    Value = 0,
                    Location = new System.Drawing.Point(150, 50),
                    Size = new System.Drawing.Size(50, 25)
                };
                quantityUpDown.ValueChanged += QuantityUpDown_ValueChanged;

                productBox.Controls.AddRange(new Control[] { checkBox, sizeComboBox, extraShotCheckBox, quantityUpDown });
                panel.Controls.Add(productBox);
            }

            tabPage.Controls.Add(panel);
        }

        private void ProductCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Product product = (Product)checkBox.Tag;
            GroupBox productBox = (GroupBox)checkBox.Parent;

            ComboBox sizeComboBox = productBox.Controls.OfType<ComboBox>().First();
            CheckBox extraShotCheckBox = productBox.Controls.OfType<CheckBox>().ElementAt(1);
            NumericUpDown quantityUpDown = productBox.Controls.OfType<NumericUpDown>().First();

            sizeComboBox.Enabled = checkBox.Checked;
            extraShotCheckBox.Enabled = checkBox.Checked;
            quantityUpDown.Enabled = checkBox.Checked;

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

        private void QuantityUpDown_ValueChanged(object sender, EventArgs e)
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

            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                foreach (GroupBox productBox in tabPage.Controls[0].Controls.OfType<GroupBox>())
                {
                    CheckBox checkBox = productBox.Controls.OfType<CheckBox>().First();
                    if (checkBox.Checked)
                    {
                        Product product = (Product)checkBox.Tag;
                        ComboBox sizeComboBox = productBox.Controls.OfType<ComboBox>().First();
                        CheckBox extraShotCheckBox = productBox.Controls.OfType<CheckBox>().ElementAt(1);
                        NumericUpDown quantityUpDown = productBox.Controls.OfType<NumericUpDown>().First();

                        OrderItem item = new OrderItem(product, sizeComboBox.Text, extraShotCheckBox.Checked, (int)quantityUpDown.Value);
                        orderItems.Add(item);

                        decimal itemPrice = item.Product.Price;
                        if (item.Size == "16oz") itemPrice += 20;
                        if (item.ExtraShot) itemPrice += 20;

                        string itemDescription = $"{item.Product.Name} ({item.Size})";
                        if (item.ExtraShot) itemDescription += " +Shot";

                        decimal totalItemPrice = itemPrice * item.Quantity;
                        listBoxOrderSummary.Items.Add($"{itemDescription} x{item.Quantity} - ₱{totalItemPrice:F2}");
                        total += totalItemPrice;
                    }
                }
            }

            labelTotal.Text = $"Total: ₱{total:F2}";
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

        public OrderItem(Product product, string size, bool extraShot, int quantity)
        {
            Product = product;
            Size = size;
            ExtraShot = extraShot;
            Quantity = quantity;
        }
    }
}