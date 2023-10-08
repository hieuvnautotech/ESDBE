using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Standard.Information;
using ESD.Services.WMS.Material;
using ESD.Models.Dtos.WMS.Material;
using ESD.Services.WMS.WIP;
using ESD.Models.Dtos;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class WIPReturnController : ControllerBase
    {
        private readonly ISlitReturnService _SlitReturnService;
        private readonly IWIPReturnService _WIPReturnService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public WIPReturnController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, ISlitReturnService SlitReturnService,
            IJwtService jwtService, ICommonMasterService commonMasterService, IWIPReturnService WIPReturnService)
        {
            _SlitReturnService = SlitReturnService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
            _WIPReturnService = WIPReturnService;
        }

        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.WIPRETURN_READ)]
        public async Task<IActionResult> GetAll([FromQuery] WIPReturnMaterialDto model)
        {
            var returnData = await _WIPReturnService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_CREATE)]
        public async Task<IActionResult> Create([FromBody] WIPReturnMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WIPRMId = AutoId.AutoGenerate();

            var result = await _WIPReturnService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_UPDATE)]
        public async Task<IActionResult> Update([FromBody] WIPReturnMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WIPReturnService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_DELETE)]
        public async Task<IActionResult> Delete([FromBody] WIPReturnMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _WIPReturnService.Delete(model);

            return Ok(result);
        }
        #endregion

        #region Lot
        [HttpGet("detail")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] WIPReturnMaterialLotDto model)
        {
            var returnData = await _WIPReturnService.GetDetail(model);
            return Ok(returnData);
        }

        [HttpGet("detail-history")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_READ)]
        public async Task<IActionResult> GetDetailHistory([FromQuery] WIPReturnMaterialLotDto model)
        {
            var returnData = await _WIPReturnService.GetDetailHistory(model);
            return Ok(returnData);
        }

        [HttpPost("create-detail/{RMId}")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_CREATE)]
        public async Task<IActionResult> Create(long RMId, [FromBody] List<long> model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token); 
            var createdBy = long.Parse(userId);

            var result = await _WIPReturnService.CreateDetail(model, RMId, createdBy);

            return Ok(result);
        }

        [HttpDelete("delete-detail")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_DELETE)]
        public async Task<IActionResult> Delete([FromBody] WIPReturnMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _WIPReturnService.DeleteLot(model);

            return Ok(result);
        }

        [HttpPost("scan-detail-lot")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] WIPReturnMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WIPReturnService.ScanLot(model);

            return Ok(result);
        }

        [HttpPost("scan-detail-lot-wip")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_CREATE)]
        public async Task<IActionResult> ScanInWIP([FromBody] WIPReturnMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WIPReturnService.ScanLot(model);

            return Ok(result);
        }
        #endregion

        [HttpGet("get-material-wip")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_CREATE)]
        public async Task<IActionResult> GetMaterial([FromQuery] BaseModel model, string MaterialLotCode, string MaterialCode, string ProductCode, long RMId)
        {
            var result = await _WIPReturnService.GetMaterial(model, MaterialLotCode, MaterialCode, ProductCode, RMId);
            return Ok(result);
        }

        [HttpGet("view-lot")]
        [PermissionAuthorization(PermissionConst.WIPRETURN_READ)]
        public async Task<IActionResult> GetViewLot([FromQuery] WIPReturnMaterialLotDto model)
        {
            var returnData = await _WIPReturnService.GetDetailLotByRMId(model);
            return Ok(returnData);
        }
    }
}
