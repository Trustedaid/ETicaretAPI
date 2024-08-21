using MediatR;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.SetShowcaseImage;

public class SetShowcaseImageCommandRequest : IRequest<SetShowcaseImageCommandResponse>
{
    public string ImageId { get; set; }
    public string ProductId { get; set; }
}