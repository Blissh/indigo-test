using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;

namespace ProductsAPI.Application.Interfaces
{
    /// <summary>
    /// Servicio para gestión de ventas.
    /// </summary>
    public interface ISalesService
    {
        /// <summary>
        /// Crea una nueva venta validando stock y productos.
        /// </summary>
        Task<SaleResponse?> CreateSale(CreateSaleRequest request);
        
        /// <summary>
        /// Obtiene una venta por su ID.
        /// </summary>
        Task<SaleResponse?> GetSaleById(int id);
        
        /// <summary>
        /// Obtiene todas las ventas del sistema.
        /// </summary>
        Task<List<SaleResponse>?> GetSales();
        
        /// <summary>
        /// Elimina una venta y sus items (cascada).
        /// </summary>
        Task<SaleResponse?> DeleteSale(int id);
    }
}
