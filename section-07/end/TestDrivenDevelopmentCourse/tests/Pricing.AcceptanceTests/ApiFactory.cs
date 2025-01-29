using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pricing.Api;
using Pricing.Infrastructure;
using Testcontainers.PostgreSql;

namespace Pricing.AcceptanceTests;


public class ApiFactory : WebApplicationFactory<IApiAssemblyMarker>, 
    IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        await new DatabaseInitializer(
                new NpgsqlConnectionFactory(_postgreSqlContainer.GetConnectionString()))
            .InitializeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureTestServices(
            services =>
            {
                services.RemoveAll(typeof(IDbConnectionFactory));
                services.AddSingleton<IDbConnectionFactory>(_ =>
                    new NpgsqlConnectionFactory(_postgreSqlContainer.GetConnectionString()));
            });
    }

    public Task DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }
}