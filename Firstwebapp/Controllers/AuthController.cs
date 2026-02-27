using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Firstwebapp.ViewModels;
using Firstwebapp.Data;
using Firstwebapp.Services.Interface;
using Firstwebapp.Models;  // For UserStatus
using Microsoft.EntityFrameworkCore;  // For FirstOrDefaultAsync

namespace Firstwebapp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context; 
        
        public AuthController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // GET: /Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Register(model);
                if (result)
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                ModelState.AddModelError("", "User with this email already exists");
            }
            return View(model);
        }

        // GET: /Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Login(model);
                if (result)
                {
                    return RedirectToAction("Dashboard", "Home");
                }
        
                // Check if user exists but is deactivated
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null && user.Status == UserStatus.Inactive)
                {
                    ModelState.AddModelError("", "Your account has been deactivated. Please contact administrator.");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }
            return View(model);
        }

        // POST: /Auth/Logout
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Auth/Profile
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _authService.GetCurrentUser();
            if (user == null)
                return RedirectToAction("Login");
            
            return View(user);
        }

        // GET: /Auth/ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Auth/ForgotPassword
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _authService.ForgotPassword(model);
                TempData["Success"] = "If your email exists, you will receive a password reset link.";
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        // GET: /Auth/ForgotPasswordConfirmation
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Auth/ResetPassword
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }
    
            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };
            return View(model);
        }

        // POST: /Auth/ResetPassword
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.ResetPassword(model);
            if (result)
            {
                TempData["Success"] = "Password reset successfully. Please login with your new password.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Invalid or expired reset token");
            return View(model);
        }
    }
}