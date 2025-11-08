using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.Entities;

namespace ProductsAPI.API
{
    public static class AuthenticationEndpoints
    {
        private static User user = new User();
        public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/auth/login", [AllowAnonymous] async (HttpContext context, IUserAuthService userAuthService) =>
            {
                var request = await context.Request.ReadFromJsonAsync<LoginRequest>();
                if (request is null) return Results.BadRequest(new { error = "Request is null" });

                var token = await userAuthService.LoginUser(request);
                if (token is null) return Results.Json(new { error = "Invalid credentials" }, statusCode: StatusCodes.Status401Unauthorized);

                return Results.Ok(new { token = token });
            }).WithTags("Authentication")
            .Accepts<LoginRequest>("application/json")
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("LoginUser");

            endpoints.MapPost("/auth/register", [AllowAnonymous] async (HttpContext context, IUserAuthService userAuthService) =>
            {
                var request = await context.Request.ReadFromJsonAsync<RegisterUserRequest>();
                if (request is null) return Results.BadRequest(new { error = "Request is null" });

                var user = await userAuthService.RegisterUser(request);
                if (user is null) return Results.BadRequest(new { error = "User not created" });

                return Results.Ok(new { user = user });
            }).WithTags("Authentication")
            .Accepts<RegisterUserRequest>("application/json")
            .Produces<User>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("RegisterUser");
            
            return endpoints;
        }
    }
}
