using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Utils;

namespace InventoryOrderSystem.Services
{
    public class DatabaseManager
    {
        private readonly string connectionString;
        private const int LowInventoryThreshold = 10;

        public OrderStatistics GetOrderStatistics()
        {
            var statistics = new OrderStatistics();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Total received orders (all orders)
                string receivedQuery = "SELECT COUNT(*) FROM Orders";
                using (var command = new SQLiteCommand(receivedQuery, connection))
                {
                    statistics.ReceivedCount = Convert.ToInt32(command.ExecuteScalar());
                }

                // Processing orders (not paid and not voided)
                string processingQuery = "SELECT COUNT(*) FROM Orders WHERE Status = 'Active'";
                using (var command = new SQLiteCommand(processingQuery, connection))
                {
                    statistics.ProcessingCount = Convert.ToInt32(command.ExecuteScalar());
                }

                // Paid orders
                string paidQuery = "SELECT COUNT(*) FROM Orders WHERE Status = 'Paid'";
                using (var command = new SQLiteCommand(paidQuery, connection))
                {
                    statistics.PaidCount = Convert.ToInt32(command.ExecuteScalar());
                }

                // Cancelled/Voided orders
                string cancelledQuery = "SELECT COUNT(*) FROM Orders WHERE Status = 'Voided'";
                using (var command = new SQLiteCommand(cancelledQuery, connection))
                {
                    statistics.CancelledCount = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return statistics;
        }

        public OrderStatistics GetOrderStatisticsForDate(DateTime date)
        {
            var statistics = new OrderStatistics();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string dateStr = date.ToString("yyyy-MM-dd");

                // Total received orders for date
                string receivedQuery = "SELECT COUNT(*) FROM Orders WHERE date(OrderDate) = date(@Date)";
                using (var command = new SQLiteCommand(receivedQuery, connection))
                {
                    command.Parameters.AddWithValue("@Date", dateStr);
                    statistics.ReceivedCount = Convert.ToInt32(command.ExecuteScalar());
                }

                // Processing orders for date
                string processingQuery = "SELECT COUNT(*) FROM Orders WHERE date(OrderDate) = date(@Date) AND Status = 'Active'";
                using (var command = new SQLiteCommand(processingQuery, connection))
                {
                    command.Parameters.AddWithValue("@Date", dateStr);
                    statistics.ProcessingCount = Convert.ToInt32(command.ExecuteScalar());
                }

                // Paid orders for date
                string paidQuery = "SELECT COUNT(*) FROM Orders WHERE date(OrderDate) = date(@Date) AND Status = 'Paid'";
                using (var command = new SQLiteCommand(paidQuery, connection))
                {
                    command.Parameters.AddWithValue("@Date", dateStr);
                    statistics.PaidCount = Convert.ToInt32(command.ExecuteScalar());
                }

                // Cancelled orders for date
                string cancelledQuery = "SELECT COUNT(*) FROM Orders WHERE date(OrderDate) = date(@Date) AND Status = 'Voided'";
                using (var command = new SQLiteCommand(cancelledQuery, connection))
                {
                    command.Parameters.AddWithValue("@Date", dateStr);
                    statistics.CancelledCount = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return statistics;
        }

        public OrderStatistics GetOrderStatisticsForDateRange(DateTime startDate, DateTime endDate)
        {
            var statistics = new OrderStatistics();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Total received orders in range
                string receivedQuery = @"
                    SELECT COUNT(*) 
                    FROM Orders 
                    WHERE date(OrderDate) BETWEEN date(@StartDate) AND date(@EndDate)";
                using (var command = new SQLiteCommand(receivedQuery, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                    statistics.ReceivedCount = Convert.ToInt32(command.ExecuteScalar());
                }

                // Processing orders in range
                string processingQuery = @"
                    SELECT COUNT(*) 
                    FROM Orders 
                    WHERE date(OrderDate) BETWEEN date(@StartDate) AND date(@EndDate) 
                    AND Status = 'Active'";
                using (var command = new SQLiteCommand(processingQuery, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                    statistics.ProcessingCount = Convert.ToInt32(command.ExecuteScalar());
                }

                // Paid orders in range
                string paidQuery = @"
                    SELECT COUNT(*) 
                    FROM Orders 
                    WHERE date(OrderDate) BETWEEN date(@StartDate) AND date(@EndDate) 
                    AND Status = 'Paid'";
                using (var command = new SQLiteCommand(paidQuery, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                    statistics.PaidCount = Convert.ToInt32(command.ExecuteScalar());
                }

                // Cancelled orders in range
                string cancelledQuery = @"
                    SELECT COUNT(*) 
                    FROM Orders 
                    WHERE date(OrderDate) BETWEEN date(@StartDate) AND date(@EndDate) 
                    AND Status = 'Voided'";
                using (var command = new SQLiteCommand(cancelledQuery, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));
                    statistics.CancelledCount = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return statistics;
        }

        public DatabaseManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public void InitializeDatabase()
        {
            string dbPath = GetDatabasePath();
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    try
                    {
                        // First turn off foreign keys and start transaction
                        command.CommandText = @"
                    PRAGMA foreign_keys=off;
                    BEGIN TRANSACTION;";
                        command.ExecuteNonQuery();

                        // Create Users table first
                        command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        PasswordHash TEXT NOT NULL,
                        IsSuperAdmin INTEGER NOT NULL,
                        Role TEXT NOT NULL,
                        Email TEXT,
                        SecurityQuestion1 TEXT,
                        SecurityAnswer1 TEXT,
                        SecurityQuestion2 TEXT,
                        SecurityAnswer2 TEXT,
                        LastPasswordReset TEXT,
                        FailedResetAttempts INTEGER DEFAULT 0,
                        AccountStatus TEXT NOT NULL DEFAULT 'Approved'
                    )";
                        command.ExecuteNonQuery();

                        // Create or recreate InventoryItems table with Unit column
                        command.CommandText = @"
                    DROP TABLE IF EXISTS temp_InventoryItems;
                    
                    CREATE TABLE temp_InventoryItems (
                        ItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        Category TEXT NOT NULL,
                        Unit TEXT NOT NULL DEFAULT 'PCS'
                    );

                    INSERT OR IGNORE INTO temp_InventoryItems (ItemId, Name, Quantity, Category)
                    SELECT ItemId, Name, Quantity, Category 
                    FROM InventoryItems;

                    DROP TABLE IF EXISTS InventoryItems;
                    
                    CREATE TABLE InventoryItems (
                        ItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        Category TEXT NOT NULL,
                        Unit TEXT NOT NULL DEFAULT 'PCS'
                    );

                    INSERT INTO InventoryItems (ItemId, Name, Quantity, Category, Unit)
                    SELECT ItemId, Name, Quantity, Category, 'PCS'
                    FROM temp_InventoryItems;

                    DROP TABLE IF EXISTS temp_InventoryItems;";
                        command.ExecuteNonQuery();

                        // Create Orders table
                        command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Orders (
                        OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        OrderDate TEXT NOT NULL,
                        TotalAmount REAL NOT NULL,
                        PaymentMethod TEXT NOT NULL,
                        Status TEXT NOT NULL DEFAULT 'Active',
                        AmountPaid REAL,
                        ChangeAmount REAL,
                        FOREIGN KEY(UserId) REFERENCES Users(UserId)
                    )";
                        command.ExecuteNonQuery();

                        // Create OrderItems table
                        command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS OrderItems (
                        OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                        OrderId INTEGER NOT NULL,
                        ItemId INTEGER NOT NULL,
                        ProductName TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        Price REAL NOT NULL,
                        FOREIGN KEY(OrderId) REFERENCES Orders(OrderId),
                        FOREIGN KEY(ItemId) REFERENCES InventoryItems(ItemId)
                    )";
                        command.ExecuteNonQuery();

                        // Create TransactionsData table
                        command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS TransactionsData (
                        transaction_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        order_id INTEGER NOT NULL,
                        PaymentMethod TEXT NOT NULL,
                        AmountPaid REAL NOT NULL,
                        OrderDate TEXT NOT NULL,
                        FOREIGN KEY(order_id) REFERENCES Orders(OrderId)
                    )";
                        command.ExecuteNonQuery();

                        command.CommandText = "COMMIT;";
                        command.ExecuteNonQuery();

                        command.CommandText = "PRAGMA foreign_keys=on;";
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        command.CommandText = "ROLLBACK;";
                        command.ExecuteNonQuery();
                        throw new Exception($"Database initialization error: {ex.Message}", ex);
                    }
                }
            }

            // Create admin user if it doesn't exist
            CreateAdminUser("admin", "password123,", "ADMIN");
        }

        // Add this new method to handle inventory schema updates
        public void UpdateInventorySchema()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    try
                    {
                        command.CommandText = @"
                    PRAGMA foreign_keys=off;
                    
                    BEGIN TRANSACTION;
                    
                    -- Create a temporary table with the new schema
                    CREATE TABLE IF NOT EXISTS temp_InventoryItems (
                        ItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        Category TEXT NOT NULL,
                        Unit TEXT NOT NULL DEFAULT 'PCS'
                    );
                    
                    -- Copy data from the old table if it exists
                    INSERT OR IGNORE INTO temp_InventoryItems (ItemId, Name, Quantity, Category)
                    SELECT ItemId, Name, Quantity, Category 
                    FROM InventoryItems;
                    
                    -- Drop the old table
                    DROP TABLE IF EXISTS InventoryItems;
                    
                    -- Create the new table
                    CREATE TABLE InventoryItems (
                        ItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        Category TEXT NOT NULL,
                        Unit TEXT NOT NULL DEFAULT 'PCS'
                    );
                    
                    -- Copy data from the temporary table
                    INSERT INTO InventoryItems (ItemId, Name, Quantity, Category, Unit)
                    SELECT ItemId, Name, Quantity, Category, 'PCS'
                    FROM temp_InventoryItems;
                    
                    -- Drop the temporary table
                    DROP TABLE IF EXISTS temp_InventoryItems;
                    
                    COMMIT;
                    
                    PRAGMA foreign_keys=on;";

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        command.CommandText = "ROLLBACK;";
                        command.ExecuteNonQuery();
                        throw new Exception($"Inventory schema update error: {ex.Message}", ex);
                    }
                }
            }
        }

        // This method already exists in your code but let's update it to match the current database schema
        public bool VerifyPassword(int userId, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PasswordHash FROM Users WHERE UserId = @UserId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["PasswordHash"].ToString();
                            return PasswordHasher.VerifyPassword(password, storedHash);
                        }
                    }
                }
            }
            return false;
        }

        public void CreateAdminUser(string username, string password, string role)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // Check if admin user already exists
                    command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", username);
                    int userCount = Convert.ToInt32(command.ExecuteScalar());

                    if (userCount == 0)
                    {
                        // Create admin user
                        command.CommandText = @"
                INSERT INTO Users (Username, PasswordHash, IsSuperAdmin, Role) 
                VALUES (@Username, @PasswordHash, @IsSuperAdmin, @Role)";
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", PasswordHasher.HashPassword(password));
                        command.Parameters.AddWithValue("@IsSuperAdmin", role == "Admin" ? 1 : 0);
                        command.Parameters.AddWithValue("@Role", role);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void CreateUser(string username, string password, string role)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // Check if user already exists
                    command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", username);
                    int userCount = Convert.ToInt32(command.ExecuteScalar());

                    if (userCount > 0)
                    {
                        throw new Exception("Username already taken.");
                    }

                    // Insert new user with role
                    command.CommandText = @"
                INSERT INTO Users (Username, PasswordHash, IsSuperAdmin, Role) 
                VALUES (@Username, @PasswordHash, @IsSuperAdmin, @Role)";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", PasswordHasher.HashPassword(password));
                    command.Parameters.AddWithValue("@IsSuperAdmin", role == "Admin" ? 1 : 0); 
                    command.Parameters.AddWithValue("@Role", role);  
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool UserExists(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // SQL query to check if the username already exists
                    command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", username);

                    // Execute the query and get the count
                    int userCount = Convert.ToInt32(command.ExecuteScalar());
                    return userCount > 0; // Return true if the user exists, false otherwise
                }
            }
        }

        public bool CheckUserExists(string username)
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                using (var command = new SQLiteCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public bool VerifyUserSecurityInfo(string username, string email, string answer1, string answer2)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT COUNT(*) FROM Users 
                    WHERE Username = @Username 
                    AND Email = @Email 
                    AND SecurityAnswer1 = @Answer1 
                    AND SecurityAnswer2 = @Answer2";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Answer1", PasswordHasher.HashPassword(answer1.ToLower())); // Hash the answers
                    command.Parameters.AddWithValue("@Answer2", PasswordHasher.HashPassword(answer2.ToLower()));

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        // Method to track failed reset attempts
        public void IncrementFailedResetAttempts(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Users 
                    SET FailedResetAttempts = FailedResetAttempts + 1 
                    WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Get the number of failed attempts
        public int GetFailedResetAttempts(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT FailedResetAttempts FROM Users WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        // Reset failed attempts counter
        public void ResetFailedAttempts(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET FailedResetAttempts = 0 WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();
                }
            }
        }

        public DateTime? GetLastPasswordReset(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT LastPasswordReset FROM Users WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    object result = command.ExecuteScalar();

                    // Handle null values explicitly
                    if (result == null || result == DBNull.Value)
                    {
                        return default(DateTime?);  // Returns null for DateTime?
                    }

                    // Parse the date string
                    if (DateTime.TryParse(result.ToString(), out DateTime parsedDate))
                    {
                        return parsedDate;
                    }
                    return default(DateTime?);  // Returns null if parsing fails
                }
            }
        }

        public void UpdateLastPasswordReset(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
            UPDATE Users 
            SET LastPasswordReset = @LastReset 
            WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LastReset", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();
                }
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT UserId, Username, IsSuperAdmin, Role, Email,
                   SecurityQuestion1, SecurityQuestion2, LastPasswordReset, 
                   FailedResetAttempts
            FROM Users 
            WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                IsSuperAdmin = Convert.ToBoolean(reader["IsSuperAdmin"]),
                                Role = reader["Role"].ToString(),
                                Email = reader["Email"].ToString(),
                                SecurityQuestion1 = reader["SecurityQuestion1"].ToString(),
                                SecurityQuestion2 = reader["SecurityQuestion2"].ToString(),
                                LastPasswordReset = reader["LastPasswordReset"] != DBNull.Value
                                    ? (DateTime?)DateTime.Parse(reader["LastPasswordReset"].ToString())
                                    : null,
                                FailedResetAttempts = Convert.ToInt32(reader["FailedResetAttempts"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Update the CreateUser method to include security information
        public void CreateUser(string username, string password, string role, string email,
            string securityQ1, string securityA1, string securityQ2, string securityA2)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // Check if user already exists
                    command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", username);
                    int userCount = Convert.ToInt32(command.ExecuteScalar());

                    if (userCount > 0)
                    {
                        throw new Exception("Username already taken.");
                    }

                    // Insert new user with security information
                    command.CommandText = @"
                        INSERT INTO Users (
                            Username, PasswordHash, IsSuperAdmin, Role, Email,
                            SecurityQuestion1, SecurityAnswer1,
                            SecurityQuestion2, SecurityAnswer2,
                            LastPasswordReset, FailedResetAttempts
                        ) 
                        VALUES (
                            @Username, @PasswordHash, @IsSuperAdmin, @Role, @Email,
                            @SecurityQ1, @SecurityA1,
                            @SecurityQ2, @SecurityA2,
                            @LastPasswordReset, 0
                        )";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", PasswordHasher.HashPassword(password));
                    command.Parameters.AddWithValue("@IsSuperAdmin", role.ToLower() == "admin" ? 1 : 0);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@SecurityQ1", securityQ1);
                    command.Parameters.AddWithValue("@SecurityA1", PasswordHasher.HashPassword(securityA1.ToLower()));
                    command.Parameters.AddWithValue("@SecurityQ2", securityQ2);
                    command.Parameters.AddWithValue("@SecurityA2", PasswordHasher.HashPassword(securityA2.ToLower()));
                    command.Parameters.AddWithValue("@LastPasswordReset", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    command.ExecuteNonQuery();
                }
            }
        }

        // Add this method to update user security information
        public void UpdateUserSecurityInfo(int userId, string email,
            string securityQ1, string securityA1, string securityQ2, string securityA2)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    UPDATE Users 
                    SET Email = @Email,
                        SecurityQuestion1 = @SecurityQ1,
                        SecurityAnswer1 = @SecurityA1,
                        SecurityQuestion2 = @SecurityQ2,
                        SecurityAnswer2 = @SecurityA2
                    WHERE UserId = @UserId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@SecurityQ1", securityQ1);
                    command.Parameters.AddWithValue("@SecurityA1", PasswordHasher.HashPassword(securityA1.ToLower()));
                    command.Parameters.AddWithValue("@SecurityQ2", securityQ2);
                    command.Parameters.AddWithValue("@SecurityA2", PasswordHasher.HashPassword(securityA2.ToLower()));

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("User not found or information not updated.");
                    }
                }
            }
        }

        public List<InventoryItem> GetAllInventoryItems()
        {
            List<InventoryItem> items = new List<InventoryItem>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ItemId, Name, Quantity, Category, Unit FROM InventoryItems";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new InventoryItem
                            {
                                ItemId = Convert.ToInt32(reader["ItemId"]),
                                Name = reader["Name"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Category = reader["Category"].ToString(),
                                Unit = reader["Unit"].ToString()
                            });
                        }
                    }
                }
            }
            return items;
        }

        public List<InventoryItem> GetLowInventoryItems()
        {
            List<InventoryItem> lowInventoryItems = new List<InventoryItem>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM InventoryItems WHERE Quantity <= @LowInventoryThreshold";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LowInventoryThreshold", LowInventoryThreshold);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lowInventoryItems.Add(new InventoryItem
                            {
                                ItemId = Convert.ToInt32(reader["ItemId"]),
                                Name = reader["Name"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"])
                            });
                        }
                    }
                }
            }

            return lowInventoryItems;
        }

        public void AddNewInventoryItem(InventoryItem item)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
            INSERT INTO InventoryItems (Name, Quantity, Category, Unit) 
            VALUES (@Name, @Quantity, @Category, @Unit)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", item.Name);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity);
                    command.Parameters.AddWithValue("@Category", item.Category);
                    command.Parameters.AddWithValue("@Unit", string.IsNullOrEmpty(item.Unit) ? "PCS" : item.Unit);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateInventoryItem(InventoryItem item)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
            UPDATE InventoryItems 
            SET Name = @Name, 
                Quantity = @Quantity, 
                Category = @Category,
                Unit = @Unit 
            WHERE ItemId = @ItemId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", item.ItemId);
                    command.Parameters.AddWithValue("@Name", item.Name);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity);
                    command.Parameters.AddWithValue("@Category", item.Category);
                    command.Parameters.AddWithValue("@Unit", string.IsNullOrEmpty(item.Unit) ? "PCS" : item.Unit);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"Item with ID {item.ItemId} not found.");
                    }
                }
            }
        }

        public void DeleteInventoryItem(int itemId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM InventoryItems WHERE ItemId = @ItemId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", itemId);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"Item with ID {itemId} not found.");
                    }
                }
            }
        }

        public void RestockInventoryItem(int itemId, int quantityToAdd)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE InventoryItems 
                                 SET Quantity = Quantity + @QuantityToAdd 
                                 WHERE ItemId = @ItemId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", itemId);
                    command.Parameters.AddWithValue("@QuantityToAdd", quantityToAdd);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"Item with ID {itemId} not found.");
                    }
                }
            }
        }

        public void AddOrder(Order order)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string insertOrderQuery = @"
                            INSERT INTO Orders (UserId, OrderDate, TotalAmount, PaymentMethod, Status)
                            VALUES (@UserId, @OrderDate, @TotalAmount, @PaymentMethod, 'Active');
                            SELECT last_insert_rowid();";

                        using (var command = new SQLiteCommand(insertOrderQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", order.UserId);
                            command.Parameters.AddWithValue("@OrderDate", order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                            command.Parameters.AddWithValue("@PaymentMethod", order.PaymentMethod);

                            int orderId = Convert.ToInt32(command.ExecuteScalar());
                            order.OrderId = orderId;

                            foreach (var item in order.OrderItems)
                            {
                                string insertOrderItemQuery = @"
                                INSERT INTO OrderItems (OrderId, ItemId, ProductName, Quantity, Price)
                                VALUES (@OrderId, @ItemId, @ProductName, @Quantity, @Price)";

                                using (var itemCommand = new SQLiteCommand(insertOrderItemQuery, connection, transaction))
                                {
                                    itemCommand.Parameters.AddWithValue("@OrderId", orderId);
                                    itemCommand.Parameters.AddWithValue("@ItemId", item.ItemId);
                                    itemCommand.Parameters.AddWithValue("@ProductName", item.ProductName);
                                    itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                                    itemCommand.Parameters.AddWithValue("@Price", item.Price);
                                    itemCommand.ExecuteNonQuery();
                                }

                                // Update inventory
                                string updateInventoryQuery = @"
                                    UPDATE InventoryItems
                                    SET Quantity = Quantity - @Quantity
                                    WHERE ItemId = @ItemId";

                                using (var updateCommand = new SQLiteCommand(updateInventoryQuery, connection, transaction))
                                {
                                    updateCommand.Parameters.AddWithValue("@ItemId", item.ItemId);
                                    updateCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                                    updateCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT o.OrderId, o.UserId, o.OrderDate, o.TotalAmount, o.PaymentMethod, o.Status, 
                   o.AmountPaid, o.ChangeAmount,
                   oi.OrderItemId, oi.ItemId, oi.ProductName, oi.Quantity, oi.Price 
            FROM Orders o
            LEFT JOIN OrderItems oi ON o.OrderId = oi.OrderId
            ORDER BY o.OrderDate DESC";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        int currentOrderId = -1;
                        Order currentOrder = null;

                        while (reader.Read())
                        {
                            int orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));

                            if (orderId != currentOrderId)
                            {
                                if (currentOrder != null)
                                {
                                    orders.Add(currentOrder);
                                }

                                currentOrder = new Order
                                {
                                    OrderId = orderId,
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    OrderDate = DateTime.Parse(reader.GetString(reader.GetOrdinal("OrderDate"))),
                                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                    PaymentMethod = reader.GetString(reader.GetOrdinal("PaymentMethod")),
                                    Status = reader.GetString(reader.GetOrdinal("Status")),
                                    // Add these lines to properly read payment details
                                    AmountPaid = reader.IsDBNull(reader.GetOrdinal("AmountPaid")) ?
                                        null : (decimal?)reader.GetDecimal(reader.GetOrdinal("AmountPaid")),
                                    ChangeAmount = reader.IsDBNull(reader.GetOrdinal("ChangeAmount")) ?
                                        null : (decimal?)reader.GetDecimal(reader.GetOrdinal("ChangeAmount")),
                                    OrderItems = new List<OrderItem>()
                                };

                                currentOrderId = orderId;
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("OrderItemId")))
                            {
                                currentOrder.OrderItems.Add(new OrderItem
                                {
                                    OrderItemId = reader.GetInt32(reader.GetOrdinal("OrderItemId")),
                                    OrderId = orderId,
                                    ItemId = reader.GetInt32(reader.GetOrdinal("ItemId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                                });
                            }
                        }

                        if (currentOrder != null)
                        {
                            orders.Add(currentOrder);
                        }
                    }
                }
            }

            return orders;
        }

        public void VoidOrder(int orderId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string updateOrderQuery = @"
                    UPDATE Orders 
                    SET Status = 'Voided'
                    WHERE OrderId = @OrderId";
                        using (var command = new SQLiteCommand(updateOrderQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new Exception("No order was updated. The order might not exist.");
                            }
                        }

                        // Restore inventory
                        string restoreInventoryQuery = @"
                    UPDATE InventoryItems
                    SET Quantity = Quantity + (
                        SELECT OrderItems.Quantity
                        FROM OrderItems
                        WHERE OrderItems.OrderId = @OrderId
                        AND OrderItems.ItemId = InventoryItems.ItemId
                    )
                    WHERE EXISTS (
                        SELECT 1
                        FROM OrderItems
                        WHERE OrderItems.OrderId = @OrderId
                        AND OrderItems.ItemId = InventoryItems.ItemId
                    )";
                        using (var command = new SQLiteCommand(restoreInventoryQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Error voiding order: {ex.Message}", ex);
                    }
                }
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First restore the inventory quantities
                        string restoreInventoryQuery = @"
                    UPDATE InventoryItems
                    SET Quantity = Quantity + (
                        SELECT OrderItems.Quantity
                        FROM OrderItems
                        WHERE OrderItems.OrderId = @OrderId
                        AND OrderItems.ItemId = InventoryItems.ItemId
                    )
                    WHERE EXISTS (
                        SELECT 1
                        FROM OrderItems
                        WHERE OrderItems.OrderId = @OrderId
                        AND OrderItems.ItemId = InventoryItems.ItemId
                    )";
                        using (var command = new SQLiteCommand(restoreInventoryQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            command.ExecuteNonQuery();
                        }

                        // Delete order items
                        string deleteOrderItemsQuery = "DELETE FROM OrderItems WHERE OrderId = @OrderId";
                        using (var command = new SQLiteCommand(deleteOrderItemsQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            command.ExecuteNonQuery();
                        }

                        // Delete the order
                        string deleteOrderQuery = "DELETE FROM Orders WHERE OrderId = @OrderId";
                        using (var command = new SQLiteCommand(deleteOrderQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new Exception("Order not found.");
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Error deleting order: {ex.Message}", ex);
                    }
                }
            }
        }

        public int GetItemIdFromName(string itemName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ItemId FROM InventoryItems WHERE Name = @Name";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", itemName);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            throw new Exception($"Item with name '{itemName}' not found.");
        }

        public List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            List<Order> orders = new List<Order>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT o.OrderId, o.UserId, o.OrderDate, o.TotalAmount, o.PaymentMethod, o.Status, 
                   o.AmountPaid, o.ChangeAmount,
                   oi.OrderItemId, oi.ItemId, oi.ProductName, oi.Quantity, oi.Price 
            FROM Orders o
            LEFT JOIN OrderItems oi ON o.OrderId = oi.OrderId
            WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
            ORDER BY o.OrderDate DESC";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd HH:mm:ss"));

                    using (var reader = command.ExecuteReader())
                    {
                        int currentOrderId = -1;
                        Order currentOrder = null;

                        while (reader.Read())
                        {
                            int orderId = reader.GetInt32(reader.GetOrdinal("OrderId"));

                            if (orderId != currentOrderId)
                            {
                                if (currentOrder != null)
                                {
                                    orders.Add(currentOrder);
                                }

                                currentOrder = new Order
                                {
                                    OrderId = orderId,
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    OrderDate = DateTime.Parse(reader.GetString(reader.GetOrdinal("OrderDate"))),
                                    TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                    PaymentMethod = reader.GetString(reader.GetOrdinal("PaymentMethod")),
                                    Status = reader.GetString(reader.GetOrdinal("Status")),
                                    AmountPaid = reader.IsDBNull(reader.GetOrdinal("AmountPaid")) ?
                                        null : (decimal?)reader.GetDecimal(reader.GetOrdinal("AmountPaid")),
                                    ChangeAmount = reader.IsDBNull(reader.GetOrdinal("ChangeAmount")) ?
                                        null : (decimal?)reader.GetDecimal(reader.GetOrdinal("ChangeAmount")),
                                    OrderItems = new List<OrderItem>()
                                };

                                currentOrderId = orderId;
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("OrderItemId")))
                            {
                                currentOrder.OrderItems.Add(new OrderItem
                                {
                                    OrderItemId = reader.GetInt32(reader.GetOrdinal("OrderItemId")),
                                    OrderId = orderId,
                                    ItemId = reader.GetInt32(reader.GetOrdinal("ItemId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                                });
                            }
                        }

                        if (currentOrder != null)
                        {
                            orders.Add(currentOrder);
                        }
                    }
                }
            }

            return orders;
        }

        public decimal GetTotalSalesForDate(DateTime date)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                SELECT SUM(TotalAmount) 
                FROM Orders 
                WHERE date(OrderDate) = date(@Date) AND Status != 'Voided'";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                    object result = command.ExecuteScalar();
                    return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                }
            }
        }

        public int GetTransactionCountForDate(DateTime date)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                SELECT COUNT(*) 
                FROM Orders 
                WHERE date(OrderDate) = date(@Date) AND Status != 'Voided'";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public List<(string Product, int Quantity, decimal Revenue)> GetTopProductsForDate(DateTime date, int topCount = 5)
        {
            var topProducts = new List<(string Product, int Quantity, decimal Revenue)>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT oi.ProductName, SUM(oi.Quantity) as TotalQuantity, SUM(oi.Price * oi.Quantity) as TotalRevenue
                    FROM OrderItems oi
                    JOIN Orders o ON oi.OrderId = o.OrderId
                    WHERE date(o.OrderDate) = date(@Date) AND o.Status != 'Voided'
                    GROUP BY oi.ProductName
                    ORDER BY TotalRevenue DESC
                    LIMIT @TopCount";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@TopCount", topCount);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            topProducts.Add((
                                reader.GetString(0),
                                reader.GetInt32(1),
                                reader.GetDecimal(2)
                            ));
                        }
                    }
                }
            }
            return topProducts;
        }

        public void AddTransactionData(int orderId, string paymentMethod, decimal amountPaid, DateTime orderDate)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Generate the transaction ID
                        string transactionId = GenerateTransactionId(orderDate);

                        string insertTransactionQuery = @"
                    INSERT INTO TransactionsData (transaction_id, order_id, PaymentMethod, AmountPaid, OrderDate)
                    VALUES (@TransactionId, @OrderId, @PaymentMethod, @AmountPaid, @OrderDate)";

                        using (var command = new SQLiteCommand(insertTransactionQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@TransactionId", transactionId);
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                            command.Parameters.AddWithValue("@AmountPaid", amountPaid);
                            command.Parameters.AddWithValue("@OrderDate", orderDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Error adding transaction data: {ex.Message}", ex);
                    }
                }
            }
        }

        // Update the MarkOrderAsPaid method
        public void MarkOrderAsPaid(int orderId, decimal amountPaid, decimal change)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string updateOrderQuery = @"
                    UPDATE Orders 
                    SET Status = 'Paid', 
                        AmountPaid = @AmountPaid,
                        ChangeAmount = @Change
                    WHERE OrderId = @OrderId";

                        using (var command = new SQLiteCommand(updateOrderQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            command.Parameters.AddWithValue("@AmountPaid", amountPaid);
                            command.Parameters.AddWithValue("@Change", change);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                throw new Exception("No order was updated. The order might not exist.");
                            }

                            // Get order details for the transaction record
                            string getOrderDetailsQuery = @"
                        SELECT PaymentMethod, OrderDate 
                        FROM Orders 
                        WHERE OrderId = @OrderId";

                            string paymentMethod = "";
                            DateTime orderDate = DateTime.Now;

                            using (var detailsCommand = new SQLiteCommand(getOrderDetailsQuery, connection, transaction))
                            {
                                detailsCommand.Parameters.AddWithValue("@OrderId", orderId);
                                using (var reader = detailsCommand.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        paymentMethod = reader.GetString(0);
                                        orderDate = DateTime.Parse(reader.GetString(1));
                                    }
                                }
                            }

                            // Generate the transaction ID
                            string transactionId = GenerateTransactionId(orderDate);

                            // Add transaction record with the generated ID
                            string insertTransactionQuery = @"
                        INSERT INTO TransactionsData (transaction_id, order_id, PaymentMethod, AmountPaid, OrderDate)
                        VALUES (@TransactionId, @OrderId, @PaymentMethod, @AmountPaid, @OrderDate)";

                            using (var transCommand = new SQLiteCommand(insertTransactionQuery, connection, transaction))
                            {
                                transCommand.Parameters.AddWithValue("@TransactionId", transactionId);
                                transCommand.Parameters.AddWithValue("@OrderId", orderId);
                                transCommand.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                                transCommand.Parameters.AddWithValue("@AmountPaid", amountPaid);
                                transCommand.Parameters.AddWithValue("@OrderDate", orderDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                transCommand.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Error marking order as paid: {ex.Message}", ex);
                    }
                }
            }
        }

        public string GetTransactionId(int orderId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT transaction_id FROM TransactionsData WHERE order_id = @OrderId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    var result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }

        public int GetOrderIdFromTransactionId(string transactionId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT order_id FROM TransactionsData WHERE transaction_id = @TransactionId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TransactionId", transactionId);
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        private string GenerateTransactionId(DateTime orderDate)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Get the count of transactions for the current date to generate sequence
                string countQuery = @"
            SELECT COUNT(*) 
            FROM TransactionsData 
            WHERE date(OrderDate) = date(@OrderDate)";

                using (var command = new SQLiteCommand(countQuery, connection))
                {
                    command.Parameters.AddWithValue("@OrderDate", orderDate.ToString("yyyy-MM-dd"));
                    int transactionCount = Convert.ToInt32(command.ExecuteScalar()) + 1;

                    // Format: YYYYMMDDXXXX where XXXX is the sequential number
                    string dateComponent = orderDate.ToString("yyyyMMdd");
                    string sequenceComponent = transactionCount.ToString("D4"); // Pad with zeros to 4 digits

                    return $"{dateComponent}{sequenceComponent}";
                }
            }
        }

        // Add this after your GetAllUsers() method and before GetConnectionString()
        public void UpdateUserPassword(int userId, string newPassword)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    // Hash the new password using the existing PasswordHasher class
                    string hashedPassword = PasswordHasher.HashPassword(newPassword);

                    command.CommandText = "UPDATE Users SET PasswordHash = @PasswordHash WHERE UserId = @UserId";
                    command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    command.Parameters.AddWithValue("@UserId", userId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("User not found or password not updated.");
                    }
                }
            }
        }

    

        // Add method to get user by ID
        public User GetUserById(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE UserId = @UserId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                IsSuperAdmin = Convert.ToBoolean(reader["IsSuperAdmin"]),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Add method to update user information
        public void UpdateUser(int userId, string username, string role)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                UPDATE Users 
                SET Username = @Username, 
                    Role = @Role,
                    IsSuperAdmin = @IsSuperAdmin
                WHERE UserId = @UserId";

                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@IsSuperAdmin", role.ToLower() == "admin" ? 1 : 0);
                    command.Parameters.AddWithValue("@UserId", userId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("User not found or information not updated.");
                    }
                }
            }
        }

        // Add method to delete user
        public void DeleteUser(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM Users WHERE UserId = @UserId";
                    command.Parameters.AddWithValue("@UserId", userId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("User not found or could not be deleted.");
                    }
                }
            }
        }

        // Add method to get all users
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                IsSuperAdmin = Convert.ToBoolean(reader["IsSuperAdmin"]),
                                Role = reader["Role"].ToString()
                            });
                        }
                    }
                }
            }
            return users;
        }

        public void UpdateDatabaseForApproval()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    try
                    {
                        // Add AccountStatus column if it doesn't exist
                        command.CommandText = @"
                    ALTER TABLE Users 
                    ADD COLUMN AccountStatus TEXT NOT NULL DEFAULT 'Approved'";
                        command.ExecuteNonQuery();
                    }
                    catch (SQLiteException)
                    {
                        // Column might already exist
                    }
                }
            }
        }

        // Add these methods to DatabaseManager.cs
        public void CreatePendingUser(string username, string password, string role)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    if (UserExists(username))
                    {
                        throw new Exception("Username already taken.");
                    }

                    command.CommandText = @"
                INSERT INTO Users (Username, PasswordHash, IsSuperAdmin, Role, AccountStatus) 
                VALUES (@Username, @PasswordHash, @IsSuperAdmin, @Role, 'Pending')";

                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", PasswordHasher.HashPassword(password));
                    command.Parameters.AddWithValue("@IsSuperAdmin", role == "Admin" ? 1 : 0);
                    command.Parameters.AddWithValue("@Role", role);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<User> GetPendingUsers()
        {
            List<User> users = new List<User>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE AccountStatus = 'Pending'";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                Username = reader["Username"].ToString(),
                                IsSuperAdmin = Convert.ToBoolean(reader["IsSuperAdmin"]),
                                Role = reader["Role"].ToString(),
                                AccountStatus = reader["AccountStatus"].ToString()
                            });
                        }
                    }
                }
            }
            return users;
        }

        public void ApproveUser(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET AccountStatus = 'Approved' WHERE UserId = @UserId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RejectUser(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE UserId = @UserId AND AccountStatus = 'Pending'";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Modify the AuthenticateUser method to check account status
        public User AuthenticateUser(string username, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["PasswordHash"].ToString();
                            string accountStatus = reader["AccountStatus"].ToString();

                            if (accountStatus != "Approved")
                            {
                                throw new Exception("Your account is pending approval.");
                            }

                            if (PasswordHasher.VerifyPassword(password, storedHash))
                            {
                                return new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Username = reader["Username"].ToString(),
                                    IsSuperAdmin = Convert.ToBoolean(reader["IsSuperAdmin"]),
                                    Role = reader["Role"].ToString(),
                                    AccountStatus = accountStatus
                                };
                            }
                        }
                    }
                }
            }
            return null;
        }

        // Add the new methods here
        public List<string> GetSecurityQuestions()
        {
            List<string> questions = new List<string>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT SecurityQuestion1 FROM Users WHERE SecurityQuestion1 IS NOT NULL " +
                             "UNION SELECT DISTINCT SecurityQuestion2 FROM Users WHERE SecurityQuestion2 IS NOT NULL";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string question = reader[0].ToString();
                            if (!string.IsNullOrEmpty(question))
                            {
                                questions.Add(question);
                            }
                        }
                    }
                }
            }
            return questions;
        }

        public void UpdateSecurityQuestions(string oldQuestion, string newQuestion)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Update SecurityQuestion1
                        string updateQ1 = "UPDATE Users SET SecurityQuestion1 = @NewQuestion WHERE SecurityQuestion1 = @OldQuestion";
                        using (var command = new SQLiteCommand(updateQ1, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@NewQuestion", newQuestion);
                            command.Parameters.AddWithValue("@OldQuestion", oldQuestion);
                            command.ExecuteNonQuery();
                        }

                        // Update SecurityQuestion2
                        string updateQ2 = "UPDATE Users SET SecurityQuestion2 = @NewQuestion WHERE SecurityQuestion2 = @OldQuestion";
                        using (var command = new SQLiteCommand(updateQ2, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@NewQuestion", newQuestion);
                            command.Parameters.AddWithValue("@OldQuestion", oldQuestion);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Add this method to your DatabaseManager class
        public void UpdateUserSecurityAnswers(int userId, string answer1, string answer2)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                    UPDATE Users 
                    SET SecurityAnswer1 = @Answer1,
                        SecurityAnswer2 = @Answer2
                    WHERE UserId = @UserId";

                        using (var command = new SQLiteCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@Answer1", PasswordHasher.HashPassword(answer1.ToLower()));
                            command.Parameters.AddWithValue("@Answer2", PasswordHasher.HashPassword(answer2.ToLower()));

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new Exception("User not found or answers not updated.");
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public string GetConnectionString()
        {
            return connectionString;
        }

        private string GetDatabasePath()
        {
            string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            return Path.Combine(dataDirectory, "LoretasCafeDB.sqlite");
        }
    }
}