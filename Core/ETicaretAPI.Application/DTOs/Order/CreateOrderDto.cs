namespace ETicaretAPI.Application.DTOs.Order;

public class CreateOrderDto
{
    public string? CartId { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
}