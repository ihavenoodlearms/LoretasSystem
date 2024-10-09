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
                            Quantity INTEGER NOT NULL
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
                            FOREIGN KEY(UserId) REFERENCES Users(UserId)
                        )";
                    command.ExecuteNonQuery();
                }
            }

            // Create admin user
            CreateAdminUser("admin", "password123");
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
                                Quantity = Convert.ToInt32(reader["Quantity"])
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
                string query = @"INSERT INTO InventoryItems (Name, Quantity) 
                         VALUES (@Name, @Quantity)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", item.Name);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity);
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
                                 SET Name = @Name, Quantity = @Quantity 
                                 WHERE ItemId = @ItemId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", item.ItemId);
                    command.Parameters.AddWithValue("@Name", item.Name);
                    command.Parameters.AddWithValue("@Quantity", item.Quantity);
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

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Orders";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = Convert.ToInt32(reader["OrderId"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                OrderDate = DateTime.Parse(reader["OrderDate"].ToString()),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                PaymentMethod = reader["PaymentMethod"].ToString()
                            });
                        }
                    }
                }
            }

            return orders;
        }
        private string GetDatabasePath()
        {
            string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            return Path.Combine(dataDirectory, "LoretasCafeDB.sqlite");
        }

        // Add more methods for CRUD operations on InventoryItems and Orders as needed
    }
}