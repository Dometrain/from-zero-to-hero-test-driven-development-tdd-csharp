using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain;

namespace Pricing.Core.Tests.TestDoubles;

public class StubSuccessPricingStore : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}