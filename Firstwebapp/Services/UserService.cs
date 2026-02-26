using Dapper;
using Firstwebapp.Models;
using Firstwebapp.Provider;
using Firstwebapp.Services.Interface;

namespace Firstwebapp.Services
{
    public class UserService : IUserService
    {
        // Add new user - database generates Guid
        public void AddUser(UserModel user)
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                connection.Open();
                user.Status = UserStatus.Active;
                
                var sql = @"
                    INSERT INTO users (name, email, phone, age, status) 
                    VALUES (@Name, @Email, @Phone, @Age, @Status)
                    RETURNING id";
                
                user.Id = connection.ExecuteScalar<Guid>(sql, user);
            }
        }
        
        // Get user by Guid
        public UserModel GetUserById(Guid id)
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                connection.Open();
                var sql = "SELECT * FROM users WHERE id = @Id";
                return connection.QueryFirstOrDefault<UserModel>(sql, new { Id = id });
            }
        }
        
        // Rest of your methods remain the same...
        public List<UserModel> GetAllUsers()
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                connection.Open();
                var sql = "SELECT * FROM users ORDER BY name";
                return connection.Query<UserModel>(sql).ToList();
            }
        }
        
        public void UpdateUser(UserModel user)
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                connection.Open();
                var sql = @"
                    UPDATE users 
                    SET name = @Name, 
                        email = @Email, 
                        phone = @Phone, 
                        age = @Age,
                        status = @Status
                    WHERE id = @Id";
                
                connection.Execute(sql, user);
            }
        }
        
        public void DeactivateUser(Guid id)
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                connection.Open();
                var sql = "UPDATE users SET status = @Status WHERE id = @Id";
                connection.Execute(sql, new { Id = id, Status = UserStatus.Inactive });
            }
        }
        
        public void ActivateUser(Guid id)
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                connection.Open();
                var sql = "UPDATE users SET status = @Status WHERE id = @Id";
                connection.Execute(sql, new { Id = id, Status = UserStatus.Active });
            }
        }
        
        public List<UserModel> GetActiveUsers()
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                connection.Open();
                var sql = "SELECT * FROM users WHERE status = @Status ORDER BY name";
                return connection.Query<UserModel>(sql, new { Status = UserStatus.Active }).ToList();
            }
        }
        
        public List<UserModel> GetInactiveUsers()
        {
            using (var connection = ConnectionProvider.GetConnection())
            {
                connection.Open();
                var sql = "SELECT * FROM users WHERE status = @Status ORDER BY name";
                return connection.Query<UserModel>(sql, new { Status = UserStatus.Inactive }).ToList();
            }
        }
    }
}