using ProductsAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios de la aplicación
builder.AddApplicationServices();

builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
