using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.FQC;
using ESD.Services.Common;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;
using ESD.Services.WMS;

namespace ESD.Controllers.WMS.FG
{
    [Route("api/[controller]")]
    [ApiController]
    public class FGReceivingController : ControllerBase
    {
        private readonly IFGReceivingService _FGReceivingService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        public FGReceivingController(ICustomService customService, IFGReceivingService FGReceivingService, IJwtService jwtService)
        {
            _FGReceivingService = FGReceivingService;
            _jwtService = jwtService;
            _customService = customService;
        }
        #region Master
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.FGRECEIVING_READ)]
        public async Task<IActionResult> GetAll([FromQuery] FQCShippingDto model)
        {
            var returnData = await _FGReceivingService.GetAll(model);
            return Ok(returnData);
        }

        #endregion
        [HttpGet("get-detail-lot")]
        [PermissionAuthorization(PermissionConst.FGRECEIVING_READ)]
        public async Task<IActionResult> GetDetailLot([FromQuery] FQCShippingLotDto model)
        {
            var returnData = await _FGReceivingService.GetDetailLot(model);
            return Ok(returnData);
        }

        [HttpPost("scan-receiving-buyer")]
        [PermissionAuthorization(PermissionConst.FGRECEIVING_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] FQCShippingLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _FGReceivingService.ScanReceivingLot(model);

            return Ok(result);
        }

        [HttpDelete("delete-detail-lot")]
        [PermissionAuthorization(PermissionConst.FGRECEIVING_DELETE)]
        public async Task<IActionResult> DeleteDetailLot([FromBody] FQCShippingLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _FGReceivingService.DeleteLot(model);

            return Ok(result);
        }

        [HttpGet("get-detail-all-lot")]
        [PermissionAuthorization(PermissionConst.FGRECEIVING_READ)]
        public async Task<IActionResult> GetDetailAllLot([FromQuery] FQCShippingLotDto model)
        {
            var returnData = await _FGReceivingService.GetDetailLot(model);
            return Ok(returnData);
        }

        [HttpGet("get-location")]
        [PermissionAuthorization(PermissionConst.FGRECEIVING_READ)]
        public async Task<IActionResult> GetProductModel()
        {
            string Column = "LocationId, LocationCode";
            string Table = "Location";
            string Where = "isActived = 1 and AreaCode = 'FG'";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-product-list")]
        [PermissionAuthorization(PermissionConst.FGRECEIVING_READ)]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, concat(ProductCode,' - ', ProductName) as ProductCode, ProductCode as ProductCodeTemp, ProductName";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
