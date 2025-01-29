using Pricing.Core;
using Pricing.Core.ApplyPricing;

namespace Pricing.Api.Tests.TestDoubles;

public class StubApplyFailPricingManager : IPricingManager
{
    public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }
}