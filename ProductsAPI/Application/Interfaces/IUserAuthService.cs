using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.Entities;

namespace ProductsAPI.Application.Interfaces
{
    public interface IUserAuthService
    {
        Task<string?> LoginUser(LoginRequest request);
        Task<User?> RegisterUser(RegisterUserRequest request);
    }
}
