using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InventoryOrderSystem.App.Forms; // Add this at the top of the file

namespace InventoryOrderSystem.Forms
{
    public partial class PaymentMethodForm : Form
    {
        public string SelectedPaymentMethod { get; private set; }

        public PaymentMethodForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Form properties
            this.Text = "Select Payment Method";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create radio buttons
            RadioButton rbCash = new RadioButton
            {
                Text = "Cash",
                Location = new Point(50, 30),
                Checked = true,
                Font = new Font("Arial", 10, FontStyle.Regular)
            };

            RadioButton rbGCash = new RadioButton
            {
                Text = "GCash (E-Wallet)",
                Location = new Point(50, 60),
                Font = new Font("Arial", 10, FontStyle.Regular)
            };

            // Create buttons
            Button btnConfirm = new Button
            {
                Text = "Confirm",
                DialogResult = DialogResult.OK,
                Location = new Point(50, 100),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(150, 100),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(74, 44, 42),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            // Add event handlers
            btnConfirm.Click += (sender, e) =>
            {
                SelectedPaymentMethod = rbCash.Checked ? "Cash" : "GCash";
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] { rbCash, rbGCash, btnConfirm, btnCancel });
        }
    }
}