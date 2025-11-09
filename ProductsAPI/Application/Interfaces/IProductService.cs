using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;

namespace ProductsAPI.Application.Interfaces
{
    /// <summary>
    /// Servicio para gestión de productos.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Obtiene un producto por su ID.
        /// </summary>
        Task<ProductResponse?> GetProductById(int id);
        
        /// <summary>
        /// Obtiene todos los productos disponibles.
        /// </summary>
        Task<List<ProductResponse>?> GetProducts();
        
        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        Task<ProductResponse?> CreateProduct(CreateProductRequest request);
        
        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        Task<ProductResponse?> UpdateProduct(int id, UpdateProductRequest request);
        
        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        Task<ProductResponse?> DeleteProduct(int id);
    }
}
