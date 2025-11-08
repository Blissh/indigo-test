using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Application.Services;
using ProductsAPI.Domain.Entities;
using ProductsAPI.Infrastructure.Persistence;
using System.Reflection;

namespace ProductsAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (builder.Configuration == null) throw new ArgumentNullException(nameof(builder.Configuration));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection") ?? throw new InvalidOperationException("Connection string 'SqlConnection' not found.")));

            // Services
            builder.Services.AddScoped<IUserAuthService, UserAuthService>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        }
    }
}
