using System.Configuration;
using MySql.Data.MySqlClient;

namespace ParkSubscriptions;

public interface ISubscriptionRepository
{
    void Add(SubscriptionsManager.Subscription subscription);
    SubscriptionsManager.SubscriptionType? GetSubscriptionType(string licensePlate, DateTime date);
}

public class SubscriptionRepository : ISubscriptionRepository
{
    public void Add(SubscriptionsManager.Subscription subscription)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand($"INSERT INTO Subscriptions (Level, LicensePlate, ActiveOn)" +
                                           $"Values (@p3, @p1, @p2)",
                connection);

            command.Parameters.AddWithValue("@p1", subscription.LicensePlate);
            command.Parameters.AddWithValue("@p2", subscription.ActiveOn);
            command.Parameters.AddWithValue("@p3", subscription.Level);

            command.ExecuteNonQuery();
        }
    }
    
    public SubscriptionsManager.SubscriptionType? GetSubscriptionType(string licensePlate, DateTime date)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        var command = new MySqlCommand($"SELECT Level FROM Subscriptions " +
                                       $"WHERE LicensePlate = '{licensePlate}' and ActiveOn = '{date.ToString("yyyy-MM")}'",
            connection);
        var result = command.ExecuteScalar();


        SubscriptionsManager.SubscriptionType? subscriptionType
            = result is null ? null : (SubscriptionsManager.SubscriptionType)Convert.ToInt32(result);
        return subscriptionType;
    }
}