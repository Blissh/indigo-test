namespace ProductsAPI.Domain.DTOs.Data
{
    /// <summary>
    /// Información de un producto en el ranking de ventas.
    /// </summary>
    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public int Ranking { get; set; }
    }
}

