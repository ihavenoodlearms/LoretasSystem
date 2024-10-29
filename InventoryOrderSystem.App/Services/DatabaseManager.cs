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
                    // Create Users table
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                            Username TEXT NOT NULL UNIQUE,
                            PasswordHash TEXT NOT NULL,
                            IsSuperAdmin INTEGER NOT NULL
                        )";
                    command.ExecuteNonQuery();

                    // Create InventoryItems table
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS InventoryItems (
                            ItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Quantity INTEGER NOT NULL,
                            Category TEXT NOT NULL
                        )";
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
                }
            }

            // Create admin user
            CreateAdminUser("admin", "password123");
        }

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

        public void CreateAdminUser(string username, string password)
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
                            INSERT INTO Users (Username, PasswordHash, IsSuperAdmin) 
                            VALUES (@Username, @PasswordHash, @IsSuperAdmin)";
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", PasswordHasher.HashPassword(password));
                        command.Parameters.AddWithValue("@IsSuperAdmin", 1);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

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
                            if (PasswordHasher.VerifyPassword(password, storedHash))
                            {
                                return new User
                                {
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    Username = reader["Username"].ToString(),
                                    IsSuperAdmin = Convert.ToBoolean(reader["IsSuperAdmin"])
                                };
                            }
                        }
                    }
                }
            }
            return null;
        }

        public List<InventoryItem> GetAllInventoryItems()
        {
            List<InventoryItem> items = new List<InventoryItem>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM InventoryItems";
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
                                Category = reader["Category"].ToString()
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
                string query = @"INSERT INTO InventoryItems (Name, Quantity, Category) 
                     VALUES (@Name, @Quantity, @Category)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", item.Name);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity);
                    command.Parameters.AddWithValue("@Category", item.Category);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateInventoryItem(InventoryItem item)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE InventoryItems 
                             SET Name = @Name, Quantity = @Quantity, Category = @Category 
                             WHERE ItemId = @ItemId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", item.ItemId);
                    command.Parameters.AddWithValue("@Name", item.Name);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity);
                    command.Parameters.AddWithValue("@Category", item.Category);
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
                    SELECT o.*, oi.OrderItemId, oi.ItemId, oi.Quantity, oi.Price 
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

        private string GetDatabasePath()
        {
            string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            return Path.Combine(dataDirectory, "LoretasCafeDB.sqlite");
        }
    }
}