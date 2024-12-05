using ETicaretAPI.Domain.Entities.Common;

namespace ETicaretAPI.Domain.Entities;

public class CompletedOrders : BaseEntity
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    
    
}