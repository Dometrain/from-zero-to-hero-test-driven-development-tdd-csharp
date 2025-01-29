using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pricing.Api.Tests.TestDoubles;
using Pricing.Core;
using Pricing.Core.ApplyPricing;
using Pricing.Core.Tests.TestDoubles;

namespace Pricing.Api.Tests;

public class ApplyPricingEndpointSpecification
{
    private const string RequestUri = "PricingTable";

    [Fact]
    public async Task Should_return_500_when_causes_exception()
    {
        using var client =
            CreateApiWithPricingManager<StubExceptionPricingManager>()
                .CreateClient();

        var response = await client.PutAsJsonAsync(RequestUri,
            CreateRequest());

        response.Should().HaveStatusCode(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Should_return_400_when_pricing_manager_return_false()
    {
        using var client =
            CreateApiWithPricingManager<StubApplyFailPricingManager>()
                .CreateClient();

        var response = await client.PutAsJsonAsync(RequestUri,
            CreateRequest());

        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_return_200_when_pricing_manager_return_false()
    {
        using var client =
            CreateApiWithPricingManager<StubApplySucceedPricingManager>()
                .CreateClient();

        var response = await client.PutAsJsonAsync(RequestUri,
            CreateRequest());

        response.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_send_request_to_pricing_manager()
    {
        var pricingStore = new InMemoryPricingStore();
        var api = new ApiFactory(services =>
        {
            services.TryAddScoped<IPricingStore>(s =>
                pricingStore);
        });
        var client = api.CreateClient();
        var applyPricingRequest = CreateRequest();

        await client.PutAsJsonAsync(RequestUri,
            applyPricingRequest);

        pricingStore.GetPricingTable()
            .Should()
            .BeEquivalentTo(applyPricingRequest);
    }

    private static ApplyPricingRequest CreateRequest()
    {
        return new ApplyPricingRequest(new[] { new PriceTierRequest(24, 1) });
    }

    private static ApiFactory CreateApiWithPricingManager<T>()
        where T : class, IPricingManager
    {
        var api = new ApiFactory(services =>
        {
            services.RemoveAll(typeof(IPricingManager));

            services.TryAddScoped<IPricingManager, T>();
        });
        return api;
    }
}