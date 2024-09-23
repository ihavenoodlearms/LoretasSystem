namespace InventoryOrderSystem
{
    partial class LoginForm
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
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lnkForgotPassword = new System.Windows.Forms.LinkLabel();
            this.pnlImages = new System.Windows.Forms.Panel();
            this.txtUsername = new InventoryOrderSystem.PlaceholderTextBox();
            this.txtPassword = new InventoryOrderSystem.PlaceholderTextBox();
            this.pnlLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLogin
            // 
            this.pnlLogin.BackColor = System.Drawing.Color.White;
            this.pnlLogin.Controls.Add(this.lblSubtitle);
            this.pnlLogin.Controls.Add(this.lblLogin);
            this.pnlLogin.Controls.Add(this.txtUsername);
            this.pnlLogin.Controls.Add(this.txtPassword);
            this.pnlLogin.Controls.Add(this.btnLogin);
            this.pnlLogin.Controls.Add(this.lnkForgotPassword);
            this.pnlLogin.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlLogin.ForeColor = System.Drawing.SystemColors.Control;
            this.pnlLogin.Location = new System.Drawing.Point(331, 0);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Padding = new System.Windows.Forms.Padding(30);
            this.pnlLogin.Size = new System.Drawing.Size(310, 450);
            this.pnlLogin.TabIndex = 1;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.ForeColor = System.Drawing.Color.Gray;
            this.lblSubtitle.Location = new System.Drawing.Point(83, 74);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(124, 15);
            this.lblSubtitle.TabIndex = 4;
            this.lblSubtitle.Text = "COFFEE, SNACKS, TEA";
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Font = new System.Drawing.Font("Palatino Linotype", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.lblLogin.Location = new System.Drawing.Point(47, 30);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(207, 44);
            this.lblLogin.TabIndex = 3;
            this.lblLogin.Text = "Loreta\'s Cafe";
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(30, 267);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(240, 40);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "LOG IN";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lnkForgotPassword
            // 
            this.lnkForgotPassword.AutoSize = true;
            this.lnkForgotPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkForgotPassword.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(67)))), ((int)(((byte)(53)))));
            this.lnkForgotPassword.Location = new System.Drawing.Point(30, 317);
            this.lnkForgotPassword.Name = "lnkForgotPassword";
            this.lnkForgotPassword.Size = new System.Drawing.Size(121, 15);
            this.lnkForgotPassword.TabIndex = 4;
            this.lnkForgotPassword.TabStop = true;
            this.lnkForgotPassword.Text = "FORGET PASSWORD ?";
            // 
            // pnlImages
            // 
            this.pnlImages.AllowDrop = true;
            this.pnlImages.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlImages.BackColor = System.Drawing.Color.White;
            this.pnlImages.BackgroundImage = global::InventoryOrderSystem.App.Properties.Resources.loginimg;
            this.pnlImages.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlImages.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pnlImages.Location = new System.Drawing.Point(-132, -24);
            this.pnlImages.Name = "pnlImages";
            this.pnlImages.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlImages.Size = new System.Drawing.Size(595, 474);
            this.pnlImages.TabIndex = 0;
            this.pnlImages.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlImages_Paint);
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtUsername.ForeColor = System.Drawing.Color.Gray;
            this.txtUsername.IsPassword = false;
            this.txtUsername.Location = new System.Drawing.Point(30, 147);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.PlaceholderText = "USERNAME";
            this.txtUsername.Size = new System.Drawing.Size(240, 29);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.Text = "USERNAME";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPassword.ForeColor = System.Drawing.Color.Gray;
            this.txtPassword.IsPassword = true;
            this.txtPassword.Location = new System.Drawing.Point(30, 197);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PlaceholderText = "PASSWORD";
            this.txtPassword.Size = new System.Drawing.Size(240, 29);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Text = "PASSWORD";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 450);
            this.Controls.Add(this.pnlLogin);
            this.Controls.Add(this.pnlImages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.Label lblLogin;
        private InventoryOrderSystem.PlaceholderTextBox txtUsername;
        private InventoryOrderSystem.PlaceholderTextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel lnkForgotPassword;
        private System.Windows.Forms.Panel pnlImages;
        private System.Windows.Forms.Label lblSubtitle;
    }
}