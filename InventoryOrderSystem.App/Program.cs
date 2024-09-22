using System;
using System.Windows.Forms;
using InventoryOrderSystem.Services;

namespace InventoryOrderSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set the data directory
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", baseDirectory);

            // Initialize the database
            DatabaseManager dbManager = new DatabaseManager();
            dbManager.InitializeDatabase();

            // Run the LoginForm
            Application.Run(new LoginForm());
        }
    }
}