using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.DTOs.Responses;
using ProductsAPI.Domain.Entities;

namespace ProductsAPI.Application.Interfaces
{
    /// <summary>
    /// Servicio de autenticación de usuarios.
    /// </summary>
    public interface IUserAuthService
    {
        /// <summary>
        /// Autentica un usuario y genera un token JWT.
        /// </summary>
        Task<AuthResponse?> LoginUser(LoginRequest request);
        
        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        Task<UserResponse?> RegisterUser(RegisterUserRequest request);
    }
}
