using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using InventoryOrderSystem.Models;
using InventoryOrderSystem;

namespace InventoryOrderingSystem
{
    public partial class ProductManagementForm : Form
    {
        private User _currentUser;
        private DataGridView dgvProducts;
        private Button btnAdd, btnEdit, btnDelete, btnSave;
        private ComboBox cmbCategories;
        private Dictionary<string, List<Product>> _categoryProducts;
        private int _nextProductId;

        public ProductManagementForm(User user, Dictionary<string, List<Product>> categoryProducts)
        {
            _currentUser = user;
            _categoryProducts = categoryProducts;
            _nextProductId = ProductCatalog.Products.Values.Max(p => p.ItemId) + 1;
            InitializeProductManagement();
        }

        private void InitializeProductManagement()
        {
            this.Text = "Product Management";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create controls
            cmbCategories = new ComboBox
            {
                Location = new Point(20, 20),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategories.Items.AddRange(_categoryProducts.Keys.ToArray());
            cmbCategories.SelectedIndexChanged += CmbCategories_SelectedIndexChanged;

            dgvProducts = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(740, 400),
                AllowUserToAddRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvProducts.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Product Name", Width = 300 },
                new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Price", Width = 100 },
                new DataGridViewTextBoxColumn { Name = "ItemId", HeaderText = "Item ID", Width = 100 }
            });

            // Create buttons
            btnAdd = CreateButton("Add Product", 20, 480);
            btnEdit = CreateButton("Edit Product", 170, 480);
            btnDelete = CreateButton("Delete Product", 320, 480);
            btnSave = CreateButton("Save Changes", 470, 480);

            // Add event handlers
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            btnSave.Click += BtnSave_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[] { cmbCategories, dgvProducts, btnAdd, btnEdit, btnDelete, btnSave });

            if (cmbCategories.Items.Count > 0)
                cmbCategories.SelectedIndex = 0;
        }

        private Button CreateButton(string text, int x, int y)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(140, 30),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
        }

        private void CmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProductsForCategory(cmbCategories.SelectedItem.ToString());
        }

        private void LoadProductsForCategory(string category)
        {
            dgvProducts.Rows.Clear();
            if (_categoryProducts.ContainsKey(category))
            {
                foreach (var product in _categoryProducts[category])
                {
                    var catalogProduct = ProductCatalog.GetProduct(product.Name);
                    if (catalogProduct != null)
                    {
                        dgvProducts.Rows.Add(product.Name, product.Price,
                            catalogProduct.ItemId);
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!_currentUser.IsSuperAdmin)
            {
                MessageBox.Show("Only administrators can add products.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var addForm = new ProductEditForm(null, cmbCategories.SelectedItem.ToString()))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    string category = cmbCategories.SelectedItem.ToString();
                    // Create new product using the InventoryOrderSystem.Product constructor
                    var newProduct = new InventoryOrderSystem.Product(
                        addForm.EnteredProductName,
                        addForm.ProductPrice,
                        _nextProductId++
                    );

                    // Add to product catalog
                    ProductCatalog.Products[newProduct.Name] = newProduct;

                    // Add to category products
                    _categoryProducts[category].Add(newProduct);

                    LoadProductsForCategory(category);
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (!_currentUser.IsSuperAdmin)
            {
                MessageBox.Show("Only administrators can edit products.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvProducts.SelectedRows.Count == 0) return;

            var row = dgvProducts.SelectedRows[0];
            var productName = row.Cells["Name"].Value.ToString();
            var existingProduct = ProductCatalog.GetProduct(productName);

            using (var editForm = new ProductEditForm(existingProduct,
                cmbCategories.SelectedItem.ToString()))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    string category = cmbCategories.SelectedItem.ToString();

                    // Update in ProductCatalog
                    if (ProductCatalog.Products.ContainsKey(productName))
                    {
                        ProductCatalog.Products[productName].Price = editForm.ProductPrice;
                    }

                    // Update in category products
                    var categoryProduct = _categoryProducts[category]
                        .Find(p => p.Name == productName);
                    if (categoryProduct != null)
                    {
                        categoryProduct.Price = editForm.ProductPrice;
                    }

                    LoadProductsForCategory(category);
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (!_currentUser.IsSuperAdmin)
            {
                MessageBox.Show("Only administrators can delete products.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvProducts.SelectedRows.Count == 0) return;

            var result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var row = dgvProducts.SelectedRows[0];
                string productName = row.Cells["Name"].Value.ToString();
                string category = cmbCategories.SelectedItem.ToString();

                // Remove from ProductCatalog
                ProductCatalog.Products.Remove(productName);

                // Remove from category products
                _categoryProducts[category].RemoveAll(p => p.Name == productName);

                LoadProductsForCategory(category);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Since ProductCatalog is static, changes are already saved in memory
            MessageBox.Show("All changes have been saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    public class ProductEditForm : Form
    {
        private TextBox txtName, txtPrice;
        private Button btnSave, btnCancel;
        private string category;

        public string EnteredProductName => txtName.Text;  // Changed from ProductName
        public decimal ProductPrice { get; private set; }

        public ProductEditForm(InventoryOrderSystem.Product product, string category)
        {
            this.category = category;
            InitializeComponent();
            if (product != null)
            {
                txtName.Text = product.Name;
                txtPrice.Text = product.Price.ToString();
                txtName.Enabled = false;  // Don't allow name editing for existing products
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Product";

            Label lblName = new Label { Text = "Product Name:", Location = new Point(20, 20) };
            Label lblPrice = new Label { Text = "Price:", Location = new Point(20, 60) };

            txtName = new TextBox { Location = new Point(120, 20), Size = new Size(150, 20) };
            txtPrice = new TextBox { Location = new Point(120, 60), Size = new Size(150, 20) };

            btnSave = new Button
            {
                Text = "Save",
                DialogResult = DialogResult.OK,
                Location = new Point(70, 100),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(160, 100),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(140, 105, 84),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnSave.Click += BtnSave_Click;

            this.Controls.AddRange(new Control[] { lblName, lblPrice, txtName, txtPrice, btnSave, btnCancel });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            ProductPrice = price;
        }
    }
}