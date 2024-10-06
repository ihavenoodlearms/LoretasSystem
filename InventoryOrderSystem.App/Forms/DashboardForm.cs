using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using InventoryOrderingSystem;
using InventoryOrderSystem.App.Forms;
using InventoryOrderSystem.Models;

namespace InventoryOrderSystem.Forms
{
    public partial class DashboardForm : Form
    {
        private User _currentUser;
        private bool sidebarExpanded = true;

        // Sample data - replace with actual data from your database
        private decimal totalSales = 0;
        private int transactionCount = 0;
        private List<(string Product, int Quantity, decimal Revenue)> topProducts;

        public DashboardForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            lblWelcome.Text = $"Welcome, {_currentUser.Username}!";
            if (!_currentUser.IsSuperAdmin)
            {
                btnReports.Visible = false;
            }
            else
            {
                lblWelcome.Text = "Welcome!";
                btnReports.Visible = false;
                // You might want to handle the case where no user is provided
                // For example, you could show a login prompt or disable certain functionality
            }

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            SetFormSize();

            // Initially show the sidebar
            pnlSidebar.Width = 220;
            btnToggleSidebar.Visible = true;
            btnToggleSidebar.BringToFront();
            btnToggleSidebar.Location = new Point(pnlSidebar.Width + 12, 12);

            InitializeDashboardData();
            UpdateDashboard();
            AdjustMainPanelLayout();
        }

        private void SetFormSize()
        {
            Rectangle screenRect = Screen.PrimaryScreen.WorkingArea;
            int formWidth = (int)(screenRect.Width * 0.8);
            int formHeight = (int)(screenRect.Height * 0.8);
            this.Size = new Size(formWidth, formHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeDashboardData()
        {
            // TODO: Fetch this data from your database
            totalSales = 2450.00m;
            transactionCount = 25;
            topProducts = new List<(string, int, decimal)>
            {
                ("Americano", 15, 1125.00m),
                ("Triple Chocolate Mocha", 10, 850.00m),
                // Add more products as needed
            };
        }

        private void UpdateDashboard()
        {
            lblDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            lblTotalSales.Text = $"Total Sales: {totalSales:C}";
            lblTransactionCount.Text = $"Transactions: {transactionCount}";

            dgvTopProducts.Rows.Clear();
            foreach (var product in topProducts)
            {
                dgvTopProducts.Rows.Add(product.Product, product.Quantity, product.Revenue.ToString("C"));
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
                MessageBox.Show("Reports functionality not implemented yet.");
            }
        }

        private void btnToggleSidebar_Click(object sender, EventArgs e)
        {
            if (sidebarExpanded)
            {
                pnlSidebar.Width = 60;
                btnToggleSidebar.Location = new Point(pnlSidebar.Width + 12, 12);
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
                btnToggleSidebar.Location = new Point(pnlSidebar.Width + 12, 12);
                btnInventory.Text = "Inventory Management";
                btnOrders.Text = "Order Management";
                btnReports.Text = "Reports";
                btnSettings.Text = "Settings";
                lblLogo.Text = "Loreta's Cafe";
            }
            sidebarExpanded = !sidebarExpanded;
            AdjustMainPanelLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new OrderMenuForm(_currentUser).Show();
        }

        private void AdjustMainPanelLayout()
        {
            pnlMain.Location = new Point(pnlSidebar.Width, pnlHeader.Height);
            pnlMain.Size = new Size(this.ClientSize.Width - pnlSidebar.Width, this.ClientSize.Height - pnlHeader.Height);
        }

        private void tmrUpdateDashboard_Tick(object sender, EventArgs e)
        {
            UpdateDashboard();
        }
    }
}