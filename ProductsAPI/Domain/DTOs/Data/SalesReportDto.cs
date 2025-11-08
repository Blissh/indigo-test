namespace ProductsAPI.Domain.DTOs.Data
{
    public class SalesReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageTicket { get; set; }
        public List<TopProductDto> TopProducts { get; set; } = new();
        public List<SalesSummaryDto> DailySummary { get; set; } = new();
    }
}

