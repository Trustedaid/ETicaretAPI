using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Domain.Entities.Identity;

namespace ETicaretAPI.Domain.Entities;

public class Cart : BaseEntity
{
    public string UserId { get; set; }
   
    public AppUser User { get; set; }
    public Order Order { get; set; }
    public ICollection<CartItem>  CartItems { get; set; }
    
}