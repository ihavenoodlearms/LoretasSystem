﻿using System;
using System.Windows.Forms;
using InventoryOrderSystem.App.Forms;
using InventoryOrderSystem.Models;

namespace InventoryOrderSystem.Forms
{
    public partial class DashboardForm : Form
    {
        private User _currentUser;

        public DashboardForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            lblWelcome.Text = $"Welcome, {_currentUser.Username}!";

            if (!_currentUser.IsSuperAdmin)
            {
                btnReports.Visible = false;
            }
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            new InventoryForm().Show();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            new OrderForm().Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            if (_currentUser.IsSuperAdmin)
            {
                // Open reports form or show report
                MessageBox.Show("Reports functionality not implemented yet.");
            }
        }
    }
}