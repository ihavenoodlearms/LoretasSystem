using System;
using System.Windows.Forms;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;

namespace InventoryOrderSystem.App.Forms
{
    public partial class InventoryForm : Form
    {
        private readonly DatabaseManager dbManager;

        public InventoryForm()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }

        private void InventoryForm_Load(object sender, EventArgs e)
        {
            LoadInventory();
            CheckLowInventory();
        }

        private void LoadInventory()
        {
            inventoryGridView.DataSource = dbManager.GetAllInventoryItems();
            inventoryGridView.Refresh();
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
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var newItem = new InventoryItem
                {
                    Name = txtItemName.Text,
                    Quantity = int.Parse(txtQuantity.Text)
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
                    Quantity = int.Parse(txtQuantity.Text)
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
                    dbManager.DeleteInventoryItem(itemId);
                    MessageBox.Show("Item deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadInventory();
                    ClearInputFields();
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
            txtRestockQuantity.Clear();
        }
    }
}