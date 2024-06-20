using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Product.GetProductImages;

public class GetProductImagesQueryRequest : IRequest<List<GetProductImagesQueryResponse>>
{
    public string Id { get; set; }
}