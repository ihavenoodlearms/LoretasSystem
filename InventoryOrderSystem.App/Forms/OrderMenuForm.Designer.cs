namespace InventoryOrderingSystem
{
    partial class OrderMenuForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.listBoxOrderSummary = new System.Windows.Forms.ListBox();
            this.labelTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(560, 426);
            this.tabControl1.TabIndex = 0;
            // 
            // listBoxOrderSummary
            // 
            this.listBoxOrderSummary.FormattingEnabled = true;
            this.listBoxOrderSummary.ItemHeight = 15;
            this.listBoxOrderSummary.Location = new System.Drawing.Point(578, 12);
            this.listBoxOrderSummary.Name = "listBoxOrderSummary";
            this.listBoxOrderSummary.Size = new System.Drawing.Size(290, 394);
            this.listBoxOrderSummary.TabIndex = 2;
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelTotal.Location = new System.Drawing.Point(578, 417);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(89, 21);
            this.labelTotal.TabIndex = 3;
            this.labelTotal.Text = "Total: ₱0.00";
            // 
            // OrderMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 490); // Increased height to accommodate back button
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.listBoxOrderSummary);
            this.Controls.Add(this.tabControl1);
            this.Name = "OrderMenuForm";
            this.Text = "Loreta's Café - Order Menu";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ListBox listBoxOrderSummary;
        private System.Windows.Forms.Label labelTotal;
    }
}