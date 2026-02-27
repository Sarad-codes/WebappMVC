using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Firstwebapp.Models;
using Firstwebapp.Services.Interface;

namespace Firstwebapp.Controllers
{
    [Authorize]  // ALL actions in this controller require login
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /User/List
        public async Task<IActionResult> List(string filter = "All")
        {
            List<UserModel> users;
    
            switch (filter)
            {
                case "Active":
                    users = await _userService.GetActiveUsers();
                    break;
                case "Inactive":
                    users = await _userService.GetInactiveUsers();
                    break;
                default:
                    users = await _userService.GetAllUsers();
                    break;
            }
    
            ViewBag.CurrentFilter = filter;
            return View(users);
        }
        
        // GET: /User/Details/{id}
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
                
            return View(user);
        }

        // GET: /User/Edit/{id}
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
                
            return View(user);
        }

        // POST: /User/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(UserModel user)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateUser(user);
                return RedirectToAction("Details", new { id = user.Id });
            }
            return View(user);
        }

        // POST: /User/Deactivate/{id}
        [HttpPost]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            await _userService.DeactivateUser(id);
            TempData["Success"] = "User deactivated successfully";
            return RedirectToAction("List", new { filter = "All" });
        }

        // POST: /User/Activate/{id}
        [HttpPost]
        public async Task<IActionResult> Activate(Guid id)
        {
            await _userService.ActivateUser(id);
            TempData["Success"] = "User activated successfully";
            return RedirectToAction("List", new { filter = "All" });
        }
    }
}