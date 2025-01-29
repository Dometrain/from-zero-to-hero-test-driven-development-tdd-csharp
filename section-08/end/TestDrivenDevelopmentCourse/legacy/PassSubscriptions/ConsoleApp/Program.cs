using ParkSubscriptions;

var manager = new SubscriptionsManager();

manager.AddSubscription("aa-bb-cc", new DateTime(2023, 8, 1), "GOLD");

var res = manager.ApplyDiscount("aa-bb-cc", new DateTime(2023, 8, 17), 10);

Console.WriteLine($"Price after discount: {res}");