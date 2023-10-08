using ESD.Services.Common;
using ESD.Services.Standard.Information;
using ESD.Services;
using ESD.Services.WMS.Material;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;

namespace ESD.Controllers.WMS.Material
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialStockController : ControllerBase
    {
        private readonly IMaterialStockService _MaterialStockService;
        public MaterialStockController(IMaterialStockService MaterialStockService)
        {
            _MaterialStockService = MaterialStockService;
        }
        [HttpGet]
        [PermissionAuthorization(PermissionConst.MATERIALSTOCK_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialLotDto model)
        {
            var returnData = await _MaterialStockService.GetAll(model);
            return Ok(returnData);
        }

        [HttpGet("get-tab-ng")]
        [PermissionAuthorization(PermissionConst.MATERIALSTOCK_READ)]
        public async Task<IActionResult> GetAllNG([FromQuery] MaterialLotDto model)
        {
            var returnData = await _MaterialStockService.GetAllTabNG(model);
            return Ok(returnData);
        }

        [HttpGet("detail-lot")]
        [PermissionAuthorization(PermissionConst.MATERIALSTOCK_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] MaterialLotDto model)
        {
            var returnData = await _MaterialStockService.GetDetail(model);
            return Ok(returnData);
        }

        [HttpGet("detail-lot-ng")]
        [PermissionAuthorization(PermissionConst.MATERIALSTOCK_READ)]
        public async Task<IActionResult> GetDetailNG([FromQuery] MaterialLotDto model)
        {
            var returnData = await _MaterialStockService.GetDetailTabNG(model);
            return Ok(returnData);
        }
    }
}
