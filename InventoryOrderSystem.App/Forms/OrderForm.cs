using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                OrderItemsCount = o.OrderItems.Count
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

            // Adjust column widths
            dgvOrders.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvOrders.Columns["OrderDate"].Width = 150; // Adjust as needed

            // Enable sorting
            dgvOrders.Sort(dgvOrders.Columns["OrderId"], ListSortDirection.Descending);
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

        public void VoidOrder(int orderId)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to void this order?", "Void Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _dbManager.VoidOrder(orderId);
                    MessageBox.Show("Order has been voided successfully.", "Order Voided", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOrders();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while voiding the order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}