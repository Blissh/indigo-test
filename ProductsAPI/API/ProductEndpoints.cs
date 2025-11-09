using Microsoft.AspNetCore.Authorization;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;
using ProductsAPI.Infrastructure.Services;

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

            endpoints.MapPost("/products/upload-image", async (HttpContext context, IBlobStorageService blobStorageService) =>
            {
                var form = await context.Request.ReadFormAsync();
                var file = form.Files.GetFile("image");
                
                if (file == null || file.Length == 0)
                {
                    return Results.BadRequest(ApiResponse<string>.ErrorResponse("No se proporcionó ninguna imagen"));
                }

                // Validar tipo de archivo
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return Results.BadRequest(ApiResponse<string>.ErrorResponse("Tipo de archivo no permitido. Solo se permiten imágenes (jpg, jpeg, png, gif, webp)"));
                }

                // Validar tamaño (máximo 5MB)
                const long maxFileSize = 5 * 1024 * 1024; // 5MB
                if (file.Length > maxFileSize)
                {
                    return Results.BadRequest(ApiResponse<string>.ErrorResponse("El archivo es demasiado grande. Tamaño máximo: 5MB"));
                }

                try
                {
                    var containerName = "testindigo";
                    var fileName = $"acuellar/{Guid.NewGuid()}{fileExtension}";
                    var contentType = file.ContentType ?? "image/jpeg";

                    using var fileStream = file.OpenReadStream();
                    var imageUrl = await blobStorageService.UploadFileAsync(containerName, fileName, fileStream, contentType);

                    return Results.Ok(ApiResponse<string>.SuccessResponse(imageUrl, "Imagen subida exitosamente"));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ApiResponse<string>.ErrorResponse($"Error al subir la imagen: {ex.Message}"));
                }
            }).WithTags("Products")
            .RequireAuthorization()
            .WithSummary("Subir imagen de producto")
            .WithDescription("Sube una imagen al blob storage y retorna la URL. La imagen se guarda en el contenedor 'testindigo' con prefijo 'acuellar/'")
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("UploadProductImage");

            return endpoints;
        }
    }
}
