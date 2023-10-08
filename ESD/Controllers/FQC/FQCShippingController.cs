using ESD.Services.Common;
using ESD.Services;
using ESD.Services.WMS.WIP;
using Microsoft.AspNetCore.Mvc;
using ESD.Services.FQC;
using ESD.Services.Standard.Information;
using Microsoft.AspNetCore.Hosting;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.FQC;

namespace ESD.Controllers.FQC
{
    [Route("api/[controller]")]
    [ApiController]
    public class FQCShippingController : ControllerBase
    {
        private readonly IFQCShippingService _FQCShippingService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        public FQCShippingController(ICustomService customService, IFQCShippingService FQCShippingService, IJwtService jwtService)
        {
            _FQCShippingService = FQCShippingService;
            _jwtService = jwtService;
            _customService = customService;
        }
        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.FQCSHIPPING_READ)]
        public async Task<IActionResult> GetAll([FromQuery] FQCShippingDto model)
        {
            var returnData = await _FQCShippingService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.FQCSHIPPING_CREATE)]
        public async Task<IActionResult> Create([FromBody] FQCShippingDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.FQCSOId = AutoId.AutoGenerate();

            var result = await _FQCShippingService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.FQCSHIPPING_UPDATE)]
        public async Task<IActionResult> Update([FromBody] FQCShippingDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _FQCShippingService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.FQCSHIPPING_DELETE)]
        public async Task<IActionResult> Delete([FromBody] FQCShippingDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _FQCShippingService.Delete(model);

            return Ok(result);
        }
        #endregion

        #region Detail lot
        [HttpGet("get-detail-lot")]
        [PermissionAuthorization(PermissionConst.FQCSHIPPING_READ)]
        public async Task<IActionResult> GetDetailLot([FromQuery] FQCShippingLotDto model)
        {
            var returnData = await _FQCShippingService.GetDetailLot(model);
            return Ok(returnData);
        }

        [HttpGet("get-detail-all-lot")]
        [PermissionAuthorization(PermissionConst.FQCSHIPPING_READ)]
        public async Task<IActionResult> GetDetailLotByFQCSOId([FromQuery] FQCShippingLotDto model)
        {
            var returnData = await _FQCShippingService.GetDetailLot(model);
            return Ok(returnData);
        }

        [HttpPost("scan-detail-lot")]
        [PermissionAuthorization(PermissionConst.FQCSHIPPING_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] FQCShippingLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _FQCShippingService.ScanLot(model);

            return Ok(result);
        }

        //[HttpPost("scan-receiving-lot")]
        //[PermissionAuthorization(PermissionConst.FQCSHIPPING_CREATE)]
        //public async Task<IActionResult> RecevingDetailLot([FromBody] FQCShippingLotDto model)
        //{
        //    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //    var userId = _jwtService.ValidateToken(token);
        //    model.createdBy = long.Parse(userId);

        //    var result = await _FQCShippingService.ScanReceivingLot(model);

        //    return Ok(result);
        //}

        [HttpDelete("delete-detail-lot")]
        [PermissionAuthorization(PermissionConst.FQCSHIPPING_DELETE)]
        public async Task<IActionResult> DeleteDetailLot([FromBody] FQCShippingLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _FQCShippingService.DeleteLot(model);

            return Ok(result);
        }
        #endregion

        [HttpGet("get-product-list")]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, concat(ProductCode,' - ', ProductName) as ProductCode, ProductCode as ProductCodeTemp, ProductName";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
