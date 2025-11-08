using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;

namespace ProductsAPI.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse?> GetProductById(int id);
        Task<List<ProductResponse>?> GetProducts();
        Task<ProductResponse?> CreateProduct(CreateProductRequest request);
        Task<ProductResponse?> UpdateProduct(int id, UpdateProductRequest request);
        Task<ProductResponse?> DeleteProduct(int id);
    }
}
