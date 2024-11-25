namespace InventoryOrderSystem.App.Forms
{
    partial class InventoryForm
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

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.inventoryGridView = new System.Windows.Forms.DataGridView();
            this.panelControls = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblRestock = new System.Windows.Forms.Label();
            this.lblRestockQuantity = new System.Windows.Forms.Label();
            this.txtRestockQuantity = new System.Windows.Forms.TextBox();
            this.btnRestock = new System.Windows.Forms.Button();
            this.txtItemId = new System.Windows.Forms.TextBox();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryGridView)).BeginInit();
            this.panelControls.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // inventoryGridView
            // 
            this.inventoryGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inventoryGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.inventoryGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.inventoryGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inventoryGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9.75F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(110)))), ((int)(((byte)(72)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.inventoryGridView.DefaultCellStyle = dataGridViewCellStyle5;
            this.inventoryGridView.Location = new System.Drawing.Point(12, 52);
            this.inventoryGridView.Name = "inventoryGridView";
            this.inventoryGridView.Size = new System.Drawing.Size(806, 536);
            this.inventoryGridView.TabIndex = 1;
            // 
            // panelControls
            // 
            this.panelControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(67)))), ((int)(((byte)(67)))));
            this.panelControls.Controls.Add(this.lblTitle);
            this.panelControls.Controls.Add(this.btnAdd);
            this.panelControls.Controls.Add(this.btnUpdate);
            this.panelControls.Controls.Add(this.btnDelete);
            this.panelControls.Controls.Add(this.lblRestock);
            this.panelControls.Controls.Add(this.lblRestockQuantity);
            this.panelControls.Controls.Add(this.txtRestockQuantity);
            this.panelControls.Controls.Add(this.btnRestock);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControls.Location = new System.Drawing.Point(830, 40);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(200, 560);
            this.panelControls.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(15, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(87, 22);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Controls";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(110)))), ((int)(((byte)(72)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(15, 60);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(170, 35);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add New Item";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(110)))), ((int)(((byte)(72)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(15, 105);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(170, 35);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Edit Item";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Firebrick;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(15, 150);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(170, 35);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete Item";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblRestock
            // 
            this.lblRestock.AutoSize = true;
            this.lblRestock.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
            this.lblRestock.ForeColor = System.Drawing.Color.White;
            this.lblRestock.Location = new System.Drawing.Point(15, 215);
            this.lblRestock.Name = "lblRestock";
            this.lblRestock.Size = new System.Drawing.Size(74, 18);
            this.lblRestock.TabIndex = 4;
            this.lblRestock.Text = "Restock";
            // 
            // lblRestockQuantity
            // 
            this.lblRestockQuantity.AutoSize = true;
            this.lblRestockQuantity.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblRestockQuantity.ForeColor = System.Drawing.Color.White;
            this.lblRestockQuantity.Location = new System.Drawing.Point(15, 245);
            this.lblRestockQuantity.Name = "lblRestockQuantity";
            this.lblRestockQuantity.Size = new System.Drawing.Size(60, 16);
            this.lblRestockQuantity.TabIndex = 5;
            this.lblRestockQuantity.Text = "Quantity:";
            // 
            // txtRestockQuantity
            // 
            this.txtRestockQuantity.Location = new System.Drawing.Point(15, 270);
            this.txtRestockQuantity.Name = "txtRestockQuantity";
            this.txtRestockQuantity.Size = new System.Drawing.Size(170, 20);
            this.txtRestockQuantity.TabIndex = 6;
            // 
            // btnRestock
            // 
            this.btnRestock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(110)))), ((int)(((byte)(72)))));
            this.btnRestock.FlatAppearance.BorderSize = 0;
            this.btnRestock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestock.Font = new System.Drawing.Font("Arial", 9.75F);
            this.btnRestock.ForeColor = System.Drawing.Color.White;
            this.btnRestock.Location = new System.Drawing.Point(15, 305);
            this.btnRestock.Name = "btnRestock";
            this.btnRestock.Size = new System.Drawing.Size(170, 35);
            this.btnRestock.TabIndex = 7;
            this.btnRestock.Text = "Restock";
            this.btnRestock.UseVisualStyleBackColor = false;
            this.btnRestock.Click += new System.EventHandler(this.btnRestock_Click);
            // 
            // txtItemId
            // 
            this.txtItemId.Location = new System.Drawing.Point(12, 12);
            this.txtItemId.Name = "txtItemId";
            this.txtItemId.Size = new System.Drawing.Size(100, 20);
            this.txtItemId.TabIndex = 3;
            this.txtItemId.Visible = false;
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.panelTitle.Controls.Add(this.btnBack);
            this.panelTitle.Controls.Add(this.lblFormTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1030, 40);
            this.panelTitle.TabIndex = 0;
            this.panelTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitle_MouseDown);
            this.panelTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelTitle_MouseMove);
            this.panelTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelTitle_MouseUp);
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(449, 11);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(191, 18);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "Inventory Management";
            // 
            // btnBack
            // 
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnBack.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBack.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBack.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(986, 5);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(35, 29);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "X";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // InventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1030, 600);
            this.Controls.Add(this.inventoryGridView);
            this.Controls.Add(this.panelControls);
            this.Controls.Add(this.panelTitle);
            this.Controls.Add(this.txtItemId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InventoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.inventoryGridView)).EndInit();
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.DataGridView inventoryGridView;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblRestock;
        private System.Windows.Forms.Label lblRestockQuantity;
        private System.Windows.Forms.TextBox txtRestockQuantity;
        private System.Windows.Forms.Button btnRestock;
        private System.Windows.Forms.TextBox txtItemId;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblFormTitle;
        private Guna.UI2.WinForms.Guna2Button btnBack;
    }
}