using Microsoft.EntityFrameworkCore;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;
using ProductsAPI.Domain.Entities;
using ProductsAPI.Infrastructure.Persistence;

namespace ProductsAPI.Application.Services
{
    public class SalesService : ISalesService
    {
        private readonly ILogger<SalesService> _logger;
        private readonly ApplicationDbContext _context;

        public SalesService(ILogger<SalesService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<SaleResponse?> CreateSale(CreateSaleRequest request)
        {
            try
            {
                _logger.LogInformation("Intentando crear una nueva venta");
                
                // Validar que todos los productos existen y obtener sus precios
                var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
                var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
                    
                // Crear los items de la venta
                var saleItems = new List<SaleItems>();
                decimal totalSale = 0;

                foreach (var item in request.Items)
                {
                    var product = products[item.ProductId];
                    
                    // Verificar stock disponible
                    if (product.Stock < item.Quantity)
                    {
                        _logger.LogWarning("Stock insuficiente para el producto {ProductId}. Stock disponible: {Stock}, Solicitado: {Quantity}", 
                            item.ProductId, product.Stock, item.Quantity);
                        return null;
                    }

                    var itemTotal = product.Price * item.Quantity;
                    totalSale += itemTotal;

                    saleItems.Add(new SaleItems
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = product.Price,
                        Total = itemTotal,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });

                    // Actualizar stock del producto
                    product.Stock -= item.Quantity;
                }

                // Crear la venta
                var sale = new Sale
                {
                    Date = DateTime.UtcNow,
                    Total = totalSale
                };

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                // Asignar SaleId a los items y guardarlos
                foreach (var item in saleItems)
                {
                    item.SaleId = sale.Id;
                    _context.SaleItems.Add(item);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Venta creada exitosamente con ID: {SaleId}", sale.Id);

                // Mapear a SaleResponse
                return new SaleResponse
                {
                    Id = sale.Id,
                    Date = sale.Date,
                    Total = sale.Total,
                    Items = saleItems.Select(item => new SaleItemResponse
                    {
                        Id = item.Id,
                        SaleId = item.SaleId,
                        ProductId = item.ProductId,
                        ProductName = products.FirstOrDefault(p => p.Id == item.ProductId)?.Name ?? "Producto desconocido",
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = item.Total,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la venta");
                throw new ApplicationException("Error al crear la venta.", ex);
            }
        }

        public async Task<SaleResponse?> DeleteSale(int id)
        {
            try
            {
                _logger.LogInformation("Intentando eliminar la venta por ID: {Id}", id);
                
                // Cargar la venta con sus items para que EF pueda hacer la eliminación en cascada
                var sale = await _context.Sales
                    .Include(s => s.Items)
                    .FirstOrDefaultAsync(s => s.Id == id);
                    
                if (sale is null)
                {
                    _logger.LogWarning("Venta no encontrada: {Id}", id);
                    return null;
                }

                // Guardar información antes de eliminar para la respuesta
                var saleResponse = new SaleResponse
                {
                    Id = sale.Id,
                    Date = sale.Date,
                    Total = sale.Total
                };

                // Eliminar la venta
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Venta eliminada exitosamente: {Id}", id);
                return saleResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la venta: {Id}", id);
                throw new ApplicationException("Error al eliminar la venta.", ex);
            }
        }

        public async Task<SaleResponse?> GetSaleById(int id)
        {
            try
            {
                _logger.LogInformation("Intentando obtener la venta por ID: {Id}", id);
                var sale = await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
                if (sale is null)
                {
                    _logger.LogWarning("Venta no encontrada: {Id}", id);
                    return null;
                }
                return new SaleResponse
                {
                    Id = sale.Id,
                    Date = sale.Date,
                    Total = sale.Total,
                    Items = sale.Items.Select(item => new SaleItemResponse
                    {
                        Id = item.Id,
                        SaleId = item.SaleId,
                        ProductId = item.ProductId,
                        ProductName = _context.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name ?? "Producto desconocido",
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = item.Total,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt
                    }).ToList()

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la venta por ID: {Id}", id);
                throw new ApplicationException("Error al obtener la venta.", ex);
            }
        }

        public async Task<List<SaleResponse>?> GetSales()
        {
            try
            {
                _logger.LogInformation("Intentando obtener todas las ventas");
                var sales = await _context.Sales.Include(s => s.Items).ToListAsync();
                return sales.Select(sale => new SaleResponse
                {
                    Id = sale.Id,
                    Date = sale.Date,
                    Total = sale.Total,
                    Items = sale.Items.Select(item => new SaleItemResponse
                    {
                        Id = item.Id,
                        SaleId = item.SaleId,
                        ProductId = item.ProductId,
                        ProductName = _context.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name ?? "Producto desconocido",
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Total = item.Total,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las ventas");
                throw new ApplicationException("Error al obtener las ventas.", ex);
            }
        }

    }
}
