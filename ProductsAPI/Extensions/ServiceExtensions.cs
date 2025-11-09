using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Application.Services;
using ProductsAPI.Domain.Entities;
using ProductsAPI.Infrastructure.Persistence;
using ProductsAPI.Infrastructure.Services;

namespace ProductsAPI.Extensions
{
    /// <summary>
    /// Extensiones para registro de servicios de la aplicación.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registra todos los servicios de la aplicación en el contenedor de dependencias.
        /// </summary>
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (builder.Configuration == null) throw new ArgumentNullException(nameof(builder.Configuration));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection") ?? throw new InvalidOperationException("Connection string 'SqlConnection' not found.")));

            // Services
            builder.Services.AddScoped<IUserAuthService, UserAuthService>();
            builder.Services.AddScoped<IProductService, ProductServices>();
            builder.Services.AddScoped<ISalesService, SalesService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        }
    }
}
