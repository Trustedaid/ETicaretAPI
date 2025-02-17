using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Product.UpdateStockQrCodeToProduct;

public class UpdateStockToProductCommandHandler : IRequestHandler<UpdateStockToProductCommandRequest,
    UpdateStockToProductCommandResponse>
{
    private readonly IProductService _productService;

    public UpdateStockToProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<UpdateStockToProductCommandResponse> Handle(
        UpdateStockToProductCommandRequest request, CancellationToken cancellationToken)
    {
        await _productService.StockUpdateToProductAsync(request.ProductId, request.Stock);
        return new UpdateStockToProductCommandResponse();
    }
}