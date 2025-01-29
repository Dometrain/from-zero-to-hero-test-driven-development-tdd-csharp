using FluentAssertions;
using NSubstitute;
using Pricing.Core.Domain;
using Pricing.Core.TicketPrice;

namespace Pricing.Core.Tests;

public class TicketPriceServiceSpecification
{
    private readonly IReadPricingStore _pricingStore;
    private readonly IPriceCalculator _priceCalculator;
    private readonly TicketPriceService _service;

    public TicketPriceServiceSpecification()
    {
        _pricingStore = Substitute.For<IReadPricingStore>();
        _priceCalculator = Substitute.For<IPriceCalculator>();
        _service = new TicketPriceService(_pricingStore, _priceCalculator);
    }

    [Fact]
    public async Task Should_get_pricing_table_from_store()
    {
        await _service.HandleAsync(
            CreateRequest(), default);

        await _pricingStore.Received(1)
            .GetAsync(default);
    }

    [Fact]
    public async Task Should_get_price_from_calculator_using_pricing_table()
    {
        var pricingTable = new PricingTable(new[] { new PriceTier(24, 1) });
        _pricingStore.GetAsync(default)
            .Returns(pricingTable);
        var ticketPriceRequest = CreateRequest();

        await _service.HandleAsync(
            ticketPriceRequest, default);

        _priceCalculator.Received(1)
            .Calculate(pricingTable, ticketPriceRequest);
    }

    [Fact]
    public async Task Should_return_price_from_calculator()
    {
        var pricingTable = new PricingTable(new[] { new PriceTier(24, 1) });
        _pricingStore.GetAsync(default)
            .Returns(pricingTable);
        var ticketPriceRequest = CreateRequest();
        _priceCalculator.Calculate(pricingTable, ticketPriceRequest)
            .Returns(2);

        var response = await _service.HandleAsync(
            ticketPriceRequest, default);

        response.Price.Should().Be(2);
    }

    private static TicketPriceRequest CreateRequest()
    {
        return new TicketPriceRequest(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
    }
}