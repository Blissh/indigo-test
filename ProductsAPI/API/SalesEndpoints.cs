using Microsoft.AspNetCore.Authorization;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;

namespace ProductsAPI.API
{
    public static class SalesEndpoints
    {
        public static IEndpointRouteBuilder MapSalesEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/sales", async (HttpContext context, ISalesService salesService, CreateSaleRequest request) =>
            {
                var sale = await salesService.CreateSale(request);
                if (sale is null) 
                    return Results.BadRequest(ApiResponse<SaleResponse>.ErrorResponse("Error al crear la venta. Verifique que los productos existan y tengan stock suficiente"));
                
                return Results.Ok(ApiResponse<SaleResponse>.SuccessResponse(sale, "Venta creada exitosamente"));
            }).WithTags("Sales")
            .RequireAuthorization()
            .WithSummary("Crear una nueva venta")
            .WithDescription("Crea una nueva venta con los items especificados. Valida existencia de productos y stock disponible.")
            .Accepts<CreateSaleRequest>("application/json")
            .Produces<SaleResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("CreateSale");

            endpoints.MapGet("/sales/{id}", async (HttpContext context, ISalesService salesService, int id) =>
            {
                var sale = await salesService.GetSaleById(id);
                if (sale is null) 
                    return Results.NotFound(ApiResponse<SaleResponse>.ErrorResponse("Venta no encontrada"));
                
                return Results.Ok(ApiResponse<SaleResponse>.SuccessResponse(sale, "Venta obtenida exitosamente"));
            }).WithTags("Sales")
            .RequireAuthorization()
            .WithSummary("Obtener una venta por ID")
            .WithDescription("Obtiene los detalles de una venta específica incluyendo todos sus items")
            .Produces<SaleResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetSaleById");

            endpoints.MapGet("/sales", async (HttpContext context, ISalesService salesService) =>
            {
                var sales = await salesService.GetSales();
                if (sales is null || !sales.Any()) 
                    return Results.NotFound(ApiResponse<List<SaleResponse>>.ErrorResponse("No se encontraron ventas"));
                
                return Results.Ok(ApiResponse<List<SaleResponse>>.SuccessResponse(sales, "Ventas obtenidas exitosamente"));
            }).WithTags("Sales")
            .RequireAuthorization()
            .WithSummary("Obtener todas las ventas")
            .WithDescription("Obtiene todas las ventas del sistema con sus items")
            .Produces<List<SaleResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetSales");

            endpoints.MapDelete("/sales/{id}", async (HttpContext context, ISalesService salesService, int id) =>
            {
                var sale = await salesService.DeleteSale(id);
                if (sale is null) 
                    return Results.NotFound(ApiResponse<SaleResponse>.ErrorResponse("Venta no encontrada"));
                
                return Results.Ok(ApiResponse<SaleResponse>.SuccessResponse(sale, "Venta eliminada exitosamente"));
            }).WithTags("Sales")
            .RequireAuthorization()
            .WithSummary("Eliminar una venta")
            .WithDescription("Elimina una venta y todos sus items asociados (eliminación en cascada)")
            .Produces<SaleResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("DeleteSale");

            return endpoints;
        }
    }
}
