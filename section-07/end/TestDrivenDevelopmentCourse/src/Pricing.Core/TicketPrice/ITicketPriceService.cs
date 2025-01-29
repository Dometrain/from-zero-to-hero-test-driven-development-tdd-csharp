namespace Pricing.Core.TicketPrice;

public interface ITicketPriceService
{
    Task<TicketPriceResponse> HandleAsync(TicketPriceRequest request, CancellationToken cancellationToken);
}