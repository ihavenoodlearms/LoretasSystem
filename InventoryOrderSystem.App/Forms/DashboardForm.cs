using System;
using System.Windows.Forms;
using InventoryOrderingSystem;
using InventoryOrderSystem.App.Forms;
using InventoryOrderSystem.Models;

namespace InventoryOrderSystem.Forms
{
    public partial class DashboardForm : Form
    {
        private User _currentUser;
        private bool sidebarExpanded = false;

        public DashboardForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            lblWelcome.Text = $"Welcome, {_currentUser.Username}!";
            if (!_currentUser.IsSuperAdmin)
            {
                btnReports.Visible = false;
            }
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            this.Hide();
            new InventoryForm().Show();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            this.Hide();
            new OrderForm().Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            if (_currentUser.IsSuperAdmin)
            {
                // Open reports form or show report
                MessageBox.Show("Reports functionality not implemented yet.");
            }
        }

        private void btnToggleSidebar_Click(object sender, EventArgs e)
        {
            if (sidebarExpanded)
            {
                pnlSidebar.Width = 60;
                foreach (Control ctrl in pnlSidebar.Controls)
                {
                    if (ctrl is Button)
                    {
                        ctrl.Text = "";
                    }
                }
                lblLogo.Text = "LC";
            }
            else
            {
                pnlSidebar.Width = 220;
                btnInventory.Text = "Inventory Management";
                btnOrders.Text = "Order Management";
                btnReports.Text = "Reports";
                btnSettings.Text = "Settings";
                lblLogo.Text = "Loreta's Cafe";
            }
            sidebarExpanded = !sidebarExpanded;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new OrderMenuForm().Show();
        }
    }
}