using ProductsAPI.Domain.DTOs.Data;

namespace ProductsAPI.Application.Interfaces
{
    /// <summary>
    /// Servicio para generación de reportes.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Genera un reporte de ventas en el rango de fechas especificado.
        /// </summary>
        Task<SalesReportDto?> GenerateSalesReport(DateTime startDate, DateTime endDate);
    }
}
