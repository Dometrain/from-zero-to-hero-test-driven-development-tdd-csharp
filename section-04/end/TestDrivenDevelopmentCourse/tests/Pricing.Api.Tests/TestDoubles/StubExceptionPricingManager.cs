using Pricing.Core;
using Pricing.Core.Domain.Exceptions;

namespace Pricing.Api.Tests;

public class StubExceptionPricingManager: IPricingManager
{
    public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken cancellationToken)
    {
        throw new InvalidPricingTierException();
    }
}