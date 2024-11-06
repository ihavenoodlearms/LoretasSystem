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
            this.buttonPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnNewOrder = new System.Windows.Forms.Button();
            this.btnViewOrder = new System.Windows.Forms.Button();
            this.btnPayOrder = new System.Windows.Forms.Button();
            this.btnVoidOrder = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.headerLabel = new System.Windows.Forms.Label();
            this.mainContainer = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.buttonPanel.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvOrders
            // 
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrders.Location = new System.Drawing.Point(12, 86);
            this.dgvOrders.Margin = new System.Windows.Forms.Padding(12);
            this.dgvOrders.MultiSelect = false;
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrders.Size = new System.Drawing.Size(976, 412);
            this.dgvOrders.TabIndex = 1;
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
            this.buttonPanel.Controls.Add(this.btnNewOrder, 0, 0);
            this.buttonPanel.Controls.Add(this.btnViewOrder, 1, 0);
            this.buttonPanel.Controls.Add(this.btnPayOrder, 2, 0);
            this.buttonPanel.Controls.Add(this.btnVoidOrder, 3, 0);
            this.buttonPanel.Controls.Add(this.btnBack, 4, 0);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPanel.Location = new System.Drawing.Point(3, 513);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Padding = new System.Windows.Forms.Padding(10);
            this.buttonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.buttonPanel.Size = new System.Drawing.Size(994, 84);
            this.buttonPanel.TabIndex = 2;
            // 
            // btnNewOrder
            // 
            this.btnNewOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(123)))), ((int)(((byte)(122)))));
            this.btnNewOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewOrder.FlatAppearance.BorderSize = 0;
            this.btnNewOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNewOrder.ForeColor = System.Drawing.Color.White;
            this.btnNewOrder.Location = new System.Drawing.Point(15, 15);
            this.btnNewOrder.Margin = new System.Windows.Forms.Padding(5);
            this.btnNewOrder.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnNewOrder.Name = "btnNewOrder";
            this.btnNewOrder.Size = new System.Drawing.Size(152, 54);
            this.btnNewOrder.TabIndex = 0;
            this.btnNewOrder.Text = "Make Order";
            this.btnNewOrder.UseVisualStyleBackColor = false;
            // 
            // btnViewOrder
            // 
            this.btnViewOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(68)))), ((int)(((byte)(19)))));
            this.btnViewOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnViewOrder.FlatAppearance.BorderSize = 0;
            this.btnViewOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnViewOrder.ForeColor = System.Drawing.Color.White;
            this.btnViewOrder.Location = new System.Drawing.Point(177, 15);
            this.btnViewOrder.Margin = new System.Windows.Forms.Padding(5);
            this.btnViewOrder.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnViewOrder.Name = "btnViewOrder";
            this.btnViewOrder.Size = new System.Drawing.Size(152, 54);
            this.btnViewOrder.TabIndex = 1;
            this.btnViewOrder.Text = "View Order";
            this.btnViewOrder.UseVisualStyleBackColor = false;
            // 
            // btnPayOrder
            // 
            this.btnPayOrder.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnPayOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPayOrder.FlatAppearance.BorderSize = 0;
            this.btnPayOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPayOrder.ForeColor = System.Drawing.Color.White;
            this.btnPayOrder.Location = new System.Drawing.Point(339, 15);
            this.btnPayOrder.Margin = new System.Windows.Forms.Padding(5);
            this.btnPayOrder.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnPayOrder.Name = "btnPayOrder";
            this.btnPayOrder.Size = new System.Drawing.Size(152, 54);
            this.btnPayOrder.TabIndex = 2;
            this.btnPayOrder.Text = "Process Payment";
            this.btnPayOrder.UseVisualStyleBackColor = false;
            // 
            // btnVoidOrder
            // 
            this.btnVoidOrder.BackColor = System.Drawing.Color.Firebrick;
            this.btnVoidOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVoidOrder.FlatAppearance.BorderSize = 0;
            this.btnVoidOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoidOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnVoidOrder.ForeColor = System.Drawing.Color.White;
            this.btnVoidOrder.Location = new System.Drawing.Point(501, 15);
            this.btnVoidOrder.Margin = new System.Windows.Forms.Padding(5);
            this.btnVoidOrder.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnVoidOrder.Name = "btnVoidOrder";
            this.btnVoidOrder.Size = new System.Drawing.Size(152, 54);
            this.btnVoidOrder.TabIndex = 3;
            this.btnVoidOrder.Text = "Void Order";
            this.btnVoidOrder.UseVisualStyleBackColor = false;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(663, 15);
            this.btnBack.Margin = new System.Windows.Forms.Padding(5);
            this.btnBack.MinimumSize = new System.Drawing.Size(100, 35);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(152, 54);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "Back to Dashboard";
            this.btnBack.UseVisualStyleBackColor = false;
            // 
            // headerLabel
            // 
            this.headerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.headerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold);
            this.headerLabel.ForeColor = System.Drawing.Color.White;
            this.headerLabel.Location = new System.Drawing.Point(3, 0);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(994, 74);
            this.headerLabel.TabIndex = 0;
            this.headerLabel.Text = "Order Management";
            this.headerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mainContainer
            // 
            this.mainContainer.ColumnCount = 1;
            this.mainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.Controls.Add(this.headerLabel, 0, 0);
            this.mainContainer.Controls.Add(this.dgvOrders, 0, 1);
            this.mainContainer.Controls.Add(this.buttonPanel, 0, 2);
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Location = new System.Drawing.Point(0, 0);
            this.mainContainer.Name = "mainContainer";
            this.mainContainer.RowCount = 3;
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.mainContainer.Size = new System.Drawing.Size(1000, 600);
            this.mainContainer.TabIndex = 0;
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(210)))), ((int)(((byte)(181)))));
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.mainContainer);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "OrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order Management";
            this.Load += new System.EventHandler(this.OrderForm_Load);
            this.Resize += new System.EventHandler(this.OrderForm_Resize);
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