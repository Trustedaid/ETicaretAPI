using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Product.UpdateStockQrCodeToProduct;

public class UpdateStockToProductCommandRequest : IRequest<UpdateStockToProductCommandResponse>
{
    public string ProductId { get; set; }
    public int Stock { get; set; }
    
}