using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Domain.DTOs.Requests
{
    public class UpdateSaleItemRequest
    {
        [Required(ErrorMessage = "El ID del item de venta es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del item debe ser mayor a 0")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del producto es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del producto debe ser mayor a 0")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }
    }
}

