using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryOrderSystem.App.Forms
{
    public partial class AddProductForm : Form
    {
        public string EnteredProductName => txtName.Text;
        public decimal ProductPrice { get; private set; }

        public AddProductForm(string name = "", decimal price = 0)
        {
            InitializeComponent();

            // Set values if editing existing product
            if (!string.IsNullOrEmpty(name))
            {
                this.Text = "Edit Product";
                txtName.Text = name;
                txtName.ReadOnly = true;
                txtPrice.Text = price.ToString();
            }
            else
            {
                this.Text = "Add New Product";
                txtName.ReadOnly = false;
            }

            // Add validation event handlers
            txtPrice.KeyPress += TxtPrice_KeyPress;
            txtName.TextChanged += TxtName_TextChanged;
        }

        private void TxtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers, decimal point, and control characters
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && txtPrice.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a product name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (price <= 0)
            {
                MessageBox.Show("Price must be greater than zero.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            ProductPrice = price;
            this.DialogResult = DialogResult.OK;
        }
    }
}
