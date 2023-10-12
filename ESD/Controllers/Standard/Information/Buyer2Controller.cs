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

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]

    public class Buyer2Controller : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IBuyer2Service _Buyer2Service;

        public Buyer2Controller(IJwtService jwtService, IBuyer2Service Buyer2Service)
        {
            _jwtService = jwtService;
            _Buyer2Service = Buyer2Service;

        }
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.BUYER_READ)]
        public async Task<IActionResult> Get([FromQuery] BuyerDto model)
        {
            return Ok(await _Buyer2Service.Get(model));
        }

        
        
            
        
    }
}
