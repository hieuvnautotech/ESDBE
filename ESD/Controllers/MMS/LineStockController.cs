using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services.Common;
using ESD.Services.MMS;
using ESD.Services.WMS.Material;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.MMS
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineStockController : ControllerBase
    {
        private readonly ILineStockService _LineStockService;
        private readonly IJwtService _jwtService;

        public LineStockController(ILineStockService LineStockService, IJwtService jwtService)
        {
            _LineStockService = LineStockService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [PermissionAuthorization(PermissionConst.LINESTOCK_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialLotDto model)
        {
            var returnData = await _LineStockService.GetLine(model);
            return Ok(returnData);
        }
        [HttpGet("detail-lot")]
        [PermissionAuthorization(PermissionConst.LINESTOCK_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] MaterialLotDto model)
        {
            var returnData = await _LineStockService.GetLineDetail(model);
            return Ok(returnData);
        }
    }
}
