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

        public LoginForm()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                User user = dbManager.AuthenticateUser(username, password);
                if (user != null)
                {
                    MessageBox.Show("Login successful!");

                    this.Hide();

                    // Check the user's role and navigate accordingly
                    if (user.Role == "Admin")  // Admin role
                    {
                        Forms.DashboardForm dashboardForm = new Forms.DashboardForm(user);
                        dashboardForm.Show();
                    }
                    else if (user.Role == "User")  // Regular user role
                    {
                        UserForm userAccount = new UserForm(user);
                        userAccount.Show();
                    }
                    else
                    {
                        MessageBox.Show("Unknown role, please contact the administrator.", "Role Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SQLiteException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}\nPlease contact your system administrator.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}\nPlease try again or contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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