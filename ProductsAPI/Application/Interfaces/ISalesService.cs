using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;

namespace ProductsAPI.Application.Interfaces
{
    public interface ISalesService
    {
        Task<SaleResponse?> CreateSale(CreateSaleRequest request);
        Task<SaleResponse?> GetSaleById(int id);
        Task<List<SaleResponse>?> GetSales();
        Task<SaleResponse?> DeleteSale(int id);
        
    }
}
