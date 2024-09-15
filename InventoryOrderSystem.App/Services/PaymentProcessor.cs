using System;

namespace InventoryOrderSystem.Services
{
    public class PaymentProcessor
    {
        public bool ProcessCashlessPayment(decimal amount, string transactionId)
        {
            // In a real-world scenario, this would integrate with a payment gateway
            // For this example, we'll simulate a successful transaction
            Console.WriteLine($"Processing cashless payment of {amount:C} with transaction ID: {transactionId}");

            // Simulating a 90% success rate
            Random random = new Random();
            return random.Next(100) < 90;
        }
    }
}