namespace ETicaretAPI.Application.DTOs.Order;

public class CompletedOrderDto
{
    public string OrderCode { get; set;}
    public DateTime OrderDate { get; set;}
    public string Username { get; set;}
    public string UserLastName { get; set;}
    public string Email { get; set;}
}