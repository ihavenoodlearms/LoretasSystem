using System;
using System.Drawing;
using System.Windows.Forms;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;

namespace InventoryOrderSystem.Forms
{
    public partial class EditInventoryForm : Form
    {
        private readonly DatabaseManager dbManager;
        private readonly InventoryItem currentItem;
        private readonly string[] categories = { "Powder", "Syrup", "Fruit Tea Syrup", "Misc." };
        private TextBox txtItemName;
        private TextBox txtQuantity;
        private ComboBox cboCategory;

        public EditInventoryForm(InventoryItem item)
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            currentItem = item;
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Form properties
            this.Text = "Edit Item";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(255, 248, 240);

            // Create title panel
            Panel titlePanel = new Panel
            {
                BackColor = Color.FromArgb(74, 44, 42),
                Dock = DockStyle.Top,
                Height = 50
            };

            Label titleLabel = new Label
            {
                Text = "Edit Item",
                ForeColor = Color.White,
                Font = new Font("Arial Rounded MT Bold", 14, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(15, 15)
            };
            titlePanel.Controls.Add(titleLabel);

            // Create input controls
            Label lblId = new Label
            {
                Text = $"Item ID: {currentItem.ItemId}",
                Location = new Point(20, 70),
                AutoSize = true
            };

            Label lblName = new Label
            {
                Text = "Item Name:",
                Location = new Point(20, 110),
                AutoSize = true
            };

            txtItemName = new TextBox
            {
                Location = new Point(120, 110),
                Width = 220,
                Text = currentItem.Name
            };

            Label lblQuantity = new Label
            {
                Text = "Quantity:",
                Location = new Point(20, 150),
                AutoSize = true
            };

            txtQuantity = new TextBox
            {
                Location = new Point(120, 150),
                Width = 100,
                Text = currentItem.Quantity.ToString()
            };

            Label lblCategory = new Label
            {
                Text = "Category:",
                Location = new Point(20, 190),
                AutoSize = true
            };

            cboCategory = new ComboBox
            {
                Location = new Point(120, 190),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboCategory.Items.AddRange(categories);
            cboCategory.SelectedItem = currentItem.Category;

            // Create buttons
            Button btnUpdate = new Button
            {
                Text = "Update",
                Location = new Point(120, 230),
                Width = 100,
                BackColor = Color.FromArgb(82, 110, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnUpdate.Click += BtnUpdate_Click;
            btnUpdate.FlatAppearance.BorderSize = 0;

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(240, 230),
                Width = 100,
                BackColor = Color.FromArgb(102, 67, 67),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                titlePanel,
                lblId,
                lblName, txtItemName,
                lblQuantity, txtQuantity,
                lblCategory, cboCategory,
                btnUpdate, btnCancel
            });
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    var updatedItem = new InventoryItem
                    {
                        ItemId = currentItem.ItemId,
                        Name = txtItemName.Text.Trim(),
                        Quantity = int.Parse(txtQuantity.Text),
                        Category = cboCategory.SelectedItem.ToString()
                    };

                    dbManager.UpdateInventoryItem(updatedItem);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating item: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text))
            {
                MessageBox.Show("Please enter an item name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtItemName.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid quantity (must be 0 or greater).",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }
    }
}