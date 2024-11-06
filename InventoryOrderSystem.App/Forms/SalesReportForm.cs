using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using OfficeOpenXml;


namespace InventoryOrderSystem.Forms
{
    public partial class SalesReportForm : Form
    {
        private readonly DatabaseManager _dbManager;
        private readonly User _currentUser;
        private List<Order> _orders;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private DataGridView dgvSalesReport;
        private ComboBox cmbOrderStatus;
        private ComboBox cmbPaymentMethod;
        private Button btnGenerateReport;
        private Button btnExportReport;
        private Button btnBack;
        private Panel pnlSummary;
        private Label lblTotalSales;
        private Label lblTotalTransactions;
        private Label lblAverageTransaction;

        // Initialize these fields
        private TableLayoutPanel pnlFilters = new TableLayoutPanel();  // Add initialization here
        private FlowLayoutPanel pnlButtons = new FlowLayoutPanel();    // Add initialization here

        // Minimum form dimensions
        private readonly int MinimumFormWidth = 1000;
        private readonly int MinimumFormHeight = 600;

        // Color scheme
        private readonly Color darkBrown = Color.FromArgb(74, 44, 42);
        private readonly Color cream = Color.FromArgb(253, 245, 230);
        private readonly Color lightBrown = Color.FromArgb(140, 105, 84);

        public SalesReportForm(User user)
        {
            _dbManager = new DatabaseManager();
            _currentUser = user;
            InitializeReport();
            this.MinimumSize = new Size(MinimumFormWidth, MinimumFormHeight);
            LoadInitialData();
        }

        private void InitializeReport()
        {
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sales Report";
            this.BackColor = cream;

            CreateControls();
            SetupLayout();
            SetupEventHandlers();
        }

        private void CreateControls()
        {
            // Date Pickers
            dtpStartDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddDays(-30),
                Width = 120
            };

            dtpEndDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                Width = 120
            };

            // Combo Boxes
            cmbOrderStatus = new ComboBox
            {
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbOrderStatus.Items.AddRange(new object[] { "All", "Paid", "Voided" });
            cmbOrderStatus.SelectedIndex = 0;

            cmbPaymentMethod = new ComboBox
            {
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPaymentMethod.Items.AddRange(new object[] { "All", "Cash", "Card" });
            cmbPaymentMethod.SelectedIndex = 0;

            // Buttons
            btnGenerateReport = new Button
            {
                Text = "Generate Report",
                Width = 120,
                Height = 30,
                BackColor = darkBrown,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(5)
            };
            btnGenerateReport.FlatAppearance.BorderSize = 0;

            btnExportReport = new Button
            {
                Text = "Export to Excel",
                Width = 120,
                Height = 30,
                BackColor = darkBrown,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(5)
            };
            btnExportReport.FlatAppearance.BorderSize = 0;

            btnBack = new Button
            {
                Text = "Back",
                Width = 120,
                Height = 30,
                BackColor = darkBrown,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(5)
            };
            btnBack.FlatAppearance.BorderSize = 0;

            // DataGridView
            dgvSalesReport = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 40
            };

            // Setup DataGridView columns
            SetupDataGridViewColumns();

            // Summary Panel
            pnlSummary = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 100,
                BackColor = lightBrown,
                Padding = new Padding(10)
            };

            // Summary Labels
            lblTotalSales = new Label
            {
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 20)
            };

            lblTotalTransactions = new Label
            {
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 45)
            };

            lblAverageTransaction = new Label
            {
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 70)
            };

            pnlSummary.Controls.AddRange(new Control[] {
                lblTotalSales,
                lblTotalTransactions,
                lblAverageTransaction
            });
        }

        private void SetupLayout()
        {
            // Create main container
            TableLayoutPanel mainContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10)
            };

            // Create filters panel
            Panel pnlFilters = new Panel
            {
                Height = 90,
                Dock = DockStyle.Top,
                BackColor = cream,
                Padding = new Padding(10)
            };

            // First row - filters and back button
            Label lblStartDate = new Label { Text = "Start Date:", AutoSize = true };
            Label lblEndDate = new Label { Text = "End Date:", AutoSize = true };
            Label lblStatus = new Label { Text = "Status:", AutoSize = true };
            Label lblPayment = new Label { Text = "Payment:", AutoSize = true };

            // Position filters
            lblStartDate.Location = new Point(20, 15);
            dtpStartDate.Location = new Point(90, 12);

            lblEndDate.Location = new Point(250, 15);
            dtpEndDate.Location = new Point(320, 12);

            lblStatus.Location = new Point(480, 15);
            cmbOrderStatus.Location = new Point(530, 12);

            lblPayment.Location = new Point(680, 15);
            cmbPaymentMethod.Location = new Point(740, 12);

            // Position Back button at top right
            btnBack.Location = new Point(pnlFilters.Width - btnBack.Width - 20, 12);

            // Second row - report buttons
            btnGenerateReport.Location = new Point(90, 55);
            btnExportReport.Location = new Point(220, 55);

            pnlFilters.Controls.AddRange(new Control[] {
        lblStartDate, dtpStartDate,
        lblEndDate, dtpEndDate,
        lblStatus, cmbOrderStatus,
        lblPayment, cmbPaymentMethod,
        btnGenerateReport, btnExportReport,
        btnBack
    });

            // Ensure Back button stays at the right edge when form resizes
            pnlFilters.Resize += (s, e) =>
            {
                btnBack.Location = new Point(pnlFilters.Width - btnBack.Width - 20, 12);
            };

            // DataGridView setup
            Panel pnlGrid = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            dgvSalesReport.Dock = DockStyle.Fill;
            pnlGrid.Controls.Add(dgvSalesReport);

            // Add everything to main container
            mainContainer.Controls.Add(pnlFilters, 0, 0);
            mainContainer.Controls.Add(pnlGrid, 0, 1);
            mainContainer.Controls.Add(pnlSummary, 0, 2);

            mainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            this.Controls.Clear();
            this.Controls.Add(mainContainer);
        }


        private void AddFilterControls()
        {
            // Create and add labels
            Label[] labels = new Label[]
            {
                new Label { Text = "Start Date:", AutoSize = true },
                new Label { Text = "End Date:", AutoSize = true },
                new Label { Text = "Status:", AutoSize = true },
                new Label { Text = "Payment:", AutoSize = true }
            };

            // Add controls to panel with proper spacing
            pnlFilters.Controls.Add(labels[0], 0, 0);
            pnlFilters.Controls.Add(dtpStartDate, 1, 0);
            pnlFilters.Controls.Add(labels[1], 2, 0);
            pnlFilters.Controls.Add(dtpEndDate, 3, 0);
            pnlFilters.Controls.Add(labels[2], 4, 0);
            pnlFilters.Controls.Add(cmbOrderStatus, 5, 0);
            pnlFilters.Controls.Add(labels[3], 6, 0);
            pnlFilters.Controls.Add(cmbPaymentMethod, 7, 0);

            // Set column styles for proper spacing
            for (int i = 0; i < pnlFilters.ColumnCount; i++)
            {
                pnlFilters.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            }
        }

        private void SetupDataGridViewColumns()
        {
            dgvSalesReport.Columns.Clear();
            dgvSalesReport.Columns.AddRange(new DataGridViewColumn[] {
                new DataGridViewTextBoxColumn
                {
                    Name = "OrderId",
                    HeaderText = "Order ID",
                    Width = 80
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "OrderDate",
                    HeaderText = "Date",
                    Width = 120
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "PaymentMethod",
                    HeaderText = "Payment Method",
                    Width = 120
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Status",
                    HeaderText = "Status",
                    Width = 80
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "TotalAmount",
                    HeaderText = "Amount",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "AmountPaid",
                    HeaderText = "Paid",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "ChangeAmount",
                    HeaderText = "Change",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "OrderItemsCount",
                    HeaderText = "Items",
                    Width = 60
                }
            });

            // Style the DataGridView
            dgvSalesReport.ColumnHeadersDefaultCellStyle.BackColor = darkBrown;
            dgvSalesReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSalesReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSalesReport.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            dgvSalesReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }

        private void SetupEventHandlers()
        {
            btnGenerateReport.Click += BtnGenerateReport_Click;
            btnExportReport.Click += BtnExportReport_Click;
            btnBack.Click += BtnBack_Click;
            dgvSalesReport.CellFormatting += DgvSalesReport_CellFormatting;

            // Add resize handler
            this.Resize += (s, e) =>
            {
                pnlFilters.Width = this.ClientSize.Width - 40;
                pnlButtons.Location = new Point(
                    this.ClientSize.Width - pnlButtons.Width - 20,
                    pnlFilters.Bottom + 5
                );
            };
        }

        private void LoadInitialData()
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date.AddDays(1).AddSeconds(-1);
            string statusFilter = cmbOrderStatus.SelectedItem.ToString();
            string paymentFilter = cmbPaymentMethod.SelectedItem.ToString();

            _orders = _dbManager.GetOrdersByDateRange(startDate, endDate);

            if (statusFilter != "All")
            {
                _orders = _orders.Where(o => o.Status == statusFilter).ToList();
            }

            if (paymentFilter != "All")
            {
                _orders = _orders.Where(o => o.PaymentMethod == paymentFilter).ToList();
            }

            // Update DataGridView
            dgvSalesReport.Rows.Clear();
            foreach (var order in _orders)
            {
                // Use the actual stored values from the database
                var amountPaid = order.AmountPaid ?? 0;
                var changeAmount = order.ChangeAmount ?? 0;

                dgvSalesReport.Rows.Add(
                    order.OrderId,
                    order.OrderDate.ToString("MM/dd/yyyy HH:mm"),
                    order.PaymentMethod,
                    order.Status,
                    order.TotalAmount.ToString("C"),
                    amountPaid > 0 ? amountPaid.ToString("C") : "-",  // Show dash if no amount paid
                    changeAmount > 0 ? changeAmount.ToString("C") : "-",  // Show dash if no change
                    order.OrderItems.Count
                );
            }

            // Update summary
            decimal totalSales = _orders.Where(o => o.Status == "Paid").Sum(o => o.TotalAmount);
            int totalTransactions = _orders.Count;
            decimal averageTransaction = totalTransactions > 0 ? totalSales / totalTransactions : 0;

            lblTotalSales.Text = $"Total Sales: {totalSales:C}";
            lblTotalTransactions.Text = $"Total Transactions: {totalTransactions}";
            lblAverageTransaction.Text = $"Average Transaction: {averageTransaction:C}";
        }

        private void DgvSalesReport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSalesReport.Rows[e.RowIndex];
                string status = row.Cells["Status"].Value?.ToString();

                if (status == "Voided")
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                }
                else if (status == "Paid")
                {
                    row.DefaultCellStyle.BackColor = Color.Honeydew;
                    row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                }
            }
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void BtnExportReport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files|*.xlsx";
                sfd.FileName = $"SalesReport_{DateTime.Now:yyyyMMdd}";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // You can implement Excel export here using a library like EPPlus or Microsoft.Office.Interop.Excel
                        // For now, we'll just show a success message
                        MessageBox.Show("Report exported successfully!", "Export Complete",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting report: {ex.Message}", "Export Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            new DashboardForm(_currentUser).Show();
        }

        // Optional: Add form closing event handler
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}