using Firstwebapp.Models;
using Firstwebapp.Provider;
using Firstwebapp.Services;
using Firstwebapp.Services.Interface;
using Microsoft.Extensions.DependencyInjection; // Add this!

namespace Firstwebapp;

public static class DiConfig
{
    public static void UseDbContext(this WebApplicationBuilder builder)
    {
        ConnectionProvider.Initialize(builder.Configuration);
        builder.ConfigureServices(); // Renamed for clarity
    }

    public static void ConfigureServices(this WebApplicationBuilder builder) // Fixed parameter name
    {
        // Register your services here
        builder.Services.AddScoped<IUserService, UserService>();
        
        // Add more services as needed
        // builder.Services.AddScoped<IJobService, JobService>();
        // builder.Services.AddScoped<IWorkerService, WorkerService>();
    }
}