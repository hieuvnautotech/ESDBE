using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Slit;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.Slit
{
    [Route("api/slit-putaway")]
    [ApiController]
    public class SlitPutAwayController : ControllerBase
    {
        private readonly ISlitPutAwayService _slitPutAwayService;
        private readonly ICustomService _customService;
        public SlitPutAwayController(ISlitPutAwayService slitPutAwayService, ICustomService customService)
        {
            _slitPutAwayService = slitPutAwayService;
            _customService = customService;
        }
        [HttpGet]
        [PermissionAuthorization(PermissionConst.SLITPUTAWAY_READ)]
        public async Task<IActionResult> Get([FromQuery] PageModel model, DateTime? searchStartDay, DateTime? searchEndDay)
        {
            var res = await _slitPutAwayService.GetAll(model, searchStartDay, searchEndDay);
            return Ok(res);
        }

        [HttpGet("get-location-tab-raw")]
        [PermissionAuthorization(PermissionConst.SLITPUTAWAY_READ)]
        public async Task<IActionResult> GetMaterialRaw()
        {
            string Column = "LocationId, LocationCode";
            string Table = "Location";
            string Where = @"isActived = 1 and AreaCode = 'SLIT' ";

            return Ok(await _customService.GetForSelect<LocationDto>(Column, Table, Where, "LocationCode"));
        }

        [HttpPut("scan-putaway-material-raw")]
        [PermissionAuthorization(PermissionConst.SLITPUTAWAY_READ)]
        public async Task<IActionResult> ReceivingMaterialRaw([FromBody] MaterialLotDto model)
        {
            var result = await _slitPutAwayService.ScanLotMaterialRaw(model);
            return Ok(result);
        }

        [HttpPut("delete-material-raw")]
        [PermissionAuthorization(PermissionConst.SLITPUTAWAY_READ)]
        public async Task<IActionResult> DeleteRaw([FromBody] MaterialLotDto model)
        {
            var result = await _slitPutAwayService.DeleteLotRaw(model);
            return Ok(result);
        }
    }
}
