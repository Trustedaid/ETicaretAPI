﻿using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Role.UpdateRole;

public class UpdateRoleCommandHandler :  IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCommandResponse>
{
    private readonly IRoleService _roleService;

    public UpdateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        var result = await _roleService.UpdateRole(request.Id, request.Name);
        return new UpdateRoleCommandResponse
        {
            Succeeded = result
        };
    }
}