namespace Firstwebapp.Provider;

using System.Data;
using Npgsql;

public static class ConnectionProvider
{
    private static string? _connectionString;

    public static void Initialize(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public static IDbConnection GetConnection()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new InvalidOperationException(
                "ConnectionProvider is not initialized. Call Initialize() first.");

        return new NpgsqlConnection(_connectionString);
        // Don't Open() here - let the caller decide when to open
    }
}