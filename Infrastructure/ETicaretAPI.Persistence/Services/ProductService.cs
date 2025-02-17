using System.Text.Json;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Repositories;

namespace ETicaretAPI.Persistence.Services;

public class ProductService : IProductService
{
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IQRCodeService _qrCodeService;

    public ProductService(IProductReadRepository productReadRepository, IQRCodeService qrCodeService,
        IProductWriteRepository productWriteRepository)
    {
        _productReadRepository = productReadRepository;
        _qrCodeService = qrCodeService;
        _productWriteRepository = productWriteRepository;
    }

    public async Task<byte[]> QRCodeToProductAsync(string productId)
    {
        var product = await _productReadRepository.GetByIdAsync(productId);
        if (product == null)
            throw new Exception("Product not found");

        var plainObject = new
        {
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            product.CreatedDate
        };
        var plainText = JsonSerializer.Serialize(plainObject);
        return _qrCodeService.GenerateQRCode(plainText);
    }

    public async Task StockUpdateToProductAsync(string productId, int stock)
    {
        var product = await _productReadRepository.GetByIdAsync(productId);
        if (product == null)
            throw new Exception("Product not found");
        product.Stock = stock;
        await _productWriteRepository.SaveAsync();
    }
}