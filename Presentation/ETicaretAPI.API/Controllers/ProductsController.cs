using System.Net;
using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase

//                      ------------------TEST CONTROLLER----------------------------------
{
    private readonly IProductWriteRepository _productWriteRepository;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileService _fileService;


    public ProductsController(IProductWriteRepository productWriteRepository,
        IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IFileService fileService)
    {
        _productWriteRepository = productWriteRepository;
        _productReadRepository = productReadRepository;
        _webHostEnvironment = webHostEnvironment;
        _fileService = fileService;
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
        await _fileService.UploadAsync("resource/roducts-images", Request.Form.Files);
        return Ok();
    }
}