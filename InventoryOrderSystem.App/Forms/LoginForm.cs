using System;
using System.Windows.Forms;
using System.Data.SQLite;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;

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
    }
}