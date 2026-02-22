using Firstwebapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Firstwebapp.Controllers;

public class UserController : Controller
{
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Register(UserModel user)
    {
         if (ModelState.IsValid)
         {
             return View("RegistrationSuccess", user);  
         }
           return View(user);
    }
}