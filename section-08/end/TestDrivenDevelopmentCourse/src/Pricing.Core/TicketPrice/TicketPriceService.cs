namespace Pricing.Core.TicketPrice;

public class TicketPriceService : ITicketPriceService
{
    private readonly IReadPricingStore _pricingStore;
    private readonly IPriceCalculator _pricingCalculator;

    public TicketPriceService(IReadPricingStore pricingStore, IPriceCalculator pricingCalculator)
    {
        _pricingStore = pricingStore;
        _pricingCalculator = pricingCalculator;
    }

    public async Task<TicketPriceResponse> HandleAsync(TicketPriceRequest request, CancellationToken cancellationToken)
    {
        if (request.Entry >= request.Exit)
            throw new ArgumentException();
        
        var pricingTable = await _pricingStore.GetAsync(cancellationToken);

        var price = _pricingCalculator.Calculate(pricingTable, request);
        return new TicketPriceResponse(price);
    }
}