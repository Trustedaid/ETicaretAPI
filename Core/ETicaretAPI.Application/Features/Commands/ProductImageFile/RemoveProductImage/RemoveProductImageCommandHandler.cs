using ETicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;

public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductCommandResponse>
{
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductWriteRepository _productWriteRepository;
    

    public RemoveProductImageCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
    {
        _productWriteRepository = productWriteRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<RemoveProductCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.Table.Include(x => x.ProductImageFiles)
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.Id));

        var productImageFile = product?.ProductImageFiles.FirstOrDefault(x => x.Id == Guid.Parse(request.ImageId));
        if (productImageFile != null) 
            product?.ProductImageFiles.Remove(productImageFile);
        await _productWriteRepository.SaveAsync();
        
        return new();
    }
}