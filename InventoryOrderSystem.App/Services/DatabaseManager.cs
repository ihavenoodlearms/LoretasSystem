using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using InventoryOrderSystem.Models;
using InventoryOrderSystem.Utils;

namespace InventoryOrderSystem.Services
{
    public class DatabaseManager
    {
        private readonly string connectionString;

        public DatabaseManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public User AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["PasswordHash"].ToString();
                            if (PasswordHasher.VerifyPassword(password, storedHash))
                            {
                                return new User
                                {
                                    UserId = (int)reader["UserId"],
                                    Username = reader["Username"].ToString(),
                                    IsSuperAdmin = (bool)reader["IsSuperAdmin"]
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

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM InventoryItems";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new InventoryItem
                            {
                                ItemId = (int)reader["ItemId"],
                                Name = reader["Name"].ToString(),
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"]
                            });
                        }
                    }
                }
            }

            return items;
        }

        public void DeleteInventoryItem(int itemId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM InventoryItems WHERE ItemId = @ItemId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", itemId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Orders";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                UserId = (int)reader["UserId"],
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalAmount = (decimal)reader["TotalAmount"],
                                PaymentMethod = reader["PaymentMethod"].ToString()
                            });
                        }
                    }
                }
            }

            return orders;
        }

        // Add more methods for CRUD operations on InventoryItems and Orders
    }
}