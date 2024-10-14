using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InventoryOrderingSystem;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;

namespace InventoryOrderSystem.Forms
{
    public partial class OrderForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private List<Order> _orders;
        private readonly User _currentUser;

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
            var flattenedOrders = _orders.Select(o => new
            {
                o.OrderId,
                o.UserId,
                o.OrderDate,
                o.TotalAmount,
                o.PaymentMethod,
                OrderItemsCount = o.OrderItems.Count
            }).ToList();
            dgvOrders.DataSource = flattenedOrders;
            FormatDataGridView();
        }

        private void FormatDataGridView()
        {
            dgvOrders.Columns["OrderId"].HeaderText = "Order ID";
            dgvOrders.Columns["UserId"].HeaderText = "User ID";
            dgvOrders.Columns["OrderDate"].HeaderText = "Order Date";
            dgvOrders.Columns["TotalAmount"].HeaderText = "Total Amount";
            dgvOrders.Columns["PaymentMethod"].HeaderText = "Payment Method";

            // Check if the OrderItems column exists before trying to access it
            if (dgvOrders.Columns.Contains("OrderItems"))
            {
                dgvOrders.Columns["OrderItems"].Visible = false;
            }
        }
        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            this.Hide();
            new OrderMenuForm(_currentUser).Show();
        }

        private void OrderMenuForm_OrderPlaced(object sender, Order newOrder)
        {
            _dbManager.AddOrder(newOrder);
            LoadOrders();
        }

        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                Order selectedOrder = (Order)dgvOrders.SelectedRows[0].DataBoundItem;
                MessageBox.Show(GetOrderDetails(selectedOrder), "Order Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}