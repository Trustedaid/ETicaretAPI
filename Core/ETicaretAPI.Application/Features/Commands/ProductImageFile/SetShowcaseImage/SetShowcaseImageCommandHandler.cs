using ETicaretAPI.Application.Repositories.ProductImageFile;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.SetShowcaseImage;

public class
    SetShowcaseImageCommandHandler : IRequestHandler<SetShowcaseImageCommandRequest, SetShowcaseImageCommandResponse>
{
    private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

    public SetShowcaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
    {
        _productImageFileWriteRepository = productImageFileWriteRepository;
    }


    public async Task<SetShowcaseImageCommandResponse> Handle(SetShowcaseImageCommandRequest request,
        CancellationToken cancellationToken)
    {
        var query = _productImageFileWriteRepository.Table.Include(x => x.Products)
            .SelectMany(x => x.Products,
                (pif, p) => new
                {
                    pif,
                    p
                });
        var data = await query.FirstOrDefaultAsync(x => x.p.Id == Guid.Parse(request.ProductId) && x.pif.Showcase);

        if (data != null)
            data.pif.Showcase = false;

        var image = query.FirstOrDefault(x => x.pif.Id == Guid.Parse(request.ImageId));
        if (image != null)
            image.pif.Showcase = true;
        await _productImageFileWriteRepository.SaveAsync();
        return new();
    }
}