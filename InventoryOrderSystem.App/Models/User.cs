﻿namespace InventoryOrderSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}