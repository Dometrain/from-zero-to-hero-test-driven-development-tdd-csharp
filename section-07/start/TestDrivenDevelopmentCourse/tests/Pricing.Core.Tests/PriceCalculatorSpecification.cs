using System.Runtime.InteropServices;
using FluentAssertions;
using Pricing.Core.Domain;
using Pricing.Core.TicketPrice;

namespace Pricing.Core.Tests;

public class PriceCalculatorSpecification
{
    private readonly IPriceCalculator _calculator = new PriceCalculator();
    
    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    public void Should_return_1hour_price_for_30_min_ticket(int priceFirstHour)
    {
        var exit = DateTimeOffset.UtcNow;
        var entry = exit.AddMinutes(-30);
        var pricingTable = new PricingTable(new[]
        {
            new PriceTier(1, priceFirstHour),
            new PriceTier(24, 1)
        });
        
        var result = _calculator.Calculate(pricingTable, new TicketPriceRequest(entry, exit));
        
        result.Should().Be(priceFirstHour);
    }
    
    [Fact]
    public void Should_return_5_hour_price_for_4_hours_and_half()
    {
        var exit = DateTimeOffset.UtcNow;
        var entry = exit.AddHours(-4).AddMinutes(-30);
        var pricingTable = new PricingTable(new[]
        {
            new PriceTier(1, 2),
            new PriceTier(4, 1),
            new PriceTier(24, 1)
        });

        var price = _calculator.Calculate(pricingTable, new TicketPriceRequest(entry, exit));

        price.Should().Be(6);
    }

    [Fact]
    public void Should_be_max_daily_price_if_calculated_price_exceeds_it()
    {
        var exit = DateTimeOffset.UtcNow;
        var entry = exit.AddHours(-20);
        const int maxDailyPrice = 10;
        var pricingTable = new PricingTable(new[]
        {
            new PriceTier(24, 1)
        }, maxDailyPrice);

        var price = _calculator.Calculate(
            pricingTable, new TicketPriceRequest(entry, exit));

        price.Should().Be(maxDailyPrice);
    }
    
}