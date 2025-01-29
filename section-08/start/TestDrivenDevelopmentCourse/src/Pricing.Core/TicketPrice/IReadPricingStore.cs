using Pricing.Core.Domain;

namespace Pricing.Core.TicketPrice;

public interface IReadPricingStore
{
    Task<PricingTable> GetAsync(CancellationToken cancellationToken);
}