using Pricing.Core.ApplyPricing;
using Pricing.Core.Domain;

namespace Pricing.Core.Tests.TestDoubles;

public class InMemoryPricingStore : IPricingStore
{
    private PricingTable _pricingTable;
    public Task<bool> SaveAsync(PricingTable pricingTable, CancellationToken cancellationToken)
    {
        _pricingTable = pricingTable;
        return Task.FromResult(true);
    }

    public PricingTable GetPricingTable()
    {
        return _pricingTable;
    }
}