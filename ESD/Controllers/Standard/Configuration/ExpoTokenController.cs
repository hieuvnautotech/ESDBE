using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Common;
using ESD.Services.Standard.Information;

namespace ESD.Controllers.Standard.Configuration
{
    [Route("api/expo-token")]
    [ApiController]
    [AllowAll]
    public class ExpoTokenController : ControllerBase
    {
        private readonly IExpoTokenService _expoTokenService;

        public ExpoTokenController(IExpoTokenService expoTokenService)
        {
            _expoTokenService = expoTokenService;
        }

        [HttpGet("get-expo-tokens")]
        public async Task<IActionResult> GetActive()
        {
            return Ok(await _expoTokenService.GetActive());
        }

        [HttpPost("create-expo-token")]
        public async Task<IActionResult> Create([FromBody] ExpoTokenDto model)
        {

            var result = await _expoTokenService.Create(model);
            return Ok(result);
        }
    }
}
