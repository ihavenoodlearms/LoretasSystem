using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InventoryOrderSystem.Forms;
using InventoryOrderSystem.Models;

namespace InventoryOrderingSystem
{
    public partial class PaymentForm : Form
    {
        private User _currentUser;
        private List<OrderItem> _orderItems;
        private decimal _totalAmount;

        public PaymentForm(User user, List<OrderItem> orderItems, decimal totalAmount)
        {
            InitializeComponent();
            _currentUser = user;
            _orderItems = orderItems;
            _totalAmount = totalAmount;
            InitializePaymentForm();
        }

        private void InitializePaymentForm()
        {
            lblAmount.Text = $"Amount to pay: ₱{_totalAmount:F2}";
            txtTransactionId.Text = GenerateTransactionId();
        }

        private string GenerateTransactionId()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            // Implement payment processing logic here
            MessageBox.Show("Payment processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // After successful payment, you might want to save the order to a database, clear the cart, etc.
            this.Close();
            new DashboardForm(_currentUser).Show();
        }

        private void PaymentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                new OrderMenuForm(_currentUser).Show();
            }
        }
    }
}