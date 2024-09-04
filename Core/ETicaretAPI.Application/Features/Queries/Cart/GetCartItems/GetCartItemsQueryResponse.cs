namespace ETicaretAPI.Application.Features.Queries.Cart.GetCartItems;

public class GetCartItemsQueryResponse
{
    public string CartItemId { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
}