using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services.Common;
using ESD.Services.MMS;
using ESD.Services.WMS.Material;

namespace ESD.Controllers.MMS
{
    [Route("api/MMSReturnMaterial")]
    [ApiController]
    public class MMSReturnMaterialController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IMMSReturnMaterialService _MMSReturnMaterialService;
        public MMSReturnMaterialController(IJwtService jwtService,IMMSReturnMaterialService MMSReturnMaterialService)
        {
            _jwtService = jwtService;
            _MMSReturnMaterialService = MMSReturnMaterialService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] MaterialLotDto model)
        {
            var returnData = await _MMSReturnMaterialService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("mms-material-confirm")]
        public async Task<IActionResult> ConfirmMMS([FromBody] List<MMSMaterialDto> model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            var createdBy = long.Parse(userId);

            var result = await _MMSReturnMaterialService.ConfirmMMS(model, createdBy);

            return Ok(result);
        }

        [HttpPost("get-list-print-qr")]
        public async Task<IActionResult> GetListPrintQR([FromBody] List<long> listQR)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            return Ok(await _MMSReturnMaterialService.GetListPrintQR(listQR));
        }

        [HttpPut("edit-length")]
        public async Task<IActionResult> EditLength([FromBody] MaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _MMSReturnMaterialService.UpdateLength(model);

            return Ok(result);
        }

    }
}
