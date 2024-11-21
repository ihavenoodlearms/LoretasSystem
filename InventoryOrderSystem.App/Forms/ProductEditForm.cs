using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using InventoryOrderSystem;

namespace InventoryOrderSystem.App.Forms
{
    public partial class ProductEditForm : Form
    {
        private string category;

        public string EnteredProductName => txtName.Text;
        public decimal ProductPrice { get; private set; }

        // Constructor for new product
        public ProductEditForm(string category)
            : this(null, category) { }

        // Constructor for existing product
        public ProductEditForm(Product product, string category)
        {
            InitializeComponent();
            this.category = category;

            if (product != null)
            {
                txtName.Text = product.Name;
                txtPrice.Text = product.Price.ToString();
                txtName.Enabled = false;  // Don't allow name editing for existing products
                this.Text = "Edit Product";
            }
            else
            {
                this.Text = "Add New Product";
                txtName.Enabled = true;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a product name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                txtName.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price greater than 0.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                txtPrice.Focus();
                return;
            }

            // Set the price property if validation passes
            ProductPrice = price;
        }
    }
}