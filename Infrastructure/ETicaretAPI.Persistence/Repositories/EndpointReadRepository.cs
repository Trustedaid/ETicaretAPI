﻿using ETicaretAPI.Application.Repositories.Endpoint;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class EndpointReadRepository : ReadRepository<Endpoint>,IEndpointReadRepository
{
    public EndpointReadRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}