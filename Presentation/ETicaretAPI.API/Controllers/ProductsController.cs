using System.Net;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Enums;
using ETicaretAPI.Application.Features.Commands.Product.CreateProduct;
using ETicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using ETicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using ETicaretAPI.Application.Features.Commands.Product.UpdateStockQrCodeToProduct;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.SetShowcaseImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ETicaretAPI.Application.Features.Queries.Product.GetAllProduct;
using ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using ETicaretAPI.Application.Features.Queries.Product.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IProductService _productService;

    public ProductsController(IMediator mediator, IProductService productService)
    {
        _mediator = mediator;
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
    {
        var response = await _mediator.Send(getAllProductQueryRequest);
        return Ok(response);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
    {
        var response = await _mediator.Send(getByIdProductQueryRequest);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing,
        Definition = "Create Product")]
    public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
    {
        var response = await _mediator.Send(createProductCommandRequest);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating,
        Definition = "Update Product")]
    public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
    {
        var response = await _mediator.Send(updateProductCommandRequest);
        return Ok(response + "Product Updated Successfully");
    }

    [HttpDelete("{Id}")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting,
        Definition = "Delete Product")]
    public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
    {
        var response = await _mediator.Send(removeProductCommandRequest);
        return Ok(response + "Product Deleted Successfully");
    }

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing,
        Definition = "Upload Product File")]
    public async Task<IActionResult> Upload(
        [FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
    {
        uploadProductImageCommandRequest.Files = Request.Form.Files;
        var response = await _mediator.Send(uploadProductImageCommandRequest);
        return Ok(response);
    }

    [HttpGet("[action]/{Id}")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading,
        Definition = "Get Products Images")]
    public async Task<IActionResult> GetProductImages(
        [FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
    {
        var response = await _mediator.Send(getProductImagesQueryRequest);
        return Ok(response);
    }


    [HttpGet("qrcode/{productId}")]
    public async Task<IActionResult> GetQRCodeToProduct([FromRoute] string productId)
    {
        var data = await _productService.QRCodeToProductAsync(productId);
        return File(data, "image/png");
    }
    
    
    [HttpPut("qrcode")]
    public async Task<IActionResult> UpdateStockQrCodeToProduct(UpdateStockToProductCommandRequest updateStockToProductCommandRequest)
    {
        var response = await _mediator.Send(updateStockToProductCommandRequest);
        return Ok(response);
    }
    


    [HttpDelete("[action]/{Id}")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting,
        Definition = "Delete Product Image")]
    public async Task<IActionResult> DeleteProductImage(
        [FromRoute] RemoveProductImageCommandRequest removeProductImageCommandRequest, [FromQuery] string imageId)
    {
        removeProductImageCommandRequest.ImageId = imageId;
        var response = await _mediator.Send(removeProductImageCommandRequest);
        return Ok(response);
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating,
        Definition = "Set Showcase Image")]
    public async Task<IActionResult> SetShowcaseImage(
        [FromQuery] SetShowcaseImageCommandRequest setShowcaseImageCommandRequest)
    {
        var response = await _mediator.Send(setShowcaseImageCommandRequest);
        return Ok(response);
    }
}