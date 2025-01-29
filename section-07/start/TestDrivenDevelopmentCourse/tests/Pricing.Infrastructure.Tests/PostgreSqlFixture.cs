using Testcontainers.PostgreSql;

namespace Pricing.Infrastructure.Tests;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer
        = new PostgreSqlBuilder().Build();

    public IDbConnectionFactory ConnectionFactory;
    
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
       
        ConnectionFactory = new NpgsqlConnectionFactory(_postgreSqlContainer.GetConnectionString());

        await new DatabaseInitializer(ConnectionFactory).InitializeAsync();
    }

    public Task DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}