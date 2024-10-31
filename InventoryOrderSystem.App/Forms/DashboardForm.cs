using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using InventoryOrderingSystem;
using InventoryOrderSystem.App.Forms;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;
using System.Data.SQLite;

namespace InventoryOrderSystem.Forms
{
    public partial class DashboardForm : Form
    {
        private User _currentUser;
        private bool sidebarExpanded = true;
        private Panel pnlSettings;
        private DatabaseManager _dbManager;
        private DateTime currentDate;
        private OrderMenuForm _orderMenuForm;

        // Sample data - replace with actual data from your database
        private decimal totalSales = 0;
        private int transactionCount = 0;
        private List<(string Product, int Quantity, decimal Revenue)> topProducts;

        // Color scheme
        private Color darkBrown = Color.FromArgb(74, 44, 42);
        private Color cream = Color.FromArgb(253, 245, 230);
        private Color lightBrown = Color.FromArgb(140, 105, 84);
        private Color accentGreen = Color.FromArgb(91, 123, 122);

        // Add these properties to track statistics
        private int receivedOrders = 0;
        private int processingOrders = 0;
        private int paidOrders = 0;
        private int cancelledOrders = 0;

        // Add this method to update order statistics
        private void UpdateOrderStatistics()
        {
            using (var connection = new SQLiteConnection(_dbManager.GetConnectionString()))
            {
                connection.Open();

                // Get total received orders
                string receivedQuery = "SELECT COUNT(*) FROM Orders";
                using (var command = new SQLiteCommand(receivedQuery, connection))
                {
                    receivedOrders = Convert.ToInt32(command.ExecuteScalar());
                }

                // Get processing orders (not paid and not voided)
                string processingQuery = "SELECT COUNT(*) FROM Orders WHERE Status = 'Active'";
                using (var command = new SQLiteCommand(processingQuery, connection))
                {
                    processingOrders = Convert.ToInt32(command.ExecuteScalar());
                }

                // Get paid orders
                string paidQuery = "SELECT COUNT(*) FROM Orders WHERE Status = 'Paid'";
                using (var command = new SQLiteCommand(paidQuery, connection))
                {
                    paidOrders = Convert.ToInt32(command.ExecuteScalar());
                }

                // Get cancelled/voided orders
                string cancelledQuery = "SELECT COUNT(*) FROM Orders WHERE Status = 'Voided'";
                using (var command = new SQLiteCommand(cancelledQuery, connection))
                {
                    cancelledOrders = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            // Update the labels
            lblReceived.Text = receivedOrders.ToString();
            lblProcessing.Text = processingOrders.ToString();
            lblPaid.Text = paidOrders.ToString();
            lblCancelled.Text = cancelledOrders.ToString();
        }

        public DashboardForm(User user)
        {
            InitializeComponent();
            InitializeOrderStatistics();
            _currentUser = user;
            _dbManager = new DatabaseManager();  // Make sure this line exists
            currentDate = DateTime.Today;
            ApplyStyles();
            SetupDashboard();
            CreateSettingsPanel();
            UpdateOrderStatistics();
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

            // Set column headers
            dgvTopProducts.Columns.Clear();
            dgvTopProducts.Columns.Add("Product", "Product");
            dgvTopProducts.Columns.Add("Quantity", "Quantity");
            dgvTopProducts.Columns.Add("Revenue", "Revenue");

            // Adjust column widths
            dgvTopProducts.Columns["Product"].Width = 200;
            dgvTopProducts.Columns["Quantity"].Width = 100;
            dgvTopProducts.Columns["Revenue"].Width = 120;

            // Align column headers
            foreach (DataGridViewColumn column in dgvTopProducts.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
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

            var topProducts = _dbManager.GetTopProductsForDate(currentDate);
            if (topProducts.Count > 0)
            {
                dgvTopProducts.Rows.Clear();
                foreach (var product in topProducts)
                {
                    dgvTopProducts.Rows.Add(product.Product, product.Quantity, product.Revenue.ToString("C"));
                }
                RefreshDataGridView();
            }

            AdjustMainPanelLayout();

            lblWelcome.Text = _currentUser != null ? $"Welcome, {_currentUser.Username}!" : "Welcome!";
            btnReports.Visible = _currentUser?.IsSuperAdmin ?? false;

            // Initially show the sidebar
            pnlSidebar.Width = 220;
            btnToggleSidebar.Visible = true;
            btnToggleSidebar.BringToFront();
            btnToggleSidebar.Location = new Point(pnlSidebar.Width + 12, 12);

            btnSettings.Click += btnSettings_Click;

            Button btnPreviousDay = new Button
            {
                Text = "<",
                Size = new Size(30, 30),
                Location = new Point(lblDate.Left - 40, lblDate.Top)
            };
            btnPreviousDay.Click += btnPreviousDay_Click;

            Button btnNextDay = new Button
            {
                Text = ">",
                Size = new Size(30, 30),
                Location = new Point(lblDate.Right + 10, lblDate.Top)
            };
            btnNextDay.Click += btnNextDay_Click;

            pnlMain.Controls.Add(btnPreviousDay);
            pnlMain.Controls.Add(btnNextDay);
        }

        private void RefreshDataGridView()
        {
            dgvTopProducts.Refresh();
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
            // Fetch real-time data from the database
            totalSales = _dbManager.GetTotalSalesForDate(currentDate);
            transactionCount = _dbManager.GetTransactionCountForDate(currentDate);
            topProducts = _dbManager.GetTopProductsForDate(currentDate);
        }

        private void UpdateDashboard()
        {
            lblDate.Text = currentDate.ToString("MM/dd/yyyy");
            lblTotalSales.Text = $"Total Sales: {totalSales:C}";
            lblTransactionCount.Text = $"Transactions: {transactionCount}";

            dgvTopProducts.Rows.Clear();
            foreach (var product in topProducts)
            {
                dgvTopProducts.Rows.Add(product.Product, product.Quantity, product.Revenue.ToString("C"));
            }

                var statistics = _dbManager.GetOrderStatisticsForDate(currentDate);
                Console.WriteLine($"Statistics object is null: {statistics == null}");

                if (statistics != null)
                {
                    if (lblReceived != null)
                    {
                        lblReceived.Text = statistics.ReceivedCount.ToString();
                    }
                    else
                    {
                        Console.WriteLine("lblReceived is null");
                    }
                    // Similar checks for other labels...
                }
                else
                {
                    Console.WriteLine("Statistics object is null");
                }

                // Update the statistic labels
                lblReceived.Text = statistics.ReceivedCount.ToString();
                lblProcessing.Text = statistics.ProcessingCount.ToString();
                lblPaid.Text = statistics.PaidCount.ToString();
                lblCancelled.Text = statistics.CancelledCount.ToString();
        }

        // Add a method to change the current date
        private void ChangeDate(DateTime newDate)
        {
            currentDate = newDate;
            InitializeDashboardData();
            UpdateDashboard();
        }

        private void btnPreviousDay_Click(object sender, EventArgs e)
        {
            ChangeDate(currentDate.AddDays(-1));
            UpdateDashboard(); // This will now update both regular dashboard and statistics
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            ChangeDate(currentDate.AddDays(1));
            UpdateDashboard(); // This will now update both regular dashboard and statistics
        }

        private void CreateSettingsPanel()
        {
            pnlSettings = new Panel
            {
                Visible = false,
                BackColor = cream,
                Size = new Size(300, 200),
                Location = new Point((this.ClientSize.Width - 300) / 2, (this.ClientSize.Height - 200) / 2)
            };

            Label lblSettings = new Label
            {
                Text = "Settings",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = darkBrown,
                AutoSize = true,
                Location = new Point(10, 10)
            };

            Button btnSignOut = new Button
            {
                Text = "Sign Out",
                Size = new Size(200, 40),
                Location = new Point(50, 60),
                BackColor = accentGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSignOut.FlatAppearance.BorderSize = 0;
            btnSignOut.Click += BtnSignOut_Click;

            Button btnCloseSettings = new Button
            {
                Text = "Close",
                Size = new Size(200, 40),
                Location = new Point(50, 110),
                BackColor = lightBrown,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCloseSettings.FlatAppearance.BorderSize = 0;
            btnCloseSettings.Click += (s, e) => pnlSettings.Visible = false;

            pnlSettings.Controls.AddRange(new Control[] { lblSettings, btnSignOut, btnCloseSettings });
            this.Controls.Add(pnlSettings);
            pnlSettings.BringToFront();
        }

        private void BtnSignOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to sign out?", "Confirm Sign Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
                new LoginForm().Show();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            pnlSettings.Visible = true;
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
            // Refresh the dashboard data
            InitializeDashboardData();
            UpdateDashboard();
        }

        private void AdjustMainPanelLayout()
        {
            pnlMain.Location = new Point(pnlSidebar.Width, pnlHeader.Height);
            pnlMain.Size = new Size(this.ClientSize.Width - pnlSidebar.Width, this.ClientSize.Height - pnlHeader.Height);
        }

        private void tmrUpdateDashboard_Tick(object sender, EventArgs e)
        {
            UpdateDashboard();
            UpdateOrderStatistics();
        }
    }
}