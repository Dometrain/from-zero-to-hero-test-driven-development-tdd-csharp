using Pricing.Core.Domain;

namespace Pricing.Core;

public interface IPricingStore
{
    Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken);  
}