﻿using ETicaretAPI.Application.Repositories.Endpoint;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class EndpointWriteRepository : WriteRepository<Endpoint>, IEndpointWriteRepository
{
    public EndpointWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}