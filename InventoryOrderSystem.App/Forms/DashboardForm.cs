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
        private Panel pnlSettings;
        private DatabaseManager _dbManager;
        private DateTime currentDate;
        private OrderMenuForm _orderMenuForm;

        private bool isDragging = false;
        private Point dragOffset;

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
            label6.Text = receivedOrders.ToString();
            label7.Text = processingOrders.ToString();
            label8.Text = paidOrders.ToString();
            label9.Text = cancelledOrders.ToString();
        }

        public DashboardForm(User user)
        {
            InitializeComponent();

            _currentUser = user;
            _dbManager = new DatabaseManager();  // Make sure this line exists
            currentDate = DateTime.Today;
            this.Resize += DashboardForm_Resize;  // Add this line

            // Add these new lines
            this.MouseDown += Form_MouseDown;
            this.MouseMove += Form_MouseMove;
            this.MouseUp += Form_MouseUp;

            pnlHeader.MouseDown += Form_MouseDown;
            pnlHeader.MouseMove += Form_MouseMove;
            pnlHeader.MouseUp += Form_MouseUp;

            this.ResizeEnd += Form_ResizeEnd;
            this.Resize += Form_Resize;

            this.MinimumSize = new Size(1200, 700);

            SetupDashboard();
            CreateSettingsPanel();

        }



        private void DashboardForm_Resize(object sender, EventArgs e)
        {
            if (pnlOrderStats != null)
            {
                // Update panel width while maintaining margins
                pnlOrderStats.Width = pnlMain.Width - 40;
                pnlOrderStats.Location = new Point(20, dgvTopProducts.Bottom + 20);

                // Center the title
                if (lblOrderStatsTitle != null)
                {
                    lblOrderStatsTitle.Location = new Point(
                        (pnlOrderStats.Width - lblOrderStatsTitle.Width) / 2, 10);
                }
            }
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

            dgvTopProducts.GridColor = lightBrown;
            dgvTopProducts.DefaultCellStyle.Font = new Font("Arial Rounded MT Bold", 11.25F, FontStyle.Regular);
            dgvTopProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Arial Rounded MT Bold", 11.25F, FontStyle.Bold);
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



            btnSettings.Click += btnSettings_Click;

            Button btnPreviousDay = new Button
            {
                Text = "<",
                Size = new Size(40, 30),
                Font = new Font("Arial Rounded MT Bold", 12, FontStyle.Bold),
                Location = new Point(lblDate.Left - 60, lblDate.Top), // Moved further left
                BackColor = Color.FromArgb(82, 110, 72),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance =
    {
        BorderColor = Color.White,
        BorderSize = 2
    }
            };
            btnPreviousDay.Click += btnPreviousDay_Click;

            Button btnNextDay = new Button
            {
                Text = ">",
                Size = new Size(40, 30),
                Font = new Font("Arial Rounded MT Bold", 12, FontStyle.Bold),
                Location = new Point(lblDate.Right + 20, lblDate.Top), // Moved further right
                BackColor = Color.FromArgb(82, 110, 72),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance =
    {
        BorderColor = Color.White,
        BorderSize = 2
    }
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
            label6.Text = statistics.ReceivedCount.ToString();
            label7.Text = statistics.ProcessingCount.ToString();
            label8.Text = statistics.PaidCount.ToString();
            label9.Text = statistics.CancelledCount.ToString();
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
            // Create settings panel with relative size and anchoring
            pnlSettings = new Panel
            {
                Visible = false,
                BackColor = cream,
                Size = new Size((int)(this.ClientSize.Width * 0.3), (int)(this.ClientSize.Height * 0.3)), // 30% of form size
                MinimumSize = new Size(300, 200), // Minimum size to prevent too small panel
                MaximumSize = new Size(500, 300), // Maximum size to prevent too large panel
                Anchor = AnchorStyles.None // This will be centered manually
            };

            // Center the panel
            CenterSettingsPanel();

            // Add resize handler to keep panel centered
            this.Resize += (s, e) => CenterSettingsPanel();

            // Create a TableLayoutPanel for responsive layout
            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                Padding = new Padding(20),
            };

            // Set row styles for proper spacing
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));  // Title
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));  // Sign Out button
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));  // Close button
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));  // Bottom spacing

            // Title
            Label lblSettings = new Label
            {
                Text = "Settings",
                Font = new Font("Arial Rounded MT Bold", 16, FontStyle.Bold),
                ForeColor = darkBrown,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Sign Out button with responsive width
            Button btnSignOut = new Button
            {
                Text = "Sign Out",
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(40, 5, 40, 5),
                BackColor = accentGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial Rounded MT Bold", 9.75f, FontStyle.Regular)
            };
            btnSignOut.FlatAppearance.BorderSize = 0;
            btnSignOut.Click += BtnSignOut_Click;

            // Close button with responsive width
            Button btnCloseSettings = new Button
            {
                Text = "Close",
                Dock = DockStyle.Fill,
                Height = 40,
                Margin = new Padding(40, 5, 40, 5),
                BackColor = lightBrown,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial Rounded MT Bold", 9.75f, FontStyle.Regular)
            };
            btnCloseSettings.FlatAppearance.BorderSize = 0;
            btnCloseSettings.Click += (s, e) => pnlSettings.Visible = false;

            // Add controls to the layout panel
            layoutPanel.Controls.Add(lblSettings, 0, 0);
            layoutPanel.Controls.Add(btnSignOut, 0, 1);
            layoutPanel.Controls.Add(btnCloseSettings, 0, 2);

            // Add layout panel to settings panel
            pnlSettings.Controls.Add(layoutPanel);

            // Add panel to form
            this.Controls.Add(pnlSettings);
            pnlSettings.BringToFront();
        }

        private void CenterSettingsPanel()
        {
            if (pnlSettings != null)
            {
                // Adjust panel size based on form size
                pnlSettings.Size = new Size(
                    Math.Min(500, Math.Max(300, (int)(this.ClientSize.Width * 0.3))),
                    Math.Min(300, Math.Max(200, (int)(this.ClientSize.Height * 0.3)))
                );

                // Center the panel
                pnlSettings.Location = new Point(
                    (this.ClientSize.Width - pnlSettings.Width) / 2,
                    (this.ClientSize.Height - pnlSettings.Height) / 2
                );
            }
        }

        // Add this to your form's constructor or where you initialize the form
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            CenterSettingsPanel();
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
            this.Hide();
            new SalesReportForm(_currentUser).Show();
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
            UpdateLayout();
        }

        private void tmrUpdateDashboard_Tick(object sender, EventArgs e)
        {
            UpdateDashboard();
            UpdateOrderStatistics();
        }

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragOffset = new Point(e.X, e.Y);
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - dragOffset.X,
                                   currentScreenPos.Y - dragOffset.Y);
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void Form_ResizeEnd(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            pnlMain.Width = this.ClientSize.Width - pnlSidebar.Width;
            pnlMain.Height = this.ClientSize.Height - pnlHeader.Height;

            pnlHeader.Width = this.ClientSize.Width - pnlSidebar.Width;

            int availableWidth = pnlMain.Width - 40;
            int panelWidth = (availableWidth - 60) / 4;
            int startX = 20;

            panel1.Width = panelWidth;
            panel2.Width = panelWidth;
            panel3.Width = panelWidth;
            panel4.Width = panelWidth;

            panel1.Location = new Point(startX, panel1.Location.Y);
            panel2.Location = new Point(startX + panelWidth + 20, panel2.Location.Y);
            panel3.Location = new Point(startX + (panelWidth + 20) * 2, panel3.Location.Y);
            panel4.Location = new Point(startX + (panelWidth + 20) * 3, panel4.Location.Y);

            lblDate.Location = new Point(pnlMain.Width - lblDate.Width - 80, lblDate.Location.Y); // Moved label left to make room for buttons


            dgvTopProducts.Width = pnlMain.Width - 40;
            dgvTopProducts.Height = pnlMain.Height - dgvTopProducts.Top - 20;

            if (dgvTopProducts.Columns.Count > 0)
            {
                foreach (DataGridViewColumn col in dgvTopProducts.Columns)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

    }
}
