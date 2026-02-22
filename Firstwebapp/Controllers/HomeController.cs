using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Firstwebapp.Models;

namespace Firstwebapp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Dashboard()
    {
        // Sample data for the dashboard
        ViewBag.WorkerCount = 5;
        ViewBag.ActiveJobs = 3;
        ViewBag.PendingPayments = 2;
        ViewBag.OutstandingAmount = 4250.00;
    
        // Sample list of today's jobs
        var todaysJobs = new List<dynamic>
        {
            new { Customer = "Rabi khanal", Time = "8:00 AM", Worker = "Moturam", Status = "Pending" },
            new { Customer = "Dawa ", Time = "2:00 PM", Worker = "Rajesh", Status = "In Progress" },
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