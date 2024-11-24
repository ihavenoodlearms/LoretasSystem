using System;
using System.Windows.Forms;
using System.Data.SQLite;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;
using System.Drawing;
using System.Drawing.Drawing2D;
using InventoryOrderSystem.App.Forms;

namespace InventoryOrderSystem
{
    public partial class LoginForm : Form
    {
        private DatabaseManager dbManager;
        private Label lblForgotPassword;  // Add this field

        public LoginForm()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            chkPassword.CheckedChanged += chkPassword_CheckedChanged;
            txtUsername.KeyDown += TextBox_KeyDown;
            txtPassword.KeyDown += TextBox_KeyDown;
            this.AcceptButton = btnLogin;
            InitializeForgotPasswordLabel(); // Add this line
        }

        // Add this method
        private void InitializeForgotPasswordLabel()
        {
            int yPosition = btnRegister.Bottom + 10; // 10 pixels padding after register button
            lblForgotPassword = new Label
            {
                AutoSize = true,
                Cursor = Cursors.Hand,
                Font = new Font("Arial", 9F, FontStyle.Underline),
                ForeColor = Color.FromArgb(82, 110, 72),
                Location = new Point((pnlLogin.Width - 120) / 2, yPosition), // Center horizontally
                Name = "lblForgotPassword",
                Size = new Size(120, 15),
                Text = "Forgot Password?"
            };
            lblForgotPassword.Click += lblForgotPassword_Click;
            this.pnlLogin.Controls.Add(lblForgotPassword);
            lblForgotPassword.Left = (pnlLogin.Width - lblForgotPassword.Width) / 2;
        }

        private void lblForgotPassword_Click(object sender, EventArgs e)
        {
            ForgotPasswordForm forgotPasswordForm = new ForgotPasswordForm();
            this.Hide();
            forgotPasswordForm.ShowDialog();
            this.Show();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the ding sound
                if (sender == txtUsername)
                {
                    txtPassword.Focus(); // Move focus to password field if username is active
                }
                else if (sender == txtPassword)
                {
                    btnLogin_Click(sender, e); // Trigger login if password field is active
                }
            }
        }

        private void chkPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkPassword.Checked ? '\0' : '*';
            txtPassword.UseSystemPasswordChar = !chkPassword.Checked;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // First check if username exists
                bool userExists = dbManager.CheckUserExists(username);

                if (!userExists)
                {
                    MessageBox.Show("Wrong username. Please input a valid username.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Clear();
                    txtPassword.Clear();
                    txtUsername.Focus();
                    return;
                }

                // If username exists, try to authenticate
                User user = dbManager.AuthenticateUser(username, password);

                if (user != null)
                {
                    MessageBox.Show("Login successful!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();

                    // Check the user's role and navigate accordingly
                    if (user.Role == "Admin")
                    {
                        Forms.DashboardForm dashboardForm = new Forms.DashboardForm(user);
                        dashboardForm.Show();
                    }
                    else if (user.Role == "User")
                    {
                        UserForm userAccount = new UserForm(user);
                        userAccount.Show();
                    }
                    else
                    {
                        MessageBox.Show("Unknown role, please contact the administrator.",
                            "Role Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // If username exists but authentication failed, it means password is wrong
                    MessageBox.Show("Wrong password. Please input a valid password.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (SQLiteException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}\nPlease contact your system administrator.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}\nPlease try again or contact support.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
        }
    }
    public class RoundedPanel : Panel
    {
        public int CornerRadius { get; set; } = 20;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, CornerRadius, CornerRadius, 180, 90);
                path.AddArc(Width - CornerRadius, 0, CornerRadius, CornerRadius, 270, 90);
                path.AddArc(Width - CornerRadius, Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
                path.AddArc(0, Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);
                path.CloseAllFigures();

                this.Region = new Region(path);

                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                if (this.BackgroundImage != null)
                {
                    e.Graphics.SetClip(path);
                    e.Graphics.DrawImage(this.BackgroundImage, ClientRectangle);
                }
            }
        }
    }
    public class CustomTextBox : TextBox
    {
        private const int WM_PAINT = 0xF;
        private int borderBottom = 2;
        private Color borderColor = Color.Gray;
        private string _placeholderText;
        private Color _placeholderColor = Color.Gray;
        private bool _isPassword;

        public string PlaceholderText
        {
            get { return _placeholderText; }
            set
            {
                _placeholderText = value;
                SetPlaceholder(this, EventArgs.Empty);
            }
        }

        public bool IsPassword
        {
            get { return _isPassword; }
            set
            {
                _isPassword = value;
                UpdatePasswordChar();
            }
        }

        public CustomTextBox()
        {
            GotFocus += RemovePlaceholder;
            LostFocus += SetPlaceholder;
            TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            UpdatePasswordChar();
        }

        private void UpdatePasswordChar()
        {
            if (_isPassword && !string.IsNullOrEmpty(Text) && Text != _placeholderText)
            {
                UseSystemPasswordChar = true;
            }
            else
            {
                UseSystemPasswordChar = false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                using (var g = Graphics.FromHwnd(Handle))
                {
                    using (var p = new Pen(borderColor, borderBottom))
                    {
                        g.DrawLine(p, 0, Height - 1, Width, Height - 1);
                    }
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            borderColor = Color.FromArgb(101, 67, 53); // Match button color
            RemovePlaceholder(this, e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            borderColor = Color.Gray;
            SetPlaceholder(this, e);
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (Text == _placeholderText)
            {
                Text = "";
                ForeColor = SystemColors.WindowText;
            }
            UpdatePasswordChar();
        }

        private void SetPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = _placeholderText;
                ForeColor = _placeholderColor;
            }
            UpdatePasswordChar();
        }
    }
}