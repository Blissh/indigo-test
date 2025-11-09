namespace ProductsAPI.Domain.DTOs.Responses
{
    /// <summary>
    /// Respuesta de autenticación con token JWT e información del usuario.
    /// </summary>
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserResponse User { get; set; } = new();
        public DateTime ExpiresAt { get; set; }
    }
}

