using FluentAssertions;

namespace ParkSubscriptions.Tests;

public class AddSubscriptionSpecification
{
    [Theory]
    [InlineData("Gold", 1)]
    [InlineData("Partner", 2)]
    [InlineData("Economic", 3)]
    [InlineData("Weekends", 4)]
    public void When_subscription_type_return_subscription_with_level_number(string type, int expectedLevel)
    {
        var subscriptionsManager = new SubscriptionsManager();

        var subscription = subscriptionsManager.MapSubscription(
            type, "AA-11", DateTime.Today);

        subscription.Level.Should().Be(expectedLevel);
    }
    
}