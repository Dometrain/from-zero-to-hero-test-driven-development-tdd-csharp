namespace ParkSubscriptions;

public class DiscountEngine
{
    private readonly IDateTimeProvider _dateTimeProvider;
    public DiscountEngine(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public decimal GetPriceAfterDiscount(decimal price,
        string licensePlate, SubscriptionsManager.SubscriptionType subscriptionType,
        DateTime date)
    {
        var priceAfterDiscount = subscriptionType switch
        {
            SubscriptionsManager.SubscriptionType.Gold => 0,
            SubscriptionsManager.SubscriptionType.Partner when licensePlate == "AA-BB-11" => price * 0.5M,
            SubscriptionsManager.SubscriptionType.Partner when IsBefore2024() => price - price * 0.2M,
            SubscriptionsManager.SubscriptionType.Partner => price - price * 0.18M,
            SubscriptionsManager.SubscriptionType.Weekends when IsWeekend(date) => 0,
            SubscriptionsManager.SubscriptionType.Weekends => price,
            _ => price - price * 0.05M
        };

        return priceAfterDiscount;
    }

    private static bool IsWeekend(DateTime date)
        => date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    private bool IsBefore2024()
        => _dateTimeProvider.Now < new DateTime(2024, 01, 01);
}