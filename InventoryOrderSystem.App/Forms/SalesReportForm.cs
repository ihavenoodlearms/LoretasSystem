using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using InventoryOrderSystem.App.Forms;
using System.IO;


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
        private readonly Color darkBrown = Color.FromArgb(59, 48, 48);
        private readonly Color cream = Color.FromArgb(255, 240, 209);
        private readonly Color lightBrown = Color.FromArgb(102, 67, 67);

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
            this.FormBorderStyle = FormBorderStyle.None;
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
                Font = new Font(this.Font.FontFamily, 9, FontStyle.Bold),
                Width = 120
            };

            dtpEndDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today,
                Font = new Font(this.Font.FontFamily, 9, FontStyle.Bold),
                Width = 120
            };

            // Combo Boxes
            cmbOrderStatus = new ComboBox
            {
                Width = 120,
                Font = new Font(this.Font.FontFamily, 9, FontStyle.Bold),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbOrderStatus.Items.AddRange(new object[] { "All", "Paid", "Voided" });
            cmbOrderStatus.SelectedIndex = 0;

            // Update this in CreateControls() method where you initialize cmbPaymentMethod
            cmbPaymentMethod = new ComboBox
            {
                Width = 120,
                Font = new Font(this.Font.FontFamily, 9, FontStyle.Bold),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Update the items to match exactly how they're stored in the database
            cmbPaymentMethod.Items.AddRange(new object[] { "All", "Cash", "GCash" });
            cmbPaymentMethod.SelectedIndex = 0;

            // Buttons
            btnGenerateReport = new Button
            {
                Text = "Generate Report",
                Width = 120,
                Height = 30,
                BackColor = darkBrown,
                Font = new Font(this.Font.FontFamily, 8, FontStyle.Bold),
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
                Font = new Font(this.Font.FontFamily, 9, FontStyle.Bold),
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
                Font = new Font(this.Font.FontFamily, 9, FontStyle.Bold),
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
                Font = new Font(this.Font.FontFamily, 16, FontStyle.Bold),
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
                Font = new Font("Arial Rounded MT", 12, FontStyle.Bold),
                Location = new Point(10, 20)
            };

            lblTotalTransactions = new Label
            {
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Arial Rounded MT", 12, FontStyle.Bold),
                Location = new Point(10, 45)
            };

            lblAverageTransaction = new Label
            {
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Arial Rounded MT", 12, FontStyle.Bold),
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
            dgvSalesReport.ColumnHeadersDefaultCellStyle.Font = new Font("Arial Rounded MT", 9, FontStyle.Bold);
            dgvSalesReport.DefaultCellStyle.Font = new Font("Arial Rounded MT", 7, FontStyle.Bold);
            dgvSalesReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvSalesReport.BackColor = Color.FromArgb(250, 250, 250);
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

            // Get all orders within the date range
            _orders = _dbManager.GetOrdersByDateRange(startDate, endDate);

            // Apply status filter
            if (statusFilter != "All")
            {
                _orders = _orders.Where(o => o.Status == statusFilter).ToList();
            }

            // Apply payment method filter
            if (paymentFilter != "All")
            {
                _orders = _orders.Where(o => o.PaymentMethod.Equals(paymentFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Update DataGridView
            dgvSalesReport.Rows.Clear();
            foreach (var order in _orders)
            {
                var amountPaid = order.AmountPaid ?? 0;
                var changeAmount = order.ChangeAmount ?? 0;

                dgvSalesReport.Rows.Add(
                    order.OrderId,
                    order.OrderDate.ToString("MM/dd/yyyy HH:mm"),
                    order.PaymentMethod,
                    order.Status,
                    order.TotalAmount.ToString("C"),
                    amountPaid > 0 ? amountPaid.ToString("C") : "-",
                    changeAmount > 0 ? changeAmount.ToString("C") : "-",
                    order.OrderItems.Count
                );
            }

            // Update summary with filtered results only
            decimal totalSales = _orders.Where(o => o.Status == "Paid").Sum(o => o.TotalAmount);
            int totalTransactions = _orders.Count;
            decimal averageTransaction = totalTransactions > 0 ? totalSales / totalTransactions : 0;

            lblTotalSales.Text = $"Total Sales: {totalSales:C}";
            lblTotalTransactions.Text = $"Total Transactions: {totalTransactions}";
            lblAverageTransaction.Text = $"Average Transaction: {averageTransaction:C}";

            // Style rows based on payment method
            foreach (DataGridViewRow row in dgvSalesReport.Rows)
            {
                var paymentMethod = row.Cells["PaymentMethod"].Value?.ToString();
                var status = row.Cells["Status"].Value?.ToString();

                if (paymentMethod == "GCash")
                {
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                }
                else if (status == "Voided")
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

        private void DgvSalesReport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSalesReport.Rows[e.RowIndex];
                string status = row.Cells["Status"].Value?.ToString();
                string paymentMethod = row.Cells["PaymentMethod"].Value?.ToString();

                if (paymentMethod == "GCash")
                {
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                    if (status != "Voided")
                    {
                        row.DefaultCellStyle.ForeColor = Color.DarkBlue;
                    }
                }
                else if (status == "Voided")
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
                        // Set EPPlus license context
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        using (var package = new ExcelPackage())
                        {
                            var worksheet = package.Workbook.Worksheets.Add("Sales Report");

                            // Add headers
                            string[] headers = new string[] {
                        "Order ID", "Date", "Payment Method", "Status",
                        "Amount", "Paid", "Change", "Items"
                    };

                            for (int i = 0; i < headers.Length; i++)
                            {
                                worksheet.Cells[1, i + 1].Value = headers[i];
                                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                                worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(59, 48, 48));
                                worksheet.Cells[1, i + 1].Style.Font.Color.SetColor(Color.White);
                            }

                            // Add data
                            int row = 2;
                            foreach (var order in _orders)
                            {
                                worksheet.Cells[row, 1].Value = order.OrderId;
                                worksheet.Cells[row, 2].Value = order.OrderDate.ToString("MM/dd/yyyy HH:mm");
                                worksheet.Cells[row, 3].Value = order.PaymentMethod;
                                worksheet.Cells[row, 4].Value = order.Status;
                                worksheet.Cells[row, 5].Value = order.TotalAmount;
                                worksheet.Cells[row, 6].Value = order.AmountPaid ?? 0;
                                worksheet.Cells[row, 7].Value = order.ChangeAmount ?? 0;
                                worksheet.Cells[row, 8].Value = order.OrderItems.Count;

                                // Format currency cells
                                worksheet.Cells[row, 5].Style.Numberformat.Format = "₱#,##0.00";
                                worksheet.Cells[row, 6].Style.Numberformat.Format = "₱#,##0.00";
                                worksheet.Cells[row, 7].Style.Numberformat.Format = "₱#,##0.00";

                                // Add conditional formatting for Status
                                if (order.Status == "Voided")
                                {
                                    var range = worksheet.Cells[row, 1, row, 8];
                                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    range.Style.Fill.BackgroundColor.SetColor(Color.MistyRose);
                                    range.Style.Font.Color.SetColor(Color.DarkRed);
                                }
                                else if (order.Status == "Paid")
                                {
                                    var range = worksheet.Cells[row, 1, row, 8];
                                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    range.Style.Fill.BackgroundColor.SetColor(Color.Honeydew);
                                    range.Style.Font.Color.SetColor(Color.DarkGreen);
                                }

                                row++;
                            }

                            // Add summary section
                            row += 2; // Add some space
                            worksheet.Cells[row, 1].Value = "Summary";
                            worksheet.Cells[row, 1].Style.Font.Bold = true;

                            worksheet.Cells[row + 1, 1].Value = "Total Sales:";
                            worksheet.Cells[row + 1, 2].Value = _orders.Where(o => o.Status == "Paid").Sum(o => o.TotalAmount);
                            worksheet.Cells[row + 1, 2].Style.Numberformat.Format = "₱#,##0.00";

                            worksheet.Cells[row + 2, 1].Value = "Total Transactions:";
                            worksheet.Cells[row + 2, 2].Value = _orders.Count;

                            worksheet.Cells[row + 3, 1].Value = "Average Transaction:";
                            worksheet.Cells[row + 3, 2].Value = _orders.Count > 0 ?
                                _orders.Where(o => o.Status == "Paid").Sum(o => o.TotalAmount) / _orders.Count : 0;
                            worksheet.Cells[row + 3, 2].Style.Numberformat.Format = "₱#,##0.00";

                            // Auto-fit columns
                            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                            // Save the file
                            package.SaveAs(new FileInfo(sfd.FileName));
                        }

                        MessageBox.Show("Report exported successfully!", "Export Complete",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Optionally open the file
                        if (MessageBox.Show("Would you like to open the exported file?", "Open File",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(sfd.FileName);
                        }
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
            
            if (_currentUser.Role == "Admin")
            {
                this.Hide();
                DashboardForm adminDashboard = new DashboardForm(_currentUser); 
                adminDashboard.Show();
            }
            else if (_currentUser.Role == "User")
            {
              
                this.Hide();
                UserForm userDashboard = new UserForm(_currentUser);  
                userDashboard.Show();
            }
            else
            {
                MessageBox.Show("Unknown role. Please contact the administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Optional: Add form closing event handler
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}