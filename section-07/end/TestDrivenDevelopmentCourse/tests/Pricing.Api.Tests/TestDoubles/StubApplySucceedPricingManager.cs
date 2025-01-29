using Pricing.Core;
using Pricing.Core.ApplyPricing;

namespace Pricing.Api.Tests.TestDoubles;

public class StubApplySucceedPricingManager : IPricingManager
{
    public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}