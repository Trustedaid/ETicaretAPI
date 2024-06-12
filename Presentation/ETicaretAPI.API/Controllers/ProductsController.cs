using System.Net;
using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.File;
using ETicaretAPI.Application.Repositories.InvoiceFile;
using ETicaretAPI.Application.Repositories.ProductImageFile;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using File = ETicaretAPI.Domain.Entities.File;

namespace ETicaretAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase

//                      ------------------TEST CONTROLLER----------------------------------
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileWriteRepository _fileWriteRepository;
    private readonly IFileReadRepository _fileReadRepository;
    private readonly IProductImageFileReadRepository _productImageFileReadRepository;
    private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
    private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
    private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
    private readonly IStorageService _storageService;


    public ProductsController(IProductWriteRepository productWriteRepository,
        IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment,
        IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository,
        IProductImageFileReadRepository productImageFileReadRepository,
        IProductImageFileWriteRepository productImageFileWriteRepository,
        IInvoiceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository,
        IStorageService storageService)
    {
        _productWriteRepository = productWriteRepository;
        _productReadRepository = productReadRepository;
        _webHostEnvironment = webHostEnvironment;
        _fileWriteRepository = fileWriteRepository;
        _fileReadRepository = fileReadRepository;
        _productImageFileReadRepository = productImageFileReadRepository;
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _invoiceFileReadRepository = invoiceFileReadRepository;
        _invoiceFileWriteRepository = invoiceFileWriteRepository;
        _storageService = storageService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Pagination pagination)
    {
        var totalCount = _productReadRepository.GetAll(false).Count();
        var products = _productReadRepository.GetAll(false)
            .Skip(pagination.Page * pagination.Size).Take(pagination.Size)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Stock,
                x.Price,
                x.CreatedDate,
                x.UpdatedDate
            }).ToList();
        return Ok(new
        {
            totalCount,
            products
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        return Ok(await _productReadRepository.GetByIdAsync(id, false));
    }

    [HttpPost]
    public async Task<IActionResult> Post(VMCreateProduct model)
    {
        await _productWriteRepository.AddAsync(new()
        {
            Name = model.Name,
            Price = model.Price,
            Stock = model.Stock
        });
        await _productWriteRepository.SaveAsync();
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    public async Task<IActionResult> Put(VMUpdateProduct model)
    {
        Product product = await _productReadRepository.GetByIdAsync(model.Id);
        product.Stock = model.Stock;
        product.Name = model.Name;
        product.Price = model.Price;

        await _productWriteRepository.SaveAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _productWriteRepository.RemoveAsync(id);
        await _productWriteRepository.SaveAsync();

        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Upload()
    {
        var datas = await _storageService.UploadAsync("resource/files", Request.Form.Files);
        // var datas =  await _fileService.UploadAsync("resource/files", Request.Form.Files);


        await _productImageFileWriteRepository.AddRangeAsync(datas.Select(x => new ProductImageFile()
        {
            FileName = x.fileName,
            Path = x.pathOrContainerName,
            Storage = _storageService.StorageName
        }).ToList());
        await _productImageFileWriteRepository.SaveAsync();


        // await _fileWriteRepository.AddRangeAsync(datas.Select(x => new File()
        // {
        //     FileName = x.fileName,
        //     Path = x.path
        // }).ToList());
        // await _fileWriteRepository.SaveAsync();
        //


        // var d1 = _fileReadRepository.GetAll(false);
        // var d2 = _productReadRepository.GetAll(false);
        // var d3 = _productReadRepository.GetAll(false);
        return Ok();
    }
}