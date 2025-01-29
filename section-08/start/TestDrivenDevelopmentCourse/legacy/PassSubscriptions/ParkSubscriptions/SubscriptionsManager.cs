using System.Configuration;
using MySql.Data.MySqlClient;

namespace ParkSubscriptions;

public class SubscriptionsManager
{
    public void AddSubscription(string licensePlate, DateTime month, string type)
    {
        var subscription = new Subscription();
        if (type.ToLower() == "gold")
        {
            subscription.Level = 1;
            subscription.LicensePlate = licensePlate;
            subscription.ActiveOn = month.ToString("yyyy-MM");
        }
        else if (type.ToLower() == "partner")
        {
            subscription.Level = 2;
            subscription.LicensePlate = licensePlate;
            subscription.ActiveOn = month.ToString("yyyy-MM");
        }
        else if (type.ToLower() == "economic")
        {
            subscription.Level = 3;
            subscription.LicensePlate = licensePlate;
            subscription.ActiveOn = month.ToString("yyyy-MM");
        }
        else
        {
            throw new Exception();
        }


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

    public decimal ApplyDiscount(string licensePlate, DateTime date, decimal price)
    {
        decimal aux = 0;

        var connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand($"SELECT Level FROM Subscriptions " +
                                           $"WHERE LicensePlate = '{licensePlate}' and ActiveOn = '{date.ToString("yyyy-MM")}'",
                connection);
            var result = command.ExecuteScalar();


            if (result != null)
            {
                int l = Convert.ToInt32(result);
                if (l == 1)
                {
                    aux = 0;
                }
                else if (l == 2 && licensePlate == "AA-BB-11")
                {
                    aux = price * 0.5M;
                }
                else if (l == 2)
                {
                    // New discount value for 2024
                    if (DateTime.Now < new DateTime(2024, 01, 01))
                        aux = price - price * 0.2M;
                    else
                        aux = price - price * 0.18M;
                }
                else
                {
                    // TODO: Implement Custom Discount
                    // command = new SqlCommand($"SELECT Discount FROM CustomDiscount WHERE LicesePlace = '{licensePlate}'", connection);
                    // result = command.ExecuteScalar();
                    // return Convert.ToInt32(result);
                    aux = price - price * 0.05M;
                }
            }
            else
            {
                aux = price;
            }
        }

        return aux;
    }
    
    private class Subscription
    {
        public string  LicensePlate { get; set; }
        public int Level { get; set; }
        public string ActiveOn { get; set; }
    }
}