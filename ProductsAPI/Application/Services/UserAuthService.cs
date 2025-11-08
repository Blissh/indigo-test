using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ProductsAPI.Application.Interfaces;
using ProductsAPI.Domain.DTOs.Requests;
using ProductsAPI.Domain.Entities;
using ProductsAPI.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductsAPI.Application.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        private readonly ILogger<UserAuthService> _logger;

        public UserAuthService(ApplicationDbContext context, IConfiguration configuration, IPasswordHasher<User> passwordHasher, ILogger<UserAuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<string?> LoginUser(LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Intentando iniciar sesión para el usuario: {Email}", request.Email);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user is null)
                {
                    _logger.LogWarning("Usuario no encontrado: {Email}", request.Email);
                    return null;
                }

                if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
                {
                    _logger.LogWarning("Contraseña incorrecta para el usuario: {Email}", request.Email);
                    return null;
                }

                _logger.LogInformation("Login exitoso para el usuario: {Email}", request.Email);
                return GenerateToken(user);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al intentar iniciar sesión para: {Email}", request.Email);
                throw new ApplicationException("Error de acceso. Intente nuevamente.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar iniciar sesión para: {Email}", request.Email);
                throw new ApplicationException("Error inesperado durante el inicio de sesión.", ex);
            }
        }

        public async Task<User?> RegisterUser(RegisterUserRequest request)
        {
            try
            {
                _logger.LogInformation("Intentando registrar nuevo usuario: {Email}", request.Email);

                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    _logger.LogWarning("Intento de registro con email duplicado: {Email}", request.Email);
                    return null;
                }

                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuario registrado exitosamente: {Email}", request.Email);
                return user;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al registrar usuario: {Email}", request.Email);
                throw new ApplicationException("Error al guardar el usuario. Intente nuevamente.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar usuario: {Email}", request.Email);
                throw new ApplicationException("Error inesperado durante el registro de usuario.", ex);
            }
        }

        private string GenerateToken(User user)
        {
            try
            {
                _logger.LogInformation("Generando token JWT para el usuario: {Email}", user.Email);

                var jwtKey = _configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(jwtKey))
                {
                    _logger.LogError("La configuración JWT:Key no está establecida");
                    throw new InvalidOperationException("La configuración JWT no está correctamente establecida");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("Token JWT generado exitosamente para: {Email}", user.Email);

                return tokenString;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Error al generar token: Argumentos nulos para el usuario: {Email}", user.Email);
                throw new ApplicationException("Error al generar el token de autenticación. Configuración incompleta.", ex);
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(ex, "Error de seguridad al generar token para: {Email}", user.Email);
                throw new ApplicationException("Error de seguridad al generar el token.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al generar token para: {Email}", user.Email);
                throw new ApplicationException("Error inesperado al generar el token de autenticación.", ex);
            }
        }


    }
}
