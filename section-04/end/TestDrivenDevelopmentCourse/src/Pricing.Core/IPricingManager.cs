namespace Pricing.Core;

public interface IPricingManager
{
    Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken cancellationToken);
}