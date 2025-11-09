namespace ProductsAPI.Domain.DTOs.Responses
{
    /// <summary>
    /// Respuesta con información de un producto.
    /// </summary>
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}

