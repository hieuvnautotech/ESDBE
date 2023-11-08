using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Common;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Standard.Information;
using FluentValidation;
using Project1.Services;
using Microsoft.Extensions.Logging;
using Project1.Dtos;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]

    public class IOTController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IBuyer2Service _Buyer2Service;
        private readonly ILogger<IOTController> _logger;
        private readonly IIotDBService _ioTService;

        public IOTController(IJwtService jwtService, IBuyer2Service Buyer2Service, IIotDBService ioTService, ILogger<IOTController> logger)
        {
            _jwtService = jwtService;
            _Buyer2Service = Buyer2Service;
            _logger = logger;
            _ioTService = ioTService;

        }
        [HttpGet("get-all")]
        //[PermissionAuthorization(PermissionConst.BUYER_READ)]
        public async Task<IActionResult> Get([FromQuery] Sensors_modelview model)
        {
            return Ok(await _Buyer2Service.Get(model));
        }



        


        

        





    }
}
