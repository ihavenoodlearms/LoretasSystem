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
                Status = o.Status
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

            // Adjust column widths
            dgvOrders.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvOrders.Columns["OrderDate"].Width = 150; // Adjust as needed

            // Enable sorting
            dgvOrders.Sort(dgvOrders.Columns["OrderId"], ListSortDirection.Descending);

            // Add the CellFormatting event handler
            dgvOrders.CellFormatting += DgvOrders_CellFormatting;
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
                else
                {
                    row.DefaultCellStyle.BackColor = dgvOrders.DefaultCellStyle.BackColor;
                    row.DefaultCellStyle.ForeColor = dgvOrders.DefaultCellStyle.ForeColor;
                }
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            dgvOrders.CellFormatting -= DgvOrders_CellFormatting;
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
            // Ensure this method is called on the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action<object, Order>(OrderMenuForm_OrderPlaced), sender, newOrder);
                return;
            }

            if (newOrder.OrderItems.Count == 0)
            {
                MessageBox.Show("Cannot place an order with no items.", "Empty Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if the order already exists to prevent duplication
            if (!_orders.Any(o => o.OrderId == newOrder.OrderId))
            {
                _dbManager.AddOrder(newOrder);
                LoadOrders();
            }

            // Close the OrderMenuForm
            if (sender is OrderMenuForm orderMenuForm)
            {
                orderMenuForm.Close();
            }

            // Show this form
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
                    MessageBox.Show(GetOrderDetails(selectedOrder), "Order Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select an order to view.", "No Order Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string GetOrderDetails(Order order)
        {
            string details = $"Order ID: {order.OrderId}\n" +
                             $"User ID: {order.UserId}\n" +
                             $"Order Date: {order.OrderDate}\n" +
                             $"Total Amount: {order.TotalAmount:C}\n" +
                             $"Payment Method: {order.PaymentMethod}\n\n" +
                             "Order Items:\n";

            foreach (var item in order.OrderItems)
            {
                details += $"- Item ID: {item.ItemId}, Quantity: {item.Quantity}, Price: {item.Price:C}\n";
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
                MessageBox.Show("Please select an order to void.", "No Order Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void VoidOrderWithConfirmation(int orderId)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to void this order?", "Confirm Void", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                    TextBox passwordBox = new TextBox() { Left = 20, Top = 50, Width = 240, PasswordChar = '*' };
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
                        if (_dbManager.VerifyPassword(_currentUser.UserId, enteredPassword))
                        {
                            try
                            {
                                _dbManager.VoidOrder(orderId);
                                MessageBox.Show("Order has been voided successfully.", "Order Voided", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadOrders();  // Refresh the orders list
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"An error occurred while voiding the order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password. Order voiding canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Order voiding canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DashboardForm(_currentUser).Show();
        }

    }

    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public int OrderItemsCount { get; set; }
        public string Status { get; set; }  
    }
}