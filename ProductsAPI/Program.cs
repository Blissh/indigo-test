using Microsoft.EntityFrameworkCore;
using ProductsAPI.src.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string 'SqlConnection' not found.")));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
