using Microsoft.AspNetCore.Mvc;
using Firstwebapp.Models;
using Firstwebapp.Services.Interface;

namespace Firstwebapp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        public IActionResult Register(UserModel user)
        {
            if (ModelState.IsValid)
            {
                _userService.AddUser(user);
                return RedirectToAction("RegistrationSuccess", new { id = user.Id });
            }
            return View(user);
        }

        // GET: /User/RegistrationSuccess/{id}
        public IActionResult RegistrationSuccess(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();
                
            return View(user);
        }
// GET: /User/List
// GET: /User/List?filter=Active
// GET: /User/List?filter=Inactive
        public IActionResult List(string filter = "All")
        {
            List<UserModel> users;
    
            switch (filter)
            {
                case "Active":
                    users = _userService.GetActiveUsers();
                    break;
                case "Inactive":
                    users = _userService.GetInactiveUsers();
                    break;
                default:
                    users = _userService.GetAllUsers();
                    break;
            }
    
            ViewBag.CurrentFilter = filter;
            return View(users);
        }
        // GET: /User/Details/{id}
        public IActionResult Details(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();
                
            return View(user);
        }

        // GET: /User/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
                return NotFound();
                
            return View(user);
        }

        // POST: /User/Edit/{id}
        [HttpPost]
        public IActionResult Edit(UserModel user)
        {
            if (ModelState.IsValid)
            {
                _userService.UpdateUser(user);
                return RedirectToAction("Details", new { id = user.Id });
            }
            return View(user);
        }

        // POST: /User/Deactivate/{id}
        [HttpPost]
        public IActionResult Deactivate(Guid id)
        {
            _userService.DeactivateUser(id);
            TempData["Success"] = "User deactivated successfully";
            return RedirectToAction("List", new { filter = "All" });  // ← Add filter
        }

// POST: /User/Activate/{id}
        [HttpPost]
        public IActionResult Activate(Guid id)
        {
            _userService.ActivateUser(id);
            TempData["Success"] = "User activated successfully";
            return RedirectToAction("List", new { filter = "All" });  // ← Add filter
        }
    }
}