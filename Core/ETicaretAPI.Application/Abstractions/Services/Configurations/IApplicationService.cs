﻿using ETicaretAPI.Application.DTOs.Configuration;

namespace ETicaretAPI.Application.Abstractions.Services.Configurations;

public interface IApplicationService
{
   List<Menu> GetAuthorizeDefinitionEndPoints(Type type);
    
}