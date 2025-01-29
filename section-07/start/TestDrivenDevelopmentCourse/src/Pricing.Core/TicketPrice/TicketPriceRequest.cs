namespace Pricing.Core.TicketPrice;

public record TicketPriceRequest(DateTimeOffset Entry, DateTimeOffset Exit)
{
}