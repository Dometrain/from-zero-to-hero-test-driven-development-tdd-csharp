using Pricing.Core.Domain;

namespace Pricing.Core.ApplyPricing;

public interface IPricingStore
{
    Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken);  
}