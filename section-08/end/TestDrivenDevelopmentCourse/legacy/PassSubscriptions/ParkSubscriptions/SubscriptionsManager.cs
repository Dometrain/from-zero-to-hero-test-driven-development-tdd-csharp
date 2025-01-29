namespace ParkSubscriptions;

public class SubscriptionsManager
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly DiscountEngine _discountEngine;

    public SubscriptionsManager()
    {
        _discountEngine = new DiscountEngine(new SystemDateTimeProvider());
        _subscriptionRepository = new SubscriptionRepository();
    }

    public SubscriptionsManager(IDateTimeProvider dateTimeProvider,
        ISubscriptionRepository subscriptionRepository)
    {
        _discountEngine = new DiscountEngine(dateTimeProvider);
        _subscriptionRepository = subscriptionRepository;
    }

    public void AddSubscription(string licensePlate, DateTime month, string type)
    {
        var subscription = MapSubscription(type, licensePlate, month);

        _subscriptionRepository.Add(subscription);
    }

    public decimal ApplyDiscount(string licensePlate, DateTime date, decimal price)
    {
        var subscriptionType =
            _subscriptionRepository.GetSubscriptionType(licensePlate, date);

        if (subscriptionType is null)
            return price;
        
        return _discountEngine.GetPriceAfterDiscount(
            price,
            licensePlate,
            subscriptionType.Value,
            date
        );
    }
    
    public SubscriptionsManager.Subscription MapSubscription(string type, string licensePlate,
        DateTime month)
    {
        if (!Enum.TryParse<SubscriptionType>(type, ignoreCase: true, out var subscriptionType))
            throw new Exception();

        var subscription = new SubscriptionsManager.Subscription
        {
            LicensePlate = licensePlate,
            ActiveOn = month.ToString("yyyy-MM"),
            Level = (int)subscriptionType
        };

        return subscription;
    }


    public class Subscription
    {
        public string LicensePlate { get; set; }
        public int Level { get; set; }
        public string ActiveOn { get; set; }
    }

    public enum SubscriptionType
    {
        Gold = 1,
        Partner = 2,
        Economic = 3,
        Weekends = 4
    }
}

internal class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}

public interface IDateTimeProvider
{
    public DateTime Now { get; }
}