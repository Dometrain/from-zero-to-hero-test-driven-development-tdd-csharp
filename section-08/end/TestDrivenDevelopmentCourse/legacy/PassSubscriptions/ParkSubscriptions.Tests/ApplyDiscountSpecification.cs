using FluentAssertions;

namespace ParkSubscriptions.Tests;

public class ApplyDiscountSpecification
{
    [Theory]
    [InlineData(SubscriptionsManager.SubscriptionType.Gold, 0, 2023)]
    [InlineData(SubscriptionsManager.SubscriptionType.Partner, 8, 2023)]
    [InlineData(SubscriptionsManager.SubscriptionType.Partner, 8.2, 2024)]
    [InlineData(SubscriptionsManager.SubscriptionType.Economic, 9.5, 2023)]
    public void Should_apply_discount_based_on_level(SubscriptionsManager.SubscriptionType subscriptionType, decimal expectedPrice, int year)
    {
        var dateTimeProvider = new DummyDateTimeProvider(year);
        var discountEngine = new DiscountEngine(dateTimeProvider);
            
        var priceAfterDiscount =
            discountEngine.GetPriceAfterDiscount(10, "aa", 
                subscriptionType, DateTime.Now);

        priceAfterDiscount.Should()
            .Be(expectedPrice);
    }
    
    [Fact]
    public void Should_apply_100_percent_discount_to_weekends_subscriptions_on_a_weekend()
    {
        var discountEngine = new DiscountEngine(new DummyDateTimeProvider(2023));

        var priceWithDiscount = discountEngine.
            GetPriceAfterDiscount(10, 
                "AA",
                SubscriptionsManager.SubscriptionType.Weekends,
                new DateTime(2023, 08, 12));

        priceWithDiscount.Should()
            .Be(0);

    }
}