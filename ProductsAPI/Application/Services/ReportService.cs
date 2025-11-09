using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Domain.DTOs.Data;
using ProductsAPI.Infrastructure.Persistence;

namespace ProductsAPI.Application.Services
{
    /// <summary>
    /// Servicio para generación de reportes de ventas y análisis.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReportService> _logger;

        public ReportService(ApplicationDbContext context, ILogger<ReportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Genera un reporte de ventas en el rango de fechas especificado.
        /// </summary>
        public async Task<SalesReportDto?> GenerateSalesReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("Generando reporte de ventas desde {StartDate} hasta {EndDate}", startDate, endDate);

                // Validar rango de fechas
                if (startDate > endDate)
                {
                    _logger.LogWarning("Rango de fechas inválido: StartDate {StartDate} es mayor que EndDate {EndDate}", startDate, endDate);
                    return null;
                }

                // Normalizar fechas (solo fecha, sin hora)
                var normalizedStartDate = startDate.Date;
                var normalizedEndDate = endDate.Date.AddDays(1).AddTicks(-1); // Incluir todo el día final

                // Obtener todas las ventas en el rango de fechas con sus items
                var sales = await _context.Sales
                    .Include(s => s.Items)
                    .Where(s => s.Date >= normalizedStartDate && s.Date <= normalizedEndDate)
                    .ToListAsync();

                if (!sales.Any())
                {
                    _logger.LogInformation("No se encontraron ventas en el rango de fechas especificado");
                    return new SalesReportDto
                    {
                        StartDate = normalizedStartDate,
                        EndDate = endDate.Date,
                        TotalSales = 0,
                        TotalRevenue = 0,
                        AverageTicket = 0,
                        TopProducts = new List<TopProductDto>(),
                        DailySummary = new List<SalesSummaryDto>()
                    };
                }

                // Calcular métricas generales
                var totalSales = sales.Count;
                var totalRevenue = sales.Sum(s => s.Total);
                var averageTicket = totalSales > 0 ? totalRevenue / totalSales : 0;

                // Obtener todos los items de venta del rango (ya están incluidos en sales)
                var saleItems = sales
                    .SelectMany(s => s.Items)
                    .ToList();

                // Obtener IDs de productos únicos
                var productIds = saleItems.Select(si => si.ProductId).Distinct().ToList();
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id, p => p);

                // Calcular top productos
                var topProducts = saleItems
                    .GroupBy(si => si.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        QuantitySold = g.Sum(si => si.Quantity),
                        Revenue = g.Sum(si => si.Total)
                    })
                    .OrderByDescending(x => x.Revenue)
                    .Take(10)
                    .Select((x, index) => new TopProductDto
                    {
                        ProductId = x.ProductId,
                        ProductName = products.ContainsKey(x.ProductId) ? products[x.ProductId].Name : "Producto desconocido",
                        ProductImage = products.ContainsKey(x.ProductId) ? products[x.ProductId].Image : string.Empty,
                        QuantitySold = x.QuantitySold,
                        Revenue = x.Revenue,
                        Ranking = index + 1
                    })
                    .ToList();

                // Generar resumen diario
                var dailySummary = sales
                    .GroupBy(s => s.Date.Date)
                    .Select(g => new SalesSummaryDto
                    {
                        Date = g.Key,
                        TotalSales = g.Count(),
                        TotalRevenue = g.Sum(s => s.Total),
                        TotalProducts = g.SelectMany(s => s.Items).Select(i => i.ProductId).Distinct().Count()
                    })
                    .OrderBy(d => d.Date)
                    .ToList();

                var report = new SalesReportDto
                {
                    StartDate = normalizedStartDate,
                    EndDate = endDate.Date,
                    TotalSales = totalSales,
                    TotalRevenue = totalRevenue,
                    AverageTicket = averageTicket,
                    TopProducts = topProducts,
                    DailySummary = dailySummary
                };

                _logger.LogInformation("Reporte de ventas generado exitosamente. Total ventas: {TotalSales}, Revenue: {TotalRevenue}", totalSales, totalRevenue);

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de ventas desde {StartDate} hasta {EndDate}", startDate, endDate);
                throw new ApplicationException("Error al generar el reporte de ventas.", ex);
            }
        }
    }
}

