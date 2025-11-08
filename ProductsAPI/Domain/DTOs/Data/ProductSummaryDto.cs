namespace ProductsAPI.Domain.DTOs.Data
{
    public class ProductSummaryDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int Stock { get; set; }
    }
}

