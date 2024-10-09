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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.inventoryGridView = new System.Windows.Forms.DataGridView();
            this.panelItemDetails = new System.Windows.Forms.Panel();
            this.lblItemDetails = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblItemName = new System.Windows.Forms.Label();
            this.lblItemId = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.txtItemId = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panelRestock = new System.Windows.Forms.Panel();
            this.lblRestock = new System.Windows.Forms.Label();
            this.btnRestock = new System.Windows.Forms.Button();
            this.txtRestockQuantity = new System.Windows.Forms.TextBox();
            this.lblRestockQuantity = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryGridView)).BeginInit();
            this.panelItemDetails.SuspendLayout();
            this.panelRestock.SuspendLayout();
            this.SuspendLayout();
            // 
            // inventoryGridView
            // 
            this.inventoryGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inventoryGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.inventoryGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.inventoryGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inventoryGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(82)))), ((int)(((byte)(45)))));
            this.inventoryGridView.Location = new System.Drawing.Point(12, 50);
            this.inventoryGridView.Name = "inventoryGridView";
            this.inventoryGridView.Size = new System.Drawing.Size(776, 262);
            this.inventoryGridView.TabIndex = 0;
            this.inventoryGridView.SelectionChanged += new System.EventHandler(this.inventoryGridView_SelectionChanged);
            // 
            // panelItemDetails
            // 
            this.panelItemDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelItemDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(243)))), ((int)(((byte)(230)))));
            this.panelItemDetails.Controls.Add(this.lblItemDetails);
            this.panelItemDetails.Controls.Add(this.lblQuantity);
            this.panelItemDetails.Controls.Add(this.lblItemName);
            this.panelItemDetails.Controls.Add(this.lblItemId);
            this.panelItemDetails.Controls.Add(this.txtQuantity);
            this.panelItemDetails.Controls.Add(this.txtItemName);
            this.panelItemDetails.Controls.Add(this.txtItemId);
            this.panelItemDetails.Controls.Add(this.btnAdd);
            this.panelItemDetails.Controls.Add(this.btnUpdate);
            this.panelItemDetails.Controls.Add(this.btnDelete);
            this.panelItemDetails.Location = new System.Drawing.Point(12, 318);
            this.panelItemDetails.Name = "panelItemDetails";
            this.panelItemDetails.Size = new System.Drawing.Size(500, 170);
            this.panelItemDetails.TabIndex = 1;
            // 
            // lblItemDetails
            // 
            this.lblItemDetails.AutoSize = true;
            this.lblItemDetails.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.lblItemDetails.Location = new System.Drawing.Point(3, 10);
            this.lblItemDetails.Name = "lblItemDetails";
            this.lblItemDetails.Size = new System.Drawing.Size(102, 21);
            this.lblItemDetails.TabIndex = 8;
            this.lblItemDetails.Text = "Item Details";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.lblQuantity.Location = new System.Drawing.Point(20, 103);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(56, 15);
            this.lblQuantity.TabIndex = 6;
            this.lblQuantity.Text = "Quantity:";
            // 
            // lblItemName
            // 
            this.lblItemName.AutoSize = true;
            this.lblItemName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.lblItemName.Location = new System.Drawing.Point(20, 73);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(69, 15);
            this.lblItemName.TabIndex = 5;
            this.lblItemName.Text = "Item Name:";
            // 
            // lblItemId
            // 
            this.lblItemId.AutoSize = true;
            this.lblItemId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.lblItemId.Location = new System.Drawing.Point(20, 43);
            this.lblItemId.Name = "lblItemId";
            this.lblItemId.Size = new System.Drawing.Size(48, 15);
            this.lblItemId.TabIndex = 4;
            this.lblItemId.Text = "Item ID:";
            // 
            // txtQuantity
            // 
            this.txtQuantity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.txtQuantity.Location = new System.Drawing.Point(100, 100);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(100, 23);
            this.txtQuantity.TabIndex = 2;
            // 
            // txtItemName
            // 
            this.txtItemName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.txtItemName.Location = new System.Drawing.Point(100, 70);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(200, 23);
            this.txtItemName.TabIndex = 1;
            // 
            // txtItemId
            // 
            this.txtItemId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.txtItemId.Location = new System.Drawing.Point(100, 40);
            this.txtItemId.Name = "txtItemId";
            this.txtItemId.ReadOnly = true;
            this.txtItemId.Size = new System.Drawing.Size(100, 23);
            this.txtItemId.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(100, 130);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 30);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.btnUpdate.Location = new System.Drawing.Point(185, 130);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 30);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(270, 130);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 30);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // panelRestock
            // 
            this.panelRestock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRestock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(243)))), ((int)(((byte)(230)))));
            this.panelRestock.Controls.Add(this.lblRestock);
            this.panelRestock.Controls.Add(this.btnRestock);
            this.panelRestock.Controls.Add(this.txtRestockQuantity);
            this.panelRestock.Controls.Add(this.lblRestockQuantity);
            this.panelRestock.Location = new System.Drawing.Point(518, 318);
            this.panelRestock.Name = "panelRestock";
            this.panelRestock.Size = new System.Drawing.Size(270, 170);
            this.panelRestock.TabIndex = 2;
            // 
            // lblRestock
            // 
            this.lblRestock.AutoSize = true;
            this.lblRestock.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRestock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.lblRestock.Location = new System.Drawing.Point(3, 10);
            this.lblRestock.Name = "lblRestock";
            this.lblRestock.Size = new System.Drawing.Size(69, 21);
            this.lblRestock.TabIndex = 9;
            this.lblRestock.Text = "Restock";
            // 
            // btnRestock
            // 
            this.btnRestock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnRestock.FlatAppearance.BorderSize = 0;
            this.btnRestock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestock.ForeColor = System.Drawing.Color.White;
            this.btnRestock.Location = new System.Drawing.Point(20, 100);
            this.btnRestock.Name = "btnRestock";
            this.btnRestock.Size = new System.Drawing.Size(75, 30);
            this.btnRestock.TabIndex = 7;
            this.btnRestock.Text = "Restock";
            this.btnRestock.UseVisualStyleBackColor = false;
            this.btnRestock.Click += new System.EventHandler(this.btnRestock_Click);
            // 
            // txtRestockQuantity
            // 
            this.txtRestockQuantity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.txtRestockQuantity.Location = new System.Drawing.Point(20, 70);
            this.txtRestockQuantity.Name = "txtRestockQuantity";
            this.txtRestockQuantity.Size = new System.Drawing.Size(100, 23);
            this.txtRestockQuantity.TabIndex = 6;
            // 
            // lblRestockQuantity
            // 
            this.lblRestockQuantity.AutoSize = true;
            this.lblRestockQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(33)))));
            this.lblRestockQuantity.Location = new System.Drawing.Point(20, 50);
            this.lblRestockQuantity.Name = "lblRestockQuantity";
            this.lblRestockQuantity.Size = new System.Drawing.Size(100, 15);
            this.lblRestockQuantity.TabIndex = 8;
            this.lblRestockQuantity.Text = "Restock Quantity:";
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(12, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(125, 30);
            this.btnBack.TabIndex = 10;
            this.btnBack.Text = "Back to  Dashboard";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // InventoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.panelRestock);
            this.Controls.Add(this.panelItemDetails);
            this.Controls.Add(this.inventoryGridView);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "InventoryForm";
            this.Text = "Inventory Management";
            this.Load += new System.EventHandler(this.InventoryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inventoryGridView)).EndInit();
            this.panelItemDetails.ResumeLayout(false);
            this.panelItemDetails.PerformLayout();
            this.panelRestock.ResumeLayout(false);
            this.panelRestock.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.DataGridView inventoryGridView;
        private System.Windows.Forms.Panel panelItemDetails;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.Label lblItemId;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.TextBox txtItemId;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel panelRestock;
        private System.Windows.Forms.Button btnRestock;
        private System.Windows.Forms.TextBox txtRestockQuantity;
        private System.Windows.Forms.Label lblRestockQuantity;
        private System.Windows.Forms.Label lblItemDetails;
        private System.Windows.Forms.Label lblRestock;
    }
}