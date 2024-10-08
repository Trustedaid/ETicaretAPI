﻿using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Domain.Entities.Identity;

public class AppUser : IdentityUser<string>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDate { get; set; }
    
    public ICollection<Cart> Carts { get; set; }
    
    
}