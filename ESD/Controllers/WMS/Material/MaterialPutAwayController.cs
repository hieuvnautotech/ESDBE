using ESD.Services.WMS.Material;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;
using ESD.Services.Common;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;

namespace ESD.Controllers.WMS.Material
{
    [Route("api/material-putaway")]
    [ApiController]
    public class MaterialPutAwayController : ControllerBase
    {
        private readonly IMaterialPutAwayService _materialPutAwayService;
        private readonly ICustomService _customService;
        public MaterialPutAwayController( IMaterialPutAwayService materialPutAwayService, ICustomService customService)
        {
            _materialPutAwayService = materialPutAwayService;
            _customService = customService;
        }
        [HttpGet]
        [PermissionAuthorization(PermissionConst.MATERIALPUTAWAY_READ)]
        public async Task<IActionResult> Get([FromQuery] PageModel model, DateTime? searchStartDay, DateTime? searchEndDay)
        {
            var res = await _materialPutAwayService.GetAll(model, searchStartDay, searchEndDay);
            return Ok(res);
        }

        [HttpPut("scan-putaway")]
        [PermissionAuthorization(PermissionConst.MATERIALPUTAWAY_CREATE)]
        public async Task<IActionResult> ReceivingMaterial([FromBody] MaterialLotDto model)
        {
            var result = await _materialPutAwayService.ScanLot(model); ;
            return Ok(result);
        }


        [HttpPut("delete")]
        [PermissionAuthorization(PermissionConst.MATERIALPUTAWAY_DELETE)]
        public async Task<IActionResult> Delete([FromBody] MaterialLotDto model)
        {
            var result = await _materialPutAwayService.DeleteLot(model);
            return Ok(result);
        }

        [HttpGet("get-location")]
        public async Task<IActionResult> GetMaterial()
        {
            string Column = "LocationId, LocationCode";
            string Table = "Location";
            string Where = @"isActived = 1 and AreaCode = 'WMS' ";

            return Ok(await _customService.GetForSelect<LocationDto>(Column, Table, Where, "LocationCode"));
        }
    }
}
