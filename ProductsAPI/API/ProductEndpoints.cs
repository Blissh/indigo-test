using Microsoft.AspNetCore.Authorization;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;

namespace ProductsAPI.API
{
    public static class ProductEndpoints
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/products/{id}", async (HttpContext context, IProductService productService, int id) =>
            {
                var product = await productService.GetProductById(id);
                if (product is null) return Results.NotFound(ApiResponse<ProductResponse>.ErrorResponse("Producto no encontrado"));
                return Results.Ok(ApiResponse<ProductResponse>.SuccessResponse(product, "Producto obtenido exitosamente"));
            }).WithTags("Products")
            .RequireAuthorization()
            .WithSummary("Obtener un producto por ID")
            .Produces<ProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetProductById");

            endpoints.MapGet("/products", async (HttpContext context, IProductService productService) =>
            {
                var products = await productService.GetProducts();
                if (products is null) return Results.NotFound(ApiResponse<List<ProductResponse>>.ErrorResponse("No se encontraron productos"));
                return Results.Ok(ApiResponse<List<ProductResponse>>.SuccessResponse(products, "Productos obtenidos exitosamente"));
            }).WithTags("Products")
            .RequireAuthorization()
            .WithSummary("Obtener todos los productos")
            .Produces<List<ProductResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetProducts");

            endpoints.MapPost("/products", async (HttpContext context, IProductService productService, CreateProductRequest request) =>
            {
                var product = await productService.CreateProduct(request);
                if (product is null) return Results.BadRequest(ApiResponse<ProductResponse>.ErrorResponse("Error al crear el producto"));
                return Results.Ok(ApiResponse<ProductResponse>.SuccessResponse(product, "Producto creado exitosamente"));
            }).WithTags("Products")
            .RequireAuthorization()
            .WithSummary("Crear un producto")
            .Accepts<CreateProductRequest>("application/json")
            .Produces<ProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("CreateProduct");

            endpoints.MapPut("/products/{id}", async (HttpContext context, IProductService productService, int id, UpdateProductRequest request) =>
            {
                var product = await productService.UpdateProduct(id, request);
                if (product is null) return Results.NotFound(ApiResponse<ProductResponse>.ErrorResponse("Producto no encontrado"));
                return Results.Ok(ApiResponse<ProductResponse>.SuccessResponse(product, "Producto actualizado exitosamente"));
            }).WithTags("Products")
            .RequireAuthorization()
            .WithSummary("Actualizar un producto")
            .Accepts<UpdateProductRequest>("application/json")
            .Produces<ProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("UpdateProduct");

            endpoints.MapDelete("/products/{id}", async (HttpContext context, IProductService productService, int id) =>
            {
                var product = await productService.DeleteProduct(id);
                if (product is null) return Results.NotFound(ApiResponse<ProductResponse>.ErrorResponse("Producto no encontrado"));
                return Results.Ok(ApiResponse<ProductResponse>.SuccessResponse(product, "Producto eliminado exitosamente"));
            }).WithTags("Products")
            .RequireAuthorization()
            .WithSummary("Eliminar un producto")
            .Produces<ProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("DeleteProduct");

            return endpoints;
        }
    }
}
