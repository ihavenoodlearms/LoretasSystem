﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using InventoryOrderingSystem;
using InventoryOrderSystem.App.Forms;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;
using InventoryOrderSystem.Utils;

namespace InventoryOrderSystem.Forms
{
    public partial class OrderForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private List<Order> _orders;
        private SortableBindingList<OrderViewModel> _bindingOrders;
        private readonly User _currentUser;
        private OrderMenuForm _orderMenuForm;

        public OrderForm(User user)
        {
            InitializeComponent();
            _dbManager = new DatabaseManager();
            _currentUser = user;
            SetupEventHandlers();

        }

        private void SetupEventHandlers()
        {
            // Wire up the button click events
            btnNewOrder.Click += btnNewOrder_Click;
            btnViewOrder.Click += btnViewOrder_Click;
            btnVoidOrder.Click += btnVoidOrder_Click_1;
            btnPayOrder.Click += BtnPayOrder_Click;
            btnBack.Click += btnBack_Click;
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            // Get all orders from database
            _orders = _dbManager.GetAllOrders();

            // Filter orders to show only active ones
            var activeOrders = _orders.Where(o => o.Status != "Paid" && o.Status != "Voided");

            var orderViewModels = activeOrders.Select(o => new OrderViewModel
            {
                OrderId = o.OrderId,
                transaction_id = _dbManager.GetTransactionId(o.OrderId),
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                PaymentMethod = o.PaymentMethod,
                OrderItemsCount = o.OrderItems.Count,
                Status = o.Status,
                AmountPaid = o.AmountPaid,
                ChangeAmount = o.ChangeAmount
            }).ToList();

            _bindingOrders = new SortableBindingList<OrderViewModel>(orderViewModels);
            dgvOrders.DataSource = _bindingOrders;
            FormatDataGridView();
        }

        // Add a method to refresh the grid after payment or void operations
        private void RefreshOrderGrid()
        {
            LoadOrders();
        }


        private void FormatDataGridView()
        {
            dgvOrders.Columns["OrderId"].Visible = false;
            dgvOrders.Columns["transaction_id"].HeaderText = "Transaction ID";
            dgvOrders.Columns["UserId"].HeaderText = "User ID";
            dgvOrders.Columns["OrderDate"].HeaderText = "Order Date";
            dgvOrders.Columns["TotalAmount"].HeaderText = "Total Amount";
            dgvOrders.Columns["PaymentMethod"].HeaderText = "Payment Method";
            dgvOrders.Columns["OrderItemsCount"].HeaderText = "Items Count";
            dgvOrders.Columns["Status"].HeaderText = "Status";
            dgvOrders.Columns["AmountPaid"].HeaderText = "Amount Paid";
            dgvOrders.Columns["ChangeAmount"].HeaderText = "Change Amount";

            // Format currency columns
            dgvOrders.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
            dgvOrders.Columns["AmountPaid"].DefaultCellStyle.Format = "C2";
            dgvOrders.Columns["ChangeAmount"].DefaultCellStyle.Format = "C2";

            // Make DataGridView fill the space
            dgvOrders.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set preferred column widths (adjusted for transaction_id)
            dgvOrders.Columns["transaction_id"].FillWeight = 15;
            dgvOrders.Columns["UserId"].FillWeight = 8;
            dgvOrders.Columns["OrderDate"].FillWeight = 15;
            dgvOrders.Columns["TotalAmount"].FillWeight = 12;
            dgvOrders.Columns["PaymentMethod"].FillWeight = 12;
            dgvOrders.Columns["OrderItemsCount"].FillWeight = 10;
            dgvOrders.Columns["Status"].FillWeight = 10;
            dgvOrders.Columns["AmountPaid"].FillWeight = 12;
            dgvOrders.Columns["ChangeAmount"].FillWeight = 13;

            // Enable sorting
            dgvOrders.Sort(dgvOrders.Columns["OrderDate"], ListSortDirection.Descending);

            dgvOrders.CellFormatting += DgvOrders_CellFormatting;
        }

        private void InitializeDataGridView()
        {
            // Initialize DataGridView
            dgvOrders = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(dgvOrders)).BeginInit();

            // Basic form settings
            this.Text = "Order Management";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Configure DataGridView
            dgvOrders.Dock = DockStyle.Top;
            dgvOrders.Height = 450;
            dgvOrders.MultiSelect = false;
            dgvOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvOrders.ReadOnly = true;
            dgvOrders.AllowUserToAddRows = false;
            this.Controls.Add(dgvOrders);

            // Create button panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 100
            };
            this.Controls.Add(buttonPanel);

            // Initialize buttons
            Button btnNewOrder = new Button
            {
                Text = "New Order",
                Location = new Point(20, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White
            };
            btnNewOrder.Click += btnNewOrder_Click;

            Button btnViewOrder = new Button
            {
                Text = "View Order",
                Location = new Point(btnNewOrder.Right + 10, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White
            };
            btnViewOrder.Click += btnViewOrder_Click;

            Button btnVoidOrder = new Button
            {
                Text = "Void Order",
                Location = new Point(btnViewOrder.Right + 10, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White
            };
            btnVoidOrder.Click += btnVoidOrder_Click_1;

            Button btnPayOrder = new Button
            {
                Text = "Process Payment",
                Location = new Point(btnVoidOrder.Right + 10, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White
            };
            btnPayOrder.Click += BtnPayOrder_Click;

            // Add the View History button
            Button btnViewHistory = new Button
            {
                Text = "View History",
                Location = new Point(btnPayOrder.Right + 10, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White
            };
            btnViewHistory.Click += btnViewHistory_Click;

            Button btnBack = new Button
            {
                Text = "Back",
                Location = new Point(btnViewHistory.Right + 10, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White
            };
            btnBack.Click += btnBack_Click;

            Button btnEditOrder = new Button
            {
                Text = "Edit Order",
                Location = new Point(btnViewOrder.Right + 10, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White
            };
            btnEditOrder.Click += btnEditOrder_Click;

            Button btnDeleteOrder = new Button
            {
                Text = "Delete Order",
                Location = new Point(btnEditOrder.Right + 10, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White
            };
            btnDeleteOrder.Click += btnDeleteOrder_Click;

            buttonPanel.Controls.AddRange(new Control[] {
    btnNewOrder, btnViewOrder, btnEditOrder, btnDeleteOrder, btnVoidOrder, btnPayOrder, btnViewHistory, btnBack
});

            ((System.ComponentModel.ISupportInitialize)(dgvOrders)).EndInit();
        }

        // Add these new methods to OrderForm.cs
        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to edit.", "No Order Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvOrders.SelectedRows[0];
            string currentStatus = selectedRow.Cells["Status"].Value?.ToString();
            int orderId = Convert.ToInt32(selectedRow.Cells["OrderId"].Value);

            if (currentStatus == "Paid" || currentStatus == "Voided")
            {
                MessageBox.Show("Cannot edit a paid or voided order.", "Invalid Operation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the original order
            Order originalOrder = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (originalOrder == null)
            {
                MessageBox.Show("Order not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verify password before editing
            using (var passwordForm = new Form())
            {
                passwordForm.Text = "Enter Password";
                passwordForm.Size = new Size(300, 150);
                passwordForm.StartPosition = FormStartPosition.CenterParent;
                passwordForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                passwordForm.MaximizeBox = false;
                passwordForm.MinimizeBox = false;

                Label passwordLabel = new Label()
                {
                    Left = 20,
                    Top = 20,
                    Text = "Enter your password:",
                    AutoSize = true
                };

                TextBox passwordBox = new TextBox()
                {
                    Left = 20,
                    Top = 50,
                    Width = 240,
                    PasswordChar = '*'
                };

                Button confirmButton = new Button()
                {
                    Text = "Confirm",
                    Left = 100,
                    Top = 80,
                    DialogResult = DialogResult.OK
                };

                confirmButton.Click += (s, evt) => { passwordForm.Close(); };

                passwordForm.Controls.Add(passwordLabel);
                passwordForm.Controls.Add(passwordBox);
                passwordForm.Controls.Add(confirmButton);
                passwordForm.AcceptButton = confirmButton;

                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    if (_dbManager.VerifyPassword(_currentUser.UserId, passwordBox.Text))
                    {
                        // Create new OrderMenuForm for editing
                        var orderMenuForm = new OrderMenuForm(_currentUser);
                        orderMenuForm.LoadExistingOrder(originalOrder);

                        // Add FormClosed event handler similar to new order
                        orderMenuForm.FormClosed += (s, args) => this.Show();

                        orderMenuForm.OrderPlaced += (s, newOrder) =>
                        {
                            try
                            {
                                // Delete the old order
                                _dbManager.DeleteOrder(orderId);

                                // Add the new order
                                _dbManager.AddOrder(newOrder);

                                RefreshOrderGrid();
                                MessageBox.Show("Order updated successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Close the OrderMenuForm and show OrderForm
                                if (s is OrderMenuForm form)
                                {
                                    form.Close();
                                }
                                this.Show();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error updating order: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };

                        this.Hide();
                        orderMenuForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect password.", "Authentication Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

            private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to delete.", "No Order Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvOrders.SelectedRows[0];
            string currentStatus = selectedRow.Cells["Status"].Value?.ToString();
            int orderId = Convert.ToInt32(selectedRow.Cells["OrderId"].Value);

            if (currentStatus == "Paid" || currentStatus == "Voided")
            {
                MessageBox.Show("Cannot delete a paid or voided order.", "Invalid Operation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verify password before deleting
            using (var passwordForm = new Form())
            {
                passwordForm.Text = "Enter Password";
                passwordForm.Size = new Size(300, 150);
                passwordForm.StartPosition = FormStartPosition.CenterParent;
                passwordForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                passwordForm.MaximizeBox = false;
                passwordForm.MinimizeBox = false;

                Label passwordLabel = new Label()
                {
                    Left = 20,
                    Top = 20,
                    Text = "Enter your password:",
                    AutoSize = true
                };

                TextBox passwordBox = new TextBox()
                {
                    Left = 20,
                    Top = 50,
                    Width = 240,
                    PasswordChar = '*'
                };

                Button confirmButton = new Button()
                {
                    Text = "Confirm",
                    Left = 100,
                    Top = 80,
                    DialogResult = DialogResult.OK
                };

                confirmButton.Click += (s, evt) => { passwordForm.Close(); };

                passwordForm.Controls.Add(passwordLabel);
                passwordForm.Controls.Add(passwordBox);
                passwordForm.Controls.Add(confirmButton);
                passwordForm.AcceptButton = confirmButton;

                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    if (_dbManager.VerifyPassword(_currentUser.UserId, passwordBox.Text))
                    {
                        DialogResult confirmDelete = MessageBox.Show(
                            "Are you sure you want to permanently delete this order?",
                            "Confirm Delete",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (confirmDelete == DialogResult.Yes)
                        {
                            try
                            {
                                _dbManager.DeleteOrder(orderId);
                                RefreshOrderGrid();
                                MessageBox.Show("Order deleted successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error deleting order: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect password.", "Authentication Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnPayOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to process payment.", "No Order Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvOrders.SelectedRows[0];
            string currentStatus = selectedRow.Cells["Status"].Value?.ToString();

            // Get OrderId directly
            int orderId = Convert.ToInt32(selectedRow.Cells["OrderId"].Value);

            if (currentStatus == "Paid")
            {
                MessageBox.Show("This order has already been paid.", "Already Paid",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (currentStatus == "Voided")
            {
                MessageBox.Show("Cannot process payment for a voided order.", "Order Voided",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalAmount = Convert.ToDecimal(selectedRow.Cells["TotalAmount"].Value);

            using (var paymentForm = new Form())
            {
                paymentForm.Text = "Process Payment";
                paymentForm.Size = new Size(300, 200);
                paymentForm.StartPosition = FormStartPosition.CenterParent;
                paymentForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                paymentForm.MaximizeBox = false;
                paymentForm.MinimizeBox = false;

                Label totalLabel = new Label
                {
                    Text = $"Total Amount: ₱{totalAmount:F2}",
                    Location = new Point(20, 20),
                    AutoSize = true
                };

                Label amountPaidLabel = new Label
                {
                    Text = "Amount Paid:",
                    Location = new Point(20, 50),
                    AutoSize = true
                };

                TextBox amountPaidBox = new TextBox
                {
                    Location = new Point(120, 47),
                    Width = 140
                };

                Label changeLabel = new Label
                {
                    Text = "Change: ₱0.00",
                    Location = new Point(20, 80),
                    AutoSize = true
                };

                amountPaidBox.TextChanged += (s, evt) =>
                {
                    if (decimal.TryParse(amountPaidBox.Text, out decimal amountPaid))
                    {
                        decimal change = amountPaid - totalAmount;
                        changeLabel.Text = $"Change: ₱{Math.Max(0, change):F2}";
                    }
                    else
                    {
                        changeLabel.Text = "Change: ₱0.00";
                    }
                };

                Button confirmButton = new Button
                {
                    Text = "Confirm Payment",
                    Location = new Point(85, 120),
                    Size = new Size(120, 30),
                    DialogResult = DialogResult.None
                };

                confirmButton.Click += (s, evt) =>
                {
                    if (!decimal.TryParse(amountPaidBox.Text, out decimal amountPaid))
                    {
                        MessageBox.Show("Please enter a valid amount.", "Invalid Amount",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (amountPaid < totalAmount)
                    {
                        MessageBox.Show("Amount paid must be greater than or equal to the total amount.",
                            "Insufficient Amount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    paymentForm.DialogResult = DialogResult.OK;
                    paymentForm.Close();
                };

                paymentForm.Controls.AddRange(new Control[] {
            totalLabel, amountPaidLabel, amountPaidBox, changeLabel, confirmButton
        });

                if (paymentForm.ShowDialog() == DialogResult.OK)
                {
                    decimal amountPaid = decimal.Parse(amountPaidBox.Text);
                    decimal change = amountPaid - totalAmount;

                    try
                    {
                        _dbManager.MarkOrderAsPaid(orderId, amountPaid, change);
                        RefreshOrderGrid(); // Use the new refresh method

                        MessageBox.Show($"Payment processed successfully!\nChange: ₱{change:F2}",
                            "Payment Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error processing payment: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvOrders.Rows[e.RowIndex];
                string status = row.Cells["Status"].Value?.ToString();

                if (status == "Voided")
                {
                    row.DefaultCellStyle.BackColor = Color.LightPink;
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                }
                else if (status == "Paid")
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = dgvOrders.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.ForeColor = dgvOrders.DefaultCellStyle.ForeColor;
                }
            }
        }

        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            if (_orderMenuForm == null || _orderMenuForm.IsDisposed)
            {
                _orderMenuForm = new OrderMenuForm(_currentUser);
                _orderMenuForm.OrderPlaced += OrderMenuForm_OrderPlaced;
                _orderMenuForm.FormClosed += (s, args) => this.Show();
            }
            _orderMenuForm.Show();
            this.Hide();
        }

        private void OrderMenuForm_OrderPlaced(object sender, Order newOrder)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object, Order>(OrderMenuForm_OrderPlaced), sender, newOrder);
                return;
            }

            if (newOrder.OrderItems.Count == 0)
            {
                MessageBox.Show("Cannot place an order with no items.", "Empty Order",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_orders.Any(o => o.OrderId == newOrder.OrderId))
            {
                _dbManager.AddOrder(newOrder);
                LoadOrders();
            }

            if (sender is OrderMenuForm orderMenuForm)
            {
                orderMenuForm.Close();
            }

            this.Show();
        }

        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to view.", "No Order Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedRow = dgvOrders.SelectedRows[0];
                var orderId = Convert.ToInt32(selectedRow.Cells["OrderId"].Value);
                var transactionIdCell = selectedRow.Cells["transaction_id"].Value;
                string transactionId = transactionIdCell?.ToString() ?? "Not Generated Yet";

                // Get the order directly using OrderId from the orders list
                Order selectedOrder = _orders.FirstOrDefault(o => o.OrderId == orderId);
                if (selectedOrder != null)
                {
                    MessageBox.Show(GetOrderDetails(selectedOrder, transactionId), "Order Details",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Order details could not be found.",
                        "Order Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing order details: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Updated GetOrderDetails method to handle missing transaction ID
        private string GetOrderDetails(Order order, string transactionId)
        {
            string details = $"Transaction ID: {transactionId}\n" +
                           $"Order ID: {order.OrderId}\n" +  // Added Order ID to display
                           $"User ID: {order.UserId}\n" +
                           $"Order Date: {order.OrderDate}\n" +
                           $"Total Amount: {order.TotalAmount:C}\n" +
                           $"Payment Method: {order.PaymentMethod}\n" +
                           $"Status: {order.Status}\n\n" +
                           "Order Items:\n";

            foreach (var item in order.OrderItems)
            {
                details += $"- {item.ProductName}, Quantity: {item.Quantity}, Price: {item.Price:C}\n";
            }

            // Add total amount summary at the end
            details += $"\nTotal Amount: {order.TotalAmount:C}";

            return details;
        }

        private void btnVoidOrder_Click_1(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to void.", "No Order Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedRow = dgvOrders.SelectedRows[0];
                var orderId = Convert.ToInt32(selectedRow.Cells["OrderId"].Value);
                string currentStatus = selectedRow.Cells["Status"].Value?.ToString();

                // Check if order is already voided or paid
                if (currentStatus == "Voided")
                {
                    MessageBox.Show("This order has already been voided.", "Already Voided",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (currentStatus == "Paid")
                {
                    MessageBox.Show("Cannot void a paid order.", "Order Paid",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                VoidOrderWithConfirmation(orderId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error voiding order: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VoidOrderWithConfirmation(int orderId)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to void this order?",
                "Confirm Void", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (var passwordForm = new Form())
                {
                    passwordForm.Text = "Enter Password";
                    passwordForm.Size = new Size(300, 150);
                    passwordForm.StartPosition = FormStartPosition.CenterParent;
                    passwordForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    passwordForm.MaximizeBox = false;
                    passwordForm.MinimizeBox = false;

                    Label passwordLabel = new Label()
                    {
                        Left = 20,
                        Top = 20,
                        Text = "Enter your password:",
                        AutoSize = true
                    };

                    TextBox passwordBox = new TextBox()
                    {
                        Left = 20,
                        Top = 50,
                        Width = 240,
                        PasswordChar = '*'
                    };

                    Button confirmButton = new Button()
                    {
                        Text = "Confirm",
                        Left = 100,
                        Top = 80,
                        DialogResult = DialogResult.OK
                    };

                    confirmButton.Click += (s, evt) => { passwordForm.Close(); };

                    passwordForm.Controls.Add(passwordLabel);
                    passwordForm.Controls.Add(passwordBox);
                    passwordForm.Controls.Add(confirmButton);

                    passwordForm.AcceptButton = confirmButton;

                    if (passwordForm.ShowDialog(this) == DialogResult.OK)
                    {
                        string enteredPassword = passwordBox.Text;

                        // Verify the password
                        if (_dbManager.VerifyPassword(_currentUser.UserId, enteredPassword))
                        {
                            try
                            {
                                _dbManager.VoidOrder(orderId);
                                RefreshOrderGrid();
                                MessageBox.Show("Order has been voided successfully.", "Order Voided",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"An error occurred while voiding the order: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password. Order voiding canceled.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Order voiding canceled.", "Canceled",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnViewHistory_Click(object sender, EventArgs e)
        {
            // Get all orders including paid and voided
            var allOrders = _orders.Select(o => new OrderViewModel
            {
                OrderId = o.OrderId,
                transaction_id = _dbManager.GetTransactionId(o.OrderId),
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                PaymentMethod = o.PaymentMethod,
                OrderItemsCount = o.OrderItems.Count,
                Status = o.Status,
                AmountPaid = o.AmountPaid,
                ChangeAmount = o.ChangeAmount
            }).ToList();

            using (var historyForm = new Form())
            {
                historyForm.Text = "Order History";
                historyForm.Size = new Size(1000, 600);
                historyForm.StartPosition = FormStartPosition.CenterParent;

                var historyGrid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    MultiSelect = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    BackColor = Color.FromArgb(255, 240, 209),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                };

                // First bind the data
                historyGrid.DataSource = new SortableBindingList<OrderViewModel>(allOrders);

                // Then format after data is bound
                try
                {
                    FormatDataGridViewForHistory(historyGrid);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error formatting grid: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                historyForm.Controls.Add(historyGrid);
                historyForm.ShowDialog();
            }
        }

        private void FormatDataGridViewForHistory(DataGridView gridView)
        {
            if (gridView?.Columns == null || gridView.Columns.Count == 0)
                return;

            try
            {
                // Apply the same formatting as the main grid
                if (gridView.Columns.Contains("OrderId"))
                    gridView.Columns["OrderId"].Visible = false;

                if (gridView.Columns.Contains("transaction_id"))
                {
                    gridView.Columns["transaction_id"].HeaderText = "Transaction ID";
                    gridView.Columns["transaction_id"].FillWeight = 15;
                }

                var columnSettings = new Dictionary<string, (string HeaderText, int FillWeight)>
        {
            { "UserId", ("User ID", 8) },
            { "OrderDate", ("Order Date", 15) },
            { "TotalAmount", ("Total Amount", 12) },
            { "PaymentMethod", ("Payment Method", 12) },
            { "OrderItemsCount", ("Items Count", 10) },
            { "Status", ("Status", 10) },
            { "AmountPaid", ("Amount Paid", 12) },
            { "ChangeAmount", ("Change Amount", 13) }
        };

                foreach (var setting in columnSettings)
                {
                    if (gridView.Columns.Contains(setting.Key))
                    {
                        var column = gridView.Columns[setting.Key];
                        column.HeaderText = setting.Value.HeaderText;
                        column.FillWeight = setting.Value.FillWeight;

                        // Apply currency format to amount columns
                        if (setting.Key == "TotalAmount" || setting.Key == "AmountPaid" || setting.Key == "ChangeAmount")
                        {
                            column.DefaultCellStyle.Format = "C2";
                        }
                    }
                }

                // Add cell formatting for status colors
                gridView.CellFormatting += (sender, e) =>
                {
                    if (e.RowIndex >= 0 && gridView.Columns.Contains("Status"))
                    {
                        DataGridViewRow row = gridView.Rows[e.RowIndex];
                        string status = row.Cells["Status"].Value?.ToString();

                        if (status == "Voided")
                        {
                            row.DefaultCellStyle.BackColor = Color.LightPink;
                            row.DefaultCellStyle.ForeColor = Color.DarkRed;
                        }
                        else if (status == "Paid")
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGreen;
                            row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = gridView.DefaultCellStyle.BackColor;
                            row.DefaultCellStyle.ForeColor = gridView.DefaultCellStyle.ForeColor;
                        }
                    }
                };

                // Sort by OrderDate descending
                if (gridView.Columns.Contains("OrderDate"))
                {
                    gridView.Sort(gridView.Columns["OrderDate"], ListSortDirection.Descending);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error formatting grid: {ex.Message}", ex);
            }
        }

        private void OrderForm_Resize(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Check the role of the current user
            if (_currentUser.Role == "Admin")
            {
                this.Hide();
                DashboardForm adminDashboard = new DashboardForm(_currentUser);
                adminDashboard.Show();
            }
            else if (_currentUser.Role == "User")
            {

                this.Hide();
                UserForm userDashboard = new UserForm(_currentUser);
                userDashboard.Show();
            }
            else
            {
                MessageBox.Show("Unknown role. Please contact the administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            dgvOrders.CellFormatting -= DgvOrders_CellFormatting;
        }
    }
}