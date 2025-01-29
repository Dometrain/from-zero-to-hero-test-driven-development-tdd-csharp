using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pricing.Core;
using Pricing.Core.ApplyPricing;
using Pricing.Core.TicketPrice;
using Pricing.Infrastructure;

namespace Pricing.Api.Tests;

public class ApiFactory: WebApplicationFactory<IApiAssemblyMarker>
{
    private readonly Action<IServiceCollection> _configure;

    public ApiFactory(Action<IServiceCollection> configure)
    {
        _configure = configure;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureTestServices(
            services =>
            {
                services.RemoveAll(typeof(IDbConnectionFactory));
                services.RemoveAll(typeof(DatabaseInitializer));
                services.RemoveAll(typeof(IPricingStore));
                services.RemoveAll(typeof(IReadPricingStore));
                services.AddScoped<IReadPricingStore>(s => null!);
                _configure(services);

            }
        );
    }
}