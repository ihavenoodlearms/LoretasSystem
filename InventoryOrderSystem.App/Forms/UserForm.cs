using InventoryOrderingSystem;
using InventoryOrderSystem.Forms;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryOrderSystem.App.Forms
{
    public partial class UserForm : Form
    {
        private User _currentUser;
        private Panel pnlSettings;
        private DatabaseManager _dbManager;
        private DateTime currentDate;
        private OrderMenuForm _orderMenuForm;
        private bool mouseDown;
        private Point lastLocation;

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

        public UserForm(User user)
        {
            InitializeComponent();

            // Enable form movement
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Enable form movement by grabbing the header panel
            pnlHeader.MouseDown += (s, e) => {
                mouseDown = true;
                lastLocation = e.Location;
            };

            pnlHeader.MouseMove += (s, e) => {
                if (mouseDown)
                {
                    this.Location = new Point(
                        (this.Location.X - lastLocation.X) + e.X,
                        (this.Location.Y - lastLocation.Y) + e.Y);
                    this.Update();
                }
            };

            pnlHeader.MouseUp += (s, e) => mouseDown = false;

            // Make form responsive
            this.MinimumSize = new Size(1366, 768);
            this.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            _currentUser = user;
            _dbManager = new DatabaseManager();
            currentDate = DateTime.Today;
            SetupDashboard();
            CreateSettingsPanel();

            // Start the dashboard update timer
            Timer updateTimer = new Timer();
            updateTimer.Interval = 5000; // Update every 5 seconds
            updateTimer.Tick += tmrUpdateDashboard_Tick;
            updateTimer.Start();
        }

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
            if (label6 != null) label6.Text = receivedOrders.ToString();
            if (label7 != null) label7.Text = processingOrders.ToString();
            if (label8 != null) label8.Text = paidOrders.ToString();
            if (label9 != null) label9.Text = cancelledOrders.ToString();
        }

        private void StyleButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = darkBrown;
            btn.ForeColor = cream;
            btn.Font = new Font("Arial Rounded MT Bold", 11.25F, FontStyle.Regular);
            btn.Padding = new Padding(10, 0, 0, 0);
            btn.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void StyleLabel(Label lbl, float fontSize, FontStyle fontStyle)
        {
            lbl.Font = new Font("Arial Rounded MT Bold", fontSize, fontStyle);
            lbl.ForeColor = darkBrown;
        }

        private void StyleDataGridView()
        {
            // Basic styling
            dgvTopProducts.GridColor = lightBrown;
            dgvTopProducts.DefaultCellStyle.Font = new Font("Arial Rounded MT Bold", 11.25F, FontStyle.Regular);
            dgvTopProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Arial Rounded MT Bold", 11.25F, FontStyle.Bold);
            dgvTopProducts.ColumnHeadersDefaultCellStyle.BackColor = darkBrown;
            dgvTopProducts.ColumnHeadersDefaultCellStyle.ForeColor = cream;
            dgvTopProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTopProducts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            // Ensure columns exist
            if (dgvTopProducts.Columns.Count == 0)
            {
                // Set column headers
                dgvTopProducts.Columns.Clear();
                dgvTopProducts.Columns.Add("Product", "Product");
                dgvTopProducts.Columns.Add("Quantity", "Quantity");
                dgvTopProducts.Columns.Add("Revenue", "Revenue");

                // Set column proportions
                dgvTopProducts.Columns["Product"].FillWeight = 50;
                dgvTopProducts.Columns["Quantity"].FillWeight = 25;
                dgvTopProducts.Columns["Revenue"].FillWeight = 25;

                // Align column headers
                foreach (DataGridViewColumn column in dgvTopProducts.Columns)
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            // Additional styling
            dgvTopProducts.EnableHeadersVisualStyles = false;
            dgvTopProducts.RowHeadersVisible = false;
            dgvTopProducts.AllowUserToAddRows = false;
            dgvTopProducts.AllowUserToDeleteRows = false;
            dgvTopProducts.ReadOnly = true;
            dgvTopProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void StyleMenuButton()
        {
            btnNewOrder.BackColor = accentGreen;
            btnNewOrder.ForeColor = Color.White;
            btnNewOrder.Font = new Font("Arial Rounded MT Bold", 10F, FontStyle.Bold);
            btnNewOrder.FlatStyle = FlatStyle.Flat;
            btnNewOrder.FlatAppearance.BorderSize = 0;
        }

        private void SetupDashboard()
        {
            SetFormSize();
            InitializeDashboardData();
            UpdateDashboard();
            StyleDataGridView();

            var topProducts = _dbManager.GetTopProductsForDate(currentDate);
            if (topProducts.Count > 0)
            {
                dgvTopProducts.Rows.Clear();
                foreach (var product in topProducts)
                {
                    dgvTopProducts.Rows.Add(product.Product, product.Quantity, product.Revenue.ToString("C"));
                }
            }

            AdjustMainPanelLayout();

            lblWelcome.Text = _currentUser != null ? $"Welcome, {_currentUser.Username}!" : "Welcome!";
            btnReports.Visible = _currentUser?.IsSuperAdmin ?? false;

            // Create and style navigation buttons
            CreateDateNavigationButtons();
        }

        private void CreateDateNavigationButtons()
        {
            Button btnPreviousDay = new Button
            {
                Text = "<",
                Size = new Size(40, 30),
                Font = new Font("Arial Rounded MT Bold", 12, FontStyle.Bold),
                BackColor = accentGreen,
                ForeColor = cream,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(5),
                Cursor = Cursors.Hand
            };
            btnPreviousDay.FlatAppearance.BorderSize = 0;
            btnPreviousDay.Click += btnPreviousDay_Click;

            Button btnNextDay = new Button
            {
                Text = ">",
                Size = new Size(40, 30),
                Font = new Font("Arial Rounded MT Bold", 12, FontStyle.Bold),
                BackColor = accentGreen,
                ForeColor = cream,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(5),
                Cursor = Cursors.Hand
            };
            btnNextDay.FlatAppearance.BorderSize = 0;
            btnNextDay.Click += btnNextDay_Click;

            // Create container panel for date navigation
            Panel dateNavPanel = new Panel
            {
                Height = 40,
                Dock = DockStyle.Top,
                Padding = new Padding(10)
            };

            // Add controls to date navigation panel
            FlowLayoutPanel dateControls = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true
            };

            dateControls.Controls.AddRange(new Control[] { btnPreviousDay, lblDate, btnNextDay });
            dateNavPanel.Controls.Add(dateControls);
            pnlMain.Controls.Add(dateNavPanel);
        }

        private void SetFormSize()
        {
            Rectangle screenRect = Screen.PrimaryScreen.WorkingArea;
            int formWidth = (int)(screenRect.Width * 0.95); // Increased from 0.8 to 0.95
            int formHeight = (int)(screenRect.Height * 0.9); // Increased from 0.8 to 0.9
            this.Size = new Size(formWidth, formHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeDashboardData()
        {
            totalSales = _dbManager.GetTotalSalesForDate(currentDate);
            transactionCount = _dbManager.GetTransactionCountForDate(currentDate);
            topProducts = _dbManager.GetTopProductsForDate(currentDate);
        }

        private void UpdateDashboard()
        {
            lblDate.Text = currentDate.ToString("MM/dd/yyyy");
            lblTotalSales.Text = $"Total Sales: {totalSales:C}";
            lblTransactionCount.Text = $"Transactions: {transactionCount}";

            // Check if columns exist, if not create them
            if (dgvTopProducts.Columns.Count == 0)
            {
                dgvTopProducts.Columns.Clear();
                dgvTopProducts.Columns.Add("Product", "Product");
                dgvTopProducts.Columns.Add("Quantity", "Quantity");
                dgvTopProducts.Columns.Add("Revenue", "Revenue");

                // Set column proportions
                dgvTopProducts.Columns["Product"].FillWeight = 50;
                dgvTopProducts.Columns["Quantity"].FillWeight = 25;
                dgvTopProducts.Columns["Revenue"].FillWeight = 25;

                // Align column headers
                foreach (DataGridViewColumn column in dgvTopProducts.Columns)
                {
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

            // Clear existing rows and add new data
            dgvTopProducts.Rows.Clear();
            if (topProducts != null)
            {
                foreach (var product in topProducts)
                {
                    dgvTopProducts.Rows.Add(product.Product, product.Quantity, product.Revenue.ToString("C"));
                }
            }

            UpdateOrderStatistics();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void AdjustMainPanelLayout()
        {
            // Calculate available space
            int availableWidth = this.ClientSize.Width - pnlSidebar.Width;
            int availableHeight = this.ClientSize.Height - pnlHeader.Height;

            // Adjust main panel
            pnlMain.Location = new Point(pnlSidebar.Width, pnlHeader.Height);
            pnlMain.Size = new Size(availableWidth, availableHeight);

            // Create a FlowLayoutPanel for statistics boxes
            FlowLayoutPanel statsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false, // Changed to false to prevent wrapping
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(20), // Increased padding
                Height = 150
            };

            // Style and add statistics boxes
            var statBoxes = new[] {
                (Label: label6, Title: "Received Orders"),
                (Label: label7, Title: "Processing Orders"),
                (Label: label8, Title: "Paid Orders"),
                (Label: label9, Title: "Cancelled Orders")
            };

            int boxWidth = (availableWidth - statsPanel.Padding.Horizontal - (statBoxes.Length + 1) * 20) / statBoxes.Length;

            foreach (var stat in statBoxes)
            {
                Panel statBox = new Panel
                {
                    BackColor = lightBrown,
                    Size = new Size(boxWidth, 120),
                    Margin = new Padding(10), // Reduced margin to fit all boxes
                    Padding = new Padding(10)
                };

                Label titleLabel = new Label
                {
                    Text = stat.Title,
                    Font = new Font("Arial Rounded MT Bold", 12),
                    ForeColor = cream,
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 30
                };

                stat.Label.Font = new Font("Arial Rounded MT Bold", 24, FontStyle.Bold);
                stat.Label.ForeColor = cream;
                stat.Label.Dock = DockStyle.Fill;
                stat.Label.TextAlign = ContentAlignment.MiddleCenter;

                statBox.Controls.Add(stat.Label);
                statBox.Controls.Add(titleLabel);
                statsPanel.Controls.Add(statBox);
            }

            // Clear and rebuild main panel
            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(statsPanel);

            // Add sales summary panel
            Panel salesPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                Padding = new Padding(20, 10, 20, 10) // Increased horizontal padding
            };


            lblTotalSales.Font = new Font("Arial Rounded MT Bold", 14);
            lblTransactionCount.Font = new Font("Arial Rounded MT Bold", 14);
            lblTotalSales.Location = new Point(20, 10);
            lblTransactionCount.Location = new Point(20, 40);

            salesPanel.Controls.AddRange(new Control[] { lblTotalSales, lblTransactionCount });
            pnlMain.Controls.Add(salesPanel);

            // Position dgvTopProducts below stats
            int topProductsTop = statsPanel.Bottom + salesPanel.Height + 10;
            dgvTopProducts.Location = new Point(10, topProductsTop);
            dgvTopProducts.Size = new Size(availableWidth - 20, availableHeight - topProductsTop - 20);
            pnlMain.Controls.Add(dgvTopProducts);
        }

        // Event handlers
        private void btnPreviousDay_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddDays(-1);
            InitializeDashboardData();
            UpdateDashboard();
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddDays(1);
            InitializeDashboardData();
            UpdateDashboard();
        }

        private void OrderMenuForm_OrderPlaced(object sender, Order newOrder)
        {
            InitializeDashboardData();
            UpdateDashboard();
        }

        private void tmrUpdateDashboard_Tick(object sender, EventArgs e)
        {
            UpdateDashboard();
            UpdateOrderStatistics();
        }

        private void btnOrders_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new OrderForm(_currentUser).Show();
        }

        private void btnReports_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            new SalesReportForm(_currentUser).Show();
        }

        private void btnMenu_Click_1(object sender, EventArgs e)
        {
            if (_orderMenuForm == null || _orderMenuForm.IsDisposed)
            {
                _orderMenuForm = new OrderMenuForm(_currentUser);
                _orderMenuForm.OrderPlaced += (s, order) =>
                {
                    // When an order is placed, update the dashboard data
                    InitializeDashboardData();
                    UpdateDashboard();

                    // Store the order in the database
                    _dbManager.AddOrder(order);

                    // Show confirmation
                    MessageBox.Show("Order placed successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                };
                _orderMenuForm.FormClosed += (s, args) =>
                {
                    this.Show();
                    // Refresh dashboard data when returning
                    InitializeDashboardData();
                    UpdateDashboard();
                };
            }
            _orderMenuForm.Show();
            this.Hide();
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

        private void CreateSettingsPanel()
        {
            pnlSettings = new Panel
            {
                Visible = false,
                BackColor = cream,
                Size = new Size((int)(this.ClientSize.Width * 0.3), (int)(this.ClientSize.Height * 0.3)),
                MinimumSize = new Size(300, 200),
                MaximumSize = new Size(500, 300),
                Anchor = AnchorStyles.None
            };

            CenterSettingsPanel();
            this.Resize += (s, e) => CenterSettingsPanel();

            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                Padding = new Padding(20),
            };

            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));

            Label lblSettings = new Label
            {
                Text = "Settings",
                Font = new Font("Arial Rounded MT Bold", 16, FontStyle.Bold),
                ForeColor = darkBrown,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button btnSignOut = new Button
            {
                Text = "Sign Out",
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(40, 5, 40, 5),
                BackColor = accentGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial Rounded MT Bold", 9.75f, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnSignOut.FlatAppearance.BorderSize = 0;
            btnSignOut.Click += BtnSignOut_Click;

            Button btnCloseSettings = new Button
            {
                Text = "Close",
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(40, 5, 40, 5),
                BackColor = lightBrown,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial Rounded MT Bold", 9.75f, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            btnCloseSettings.FlatAppearance.BorderSize = 0;
            btnCloseSettings.Click += (s, e) => pnlSettings.Visible = false;

            layoutPanel.Controls.Add(lblSettings, 0, 0);
            layoutPanel.Controls.Add(btnSignOut, 0, 1);
            layoutPanel.Controls.Add(btnCloseSettings, 0, 2);

            pnlSettings.Controls.Add(layoutPanel);
            this.Controls.Add(pnlSettings);
            pnlSettings.BringToFront();
        }

        private void CenterSettingsPanel()
        {
            if (pnlSettings != null)
            {
                pnlSettings.Size = new Size(
                    Math.Min(500, Math.Max(300, (int)(this.ClientSize.Width * 0.3))),
                    Math.Min(300, Math.Max(200, (int)(this.ClientSize.Height * 0.3)))
                );

                pnlSettings.Location = new Point(
                    (this.ClientSize.Width - pnlSettings.Width) / 2,
                    (this.ClientSize.Height - pnlSettings.Height) / 2
                );
            }
        }

        private void BtnSignOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to sign out?",
                "Confirm Sign Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                this.Hide();
                new LoginForm().Show();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            CenterSettingsPanel();
            AdjustMainPanelLayout();
        }
    }
}