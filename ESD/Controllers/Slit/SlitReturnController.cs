using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Standard.Information;
using ESD.Services.WMS.Material;
using ESD.Models.Dtos.WMS.Material;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlitReturnController : ControllerBase
    {
        private readonly ISlitReturnService _SlitReturnService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SlitReturnController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, ISlitReturnService SlitReturnService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _SlitReturnService = SlitReturnService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.SLITRETURN_READ)]
        public async Task<IActionResult> GetAll([FromQuery] ReturnMaterialDto model)
        {
            var returnData = await _SlitReturnService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_CREATE)]
        public async Task<IActionResult> Create([FromBody] ReturnMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.RMId = AutoId.AutoGenerate();

            var result = await _SlitReturnService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_UPDATE)]
        public async Task<IActionResult> Update([FromBody] ReturnMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _SlitReturnService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_DELETE)]
        public async Task<IActionResult> Delete([FromBody] ReturnMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _SlitReturnService.Delete(model);

            return Ok(result);
        }
        #endregion

        #region Lot
        [HttpGet("detail")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] ReturnMaterialLotDto model)
        {
            var returnData = await _SlitReturnService.GetDetail(model);
            return Ok(returnData);
        }

        [HttpGet("detail-history")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_READ)]
        public async Task<IActionResult> GetDetailHistory([FromQuery] ReturnMaterialLotDto model)
        {
            var returnData = await _SlitReturnService.GetDetailHistory(model);
            return Ok(returnData);
        }

        [HttpPost("create-detail/{RMId}")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_CREATE)]
        public async Task<IActionResult> Create(long RMId, [FromBody] List<long> model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token); 
            var createdBy = long.Parse(userId);

            var result = await _SlitReturnService.CreateDetail(model, RMId, createdBy);

            return Ok(result);
        }

        [HttpDelete("delete-detail")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_DELETE)]
        public async Task<IActionResult> Delete([FromBody] ReturnMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _SlitReturnService.DeleteLot(model);

            return Ok(result);
        }

        [HttpPost("scan-detail-lot")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] ReturnMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _SlitReturnService.ScanLot(model);

            return Ok(result);
        }

        [HttpPost("scan-detail-lot-wip")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_CREATE)]
        public async Task<IActionResult> ScanInWIP([FromBody] ReturnMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _SlitReturnService.ScanLotInWIP(model);

            return Ok(result);
        }
        #endregion

        [HttpGet("get-material")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_READ)]
        public async Task<IActionResult> GetMaterial([FromQuery] BaseModel model, string MaterialLotCode, string MaterialCode, string ProductCode, long RMId)
        {
            var result = await _SlitReturnService.GetMaterial(model, MaterialLotCode, MaterialCode, ProductCode, RMId);
            return Ok(result);
        }

        [HttpGet("get-material-wip")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_READ)]
        public async Task<IActionResult> GetMaterialWIP([FromQuery] BaseModel model, string MaterialLotCode, string MaterialCode, string ProductCode, long RMId)
        {
            var result = await _SlitReturnService.GetMaterialWIP(model, MaterialLotCode, MaterialCode, ProductCode, RMId);
            return Ok(result);
        }

        [HttpGet("get-location-list")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_READ)]
        public async Task<IActionResult> GetLocation()
        {
            var list = await _commonMasterService.GetForSelect("LOCATION");
            list.Data = list.Data.Where(x => x.commonDetailName != "CUTTING ROOM" && x.commonDetailName != "WMS WAREHOUSE" && x.commonDetailName != "LINE");
            return Ok(list);
        }

        [HttpGet("get-product-list")]
        [PermissionAuthorization(PermissionConst.SLITRETURN_READ)]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, ProductCode";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
