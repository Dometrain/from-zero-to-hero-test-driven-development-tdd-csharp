using Dapper;

namespace Pricing.Infrastructure;

public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(
            @"CREATE TABLE IF NOT EXISTS pricing (
            key TEXT PRIMARY KEY, 
            document TEXT NOT NULL)"
        );
    }
}