using InventoryOrderSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryOrderSystem.App.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly DatabaseManager _dbManager;

        public RegisterForm()
        {
            InitializeComponent();
            _dbManager = new DatabaseManager(); // Your existing DatabaseManager class

            // Initialize ComboBox with roles
            
            cmbRole.Items.Add("User");
            cmbRole.SelectedIndex = 0;  // Default role is User
        }

        // Modify your RegisterForm.cs btnRegister_Click method
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPass.Text;
            string confirmPassword = txtConfirm.Text;
            string role = cmbRole.SelectedItem.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblErrorMessage.Text = "Username and password cannot be empty.";
                lblErrorMessage.Visible = true;
                return;
            }

            if (password != confirmPassword)
            {
                lblErrorMessage.Text = "Passwords do not match.";
                lblErrorMessage.Visible = true;
                return;
            }

            try
            {
                _dbManager.CreatePendingUser(username, password, role);
                MessageBox.Show("Registration successful! Please wait for admin approval.",
                               "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                new LoginForm().Show();
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = $"Error: {ex.Message}";
                lblErrorMessage.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm login = new LoginForm();
            login.Show();
        }
    }

    }


