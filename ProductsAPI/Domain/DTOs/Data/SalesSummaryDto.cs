namespace ProductsAPI.Domain.DTOs.Data
{
    public class SalesSummaryDto
    {
        public DateTime Date { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProducts { get; set; }
    }
}

