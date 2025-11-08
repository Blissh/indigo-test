using Microsoft.EntityFrameworkCore;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;
using ProductsAPI.Domain.Entities;
using ProductsAPI.Infrastructure.Persistence;

namespace ProductsAPI.Application.Services
{
    public class ProductServices : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductServices> _logger;

        public ProductServices(ApplicationDbContext context, ILogger<ProductServices> logger)
        {
            _context = context;
            _logger = logger;          
        }
        
        public async Task<ProductResponse?> GetProductById(int id)
        {
            try
            {
                _logger.LogInformation("Intentando obtener el producto por ID: {Id}", id);
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product is null)
                {
                    _logger.LogWarning("Producto no encontrado: {Id}", id);
                    return null;
                }
                return new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    Image = product.Image
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto por ID: {Id}", id);
                throw new ApplicationException("Error al obtener el producto.", ex);
            }
        }

        public async Task<List<ProductResponse>?> GetProducts()
        {
            try
            {
                _logger.LogInformation("Intentando obtener todos los productos");
                var products = await _context.Products.ToListAsync();
                if (products?.Count == 0)
                {
                    _logger.LogWarning("No se encontraron productos");
                    return null;
                }

                return products.Select(p => new ProductResponse
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Stock = p.Stock,
                        Image = p.Image
                    }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los productos");
                throw new ApplicationException("Error al obtener los productos.", ex);
            }
        }

        public async Task<ProductResponse?> CreateProduct(CreateProductRequest request)
        {
            try
            {
                _logger.LogInformation("Intentando crear un nuevo producto: {Name}", request.Name);
                var product = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Stock = request.Stock,
                    Image = request.Image
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Producto creado exitosamente: {Name}", request.Name);
                
                return new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    Image = product.Image
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el producto: {Name}", request.Name);
                throw new ApplicationException("Error al crear el producto.", ex);
            }
        }

            public async Task<ProductResponse?> UpdateProduct(int id, UpdateProductRequest request)
        {
            try
            {
                _logger.LogInformation("Intentando actualizar el producto por ID: {Id}", id);
                var product = await _context.Products.FindAsync(id);
                if (product is null)
                {
                    _logger.LogWarning("Producto no encontrado: {Id}", id);
                    return null;
                }
                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                product.Stock = request.Stock;
                product.Image = request.Image;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Producto actualizado exitosamente: {Name}", request.Name);
                return new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    Image = product.Image
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el producto: {Name}", request.Name);
                throw new ApplicationException("Error al actualizar el producto.", ex);
            }
        }

        public async Task<ProductResponse?> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation("Intentando eliminar el producto por ID: {Id}", id);
                var product = await _context.Products.FindAsync(id);
                if (product is null)
                {
                    _logger.LogWarning("Producto no encontrado: {Id}", id);
                    return null;
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Producto eliminado exitosamente: {Name}", product.Name);
                return new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    Image = product.Image
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto: {Id}", id);
                throw new ApplicationException("Error al eliminar el producto.", ex);
            }
        }
    }
}
