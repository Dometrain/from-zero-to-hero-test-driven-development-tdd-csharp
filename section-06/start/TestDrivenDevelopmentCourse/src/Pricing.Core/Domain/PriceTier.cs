using Pricing.Core.Domain.Exceptions;

namespace Pricing.Core.Domain;

public class PriceTier
{
    public PriceTier(int hourLimit, decimal price)
    {
        if (hourLimit is < 1 or > 24)
            throw new InvalidPricingTierException();
        if (price < 0)
            throw new InvalidPricingTierException();

        HourLimit = hourLimit;
        Price = price;
    }

    public decimal Price { get; }
    public int HourLimit { get; }
}