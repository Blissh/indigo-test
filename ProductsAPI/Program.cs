using ProductsAPI.API;
using ProductsAPI.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios de la aplicación
builder.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGroup("/api/v1/")
    .WithTags("ProductsAPI")
    .MapAuthenticationEndpoints()
    .MapProductEndpoints();


app.Run();
