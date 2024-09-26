namespace InventoryOrderSystem.Forms
{
    partial class DashboardForm 
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
            this.btnInventory = new System.Windows.Forms.Button();
            this.btnOrders = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.panel15 = new System.Windows.Forms.Panel();
            this.panel16 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInventory
            // 
            this.btnInventory.Location = new System.Drawing.Point(0, 133);
            this.btnInventory.Name = "btnInventory";
            this.btnInventory.Size = new System.Drawing.Size(148, 50);
            this.btnInventory.TabIndex = 0;
            this.btnInventory.Text = "Inventory Management";
            this.btnInventory.UseVisualStyleBackColor = true;
            this.btnInventory.Click += new System.EventHandler(this.btnInventory_Click);
            // 
            // btnOrders
            // 
            this.btnOrders.Location = new System.Drawing.Point(0, 206);
            this.btnOrders.Name = "btnOrders";
            this.btnOrders.Size = new System.Drawing.Size(148, 50);
            this.btnOrders.TabIndex = 1;
            this.btnOrders.Text = "Order Management";
            this.btnOrders.UseVisualStyleBackColor = true;
            this.btnOrders.Click += new System.EventHandler(this.btnOrders_Click);
            // 
            // btnReports
            // 
            this.btnReports.Location = new System.Drawing.Point(0, 278);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(148, 50);
            this.btnReports.TabIndex = 2;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.Location = new System.Drawing.Point(663, 41);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(101, 24);
            this.lblWelcome.TabIndex = 3;
            this.lblWelcome.Text = "Welcome, ";
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(0, 343);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(148, 50);
            this.btnSettings.TabIndex = 4;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOrders);
            this.panel1.Controls.Add(this.btnSettings);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btnInventory);
            this.panel1.Controls.Add(this.btnReports);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(148, 729);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblWelcome);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(148, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(860, 100);
            this.panel2.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 32);
            this.label1.TabIndex = 7;
            this.label1.Text = "Loreta\'s Cafe";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.08425F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.91575F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.Controls.Add(this.panel16, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel15, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel14, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel13, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel12, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel11, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel10, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel9, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel8, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(189, 327);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(771, 262);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(148, 629);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 100);
            this.panel3.TabIndex = 8;
            // 
            // panel4
            // 
            this.panel4.Location = new System.Drawing.Point(808, 683);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 46);
            this.panel4.TabIndex = 9;
            // 
            // panel5
            // 
            this.panel5.Location = new System.Drawing.Point(348, 683);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(463, 46);
            this.panel5.TabIndex = 10;
            // 
            // panel6
            // 
            this.panel6.Location = new System.Drawing.Point(148, 683);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(200, 46);
            this.panel6.TabIndex = 11;
            // 
            // panel7
            // 
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(147, 125);
            this.panel7.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Location = new System.Drawing.Point(156, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(153, 125);
            this.panel8.TabIndex = 1;
            // 
            // panel9
            // 
            this.panel9.Location = new System.Drawing.Point(315, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(150, 125);
            this.panel9.TabIndex = 2;
            // 
            // panel10
            // 
            this.panel10.Location = new System.Drawing.Point(471, 3);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(146, 125);
            this.panel10.TabIndex = 3;
            // 
            // panel11
            // 
            this.panel11.Location = new System.Drawing.Point(623, 3);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(145, 125);
            this.panel11.TabIndex = 4;
            // 
            // panel12
            // 
            this.panel12.Location = new System.Drawing.Point(3, 134);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(147, 125);
            this.panel12.TabIndex = 5;
            // 
            // panel13
            // 
            this.panel13.Location = new System.Drawing.Point(156, 134);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(153, 125);
            this.panel13.TabIndex = 6;
            // 
            // panel14
            // 
            this.panel14.Location = new System.Drawing.Point(315, 134);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(150, 125);
            this.panel14.TabIndex = 7;
            // 
            // panel15
            // 
            this.panel15.Location = new System.Drawing.Point(471, 134);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(146, 125);
            this.panel15.TabIndex = 8;
            // 
            // panel16
            // 
            this.panel16.Location = new System.Drawing.Point(623, 134);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(145, 125);
            this.panel16.TabIndex = 9;
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "DashboardForm";
            this.Text = "Dashboard - Loreta\'s Cafe";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInventory;
        private System.Windows.Forms.Button btnOrders;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
    }
}