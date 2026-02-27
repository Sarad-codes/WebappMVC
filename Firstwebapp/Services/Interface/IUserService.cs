using Firstwebapp.Models;
using Firstwebapp.ViewModels;  // Add this!

namespace Firstwebapp.Services.Interface
{
    public interface IUserService
    {
        // Get all users
        Task<List<UserModel>> GetAllUsers();
        
        // Get one user
        Task<UserModel> GetUserById(Guid id);
        
        // Add a new user from UserModel (admin)
        Task AddUser(UserModel user);
        
        // NEW: Add user from RegisterViewModel (registration)
        Task AddUserFromViewModel(RegisterViewModel model);
        
        // Update a user
        Task UpdateUser(UserModel user);
        
        // Get only active users
        Task<List<UserModel>> GetActiveUsers();

        // Get only inactive users
        Task<List<UserModel>> GetInactiveUsers();
        
        // Activate a user
        Task ActivateUser(Guid id);
        
        // Deactivate a user
        Task DeactivateUser(Guid id);
    }
}