using System;

namespace InventoryOrderSystem.Models
{
    public class OrderStatistics
    {
        public int ReceivedCount { get; set; } = 0;
        public int ProcessingCount { get; set; } = 0;
        public int PaidCount { get; set; } = 0;
        public int CancelledCount { get; set; } = 0;

        // Additional properties for financial statistics
        public decimal TotalRevenue { get; set; } = 0;
        public decimal ProcessingRevenue { get; set; } = 0;
        public decimal PaidRevenue { get; set; } = 0;
        public decimal CancelledRevenue { get; set; } = 0;

        // Date information
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Calculated properties
        public decimal AverageOrderValue => ReceivedCount > 0 ? TotalRevenue / ReceivedCount : 0;
        public double PaymentRate => ReceivedCount > 0 ? (double)PaidCount / ReceivedCount * 100 : 0;
        public double CancellationRate => ReceivedCount > 0 ? (double)CancelledCount / ReceivedCount * 100 : 0;

        // Constructor for single date statistics
        public OrderStatistics()
        {
        }

        // Constructor for date range statistics
        public OrderStatistics(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        // Method to reset all counters
        public void Reset()
        {
            ReceivedCount = 0;
            ProcessingCount = 0;
            PaidCount = 0;
            CancelledCount = 0;
            TotalRevenue = 0;
            ProcessingRevenue = 0;
            PaidRevenue = 0;
            CancelledRevenue = 0;
        }

        // Method to combine statistics (useful for aggregating multiple periods)
        public void Combine(OrderStatistics other)
        {
            if (other == null) return;

            ReceivedCount += other.ReceivedCount;
            ProcessingCount += other.ProcessingCount;
            PaidCount += other.PaidCount;
            CancelledCount += other.CancelledCount;
            TotalRevenue += other.TotalRevenue;
            ProcessingRevenue += other.ProcessingRevenue;
            PaidRevenue += other.PaidRevenue;
            CancelledRevenue += other.CancelledRevenue;

            // Update date range if necessary
            if (StartDate == null || (other.StartDate != null && other.StartDate < StartDate))
                StartDate = other.StartDate;
            if (EndDate == null || (other.EndDate != null && other.EndDate > EndDate))
                EndDate = other.EndDate;
        }

        // Override ToString for easy debugging and logging
        public override string ToString()
        {
            return $"Order Statistics:\n" +
                   $"Date Range: {(StartDate?.ToString("d") ?? "N/A")} to {(EndDate?.ToString("d") ?? "N/A")}\n" +
                   $"Received Orders: {ReceivedCount}\n" +
                   $"Processing Orders: {ProcessingCount}\n" +
                   $"Paid Orders: {PaidCount}\n" +
                   $"Cancelled Orders: {CancelledCount}\n" +
                   $"Total Revenue: {TotalRevenue:C2}\n" +
                   $"Payment Rate: {PaymentRate:F1}%\n" +
                   $"Cancellation Rate: {CancellationRate:F1}%\n" +
                   $"Average Order Value: {AverageOrderValue:C2}";
        }

        // Helper method to format currency values
        public string FormatCurrency(decimal amount)
        {
            return amount.ToString("C2");
        }

        // Helper method to format percentages
        public string FormatPercentage(double percentage)
        {
            return percentage.ToString("F1") + "%";
        }

        // Method to get a summary of the statistics
        public string GetSummary()
        {
            return $"Summary ({(StartDate?.ToString("d") ?? "N/A")} - {(EndDate?.ToString("d") ?? "N/A")})\n" +
                   $"Total Orders: {ReceivedCount}\n" +
                   $"Active: {ProcessingCount} | Paid: {PaidCount} | Cancelled: {CancelledCount}\n" +
                   $"Total Revenue: {FormatCurrency(TotalRevenue)}";
        }

        // Method to check if the statistics are empty
        public bool IsEmpty()
        {
            return ReceivedCount == 0 &&
                   ProcessingCount == 0 &&
                   PaidCount == 0 &&
                   CancelledCount == 0 &&
                   TotalRevenue == 0;
        }

        // Method to validate the statistics
        public bool IsValid()
        {
            // Basic validation rules
            if (ReceivedCount < 0 || ProcessingCount < 0 || PaidCount < 0 || CancelledCount < 0)
                return false;

            // The sum of processing, paid, and cancelled should equal received
            if (ProcessingCount + PaidCount + CancelledCount != ReceivedCount)
                return false;

            // Revenue validations
            if (TotalRevenue < 0 || ProcessingRevenue < 0 || PaidRevenue < 0 || CancelledRevenue < 0)
                return false;

            // Date range validation
            if (StartDate.HasValue && EndDate.HasValue && StartDate > EndDate)
                return false;

            return true;
        }
    }
}
