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
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.btnNewOrder = new System.Windows.Forms.Button();
            this.btnViewOrder = new System.Windows.Forms.Button();
            this.btnVoidOrder = new System.Windows.Forms.Button();
            this.btnPayOrder = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.headerLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvOrders
            // 
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrders.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
            this.buttonPanel.Controls.Add(this.btnNewOrder);
            this.buttonPanel.Controls.Add(this.btnViewOrder);
            this.buttonPanel.Controls.Add(this.btnVoidOrder);
            this.buttonPanel.Controls.Add(this.btnPayOrder);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 510);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(1000, 90);
            this.buttonPanel.TabIndex = 2;
            // 
            // btnNewOrder
            // 
            this.btnNewOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(123)))), ((int)(((byte)(122)))));
            this.btnNewOrder.FlatAppearance.BorderSize = 0;
            this.btnNewOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNewOrder.ForeColor = System.Drawing.Color.White;
            this.btnNewOrder.Location = new System.Drawing.Point(12, 27);
            this.btnNewOrder.Name = "btnNewOrder";
            this.btnNewOrder.Size = new System.Drawing.Size(150, 35);
            this.btnNewOrder.TabIndex = 0;
            this.btnNewOrder.Text = "Make Order";
            this.btnNewOrder.UseVisualStyleBackColor = false;
            // 
            // btnViewOrder
            // 
            this.btnViewOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(68)))), ((int)(((byte)(19)))));
            this.btnViewOrder.FlatAppearance.BorderSize = 0;
            this.btnViewOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnViewOrder.ForeColor = System.Drawing.Color.White;
            this.btnViewOrder.Location = new System.Drawing.Point(168, 27);
            this.btnViewOrder.Name = "btnViewOrder";
            this.btnViewOrder.Size = new System.Drawing.Size(150, 35);
            this.btnViewOrder.TabIndex = 1;
            this.btnViewOrder.Text = "View Order";
            this.btnViewOrder.UseVisualStyleBackColor = false;
            // 
            // btnVoidOrder
            // 
            this.btnVoidOrder.BackColor = System.Drawing.Color.Firebrick;
            this.btnVoidOrder.FlatAppearance.BorderSize = 0;
            this.btnVoidOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoidOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnVoidOrder.ForeColor = System.Drawing.Color.White;
            this.btnVoidOrder.Location = new System.Drawing.Point(838, 27);
            this.btnVoidOrder.Name = "btnVoidOrder";
            this.btnVoidOrder.Size = new System.Drawing.Size(150, 35);
            this.btnVoidOrder.TabIndex = 2;
            this.btnVoidOrder.Text = "Void Order";
            this.btnVoidOrder.UseVisualStyleBackColor = false;
            // 
            // btnPayOrder
            // 
            this.btnPayOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.btnPayOrder.FlatAppearance.BorderSize = 0;
            this.btnPayOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPayOrder.ForeColor = System.Drawing.Color.White;
            this.btnPayOrder.Location = new System.Drawing.Point(324, 27);
            this.btnPayOrder.Name = "btnPayOrder";
            this.btnPayOrder.Size = new System.Drawing.Size(150, 35);
            this.btnPayOrder.TabIndex = 3;
            this.btnPayOrder.Text = "Process Payment";
            this.btnPayOrder.UseVisualStyleBackColor = false;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(0, 18);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(150, 35);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "Back to Dashboard";
            this.btnBack.UseVisualStyleBackColor = false;
            // 
            // headerLabel
            // 
            this.headerLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.headerLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold);
            this.headerLabel.ForeColor = System.Drawing.Color.White;
            this.headerLabel.Location = new System.Drawing.Point(0, 0);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(1000, 74);
            this.headerLabel.TabIndex = 0;
            this.headerLabel.Text = "Order Management";
            this.headerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(210)))), ((int)(((byte)(181)))));
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.headerLabel);
            this.Controls.Add(this.dgvOrders);
            this.Controls.Add(this.buttonPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "OrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Order Management";
            this.Load += new System.EventHandler(this.OrderForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button btnNewOrder;
        private System.Windows.Forms.Button btnViewOrder;
        private System.Windows.Forms.Button btnVoidOrder;
        private System.Windows.Forms.Button btnPayOrder;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label headerLabel;
    }
}