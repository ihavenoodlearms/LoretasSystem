namespace InventoryOrderSystem.App.Forms
{
    partial class ProductManagementForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();

            // cmbCategories
            this.cmbCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategories.Location = new System.Drawing.Point(20, 20);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(300, 30);

            // dgvProducts
            this.dgvProducts.Location = new System.Drawing.Point(20, 60);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.Size = new System.Drawing.Size(740, 400);
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.ReadOnly = true;

            // btnAdd
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(20, 480);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(160, 40);
            this.btnAdd.Text = "Add Product";
            this.btnAdd.UseVisualStyleBackColor = false;

            // btnEdit
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(200, 480);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(160, 40);
            this.btnEdit.Text = "Edit Product";
            this.btnEdit.UseVisualStyleBackColor = false;

            // btnDelete
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(380, 480);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(160, 40);
            this.btnDelete.Text = "Delete Product";
            this.btnDelete.UseVisualStyleBackColor = false;

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(560, 480);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 40);
            this.btnSave.Text = "Save Changes";
            this.btnSave.UseVisualStyleBackColor = false;

            // ProductManagementForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 550);
            this.Controls.Add(this.cmbCategories);
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ProductManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.ComboBox cmbCategories;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
    }
}