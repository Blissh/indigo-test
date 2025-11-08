using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Domain.DTOs.Requests
{
    public class CreateSaleRequest
    {
        [Required(ErrorMessage = "Los items de la venta son requeridos")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un item en la venta")]
        public List<CreateSaleItemRequest> Items { get; set; } = new();
    }
}

