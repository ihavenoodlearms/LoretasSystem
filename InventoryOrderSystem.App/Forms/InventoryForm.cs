using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using InventoryOrderSystem.Forms;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;
using InventoryOrderSystem.Utils;

namespace InventoryOrderSystem.App.Forms
{
    public partial class InventoryForm : Form
    {
        private readonly DatabaseManager dbManager;
        private readonly User _currentUser;
        private readonly string[] categories = { "Powder", "Syrup", "Fruit Tea Syrup", "Misc." };

        public InventoryForm(User user)
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            _currentUser = user;

            // Initialize the category combo box
            cboCategory.Items.AddRange(categories);
            cboCategory.SelectedIndex = 0;

            // Enable sorting for the DataGridView
            inventoryGridView.AllowUserToOrderColumns = true;
            inventoryGridView.AllowUserToResizeColumns = true;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DashboardForm(_currentUser).Show();
        }

        private void InventoryForm_Load(object sender, EventArgs e)
        {
            LoadInventory();
            CheckLowInventory();
        }

        private void LoadInventory()
        {
            var items = dbManager.GetAllInventoryItems();

            // Convert the list to a SortableBindingList
            var sortableList = new SortableBindingList<InventoryItem>(items);

            // Set the DataGridView's DataSource to the SortableBindingList
            inventoryGridView.DataSource = sortableList;

            // Configure DataGridView properties
            inventoryGridView.ReadOnly = true;
            inventoryGridView.AllowUserToAddRows = false;
            inventoryGridView.AllowUserToDeleteRows = false;
            inventoryGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Ensure all columns are visible and properly named
            inventoryGridView.Columns["ItemId"].HeaderText = "Item ID";
            inventoryGridView.Columns["Name"].HeaderText = "Item Name";
            inventoryGridView.Columns["Quantity"].HeaderText = "Quantity";
            inventoryGridView.Columns["Category"].HeaderText = "Category";

            // Set column properties
            inventoryGridView.Columns["ItemId"].ReadOnly = true;
            inventoryGridView.Columns["Name"].ReadOnly = true;
            inventoryGridView.Columns["Quantity"].ReadOnly = true;
            inventoryGridView.Columns["Category"].ReadOnly = true;

            // Apply the initial sort here, after the data has been loaded
            inventoryGridView.Sort(inventoryGridView.Columns["ItemId"], ListSortDirection.Ascending);

            // Attach the ColumnHeader_Click method to the ColumnHeaderMouseClick event
            inventoryGridView.ColumnHeaderMouseClick -= ColumnHeader_Click; // Remove any existing handler
            inventoryGridView.ColumnHeaderMouseClick += ColumnHeader_Click; // Add the new handler

            inventoryGridView.Refresh();
        }

        private void ColumnHeader_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = inventoryGridView.Columns[e.ColumnIndex];
            DataGridViewColumn oldColumn = inventoryGridView.SortedColumn;
            ListSortDirection direction;

            // If oldColumn is null, then the DataGridView is not currently sorted.
            if (oldColumn != null)
            {
                // Sort the same column again, reversing the SortOrder.
                if (oldColumn == newColumn &&
                    inventoryGridView.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    // Sort a new column and remove the old SortGlyph.
                    direction = ListSortDirection.Ascending;
                    oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

            // Sort the selected column.
            inventoryGridView.Sort(newColumn, direction);
            newColumn.HeaderCell.SortGlyphDirection =
                direction == ListSortDirection.Ascending ?
                SortOrder.Ascending : SortOrder.Descending;
        }

        private void CheckLowInventory()
        {
            var lowItems = dbManager.GetLowInventoryItems();
            if (lowItems.Count > 0)
            {
                string message = "The following items are low in inventory:\n";
                foreach (var item in lowItems)
                {
                    message += $"{item.Name}: {item.Quantity}\n";
                }
                MessageBox.Show(message, "Low Inventory Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void inventoryGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (inventoryGridView.SelectedRows.Count > 0)
            {
                var selectedItem = (InventoryItem)inventoryGridView.SelectedRows[0].DataBoundItem;
                txtItemId.Text = selectedItem.ItemId.ToString();
                txtItemName.Text = selectedItem.Name;
                txtQuantity.Text = selectedItem.Quantity.ToString();
                cboCategory.SelectedItem = selectedItem.Category;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var newItem = new InventoryItem
                {
                    Name = txtItemName.Text,
                    Quantity = int.Parse(txtQuantity.Text),
                    Category = cboCategory.SelectedItem.ToString()
                };

                dbManager.AddNewInventoryItem(newItem);
                MessageBox.Show("Item added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInventory();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var updatedItem = new InventoryItem
                {
                    ItemId = int.Parse(txtItemId.Text),
                    Name = txtItemName.Text,
                    Quantity = int.Parse(txtQuantity.Text),
                    Category = cboCategory.SelectedItem.ToString()
                };

                dbManager.UpdateInventoryItem(updatedItem);
                MessageBox.Show("Item updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInventory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemId.Text))
            {
                MessageBox.Show("Please select an item to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int itemId = int.Parse(txtItemId.Text);
                DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Create and show password input dialog
                    using (var passwordForm = new Form())
                    {
                        passwordForm.Text = "Enter Password";
                        passwordForm.Size = new Size(300, 150);
                        passwordForm.StartPosition = FormStartPosition.CenterParent;
                        passwordForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                        passwordForm.MaximizeBox = false;
                        passwordForm.MinimizeBox = false;

                        Label passwordLabel = new Label() { Left = 20, Top = 20, Text = "Enter your password:", AutoSize = true };
                        TextBox passwordBox = new TextBox() { Left = 20, Top = 50, Width = 240, PasswordChar = '*'};
                        Button confirmButton = new Button() { Text = "Confirm", Left = 100, Top = 80, DialogResult = DialogResult.OK };

                        confirmButton.Click += (s, evt) => { passwordForm.Close(); };

                        passwordForm.Controls.Add(passwordLabel);
                        passwordForm.Controls.Add(passwordBox);
                        passwordForm.Controls.Add(confirmButton);

                        passwordForm.AcceptButton = confirmButton;

                        if (passwordForm.ShowDialog(this) == DialogResult.OK)
                        {
                            string enteredPassword = passwordBox.Text;

                            // Verify the password
                            if (dbManager.VerifyPassword(_currentUser.UserId, enteredPassword))
                            {
                                dbManager.DeleteInventoryItem(itemId);
                                MessageBox.Show("Item deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadInventory();
                                ClearInputFields();
                            }
                            else
                            {
                                MessageBox.Show("Incorrect password. Item deletion canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Item deletion canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestock_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemId.Text) || string.IsNullOrEmpty(txtRestockQuantity.Text))
            {
                MessageBox.Show("Please select an item and enter a restock quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int itemId = int.Parse(txtItemId.Text);
                int restockQuantity = int.Parse(txtRestockQuantity.Text);

                dbManager.RestockInventoryItem(itemId, restockQuantity);
                MessageBox.Show("Item restocked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInventory();
                txtRestockQuantity.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error restocking item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputFields()
        {
            txtItemId.Clear();
            txtItemName.Clear();
            txtQuantity.Clear();
            cboCategory.SelectedIndex = 0;
            txtRestockQuantity.Clear();
        }
    }
}