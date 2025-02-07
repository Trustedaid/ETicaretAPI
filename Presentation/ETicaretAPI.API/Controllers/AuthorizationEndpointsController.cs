using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaretAPI.Application.Features.Commands.AuthorizationEndpoint.AssignRoleEndpoint;
using ETicaretAPI.Application.Features.Queries.AuthorizationEndpoint.GetRolesToEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        
        public async Task<IActionResult> GetRolesToEndpoint(GetRolesToEndpointQueryRequest rolesToEndpointQueryRequest)
        {
            var response = await _mediator.Send(rolesToEndpointQueryRequest);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> AssignRoleEndpoint(
            AssignRoleEndpointCommandRequest assignRoleEndpointCommandRequest)
        {
            assignRoleEndpointCommandRequest.Type = typeof(Program);
            var response = await _mediator.Send(assignRoleEndpointCommandRequest);
            
            return Ok(response);
        }
        
        
    }
   
}