using System;
using System.Windows.Forms;
using InventoryOrderSystem.Services;
using InventoryOrderSystem.Models;

namespace InventoryOrderSystem.Forms
{
    public partial class OrderForm : Form
    {
        private DatabaseManager _dbManager;

        public OrderForm()
        {
            InitializeComponent();
            _dbManager = new DatabaseManager();
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            RefreshOrderList();
        }

        private void RefreshOrderList()
        {
            dgvOrders.DataSource = _dbManager.GetAllOrders();
        }

        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            // Open a new form or dialog to create a new order
        }

        private void btnViewOrder_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                Order selectedOrder = (Order)dgvOrders.SelectedRows[0].DataBoundItem;
                // Open a new form or dialog to view the selected order details
            }
        }
    }
}