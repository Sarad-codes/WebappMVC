using Firstwebapp.Models;

namespace Firstwebapp.Services.Interface
{
    public interface IUserService
    {
        // Get all users
        List<UserModel> GetAllUsers();
        
        // Get one user
        UserModel GetUserById(Guid id);
        
        // Add a new user
        void AddUser(UserModel user);
        
        // Update a user
        void UpdateUser(UserModel user);
        // Get only active users
        List<UserModel> GetActiveUsers();

         // Get only inactive users
        List<UserModel> GetInactiveUsers();
        
        // Activate a user
        void ActivateUser(Guid id);
        
        //Deactive a user
        void DeactivateUser(Guid id);
    }
}