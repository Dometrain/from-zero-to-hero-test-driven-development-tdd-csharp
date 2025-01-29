namespace Pricing.Core.TicketPrice.Extensions;

public static class TicketPriceRequestExtensions
{
    public static int GetDurationInHours(this TicketPriceRequest self)
    {
        const int upperHourLimit = 59;
        const int oneHourInMinutes = 60;

        var ticketDuration =
            (int)(self.Exit - self.Entry).TotalMinutes;

        return (ticketDuration + upperHourLimit) / oneHourInMinutes;
    }
}