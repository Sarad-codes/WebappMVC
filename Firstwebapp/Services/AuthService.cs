using Firstwebapp.Models;
using Firstwebapp.Data;
using Firstwebapp.ViewModels;
using Firstwebapp.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;  
using BCrypt.Net;

namespace Firstwebapp.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Register(RegisterViewModel model)
        {
            // Check if user already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);
            
            if (existingUser != null)
                return false;

            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Create new user with hashed password
            var user = new UserModel
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Age = model.Age,
                Status = UserStatus.Active,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            // Auto-login after registration
            await SignInUser(user);
            return true;
        }

        public async Task<bool> Login(LoginViewModel model)
        {
            // Find user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);
    
            if (user == null)
                return false;

            // 🚫 Check if user is deactivated
            if (user.Status == UserStatus.Inactive)
            {
                Console.WriteLine($"Blocked login attempt for deactivated user: {user.Email}");
                return false; // Deactivated users cannot login
            }

            // Verify password hash
            bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
    
            if (!validPassword)
                return false;

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            await SignInUser(user, model.RememberMe);
            return true;
        }

        private async Task SignInUser(UserModel user, bool isPersistent = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = isPersistent ? DateTimeOffset.UtcNow.AddDays(7) : (DateTimeOffset?)null
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<UserProfileViewModel> GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null)
                return null;

            return new UserProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Age = user.Age,
                Status = user.Status,
                CreatedAt = user.CreatedAt
            };
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated ?? false;
        }

        // ✅ FIXED: Single ForgotPassword method with dynamic port
        public async Task<bool> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                // Don't reveal that user doesn't exist (security)
                return true;
            }

            // Generate reset token
            user.PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.ResetTokenExpires = DateTime.UtcNow.AddHours(24);
    
            await _context.SaveChangesAsync();

            // Get the current request's base URL (dynamic port)
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
    
            Console.WriteLine($"Reset Link: {baseUrl}/Auth/ResetPassword?token={user.PasswordResetToken}&email={user.Email}");
            Console.WriteLine($"Token: {user.PasswordResetToken}");
    
            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || user.PasswordResetToken != model.Token || user.ResetTokenExpires < DateTime.UtcNow)
            {
                return false; // Invalid token or expired
            }

            // Update password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
    
            // Clear reset token
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;
    
            await _context.SaveChangesAsync();
    
            return true;
        }
    }
}