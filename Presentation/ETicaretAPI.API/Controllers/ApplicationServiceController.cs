using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ETicaretAPI.Application.Abstractions.Services.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationServiceController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationServiceController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpGet]
        public IActionResult GetAuthorizeDefinitionEndpoints()
        {
            var data = _applicationService.GetAuthorizeDefinitionEndPoints(typeof(Program));
            return Ok(data);
        }
    }
}