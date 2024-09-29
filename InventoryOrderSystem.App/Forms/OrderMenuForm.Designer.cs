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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.logoLabel = new System.Windows.Forms.Label();
            this.panelCategories = new System.Windows.Forms.Panel();
            this.btnFrappes = new System.Windows.Forms.Button();
            this.btnSnacks = new System.Windows.Forms.Button();
            this.btnCoffeesTeas = new System.Windows.Forms.Button();
            this.labelIcedCoffees = new System.Windows.Forms.Label();
            this.flowLayoutPanelProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.panelHeader.SuspendLayout();
            this.panelCategories.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.logoLabel);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(686, 52);
            this.panelHeader.TabIndex = 0;
            // 
            // logoLabel
            // 
            this.logoLabel.AutoSize = true;
            this.logoLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.logoLabel.Location = new System.Drawing.Point(10, 8);
            this.logoLabel.Name = "logoLabel";
            this.logoLabel.Size = new System.Drawing.Size(160, 32);
            this.logoLabel.TabIndex = 0;
            this.logoLabel.Text = "Loreta\'s Café";
            // 
            // panelCategories
            // 
            this.panelCategories.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelCategories.Controls.Add(this.btnFrappes);
            this.panelCategories.Controls.Add(this.btnSnacks);
            this.panelCategories.Controls.Add(this.btnCoffeesTeas);
            this.panelCategories.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCategories.Location = new System.Drawing.Point(0, 52);
            this.panelCategories.Name = "panelCategories";
            this.panelCategories.Size = new System.Drawing.Size(686, 43);
            this.panelCategories.TabIndex = 1;
            // 
            // btnFrappes
            // 
            this.btnFrappes.FlatAppearance.BorderSize = 0;
            this.btnFrappes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFrappes.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnFrappes.Location = new System.Drawing.Point(343, 0);
            this.btnFrappes.Name = "btnFrappes";
            this.btnFrappes.Size = new System.Drawing.Size(257, 43);
            this.btnFrappes.TabIndex = 2;
            this.btnFrappes.Text = "FRAPPES, FRUIT MILKS, & MORE";
            this.btnFrappes.UseVisualStyleBackColor = true;
            // 
            // btnSnacks
            // 
            this.btnSnacks.FlatAppearance.BorderSize = 0;
            this.btnSnacks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSnacks.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnSnacks.Location = new System.Drawing.Point(171, 0);
            this.btnSnacks.Name = "btnSnacks";
            this.btnSnacks.Size = new System.Drawing.Size(171, 43);
            this.btnSnacks.TabIndex = 1;
            this.btnSnacks.Text = "SNACKS";
            this.btnSnacks.UseVisualStyleBackColor = true;
            // 
            // btnCoffeesTeas
            // 
            this.btnCoffeesTeas.FlatAppearance.BorderSize = 0;
            this.btnCoffeesTeas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCoffeesTeas.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCoffeesTeas.Location = new System.Drawing.Point(0, 0);
            this.btnCoffeesTeas.Name = "btnCoffeesTeas";
            this.btnCoffeesTeas.Size = new System.Drawing.Size(171, 43);
            this.btnCoffeesTeas.TabIndex = 0;
            this.btnCoffeesTeas.Text = "COFFEES & TEAS";
            this.btnCoffeesTeas.UseVisualStyleBackColor = true;
            // 
            // labelIcedCoffees
            // 
            this.labelIcedCoffees.AutoSize = true;
            this.labelIcedCoffees.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelIcedCoffees.Location = new System.Drawing.Point(10, 104);
            this.labelIcedCoffees.Name = "labelIcedCoffees";
            this.labelIcedCoffees.Size = new System.Drawing.Size(136, 25);
            this.labelIcedCoffees.TabIndex = 2;
            this.labelIcedCoffees.Text = "ICED COFFEES";
            // 
            // flowLayoutPanelProducts
            // 
            this.flowLayoutPanelProducts.AutoScroll = true;
            this.flowLayoutPanelProducts.Location = new System.Drawing.Point(10, 130);
            this.flowLayoutPanelProducts.Name = "flowLayoutPanelProducts";
            this.flowLayoutPanelProducts.Size = new System.Drawing.Size(665, 347);
            this.flowLayoutPanelProducts.TabIndex = 3;
            // 
            // OrderMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 520);
            this.Controls.Add(this.flowLayoutPanelProducts);
            this.Controls.Add(this.labelIcedCoffees);
            this.Controls.Add(this.panelCategories);
            this.Controls.Add(this.panelHeader);
            this.Name = "OrderMenuForm";
            this.Text = "Loreta\'s Café - Order Menu";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelCategories.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label logoLabel;
        private System.Windows.Forms.Panel panelCategories;
        private System.Windows.Forms.Button btnFrappes;
        private System.Windows.Forms.Button btnSnacks;
        private System.Windows.Forms.Button btnCoffeesTeas;
        private System.Windows.Forms.Label labelIcedCoffees;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelProducts;
    }
}