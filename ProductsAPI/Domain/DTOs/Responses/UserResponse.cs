namespace ProductsAPI.Domain.DTOs.Responses
{
    /// <summary>
    /// Respuesta con información de usuario (sin contraseña).
    /// </summary>
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

