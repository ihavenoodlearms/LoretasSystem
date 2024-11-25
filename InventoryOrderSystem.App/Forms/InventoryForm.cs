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
        private bool mouseDown;
        private Point lastLocation;
        //private bool isAddingNew = false;

        public InventoryForm(User user)
        {
            InitializeComponent();

            // Form properties
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Database and user initialization
            dbManager = new DatabaseManager();
            _currentUser = user;

            // Style the buttons
            ConfigureButtons();

            // Initialize components
            SetupDataGridView();
            InitializeTitleBar();
            //UpdateMainFormLayout();

            // Wire up events
            inventoryGridView.CellDoubleClick += inventoryGridView_CellDoubleClick;
            inventoryGridView.SelectionChanged += inventoryGridView_SelectionChanged;

            // Load initial data
            LoadInventory();
        }

        private void SetupDataGridView()
        {
            inventoryGridView.AllowUserToOrderColumns = true;
            inventoryGridView.AllowUserToResizeColumns = true;
            inventoryGridView.MultiSelect = false;
            inventoryGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void ConfigureButtons()
        {
            // Style Add button
            btnAdd.BackColor = Color.FromArgb(82, 110, 72);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Text = "Add New Item";
            btnAdd.Size = new Size(180, 35);

            // Style Update button
            btnUpdate.BackColor = Color.FromArgb(82, 110, 72);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Enabled = false;
            btnUpdate.Text = "Edit Item";
            btnUpdate.Size = new Size(180, 35);

            // Style Delete button
            btnDelete.BackColor = Color.FromArgb(102, 67, 67);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Enabled = false;
            btnDelete.Text = "Delete Item";
            btnDelete.Size = new Size(180, 35);

            // Style Restock button
            btnRestock.BackColor = Color.FromArgb(82, 110, 72);
            btnRestock.ForeColor = Color.White;
            btnRestock.FlatStyle = FlatStyle.Flat;
            btnRestock.FlatAppearance.BorderSize = 0;
            btnRestock.Enabled = false;
            btnRestock.Text = "Restock";
            btnRestock.Size = new Size(180, 35);

            // Disable restock quantity textbox initially
            txtRestockQuantity.Enabled = false;
        }

        private void ConfigureButton(Button button, string text, bool isDelete = false)
        {
            button.Text = text;
            button.BackColor = isDelete ? Color.FromArgb(102, 67, 67) : Color.FromArgb(82, 110, 72);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Width = 150;
            button.Height = 35;
        }

        private void ClearInputFields()
        {
            txtItemId.Clear();
            txtRestockQuantity.Clear();
        }

        private void InitializeTitleBar()
        {
            Panel titleBar = new Panel
            {
                BackColor = Color.FromArgb(74, 44, 42),
                Dock = DockStyle.Top,
                Height = 30
            };

            titleBar.MouseDown += (s, e) =>
            {
                mouseDown = true;
                lastLocation = e.Location;
            };

            titleBar.MouseMove += (s, e) =>
            {
                if (mouseDown)
                {
                    this.Location = new Point(
                        (this.Location.X - lastLocation.X) + e.X,
                        (this.Location.Y - lastLocation.Y) + e.Y);
                    this.Update();
                }
            };

            titleBar.MouseUp += (s, e) =>
            {
                mouseDown = false;
            };

            Label titleLabel = new Label
            {
                Text = "Inventory",
                ForeColor = Color.White,
                Font = new Font("Arial Rounded MT Bold", 12, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(10, 5)
            };

            titleBar.Controls.Add(titleLabel);
            this.Controls.Add(titleBar);

            // Adjust other controls to appear below title bar
            foreach (Control control in this.Controls)
            {
                if (control != titleBar)
                {
                    control.Top += titleBar.Height;
                }
            }
        }

        /*private void UpdateMainFormLayout()
        {
            // Configure main DataGridView
            inventoryGridView.Dock = DockStyle.None;
            inventoryGridView.Location = new Point(12, 60);
            inventoryGridView.Width = this.ClientSize.Width - 200; // Reduced width to make room for right panel
            inventoryGridView.Height = this.ClientSize.Height - 80;
            inventoryGridView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            // Create and configure right side panel for all controls
            Panel rightPanel = new Panel
            {
                Width = 180,
                Dock = DockStyle.Right,
                BackColor = Color.FromArgb(102, 67, 67),
                Padding = new Padding(10)
            };

            // Configure controls layout in right panel
            Label lblTitle = new Label
            {
                Text = "Controls",
                ForeColor = Color.White,
                Font = new Font("Arial Rounded MT Bold", 14, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(10, 20)
            };

            // Position buttons vertically in the panel
            btnAdd.Location = new Point(15, 60);
            btnUpdate.Location = new Point(15, 105);
            btnDelete.Location = new Point(15, 150);

            // Position restock controls
            Label lblRestock = new Label
            {
                Text = "Restock",
                ForeColor = Color.White,
                Font = new Font("Arial Rounded MT Bold", 12),
                AutoSize = true,
                Location = new Point(15, 215)
            };

            Label lblQuantity = new Label
            {
                Text = "Quantity:",
                ForeColor = Color.White,
                Font = new Font("Arial Rounded MT Bold", 10),
                AutoSize = true,
                Location = new Point(15, 245)
            };

            txtRestockQuantity.Location = new Point(15, 270);
            txtRestockQuantity.Width = 150;

            btnRestock.Location = new Point(15, 305);

            // Add all controls to right panel
            rightPanel.Controls.AddRange(new Control[] {
            lblTitle,
            btnAdd,
            btnUpdate,
            btnDelete,
            lblRestock,
            lblQuantity,
            txtRestockQuantity,
            btnRestock
        });

            // Add the right panel to form
            this.Controls.Add(rightPanel);

            // Make sure the panel is on top of other controls
            rightPanel.BringToFront();
        } */

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
            var sortableList = new SortableBindingList<InventoryItem>(items);
            inventoryGridView.DataSource = sortableList;

            // Configure DataGridView properties
            inventoryGridView.ReadOnly = true;
            inventoryGridView.AllowUserToAddRows = false;
            inventoryGridView.AllowUserToDeleteRows = false;
            inventoryGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Configure columns
            inventoryGridView.Columns["ItemId"].HeaderText = "Item ID";
            inventoryGridView.Columns["Name"].HeaderText = "Item Name";
            inventoryGridView.Columns["Quantity"].HeaderText = "Quantity";
            inventoryGridView.Columns["Category"].HeaderText = "Category";

            // Set column properties
            inventoryGridView.Columns["ItemId"].ReadOnly = true;
            inventoryGridView.Columns["Name"].ReadOnly = true;
            inventoryGridView.Columns["Quantity"].ReadOnly = true;
            inventoryGridView.Columns["Category"].ReadOnly = true;

            // Apply initial sort
            inventoryGridView.Sort(inventoryGridView.Columns["ItemId"], ListSortDirection.Ascending);

            // Setup column click handler
            inventoryGridView.ColumnHeaderMouseClick -= ColumnHeader_Click;
            inventoryGridView.ColumnHeaderMouseClick += ColumnHeader_Click;

            inventoryGridView.Refresh();
        }

        private void ColumnHeader_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn newColumn = inventoryGridView.Columns[e.ColumnIndex];
            DataGridViewColumn oldColumn = inventoryGridView.SortedColumn;
            ListSortDirection direction;

            if (oldColumn != null)
            {
                if (oldColumn == newColumn && inventoryGridView.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    direction = ListSortDirection.Ascending;
                    oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

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

                // Enable buttons and restock functionality
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                txtRestockQuantity.Enabled = true;
                btnRestock.Enabled = true;
            }
            else
            {
                txtItemId.Clear();
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                txtRestockQuantity.Enabled = false;
                btnRestock.Enabled = false;
            }
        }

        // Add this method to handle form state clearing
        private void ResetFormState()
        {
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnRestock.Enabled = false;
            txtRestockQuantity.Enabled = false;
            ClearInputFields();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddInventoryForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    LoadInventory();
                    ResetFormState();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (inventoryGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to edit.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedItem = (InventoryItem)inventoryGridView.SelectedRows[0].DataBoundItem;

            using (var editForm = new EditInventoryForm(selectedItem))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadInventory();
                    ResetFormState();
                }
            }
        }

        private void inventoryGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedItem = (InventoryItem)inventoryGridView.Rows[e.RowIndex].DataBoundItem;
                using (var editForm = new EditInventoryForm(selectedItem))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadInventory();
                        ClearInputFields();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (inventoryGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to delete.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedItem = (InventoryItem)inventoryGridView.SelectedRows[0].DataBoundItem;

            try
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete the item:\n\n{selectedItem.Name}?\n\nThis action cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    // Show password confirmation dialog
                    using (var passwordForm = new Form())
                    {
                        passwordForm.Text = "Security Verification";
                        passwordForm.Size = new Size(300, 180);
                        passwordForm.StartPosition = FormStartPosition.CenterParent;
                        passwordForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                        passwordForm.MaximizeBox = false;
                        passwordForm.MinimizeBox = false;
                        passwordForm.BackColor = Color.FromArgb(255, 248, 240);

                        Label infoLabel = new Label()
                        {
                            Left = 20,
                            Top = 20,
                            Width = 260,
                            Text = "Please enter your password to confirm deletion:",
                            AutoSize = false
                        };

                        TextBox passwordBox = new TextBox()
                        {
                            Left = 20,
                            Top = 50,
                            Width = 240,
                            PasswordChar = '•'
                        };

                        Button confirmButton = new Button()
                        {
                            Text = "Confirm Delete",
                            Left = 85,
                            Top = 90,
                            Width = 120,
                            DialogResult = DialogResult.OK,
                            BackColor = Color.FromArgb(82, 110, 72),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat
                        };
                        confirmButton.FlatAppearance.BorderSize = 0;

                        passwordForm.Controls.AddRange(new Control[] { infoLabel, passwordBox, confirmButton });
                        passwordForm.AcceptButton = confirmButton;

                        if (passwordForm.ShowDialog(this) == DialogResult.OK)
                        {
                            if (dbManager.VerifyPassword(_currentUser.UserId, passwordBox.Text))
                            {
                                dbManager.DeleteInventoryItem(selectedItem.ItemId);
                                MessageBox.Show("Item deleted successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadInventory();
                                ClearInputFields();
                            }
                            else
                            {
                                MessageBox.Show("Incorrect password. Item deletion canceled.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting item: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestock_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtItemId.Text) || string.IsNullOrEmpty(txtRestockQuantity.Text))
            {
                MessageBox.Show("Please select an item and enter a restock quantity.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int itemId = int.Parse(txtItemId.Text);
                int restockQuantity = int.Parse(txtRestockQuantity.Text);

                dbManager.RestockInventoryItem(itemId, restockQuantity);
                MessageBox.Show("Item restocked successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadInventory();
                txtRestockQuantity.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error restocking item: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void panelTitle_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void panelTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X,
                    (this.Location.Y - lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        private void panelTitle_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DashboardForm(_currentUser).Show();
        }
    }
}