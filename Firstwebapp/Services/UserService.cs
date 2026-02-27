using Firstwebapp.Models;
using Firstwebapp.Services.Interface;
using Firstwebapp.Data;
using Microsoft.EntityFrameworkCore;
using Firstwebapp.ViewModels;  // Add this!

namespace Firstwebapp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all users
        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _context.Users
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        // Get user by ID
        public async Task<UserModel> GetUserById(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Add new user from UserModel (for admin use)
        public async Task AddUser(UserModel user)
        {
            user.Id = Guid.NewGuid();
            user.Status = UserStatus.Active;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // NEW: Add new user from RegisterViewModel (for registration)
        public async Task AddUserFromViewModel(RegisterViewModel model)
        {
            var user = new UserModel
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Age = model.Age,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
                // Password is handled separately by AuthService
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Update user
        public async Task UpdateUser(UserModel user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Age = user.Age;
                existingUser.Status = user.Status;
                
                await _context.SaveChangesAsync();
            }
        }

        // Deactivate user
        public async Task DeactivateUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Status = UserStatus.Inactive;
                await _context.SaveChangesAsync();
            }
        }

        // Activate user
        public async Task ActivateUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Status = UserStatus.Active;
                await _context.SaveChangesAsync();
            }
        }

        // Get active users
        public async Task<List<UserModel>> GetActiveUsers()
        {
            return await _context.Users
                .Where(u => u.Status == UserStatus.Active)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        // Get inactive users
        public async Task<List<UserModel>> GetInactiveUsers()
        {
            return await _context.Users
                .Where(u => u.Status == UserStatus.Inactive)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }
    }
}