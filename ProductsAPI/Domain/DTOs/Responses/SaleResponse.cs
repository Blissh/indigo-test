namespace ProductsAPI.Domain.DTOs.Responses
{
    /// <summary>
    /// Respuesta con información de una venta y sus items.
    /// </summary>
    public class SaleResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public List<SaleItemResponse> Items { get; set; } = new();
    }
}

