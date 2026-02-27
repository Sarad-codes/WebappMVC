using Firstwebapp.ViewModels;

namespace Firstwebapp.Services.Interface
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterViewModel model);
        Task<bool> Login(LoginViewModel model);
        Task Logout();
        Task<UserProfileViewModel> GetCurrentUser();
        bool IsAuthenticated();
        Task<bool> ForgotPassword(ForgotPasswordViewModel model);
        Task<bool> ResetPassword(ResetPasswordViewModel model);
    }
}