namespace ProductsAPI.Domain.DTOs.Responses
{
    /// <summary>
    /// Respuesta con información de un item de venta.
    /// </summary>
    public class SaleItemResponse
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

