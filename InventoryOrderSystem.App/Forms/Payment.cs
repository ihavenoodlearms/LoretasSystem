using System;
using System.Windows.Forms;
using InventoryOrderSystem.Services;

namespace InventoryOrderSystem.Forms
{
    public partial class PaymentForm : Form
    {
        private PaymentProcessor _paymentProcessor;
        private decimal _amount;

        public PaymentForm(decimal amount)
        {
            InitializeComponent();
            _paymentProcessor = new PaymentProcessor();
            _amount = amount;
            lblAmount.Text = $"Amount to pay: {_amount:C}";
        }

        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            string transactionId = txtTransactionId.Text;
            bool success = _paymentProcessor.ProcessCashlessPayment(_amount, transactionId);

            if (success)
            {
                MessageBox.Show("Payment processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Payment processing failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}