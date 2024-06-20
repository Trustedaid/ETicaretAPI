using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.ProductImageFile;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;

public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest,
    UploadProductImageCommandResponse>
{
    private readonly IStorageService _storageService;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

    public UploadProductImageCommandHandler(IStorageService storageService, IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
    {
        _storageService = storageService;
        _productReadRepository = productReadRepository;
        _productImageFileWriteRepository = productImageFileWriteRepository;
    }

    public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _storageService.UploadAsync("photo-images", request.Files);

        var product = await _productReadRepository.GetByIdAsync(request.Id);


        await _productImageFileWriteRepository.AddRangeAsync(result.Select(x => new Domain.Entities.ProductImageFile
        {
            FileName = x.fileName,
            Path = x.pathOrContainerName,
            Storage = _storageService.StorageName,
            Products = new List<Domain.Entities.Product>() { product }
        }).ToList());

        await _productImageFileWriteRepository.SaveAsync();
        return new();
    }
}