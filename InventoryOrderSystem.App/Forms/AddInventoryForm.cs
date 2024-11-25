using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;

namespace InventoryOrderSystem.Forms
{
    public partial class AddInventoryForm : Form
    {
        private readonly DatabaseManager dbManager;
        private readonly string[] categories = { "Powder", "Syrup", "Fruit Tea Syrup", "Misc." };
        private TextBox txtItemName;
        private TextBox txtQuantity;
        private ComboBox cboCategory;
        private ComboBox cboUnit;
        private bool mouseDown;
        private Point lastLocation;

        private readonly Dictionary<string, string[]> categoryUnits = new Dictionary<string, string[]>
        {
            { "Powder", new[] { "KG", "G", "OZ", "LB" } },
            { "Syrup", new[] { "L", "ML", "GAL", "OZ" } },
            { "Fruit Tea Syrup", new[] { "L", "ML", "GAL", "OZ" } },
            { "Misc.", new[] { "PCS", "PACK", "BOX" } }
        };

        public AddInventoryForm()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Form properties
            this.Text = "Add New Item";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(255, 248, 240);

            // Create title panel
            Panel titlePanel = new Panel
            {
                BackColor = Color.FromArgb(74, 44, 42),
                Dock = DockStyle.Top,
                Height = 40
            };

            titlePanel.MouseDown += (s, e) =>
            {
                mouseDown = true;
                lastLocation = e.Location;
            };

            titlePanel.MouseMove += (s, e) =>
            {
                if (mouseDown)
                {
                    this.Location = new Point(
                        (this.Location.X - lastLocation.X) + e.X,
                        (this.Location.Y - lastLocation.Y) + e.Y);
                    this.Update();
                }
            };

            titlePanel.MouseUp += (s, e) =>
            {
                mouseDown = false;
            };

            Label titleLabel = new Label
            {
                Text = "Add New Item",
                ForeColor = Color.White,
                Font = new Font("Arial Rounded MT Bold", 14, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(15, 10)
            };

            Button closeButton = new Button
            {
                Text = "×",
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(30, 30),
                Location = new Point(this.Width - 40, 5),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(74, 44, 42)
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => this.Close();

            titlePanel.Controls.AddRange(new Control[] { titleLabel, closeButton });

            // Create input controls with padding from top
            int startY = 70;  // Starting Y position after title panel

            Label lblName = new Label
            {
                Text = "Item Name:",
                Location = new Point(20, startY),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            txtItemName = new TextBox
            {
                Location = new Point(120, startY),
                Width = 220,
                Font = new Font("Arial", 10)
            };

            Label lblQuantity = new Label
            {
                Text = "Quantity:",
                Location = new Point(20, startY + 40),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            txtQuantity = new TextBox
            {
                Location = new Point(120, startY + 40),
                Width = 100,
                Font = new Font("Arial", 10)
            };

            Label lblCategory = new Label
            {
                Text = "Category:",
                Location = new Point(20, startY + 80),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            cboCategory = new ComboBox
            {
                Location = new Point(120, startY + 80),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10)
            };
            cboCategory.Items.AddRange(categories);
            cboCategory.SelectedIndex = 0;

            Label lblUnit = new Label
            {
                Text = "Unit:",
                Location = new Point(20, startY + 120),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };

            cboUnit = new ComboBox
            {
                Location = new Point(120, startY + 120),
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Arial", 10)
            };

            // Add event handler for category selection change
            cboCategory.SelectedIndexChanged += (s, e) =>
            {
                UpdateUnitOptions();
            };

            // Initialize unit options based on default category
            UpdateUnitOptions();

            // Create buttons
            Button btnSave = new Button
            {
                Text = "Save",
                Location = new Point(120, startY + 170),
                Width = 100,
                Height = 35,
                BackColor = Color.FromArgb(82, 110, 72),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10)
            };
            btnSave.Click += BtnSave_Click;
            btnSave.FlatAppearance.BorderSize = 0;

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(240, startY + 170),
                Width = 100,
                Height = 35,
                BackColor = Color.FromArgb(102, 67, 67),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel,
                Font = new Font("Arial", 10)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                titlePanel,
                lblName, txtItemName,
                lblQuantity, txtQuantity,
                lblCategory, cboCategory,
                lblUnit, cboUnit,
                btnSave, btnCancel
            });
        }

        private void UpdateUnitOptions()
        {
            cboUnit.Items.Clear();
            string selectedCategory = cboCategory.SelectedItem.ToString();
            if (categoryUnits.ContainsKey(selectedCategory))
            {
                cboUnit.Items.AddRange(categoryUnits[selectedCategory]);
                cboUnit.SelectedIndex = 0;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    var newItem = new InventoryItem
                    {
                        Name = txtItemName.Text.Trim(),
                        Quantity = int.Parse(txtQuantity.Text),
                        Category = cboCategory.SelectedItem.ToString(),
                        Unit = cboUnit.SelectedItem.ToString()
                    };

                    dbManager.AddNewInventoryItem(newItem);
                    MessageBox.Show("Item added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding item: {ex.Message}", "Error",
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

            if (cboCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCategory.Focus();
                return false;
            }

            if (cboUnit.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a unit.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboUnit.Focus();
                return false;
            }

            return true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Draw border
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                Color.FromArgb(74, 44, 42), 2, ButtonBorderStyle.Solid,
                Color.FromArgb(74, 44, 42), 2, ButtonBorderStyle.Solid,
                Color.FromArgb(74, 44, 42), 2, ButtonBorderStyle.Solid,
                Color.FromArgb(74, 44, 42), 2, ButtonBorderStyle.Solid);
        }
    }
}