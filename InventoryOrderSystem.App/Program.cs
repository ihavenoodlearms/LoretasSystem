using System;
using System.IO;
using System.Windows.Forms;
using InventoryOrderSystem.Services;
using OfficeOpenXml;

namespace InventoryOrderSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                if (!Environment.Is64BitProcess)
                {
                    MessageBox.Show("This application must run in 64-bit mode.",
                        "Architecture Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Set the data directory
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                AppDomain.CurrentDomain.SetData("DataDirectory", baseDirectory);

                // Add logging to track initialization
                string logPath = Path.Combine(baseDirectory, "startup_log.txt");
                File.WriteAllText(logPath, $"Application starting at: {DateTime.Now}\n");
                File.AppendAllText(logPath, $"Base Directory: {baseDirectory}\n");

                string x64Path = Path.Combine(baseDirectory, "x64");
                Environment.SetEnvironmentVariable("PATH",
                    Environment.GetEnvironmentVariable("PATH") + ";" + x64Path);

                // Initialize the database
                DatabaseManager dbManager = new DatabaseManager();
                File.AppendAllText(logPath, "Initializing database...\n");
                dbManager.InitializeDatabase();
                File.AppendAllText(logPath, "Database initialized successfully\n");

                // Check if database file exists
                string dbPath = Path.Combine(baseDirectory, "LoretasCafeDB.sqlite");
                File.AppendAllText(logPath, $"Database path: {dbPath}\n");
                File.AppendAllText(logPath, $"Database exists: {File.Exists(dbPath)}\n");

                // Run the LoginForm
                File.AppendAllText(logPath, "Starting LoginForm...\n");
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string errorLogPath = Path.Combine(baseDir, "error_log.txt");
                File.WriteAllText(errorLogPath,
                    $"Error occurred at: {DateTime.Now}\n" +
                    $"Error message: {ex.Message}\n" +
                    $"Stack trace: {ex.StackTrace}\n");

                MessageBox.Show(
                    $"An error occurred while starting the application.\n" +
                    $"Error: {ex.Message}\n" +
                    $"Please check the error log at:\n{errorLogPath}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void LoadNativeDlls()
        {
            var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
            var x86Path = Path.Combine(applicationPath, "x86");
            var x64Path = Path.Combine(applicationPath, "x64");

            // Add both paths to the DLL search path
            var path = Environment.GetEnvironmentVariable("PATH") ?? "";
            path = $"{x86Path};{x64Path};{path}";
            Environment.SetEnvironmentVariable("PATH", path);
        }
    }
}