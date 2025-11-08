namespace ProductsAPI.Domain.DTOs.Responses
{
    public class SaleResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public List<SaleItemResponse> Items { get; set; } = new();
    }
}

