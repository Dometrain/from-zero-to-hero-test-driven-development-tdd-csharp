using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Pricing.Api.TicketPrice;
using Pricing.Core;
using Pricing.Core.TicketPrice;

namespace Pricing.Api.Tests;

public class TicketPriceEndpointSpecification
{
    [Fact]
    public async Task Should_return_200_with_price_when_get_price()
    {
        var exit = DateTimeOffset.UtcNow;
        var entry = exit.AddMinutes(-30);
        var ticketPriceService = Substitute.For<ITicketPriceService>();
        var ticketPriceRequest = new TicketPriceRequest(entry, exit);
        ticketPriceService.HandleAsync(ticketPriceRequest, default)
            .Returns(new TicketPriceResponse(2));

        var result = await TicketPriceEndpoint.HandleAsync(
            entry,
            exit,
            ticketPriceService,
            default
        );

        result.Should().BeOfType<Ok<TicketPriceResponse>>();
        result.Value.Price.Should().Be(2);
        await ticketPriceService.Received(1)
            .HandleAsync(ticketPriceRequest, default);
    }
}