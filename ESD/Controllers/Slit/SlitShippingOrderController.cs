using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.Slit;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Slit;
using ESD.Services.Standard.Information;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.Slit
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlitShippingOrderController : ControllerBase
    {
        private readonly ISlitSOService _SlitSOService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        public SlitShippingOrderController( ICustomService customService, ISlitSOService SlitSOService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _SlitSOService = SlitSOService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _customService = customService;
        }
        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.SLITSO_READ)]
        public async Task<IActionResult> GetAll([FromQuery] SlitShippingOrderDto model)
        {
            var returnData = await _SlitSOService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.SLITSO_CREATE)]
        public async Task<IActionResult> Create([FromBody] SlitShippingOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.SlitSOId = AutoId.AutoGenerate();

            var result = await _SlitSOService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.SLITSO_UPDATE)]
        public async Task<IActionResult> Update([FromBody] SlitShippingOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _SlitSOService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.SLITSO_DELETE)]
        public async Task<IActionResult> Delete([FromBody] SlitShippingOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _SlitSOService.Delete(model);

            return Ok(result);
        }
        #endregion

        #region Detail
        [HttpGet("detail")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] SlitShippingOrderDetailDto model)
        {
            var returnData = await _SlitSOService.GetDetail(model);
            return Ok(returnData);
        }

        [HttpGet("detail-history")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetDetailHistory([FromQuery] SlitShippingOrderDetailDto model)
        {
            var returnData = await _SlitSOService.GetDetailHistory(model);
            return Ok(returnData);
        }

        [HttpPost("create-detail/{SlitSOId}")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_CREATE)]
        public async Task<IActionResult> Create(long SlitSOId, [FromBody] List<SlitShippingOrderDetailDto> model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            var createdBy = long.Parse(userId);

            var result = await _SlitSOService.CreateDetail(model, SlitSOId, createdBy);

            return Ok(result);
        }

        [HttpDelete("delete-detail")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_DELETE)]
        public async Task<IActionResult> Delete([FromBody] SlitShippingOrderDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _SlitSOService.DeleteDetail(model);

            return Ok(result);
        }
        #endregion

        #region Detail lot
        [HttpGet("get-detail-lot")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetDetailLot([FromQuery] SlitShippingOrderLotDto model)
        {
            var returnData = await _SlitSOService.GetDetailLot(model);
            return Ok(returnData);
        }

        [HttpGet("get-detail-lot-by-SlitSOId")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetDetailLotBySlitSOId(long SlitSOId)
        {
            var returnData = await _SlitSOService.GetDetailLotBySlitSOId(SlitSOId);
            return Ok(returnData);
        }

        [HttpPost("scan-detail-lot")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] SlitShippingOrderLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _SlitSOService.ScanLot(model);

            return Ok(result);
        }

        //[HttpPost("scan-receiving-lot")]
        //[PermissionAuthorization(PermissionConst.MATERIALSO_CREATE)]
        //public async Task<IActionResult> RecevingDetailLot([FromBody] SlitShippingOrderLotDto model)
        //{
        //    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //    var userId = _jwtService.ValidateToken(token);
        //    model.createdBy = long.Parse(userId);

        //    var result = await _SlitSOService.ScanReceivingLot(model);

        //    return Ok(result);
        //}

        [HttpDelete("delete-detail-lot")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_DELETE)]
        public async Task<IActionResult> DeleteDetailLot([FromBody] SlitShippingOrderLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _SlitSOService.DeleteLot(model);

            return Ok(result);
        }
        #endregion
        [HttpGet("get-material")]
        [PermissionAuthorization(PermissionConst.SLITSO_READ)]
        public async Task<IActionResult> GetMaterial([FromQuery] BaseModel model, string MaterialCode, string ProductCode, long SlitSOId)
        {
            var result = await _SlitSOService.GetMaterial(model, MaterialCode, ProductCode, SlitSOId);
            return Ok(result);
        }

        [HttpGet("get-product-list")]
        [PermissionAuthorization(PermissionConst.SLITSO_READ)]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, concat(ProductCode,' - ', ProductName) as ProductCode, ProductCode as ProductCodeTemp, ProductName";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
