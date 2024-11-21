using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using InventoryOrderSystem;
using InventoryOrderSystem.Models;

namespace InventoryOrderSystem.App.Forms
{
    public partial class ProductManagementForm : Form
    {
        private User _currentUser;
        private Dictionary<string, List<Product>> _categoryProducts;
        private string currentCategory;

        public ProductManagementForm(User user, Dictionary<string, List<Product>> categoryProducts)
        {
            InitializeComponent();
            _currentUser = user;
            _categoryProducts = DeepCopyCategoryProducts(categoryProducts);
            _categoryProducts = categoryProducts;
            SetupForm();
        }

        private Dictionary<string, List<Product>> DeepCopyCategoryProducts(Dictionary<string, List<Product>> original)
        {
            var copy = new Dictionary<string, List<Product>>();
            foreach (var kvp in original)
            {
                copy[kvp.Key] = kvp.Value.Select(p => new Product(p.Name, p.Price, p.ItemId)).ToList();
            }
            return copy;
        }

        public Dictionary<string, List<Product>> UpdatedCategoryProducts => _categoryProducts;

        private void SetupForm()
        {
            // Setup DataGridView columns
            dgvProducts.Columns.Clear();
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Product Name",
                Width = 200
            });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Price",
                HeaderText = "Price",
                Width = 100
            });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ItemId",
                HeaderText = "Item ID",
                Width = 100
            });

            // Add categories to combobox
            cmbCategories.Items.Clear();
            foreach (var category in _categoryProducts.Keys)
            {
                cmbCategories.Items.Add(category);
            }

            if (cmbCategories.Items.Count > 0)
            {
                cmbCategories.SelectedIndex = 0;
                currentCategory = cmbCategories.Items[0].ToString();
            }

            // Add event handlers
            cmbCategories.SelectedIndexChanged += (s, e) =>
            {
                currentCategory = cmbCategories.SelectedItem.ToString();
                LoadProductsForCategory();
            };

            btnAdd.Click += (s, e) => AddProduct();
            btnEdit.Click += (s, e) => EditProduct();
            btnDelete.Click += (s, e) => DeleteProduct();
            btnSave.Click += (s, e) => SaveChanges();

            // Initial load
            LoadProductsForCategory();
        }

        private void LoadProductsForCategory()
        {
            dgvProducts.Rows.Clear();
            if (currentCategory != null && _categoryProducts.ContainsKey(currentCategory))
            {
                foreach (var product in _categoryProducts[currentCategory])
                {
                    dgvProducts.Rows.Add(
                        product.Name,
                        product.Price.ToString("₱#,##0.00"),
                        product.ItemId
                    );
                }
            }
        }

        private void AddProduct()
        {
            using (var addForm = new AddProductForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Generate new ID
                        int maxId = 1;
                        if (_categoryProducts.Any())
                        {
                            maxId = _categoryProducts.Values
                                .SelectMany(x => x)
                                .Max(p => p.ItemId) + 1;
                        }

                        // Create new product
                        var newProduct = new Product(
                            addForm.ProductName,
                            addForm.ProductPrice,
                            maxId
                        );

                        // Add to category products
                        if (!_categoryProducts[currentCategory].Any(p => p.Name == newProduct.Name))
                        {
                            _categoryProducts[currentCategory].Add(newProduct);

                            // Add to product catalog
                            if (!ProductCatalog.Products.ContainsKey(newProduct.Name))
                            {
                                ProductCatalog.AddProduct(newProduct);
                            }

                            LoadProductsForCategory();
                            MessageBox.Show("Product added successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("A product with this name already exists.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding product: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EditProduct()
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to edit.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvProducts.SelectedRows[0];
            string productName = row.Cells["Name"].Value.ToString();
            decimal currentPrice = decimal.Parse(row.Cells["Price"].Value.ToString().Replace("₱", "").Trim());

            using (var editForm = new AddProductForm(productName, currentPrice))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Update in category products
                        var categoryProduct = _categoryProducts[currentCategory]
                            .FirstOrDefault(p => p.Name == productName);
                        if (categoryProduct != null)
                        {
                            categoryProduct.Price = editForm.ProductPrice;
                        }

                        // Update in product catalog
                        var catalogProduct = ProductCatalog.GetProduct(productName);
                        if (catalogProduct != null)
                        {
                            catalogProduct.Price = editForm.ProductPrice;
                        }

                        LoadProductsForCategory();
                        MessageBox.Show("Product updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating product: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DeleteProduct()
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvProducts.SelectedRows[0];
            string productName = row.Cells["Name"].Value.ToString();

            if (MessageBox.Show($"Are you sure you want to delete {productName}?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Remove from category products
                    _categoryProducts[currentCategory].RemoveAll(p => p.Name == productName);

                    // Remove from product catalog
                    ProductCatalog.DeleteProduct(productName);

                    LoadProductsForCategory();
                    MessageBox.Show("Product deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting product: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveChanges()
        {
            try
            {
                // Here you might want to add code to persist changes to a database
                MessageBox.Show("All changes have been saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}