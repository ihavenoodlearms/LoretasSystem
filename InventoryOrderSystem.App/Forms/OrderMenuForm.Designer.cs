using System;
using System.Drawing;

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
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.flowLayoutPanelCategories = new System.Windows.Forms.FlowLayoutPanel();
            this.panelProducts = new System.Windows.Forms.Panel();
            this.flowLayoutPanelProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.panelOrderSummary = new System.Windows.Forms.Panel();
            this.listBoxOrderSummary = new System.Windows.Forms.ListBox();
            this.labelTotal = new System.Windows.Forms.Label();
            this.buttonBackToDashboard = new System.Windows.Forms.Button();
            this.buttonProceedToPayment = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panelSidebar.SuspendLayout();
            this.panelProducts.SuspendLayout();
            this.panelOrderSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.panelSidebar.Controls.Add(this.flowLayoutPanelCategories);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(200, 800);
            this.panelSidebar.TabIndex = 0;
            // 
            // flowLayoutPanelCategories
            // 
            this.flowLayoutPanelCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelCategories.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelCategories.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelCategories.Name = "flowLayoutPanelCategories";
            this.flowLayoutPanelCategories.Size = new System.Drawing.Size(200, 800);
            this.flowLayoutPanelCategories.TabIndex = 0;
            // 
            // panelProducts
            // 
            this.panelProducts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(210)))), ((int)(((byte)(181)))));
            this.panelProducts.Controls.Add(this.flowLayoutPanelProducts);
            this.panelProducts.Location = new System.Drawing.Point(200, 0);
            this.panelProducts.Name = "panelProducts";
            this.panelProducts.Size = new System.Drawing.Size(1000, 600);
            this.panelProducts.TabIndex = 1;
            // 
            // flowLayoutPanelProducts
            // 
            this.flowLayoutPanelProducts.AutoScroll = true;
            this.flowLayoutPanelProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelProducts.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelProducts.Name = "flowLayoutPanelProducts";
            this.flowLayoutPanelProducts.Size = new System.Drawing.Size(1000, 600);
            this.flowLayoutPanelProducts.TabIndex = 0;
            // 
            // panelOrderSummary
            // 
            this.panelOrderSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(227)))), ((int)(((byte)(211)))));
            this.panelOrderSummary.Controls.Add(this.label1);
            this.panelOrderSummary.Controls.Add(this.listBoxOrderSummary);
            this.panelOrderSummary.Controls.Add(this.labelTotal);
            this.panelOrderSummary.Controls.Add(this.buttonBackToDashboard);
            this.panelOrderSummary.Controls.Add(this.buttonProceedToPayment);
            this.panelOrderSummary.Location = new System.Drawing.Point(200, 600);
            this.panelOrderSummary.Name = "panelOrderSummary";
            this.panelOrderSummary.Size = new System.Drawing.Size(1000, 200);
            this.panelOrderSummary.TabIndex = 2;
            // 
            // listBoxOrderSummary
            // 
            this.listBoxOrderSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(227)))), ((int)(((byte)(211)))));
            this.listBoxOrderSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxOrderSummary.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.listBoxOrderSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(20)))), ((int)(((byte)(16)))));
            this.listBoxOrderSummary.FormattingEnabled = true;
            this.listBoxOrderSummary.ItemHeight = 17;
            this.listBoxOrderSummary.Location = new System.Drawing.Point(20, 36);
            this.listBoxOrderSummary.Name = "listBoxOrderSummary";
            this.listBoxOrderSummary.Size = new System.Drawing.Size(960, 102);
            this.listBoxOrderSummary.TabIndex = 0;
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.labelTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(20)))), ((int)(((byte)(16)))));
            this.labelTotal.Location = new System.Drawing.Point(20, 140);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(115, 25);
            this.labelTotal.TabIndex = 1;
            this.labelTotal.Text = "Total: ₱0.00";
            // 
            // buttonBackToDashboard
            // 
            this.buttonBackToDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.buttonBackToDashboard.FlatAppearance.BorderSize = 0;
            this.buttonBackToDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBackToDashboard.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.buttonBackToDashboard.ForeColor = System.Drawing.Color.White;
            this.buttonBackToDashboard.Location = new System.Drawing.Point(674, 160);
            this.buttonBackToDashboard.Name = "buttonBackToDashboard";
            this.buttonBackToDashboard.Size = new System.Drawing.Size(150, 30);
            this.buttonBackToDashboard.TabIndex = 2;
            this.buttonBackToDashboard.Text = "Back to Dashboard";
            this.buttonBackToDashboard.UseVisualStyleBackColor = false;
            // 
            // buttonProceedToPayment
            // 
            this.buttonProceedToPayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(44)))), ((int)(((byte)(42)))));
            this.buttonProceedToPayment.FlatAppearance.BorderSize = 0;
            this.buttonProceedToPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProceedToPayment.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.buttonProceedToPayment.ForeColor = System.Drawing.Color.White;
            this.buttonProceedToPayment.Location = new System.Drawing.Point(830, 160);
            this.buttonProceedToPayment.Name = "buttonProceedToPayment";
            this.buttonProceedToPayment.Size = new System.Drawing.Size(150, 30);
            this.buttonProceedToPayment.TabIndex = 3;
            this.buttonProceedToPayment.Text = "Proceed to Payment";
            this.buttonProceedToPayment.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(20)))), ((int)(((byte)(16)))));
            this.label1.Location = new System.Drawing.Point(22, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Checklist";
            // 
            // OrderMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.panelOrderSummary);
            this.Controls.Add(this.panelProducts);
            this.Controls.Add(this.panelSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimumSize = new Size(1000, 600);
            this.Name = "OrderMenuForm";
            this.Resize += new EventHandler(OrderMenuForm_Resize);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loreta\'s Café - Order Menu";
            this.panelSidebar.ResumeLayout(false);
            this.panelProducts.ResumeLayout(false);
            this.panelOrderSummary.ResumeLayout(false);
            this.panelOrderSummary.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCategories;
        private System.Windows.Forms.Panel panelProducts;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelProducts;
        private System.Windows.Forms.Panel panelOrderSummary;
        private System.Windows.Forms.ListBox listBoxOrderSummary;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Button buttonBackToDashboard;
        private System.Windows.Forms.Button buttonProceedToPayment;
        private System.Windows.Forms.Label label1;
    }
}