using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;  // Add this
using Firstwebapp.Models;
using Firstwebapp.Services.Interface;

namespace Firstwebapp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    // Public pages - anyone can see
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    // Dashboard - only logged in users
    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
        // Get real user data from database
        var allUsers = await _userService.GetAllUsers();
        var activeUsers = await _userService.GetActiveUsers();
        var inactiveUsers = await _userService.GetInactiveUsers();
        
        // Dashboard stats
        ViewBag.WorkerCount = activeUsers.Count;
        ViewBag.TotalUsers = allUsers.Count;
        ViewBag.ActiveUsers = activeUsers.Count;
        ViewBag.InactiveUsers = inactiveUsers.Count;
        
        // Keep your existing job stats for now
        ViewBag.ActiveJobs = 3;
        ViewBag.PendingPayments = 2;
        ViewBag.OutstandingAmount = 4250.00;
    
        // Sample list of today's jobs
        var todaysJobs = new List<dynamic>
        {
            new { Customer = "Rabi khanal", Time = "8:00 AM", Worker = "Moturam", Status = "Pending" },
            new { Customer = "Dawa", Time = "2:00 PM", Worker = "Rajesh", Status = "In Progress" },
            new { Customer = "Keshav Dhungana", Time = "4:30 PM", Worker = "Rajendra", Status = "Pending" }
        };
    
        return View(todaysJobs);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}