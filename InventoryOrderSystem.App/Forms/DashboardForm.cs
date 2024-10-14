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

        // Color scheme
        private Color darkBrown = Color.FromArgb(74, 44, 42);
        private Color cream = Color.FromArgb(253, 245, 230);
        private Color lightBrown = Color.FromArgb(140, 105, 84);
        private Color accentGreen = Color.FromArgb(91, 123, 122);

        public DashboardForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            ApplyStyles();
            SetupDashboard();
        }

        private void ApplyStyles()
        {
            // Form styles
            this.BackColor = cream;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Sidebar styles
            pnlSidebar.BackColor = darkBrown;
            StyleButton(btnInventory);
            StyleButton(btnOrders);
            StyleButton(btnReports);
            StyleButton(btnSettings);

            // Logo styles
            lblLogo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblLogo.ForeColor = cream;

            // Header styles
            pnlHeader.BackColor = lightBrown;
            lblWelcome.ForeColor = cream;
            lblWelcome.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            // Main panel styles
            pnlMain.BackColor = cream;
            pnlMain.Padding = new Padding(20);

            StyleLabel(lblDate, 16, FontStyle.Bold);
            StyleLabel(lblTotalSales, 14, FontStyle.Regular);
            StyleLabel(lblTransactionCount, 14, FontStyle.Regular);

            // DataGridView styles
            StyleDataGridView();

            // Menu button styles
            StyleMenuButton();

            // Toggle sidebar button styles
            StyleToggleSidebarButton();
        }

        private void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = darkBrown;
            btn.ForeColor = cream;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            btn.Padding = new Padding(10, 0, 0, 0);
            btn.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void StyleLabel(Label lbl, float fontSize, FontStyle fontStyle)
        {
            lbl.Font = new Font("Segoe UI", fontSize, fontStyle);
            lbl.ForeColor = darkBrown;
        }

        private void StyleDataGridView()
        {
            dgvTopProducts.BackgroundColor = Color.White;
            dgvTopProducts.GridColor = lightBrown;
            dgvTopProducts.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            dgvTopProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvTopProducts.ColumnHeadersDefaultCellStyle.BackColor = darkBrown;
            dgvTopProducts.ColumnHeadersDefaultCellStyle.ForeColor = cream;
        }

        private void StyleMenuButton()
        {
            btnMenu.BackColor = accentGreen;
            btnMenu.ForeColor = Color.White;
            btnMenu.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnMenu.FlatStyle = FlatStyle.Flat;
            btnMenu.FlatAppearance.BorderSize = 0;
        }

        private void StyleToggleSidebarButton()
        {
            btnToggleSidebar.FlatStyle = FlatStyle.Flat;
            btnToggleSidebar.FlatAppearance.BorderSize = 0;
            btnToggleSidebar.BackColor = lightBrown;
            btnToggleSidebar.ForeColor = cream;
            btnToggleSidebar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        }

        private void SetupDashboard()
        {
            SetFormSize();
            InitializeDashboardData();
            UpdateDashboard();
            AdjustMainPanelLayout();

            lblWelcome.Text = _currentUser != null ? $"Welcome, {_currentUser.Username}!" : "Welcome!";
            btnReports.Visible = _currentUser?.IsSuperAdmin ?? false;

            // Initially show the sidebar
            pnlSidebar.Width = 220;
            btnToggleSidebar.Visible = true;
            btnToggleSidebar.BringToFront();
            btnToggleSidebar.Location = new Point(pnlSidebar.Width + 12, 12);
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
            new InventoryForm(_currentUser).Show();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            this.Hide();
            new OrderForm(_currentUser).Show();
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
                btnInventory.Text = "Inventory";
                btnOrders.Text = "Orders";
                btnReports.Text = "Reports";
                btnSettings.Text = "Settings";
                lblLogo.Text = "Loreta's Cafe";
            }
            sidebarExpanded = !sidebarExpanded;
            AdjustMainPanelLayout();
        }

        private void btnMenu_Click(object sender, EventArgs e)
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