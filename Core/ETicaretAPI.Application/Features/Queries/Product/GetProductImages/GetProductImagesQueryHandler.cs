using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Application.Features.Queries.Product.GetProductImages;

public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
{
    private readonly IProductReadRepository _productReadRepository;
    private readonly IConfiguration _configuration;

    public GetProductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
    {
        _productReadRepository = productReadRepository;
        _configuration = configuration;
    }

    public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.Table.Include(x => x.ProductImageFiles)
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.Id));


        return product?.ProductImageFiles.Select(x => new GetProductImagesQueryResponse
        {
            Path = $"{_configuration["BaseStorageURL"]}/{x.Path}",
            FileName = x.FileName,
            Id = x.Id
        }).ToList() ?? throw new InvalidOperationException();
    }
}