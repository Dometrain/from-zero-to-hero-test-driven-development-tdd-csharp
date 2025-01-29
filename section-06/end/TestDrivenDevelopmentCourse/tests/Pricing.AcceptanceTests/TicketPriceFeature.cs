using System.Net.Http.Json;
using FluentAssertions;
using Pricing.Api;
using Pricing.Core;
using Pricing.Core.ApplyPricing;
using Pricing.Core.TicketPrice;

namespace Pricing.AcceptanceTests;

public class TicketPriceFeature : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public TicketPriceFeature(ApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task Given_30min_ticket_When_1hour_has_price_2_Then_return_2()
    {
        await _client.PutAsJsonAsync("PricingTable",
            new ApplyPricingRequest(new[]
            {
                new PriceTierRequest(1, 2),
                new PriceTierRequest(24, 5)
            }));
        var exit = DateTimeOffset.UtcNow;
        var entry = exit.AddMinutes(-30);

        var response =
            await _client.GetFromJsonAsync<TicketPriceResponse>(
                $"TicketPrice?entry={entry:u}&exit={exit:u}");

        response.Should().NotBeNull();
        response.Price.Should().Be(2);
    }
}