using ProductsAPI.Domain.DTOs.Data;

namespace ProductsAPI.Application.Interfaces
{
    public interface IReportService
    {
        Task<SalesReportDto?> GenerateSalesReport(DateTime startDate, DateTime endDate);
    }
}
