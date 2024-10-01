using System;
using System.Drawing;
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

            // Set form properties for sizing
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            SetFormSize();

            // Initially hide the sidebar
            pnlSidebar.Width = 0;
            btnToggleSidebar.Visible = true;
            btnToggleSidebar.BringToFront();
            btnToggleSidebar.Location = new Point(12, 12);
        }

        private void SetFormSize()
        {
            Rectangle screenRect = Screen.PrimaryScreen.WorkingArea;
            // Calculate the form size (e.g., 80% of the screen width and height)
            int formWidth = (int)(screenRect.Width * 0.8);
            int formHeight = (int)(screenRect.Height * 0.8);
            // Set the form size
            this.Size = new Size(formWidth, formHeight);
            // Center the form on the screen
            this.StartPosition = FormStartPosition.CenterScreen;
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
                pnlSidebar.Width = 0;
                btnToggleSidebar.Location = new Point(12, 12);
            }
            else
            {
                pnlSidebar.Width = 220;
                btnInventory.Text = "Inventory Management";
                btnOrders.Text = "Order Management";
                btnReports.Text = "Reports";
                btnSettings.Text = "Settings";
                lblLogo.Text = "Loreta's Cafe";
                btnToggleSidebar.Location = new Point(pnlSidebar.Width + 12, 12);
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