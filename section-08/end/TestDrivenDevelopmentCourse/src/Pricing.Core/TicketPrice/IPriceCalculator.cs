using Pricing.Core.Domain;

namespace Pricing.Core.TicketPrice;

public interface IPriceCalculator
{
    decimal Calculate(PricingTable pricingTable, TicketPriceRequest ticketPriceRequest);
}