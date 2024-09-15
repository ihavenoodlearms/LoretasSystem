namespace InventoryOrderSystem.Forms
{
    partial class PaymentForm
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
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblTransactionId = new System.Windows.Forms.Label();
            this.txtTransactionId = new System.Windows.Forms.TextBox();
            this.btnProcessPayment = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // lblAmount
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(12, 15);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(84, 13);
            this.lblAmount.TabIndex = 0;
            this.lblAmount.Text = "Amount to pay: ";

            // lblTransactionId
            this.lblTransactionId.AutoSize = true;
            this.lblTransactionId.Location = new System.Drawing.Point(12, 45);
            this.lblTransactionId.Name = "lblTransactionId";
            this.lblTransactionId.Size = new System.Drawing.Size(80, 13);
            this.lblTransactionId.TabIndex = 1;
            this.lblTransactionId.Text = "Transaction ID:";

            // txtTransactionId
            this.txtTransactionId.Location = new System.Drawing.Point(98, 42);
            this.txtTransactionId.Name = "txtTransactionId";
            this.txtTransactionId.Size = new System.Drawing.Size(190, 20);
            this.txtTransactionId.TabIndex = 2;

            // btnProcessPayment
            this.btnProcessPayment.Location = new System.Drawing.Point(98, 75);
            this.btnProcessPayment.Name = "btnProcessPayment";
            this.btnProcessPayment.Size = new System.Drawing.Size(120, 30);
            this.btnProcessPayment.TabIndex = 3;
            this.btnProcessPayment.Text = "Process Payment";
            this.btnProcessPayment.UseVisualStyleBackColor = true;
            this.btnProcessPayment.Click += new System.EventHandler(this.btnProcessPayment_Click);

            // PaymentForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 120);
            this.Controls.Add(this.btnProcessPayment);
            this.Controls.Add(this.txtTransactionId);
            this.Controls.Add(this.lblTransactionId);
            this.Controls.Add(this.lblAmount);
            this.Name = "PaymentForm";
            this.Text = "Process Payment";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblTransactionId;
        private System.Windows.Forms.TextBox txtTransactionId;
        private System.Windows.Forms.Button btnProcessPayment;
    }
}