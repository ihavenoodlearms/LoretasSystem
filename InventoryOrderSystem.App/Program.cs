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

                // Initialize the database with full error logging
                DatabaseManager dbManager = new DatabaseManager();
                try
                {
                    File.AppendAllText(logPath, "Initializing database...\n");
                    dbManager.InitializeDatabase();
                    File.AppendAllText(logPath, "Database initialized successfully\n");

                    // Perform schema updates for inventory
                    File.AppendAllText(logPath, "Updating inventory schema...\n");
                    dbManager.UpdateInventorySchema();
                    File.AppendAllText(logPath, "Inventory schema updated successfully\n");

                    File.AppendAllText(logPath, "Updating database for approval system...\n");
                    dbManager.UpdateDatabaseForApproval();
                    File.AppendAllText(logPath, "Database updated for approval system\n");

                    File.AppendAllText(logPath, "Running database migration...\n");
                    MigrateDatabase(dbManager, logPath);
                    File.AppendAllText(logPath, "Database migration completed\n");
                }
                catch (Exception dbEx)
                {
                    File.AppendAllText(logPath, $"Database setup error: {dbEx.Message}\n");
                    File.AppendAllText(logPath, $"Stack trace: {dbEx.StackTrace}\n");
                    throw;
                }

                // Rest of your existing Main method code...
                string dbPath = Path.Combine(baseDirectory, "LoretasCafeDB.sqlite");
                File.AppendAllText(logPath, $"Database path: {dbPath}\n");
                File.AppendAllText(logPath, $"Database exists: {File.Exists(dbPath)}\n");

                try
                {
                    File.AppendAllText(logPath, "Verifying admin account...\n");
                    VerifyAdminAccount(dbManager, logPath);
                    File.AppendAllText(logPath, "Admin account verification completed\n");
                }
                catch (Exception adminEx)
                {
                    File.AppendAllText(logPath, $"Admin account verification error: {adminEx.Message}\n");
                    File.AppendAllText(logPath, $"Stack trace: {adminEx.StackTrace}\n");
                }

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

        private static void MigrateDatabase(DatabaseManager dbManager, string logPath)
        {
            try
            {
                // Create a backup of the database before migration
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string dbPath = Path.Combine(baseDirectory, "LoretasCafeDB.sqlite");
                string backupPath = Path.Combine(baseDirectory, $"LoretasCafeDB_backup_{DateTime.Now:yyyyMMddHHmmss}.sqlite");

                if (File.Exists(dbPath))
                {
                    File.Copy(dbPath, backupPath, true);
                    File.AppendAllText(logPath, $"Database backup created at: {backupPath}\n");
                }

                // Perform the migration
                using (var connection = new System.Data.SQLite.SQLiteConnection(dbManager.GetConnectionString()))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        // Check if AccountStatus column exists
                        command.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Users') WHERE name='AccountStatus'";
                        int columnExists = Convert.ToInt32(command.ExecuteScalar());

                        if (columnExists == 0)
                        {
                            File.AppendAllText(logPath, "Adding AccountStatus column...\n");
                            command.CommandText = @"
                                BEGIN TRANSACTION;
                                ALTER TABLE Users ADD COLUMN AccountStatus TEXT NOT NULL DEFAULT 'Approved';
                                UPDATE Users SET AccountStatus = 'Approved' WHERE AccountStatus IS NULL;
                                COMMIT;";
                            command.ExecuteNonQuery();
                            File.AppendAllText(logPath, "AccountStatus column added successfully\n");
                        }
                        else
                        {
                            File.AppendAllText(logPath, "AccountStatus column already exists\n");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logPath, $"Migration error: {ex.Message}\n");
                File.AppendAllText(logPath, $"Stack trace: {ex.StackTrace}\n");
                throw;
            }
        }

        private static void VerifyAdminAccount(DatabaseManager dbManager, string logPath)
        {
            try
            {
                if (!dbManager.UserExists("admin"))
                {
                    File.AppendAllText(logPath, "Creating admin account...\n");
                    dbManager.CreateAdminUser("admin", "password123", "ADMIN");
                    File.AppendAllText(logPath, "Admin account created successfully\n");
                }
                else
                {
                    // Ensure admin account is marked as approved
                    using (var connection = new System.Data.SQLite.SQLiteConnection(dbManager.GetConnectionString()))
                    {
                        connection.Open();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE Users SET AccountStatus = 'Approved' WHERE Username = 'admin'";
                            command.ExecuteNonQuery();
                        }
                    }
                    File.AppendAllText(logPath, "Existing admin account verified and approved\n");
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logPath, $"Admin account verification error: {ex.Message}\n");
                throw;
            }
        }
    }
}