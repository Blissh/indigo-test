using Microsoft.AspNetCore.Authorization;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Domain.DTOs.Data;
using ProductsAPI.Domain.DTOs.Responses;

namespace ProductsAPI.API
{
    public static class ReportsEndpoints
    {
        public static IEndpointRouteBuilder MapReportsEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/reports/sales", async (HttpContext context, IReportService reportService, DateTime? startDate, DateTime? endDate) =>
            {
                // Si no se proporcionan fechas, usar el último mes por defecto
                var defaultStartDate = startDate ?? DateTime.UtcNow.AddMonths(-1).Date;
                var defaultEndDate = endDate ?? DateTime.UtcNow.Date;

                var report = await reportService.GenerateSalesReport(defaultStartDate, defaultEndDate);
                
                if (report is null)
                    return Results.BadRequest(ApiResponse<SalesReportDto>.ErrorResponse("Error al generar el reporte. Verifique que el rango de fechas sea válido (startDate <= endDate)"));
                
                return Results.Ok(ApiResponse<SalesReportDto>.SuccessResponse(report, "Reporte de ventas generado exitosamente"));
            }).WithTags("Reports")
            .RequireAuthorization()
            .WithSummary("Generar reporte de ventas")
            .WithDescription("Genera un reporte completo de ventas en el rango de fechas especificado. Incluye métricas generales, top 10 productos y resumen diario. Si no se proporcionan fechas, usa el último mes por defecto.")
            .Produces<SalesReportDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GenerateSalesReport");

            return endpoints;
        }
    }
}
