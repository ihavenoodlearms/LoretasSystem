using System;

namespace InventoryOrderSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string SecurityQuestion1 { get; set; }
        public string SecurityAnswer1 { get; set; }
        public string SecurityQuestion2 { get; set; }
        public string SecurityAnswer2 { get; set; }
        public DateTime? LastPasswordReset { get; set; }
        public int FailedResetAttempts { get; set; }
        public string AccountStatus { get; set; }
    }
}