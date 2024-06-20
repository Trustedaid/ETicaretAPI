using ETicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;

public class RemoveProductImageCommandRequest : IRequest<RemoveProductCommandResponse>
{
    public string Id { get; set; }
    public string? ImageId { get; set; }
}