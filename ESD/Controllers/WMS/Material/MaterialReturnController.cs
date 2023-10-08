using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.WMS.Material;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Standard.Information;
using ESD.Services.WMS.Material;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.WMS.Material
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialReturnController : ControllerBase
    {
        private readonly IMaterialReturnService _MaterialReturnService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        public MaterialReturnController(IMaterialReturnService MaterialReturnService, IJwtService jwtService, ICustomService customService)
        {
            _MaterialReturnService = MaterialReturnService;
            _jwtService = jwtService;
            _customService = customService;
        }
        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.RETURNMATERIALWMS_READ)]
        public async Task<IActionResult> GetAll([FromQuery] ReturnMaterialDto model)
        {
            var returnData = await _MaterialReturnService.GetAll(model);
            return Ok(returnData);
        }
        [HttpGet("detail")]
        [PermissionAuthorization(PermissionConst.RETURNMATERIALWMS_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] ReturnMaterialLotDto model)
        {
            var returnData = await _MaterialReturnService.GetDetail(model);
            return Ok(returnData);
        }
        [HttpPost("scan-detail-lot")]
        [PermissionAuthorization(PermissionConst.RETURNMATERIALWMS_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] ReturnMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _MaterialReturnService.ScanLotReturnWMS(model);

            return Ok(result);
        }

        [HttpGet("get-location")]
        [PermissionAuthorization(PermissionConst.RETURNMATERIALWMS_READ)]
        public async Task<IActionResult> GetMaterial()
        {
            string Column = "LocationId, LocationCode";
            string Table = "Location";
            string Where = @"isActived = 1 and AreaCode = 'WMS' ";

            return Ok(await _customService.GetForSelect<LocationDto>(Column, Table, Where, "LocationCode"));
        }
        #endregion
    }
}
