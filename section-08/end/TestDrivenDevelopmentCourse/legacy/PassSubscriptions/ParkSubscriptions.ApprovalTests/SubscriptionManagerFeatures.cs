using System.Configuration;
using System.Reflection;

namespace ParkSubscriptions.ApprovalTests;

[UsesVerify]
public class SubscriptionManagerFeatures
{
    private readonly SubscriptionsManager _manager;

    public SubscriptionManagerFeatures()
    {
        _manager = new SubscriptionsManager();
#if NETCOREAPP
        // Fix: Issue https://github.com/dotnet/runtime/issues/22720
        var configFile = $"{Assembly.GetExecutingAssembly().Location}.config";
        var outputConfigFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
        File.Copy(configFile, outputConfigFile, true);
#endif
    }

    [Fact]
    public Task Given_a_gold_subscription_When_apply_discount_Then_has_expected_discount()
    {
        const string licensePlate = "aa-bb-cc";
        _manager.AddSubscription(licensePlate,
            new DateTime(2023, 8, 1), "GOLD");

        var result = _manager.ApplyDiscount(licensePlate,
            new DateTime(2023, 8, 17), 10);

        return Verify(result);
    }

    [Fact]
    public Task Given_a_partner_subscription_When_apply_discount_Then_has_expected_discount()
    {
        const string licensePlate = "aa-01-cc";
        _manager.AddSubscription(licensePlate,
            new DateTime(2023, 8, 1), "partner");

        var result = _manager.ApplyDiscount(licensePlate,
            new DateTime(2023, 8, 17), 10);

        return Verify(result);
    }

    [Fact]
    public Task Given_a_partner_subscription_before_2024_When_apply_discount_Then_has_expected_discount()
    {
        var dateTimeProvider = new FakeDateTimeProvider(new DateTime(2023, 12, 31));
        var manager = new SubscriptionsManager(dateTimeProvider, new SubscriptionRepository());
        const string licensePlate = "aa-01-cc";
        manager.AddSubscription(licensePlate,
            new DateTime(2023, 8, 1), "partner");

        var result = manager.ApplyDiscount(licensePlate,
            new DateTime(2023, 8, 17), 10);

        return Verify(result);
    }

    [Fact]
    public Task Given_a_economic_subscription_When_apply_discount_Then_has_expected_discount()
    {
        const string licensePlate = "aa-02-cc";
        _manager.AddSubscription(licensePlate,
            new DateTime(2023, 8, 1), "Economic");

        var result = _manager.ApplyDiscount(licensePlate,
            new DateTime(2023, 8, 17), 10);

        return Verify(result);
    }

    [Fact]
    public Task Given_a_partner_subscription_for_AA_BB_11_When_apply_discount_Then_has_expected_discount()
    {
        const string licensePlate = "AA-BB-11";
        _manager.AddSubscription(licensePlate,
            new DateTime(2023, 8, 1), "partner");

        var result = _manager.ApplyDiscount(licensePlate,
            new DateTime(2023, 8, 17), 10);

        return Verify(result);
    }

    [Fact]
    public Task Given_a_weekends_subscription_When_apply_discount_Then_has_expected_discount()
    {
        const string licensePlate = "aa-03-cc";
        _manager.AddSubscription(licensePlate,
            new DateTime(2023, 8, 1), "Weekends");

        var result = _manager.ApplyDiscount(licensePlate,
            new DateTime(2023, 8, 12), 10);

        return Verify(result);
    }

    [Fact]
    public Task Given_a_weekends_subscription_on_weekday_When_apply_discount_Then_has_expected_discount()
    {
        const string licensePlate = "aa-03-cc";
        _manager.AddSubscription(licensePlate,
            new DateTime(2023, 8, 1), "Weekends");

        var result = _manager.ApplyDiscount(licensePlate,
            new DateTime(2023, 8, 17), 10);

        return Verify(result);
    }
}