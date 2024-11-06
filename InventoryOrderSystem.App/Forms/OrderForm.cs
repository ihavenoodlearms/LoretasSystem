using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using InventoryOrderingSystem;
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
            _orders = _dbManager.GetAllOrders();
            var orderViewModels = _orders.Select(o => new OrderViewModel
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                PaymentMethod = o.PaymentMethod,
                OrderItemsCount = o.OrderItems.Count,
                Status = o.Status,
                AmountPaid = o.AmountPaid,      // Add this
                ChangeAmount = o.ChangeAmount    // Add this
            }).ToList();

            _bindingOrders = new SortableBindingList<OrderViewModel>(orderViewModels);
            dgvOrders.DataSource = _bindingOrders;
            FormatDataGridView();
        }

        private void FormatDataGridView()
        {
            dgvOrders.Columns["OrderId"].HeaderText = "Order ID";
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

            // Set preferred column widths (adjust percentages as needed)
            dgvOrders.Columns["OrderId"].FillWeight = 8;
            dgvOrders.Columns["UserId"].FillWeight = 8;
            dgvOrders.Columns["OrderDate"].FillWeight = 15;
            dgvOrders.Columns["TotalAmount"].FillWeight = 12;
            dgvOrders.Columns["PaymentMethod"].FillWeight = 12;
            dgvOrders.Columns["OrderItemsCount"].FillWeight = 10;
            dgvOrders.Columns["Status"].FillWeight = 10;
            dgvOrders.Columns["AmountPaid"].FillWeight = 12;
            dgvOrders.Columns["ChangeAmount"].FillWeight = 13;

            // Enable sorting
            dgvOrders.Sort(dgvOrders.Columns["OrderId"], ListSortDirection.Descending);

            // Add the CellFormatting event handler
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

            Button btnBack = new Button
            {
                Text = "Back",
                Location = new Point(btnPayOrder.Right + 10, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White
            };
            btnBack.Click += btnBack_Click;

            buttonPanel.Controls.AddRange(new Control[] {
                btnNewOrder, btnViewOrder, btnVoidOrder, btnPayOrder, btnBack
            });

            ((System.ComponentModel.ISupportInitialize)(dgvOrders)).EndInit();
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
            string currentStatus = selectedRow.Cells["Status"].Value.ToString();

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

            int orderId = Convert.ToInt32(selectedRow.Cells["OrderId"].Value);
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
                    DialogResult = DialogResult.OK
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

                        // Update the DataGridView
                        selectedRow.DefaultCellStyle.BackColor = Color.LightGreen;
                        selectedRow.Cells["Status"].Value = "Paid";

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
            if (dgvOrders.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderId"].Value);
                Order selectedOrder = _orders.FirstOrDefault(o => o.OrderId == orderId);
                if (selectedOrder != null)
                {
                    MessageBox.Show(GetOrderDetails(selectedOrder), "Order Details",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select an order to view.", "No Order Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string GetOrderDetails(Order order)
        {
            string details = $"Order ID: {order.OrderId}\n" +
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

            return details;
        }

        private void btnVoidOrder_Click_1(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dgvOrders.SelectedRows[0].Cells["OrderId"].Value);
                VoidOrderWithConfirmation(orderId);
            }
            else
            {
                MessageBox.Show("Please select an order to void.", "No Order Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void VoidOrderWithConfirmation(int orderId)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to void this order?", "Confirm Void", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                                MessageBox.Show("Order has been voided successfully.", "Order Voided",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadOrders();  // Refresh the orders list
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

        private void OrderForm_Resize(object sender, EventArgs e)
        {
            try
            {
                // Correct way to auto-resize columns
                foreach (DataGridViewColumn column in dgvOrders.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                // Alternative approach
                // dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Ensure minimum button width
                int minButtonWidth = 100;
                foreach (Control control in buttonPanel.Controls)
                {
                    if (control is Button button)
                    {
                        button.MinimumSize = new Size(minButtonWidth, button.Height);
                    }
                }

                // Update margins and spacing
                if (mainContainer != null)
                {
                    mainContainer.Padding = new Padding(10);
                }

                // Force layout update
                this.PerformLayout();
                this.Refresh();
            }
            catch (Exception ex)
            {
                // Handle or log any errors
                Console.WriteLine($"Error resizing form: {ex.Message}");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DashboardForm(_currentUser).Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            dgvOrders.CellFormatting -= DgvOrders_CellFormatting;
        }
    }
}