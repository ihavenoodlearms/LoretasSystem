using InventoryOrderSystem.App.Forms;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Services;
using System;
using System.Windows.Forms;

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
            string username = txtUsername.Text;
            string password = txtPassword.Text;

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
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }
    }
}