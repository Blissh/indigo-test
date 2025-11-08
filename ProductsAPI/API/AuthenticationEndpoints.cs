using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;
using ProductsAPI.Domain.Entities;

namespace ProductsAPI.API
{
    public static class AuthenticationEndpoints
    {
        public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/auth/login", [AllowAnonymous] async (HttpContext context, IUserAuthService userAuthService) =>
            {
                var request = await context.Request.ReadFromJsonAsync<LoginRequest>();
                if (request is null) return Results.BadRequest(new { error = "La solicitud no puede estar vacía" });

                var authResponse = await userAuthService.LoginUser(request);
                if (authResponse is null) return Results.Json(new { error = "Credenciales inválidas" }, statusCode: StatusCodes.Status401Unauthorized);

                return Results.Ok(authResponse);
            }).WithTags("Authentication")
            .WithSummary("Iniciar sesión")
            .WithDescription("Autentica un usuario con email y contraseña, devuelve un token JWT con información del usuario")
            .Accepts<LoginRequest>("application/json")
            .Produces<AuthResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("LoginUser");

            endpoints.MapPost("/auth/register", [AllowAnonymous] async (HttpContext context, IUserAuthService userAuthService) =>
            {
                var request = await context.Request.ReadFromJsonAsync<RegisterUserRequest>();
                if (request is null) return Results.BadRequest(new { error = "La solicitud no puede estar vacía" });

                var userResponse = await userAuthService.RegisterUser(request);
                if (userResponse is null) return Results.BadRequest(new { error = "No se pudo crear el usuario. El email puede estar duplicado." });

                return Results.Ok(userResponse);
            }).WithTags("Authentication")
            .WithSummary("Registrar usuario")
            .WithDescription("Crea un nuevo usuario en el sistema con nombre, email y contraseña")
            .Accepts<RegisterUserRequest>("application/json")
            .Produces<UserResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("RegisterUser");

            endpoints.MapGet("/auth/health", [AllowAnonymous] () =>
            {
                return Results.Ok(new { message = "API de autenticación está funcionando correctamente" });
            }).WithTags("Authentication")
            .WithSummary("Verificar estado de la API de autenticación")
            .WithDescription("Verifica si la API de autenticación está funcionando correctamente")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("HealthCheck");
            
            return endpoints;
        }
    }
}
