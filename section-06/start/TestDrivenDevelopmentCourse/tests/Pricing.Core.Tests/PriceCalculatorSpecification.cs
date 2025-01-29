using FluentAssertions;
using Pricing.Core.Domain;
using Pricing.Core.TicketPrice;

namespace Pricing.Core.Tests;

public class PriceCalculatorSpecification
{
    private readonly IPriceCalculator _calculator = new PriceCalculator();
    [Fact]
    public void Should_return_1hour_price_for_30_min_ticket()
    {
        var exit = DateTimeOffset.UtcNow;
        var entry = exit.AddMinutes(-30);
        var pricingTable = new PricingTable(new[]
        {
            new PriceTier(1, 2),
            new PriceTier(24, 1)
        });
        
        var result = _calculator.Calculate(pricingTable, new TicketPriceRequest(entry, exit));
        
        result.Should().Be(2);
    }
}