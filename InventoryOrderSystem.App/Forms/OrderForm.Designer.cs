namespace InventoryOrderSystem.Forms
{
    partial class OrderForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.buttonPanel = new System.Windows.Forms.TableLayoutPanel();  // Changed to TableLayoutPanel
            this.btnNewOrder = new System.Windows.Forms.Button();
            this.btnViewOrder = new System.Windows.Forms.Button();
            this.btnVoidOrder = new System.Windows.Forms.Button();
            this.btnPayOrder = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.headerLabel = new System.Windows.Forms.Label();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();  // Added container panel

            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.buttonPanel.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainContainer
            // 
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowCount = 3;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Controls.Add(this.headerLabel, 0, 0);
            this.mainContainer.Controls.Add(this.dgvOrders, 0, 1);
            this.mainContainer.Controls.Add(this.buttonPanel, 0, 2);
            // 
            // dgvOrders
            // 
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrders.Margin = new System.Windows.Forms.Padding(12);
            this.dgvOrders.MultiSelect = false;
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // buttonPanel
            // 
            this.buttonPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(210)))), ((int)(((byte)(181)))));
            this.buttonPanel.ColumnCount = 6;
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.7F));
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPanel.Padding = new System.Windows.Forms.Padding(10);
            // 
            // btnNewOrder
            // 
            this.btnNewOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(123)))), ((int)(((byte)(122)))));
            this.btnNewOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewOrder.FlatAppearance.BorderSize = 0;
            this.btnNewOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNewOrder.ForeColor = System.Drawing.Color.White;
            this.btnNewOrder.Margin = new System.Windows.Forms.Padding(5);
            this.btnNewOrder.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnNewOrder.Text = "Make Order";
            // 
            // btnViewOrder
            // 
            this.btnViewOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(68)))), ((int)(((byte)(19)))));
            this.btnViewOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnViewOrder.FlatAppearance.BorderSize = 0;
            this.btnViewOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnViewOrder.ForeColor = System.Drawing.Color.White;
            this.btnViewOrder.Margin = new System.Windows.Forms.Padding(5);
            this.btnViewOrder.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnViewOrder.Text = "View Order";
            // 
            // btnVoidOrder
            // 
            this.btnVoidOrder.BackColor = System.Drawing.Color.Firebrick;
            this.btnVoidOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVoidOrder.FlatAppearance.BorderSize = 0;
            this.btnVoidOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoidOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnVoidOrder.ForeColor = System.Drawing.Color.White;
            this.btnVoidOrder.Margin = new System.Windows.Forms.Padding(5);
            this.btnVoidOrder.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnVoidOrder.Text = "Void Order";
            // 
            // btnPayOrder
            // 
            this.btnPayOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.btnPayOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPayOrder.FlatAppearance.BorderSize = 0;
            this.btnPayOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPayOrder.ForeColor = System.Drawing.Color.White;
            this.btnPayOrder.Margin = new System.Windows.Forms.Padding(5);
            this.btnPayOrder.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnPayOrder.Text = "Process Payment";
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Margin = new System.Windows.Forms.Padding(5);
            this.btnBack.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnBack.Text = "Back to Dashboard";
            // 
            // headerLabel
            // 
            this.headerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.headerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold);
            this.headerLabel.ForeColor = System.Drawing.Color.White;
            this.headerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.headerLabel.Text = "Order Management";
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(210)))), ((int)(((byte)(181)))));
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.mainContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;  // Changed to Sizable
            this.MaximizeBox = true;  // Allow maximizing
            this.MinimumSize = new System.Drawing.Size(800, 500);  // Set minimum size
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order Management";
            this.Load += new System.EventHandler(this.OrderForm_Load);
            this.Resize += new System.EventHandler(this.OrderForm_Resize);  // Add resize handler

            // Add buttons to buttonPanel
            this.buttonPanel.Controls.Add(this.btnNewOrder, 0, 0);
            this.buttonPanel.Controls.Add(this.btnViewOrder, 1, 0);
            this.buttonPanel.Controls.Add(this.btnPayOrder, 2, 0);
            this.buttonPanel.Controls.Add(this.btnVoidOrder, 3, 0);
            this.buttonPanel.Controls.Add(this.btnBack, 4, 0);

            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.buttonPanel.ResumeLayout(false);
            this.mainContainer.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.TableLayoutPanel buttonPanel;
        private System.Windows.Forms.Button btnNewOrder;
        private System.Windows.Forms.Button btnViewOrder;
        private System.Windows.Forms.Button btnVoidOrder;
        private System.Windows.Forms.Button btnPayOrder;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.TableLayoutPanel mainContainer;
    }
}