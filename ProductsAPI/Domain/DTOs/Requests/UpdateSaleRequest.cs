using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Domain.DTOs.Requests
{
    public class UpdateSaleRequest
    {
        [Required(ErrorMessage = "Los items de la venta son requeridos")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un item en la venta")]
        public List<UpdateSaleItemRequest> Items { get; set; } = new();
    }
}

