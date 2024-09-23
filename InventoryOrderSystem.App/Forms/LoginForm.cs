using System;
using System.Windows.Forms;
using System.Data.SQLite;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;
using System.Drawing;

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
                    Forms.DashboardForm dashboardForm = new Forms.DashboardForm(user);
                    dashboardForm.Show();
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

        private void pnlImages_Paint(object sender, PaintEventArgs e)
        {
            var img = e.Graphics;
        }
    }

    public class PlaceholderTextBox : TextBox
    {
        private string _placeholderText;
        private Color _placeholderColor = Color.Gray;
        private bool _isPassword;

        public string PlaceholderText
        {
            get { return _placeholderText; }
            set
            {
                _placeholderText = value;
                SetPlaceholder();
            }
        }

        public bool IsPassword
        {
            get { return _isPassword; }
            set
            {
                _isPassword = value;
                if (_isPassword)
                {
                    PasswordChar = '•';
                }
            }
        }

        public PlaceholderTextBox()
        {
            GotFocus += RemovePlaceholder;
            LostFocus += SetPlaceholder;
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (Text == _placeholderText)
            {
                Text = "";
                ForeColor = SystemColors.WindowText;
                if (_isPassword)
                {
                    PasswordChar = '•';
                }
            }
        }

        private void SetPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = _placeholderText;
                ForeColor = _placeholderColor;
                if (_isPassword)
                {
                    PasswordChar = '\0';
                }
            }
        }

        private void SetPlaceholder()
        {
            if (string.IsNullOrEmpty(Text) || Text == _placeholderText)
            {
                Text = _placeholderText;
                ForeColor = _placeholderColor;
                if (_isPassword)
                {
                    PasswordChar = '\0';
                }
            }
        }
    }
}