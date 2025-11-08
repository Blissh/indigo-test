using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;
using ProductsAPI.Domain.Entities;

namespace ProductsAPI.Application.Interfaces
{
    public interface IUserAuthService
    {
        Task<AuthResponse?> LoginUser(LoginRequest request);
        Task<UserResponse?> RegisterUser(RegisterUserRequest request);
    }
}
