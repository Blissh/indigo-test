namespace ProductsAPI.Domain.DTOs.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserResponse User { get; set; } = new();
        public DateTime ExpiresAt { get; set; }
    }
}

