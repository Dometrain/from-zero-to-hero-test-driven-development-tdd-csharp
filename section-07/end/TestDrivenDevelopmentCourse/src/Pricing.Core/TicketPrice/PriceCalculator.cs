using Pricing.Core.Domain;
using Pricing.Core.TicketPrice.Extensions;

namespace Pricing.Core.TicketPrice;

public class PriceCalculator : IPriceCalculator
{
    public decimal Calculate(PricingTable pricingTable, TicketPriceRequest ticketPriceRequest)
    {
        var price = 0m;
        var ticketHoursToPay = ticketPriceRequest
            .GetDurationInHours();

        foreach (var tier in pricingTable.Tiers)
        {
            price += CalculateTierPrice(tier, ticketHoursToPay);
            ticketHoursToPay -= tier.HourLimit;

            if (ticketHoursToPay <= 0)
                break;
        }

        return Math.Min(price, pricingTable.GetMaxDailyPrice());
    }

    private static decimal CalculateTierPrice(PriceTier tier, int ticketHoursToPay)
    {
        var hoursInTier = Math.Min(tier.HourLimit, ticketHoursToPay);
        return tier.Price * hoursInTier;
    }
}