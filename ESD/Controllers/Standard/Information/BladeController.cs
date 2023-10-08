using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Common;
using ESD.Services.Standard.Information;
using FluentValidation;
using ESD.Services.Common.Standard.Information;
using ESD.Services;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class BladeController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IBladeService _bladeService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ILineService _lineService;
        private readonly ISupplierService _supplierService;
        private readonly ICustomService _customService;
        public BladeController(IJwtService jwtService, IBladeService bladeService, ICommonMasterService commonMasterService, ILineService lineService, ISupplierService supplierService, ICustomService customService)
        {
            _jwtService = jwtService;
            _bladeService = bladeService;
            _commonMasterService = commonMasterService;
            _lineService = lineService;
            _supplierService = supplierService;
            _customService = customService;
        }

        [HttpGet("get")]
        [PermissionAuthorization(PermissionConst.BLADE_READ)]
        public async Task<IActionResult> GetAll([FromQuery] BladeDto model)
        {
            var returnData = await _bladeService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create-Blade")]
        [PermissionAuthorization(PermissionConst.BLADE_CREATE)]
        public async Task<IActionResult> Create([FromBody] BladeDto model)
        {
            var returnData = new ResponseModel<BladeDto?>();
     
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.BladeId = AutoId.AutoGenerate();

            var result = await _bladeService.Create(model);

            return Ok(result);
        }

        [HttpPut("modify-Blade")]
        [PermissionAuthorization(PermissionConst.BLADE_UPDATE)]
        public async Task<IActionResult> Modify([FromBody] BladeDto model)
        {
            var returnData = new ResponseModel<BladeDto?>();
            
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _bladeService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete-reuse-Blade")]
        [PermissionAuthorization(PermissionConst.BLADE_DELETE)]
        public async Task<IActionResult> DeleteReuse([FromBody] BladeDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);
            var result = await _bladeService.Delete(model);
            return Ok(result);
        }

        #region check QC
        [HttpGet("get-check-qc/{QCMoldMasterId}/{MoldId}/{CheckTime}")]
        public async Task<IActionResult> GetDetailMaterialRM(long? QCMoldMasterId, long? MoldId, int? CheckTime)
        {
            var returnData = await _bladeService.getCheckQC(QCMoldMasterId, MoldId, CheckTime);
            return Ok(returnData);
        }

        [HttpGet("get-check/{MoldId}/{CheckTime}")]
        public async Task<IActionResult> GetDetailMaterialRM(long? MoldId, int? CheckTime)
        {
            var returnData = await _bladeService.GetCheckMaster(MoldId, CheckTime);
            return Ok(returnData);
        }

        [HttpPost("check-qc")]
        public async Task<IActionResult> CreateRawMaterial([FromBody] CheckMoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _bladeService.CheckQC(model);

            return Ok(result);
        }
        #endregion


        [HttpGet("get-status")]
        public async Task<IActionResult> GetStatus()
        {
            return Ok(await _commonMasterService.GetForSelect("MOLDSTATUS"));
        }

        [HttpGet("get-line")]
        public async Task<IActionResult> GetLine()
        {
            //return Ok(await _lineService.GetForSelect());
            string Column = "LineId, LineName";
            string Table = "Line";
            string Where = "isActived = 1 and AreaCode = 'SLIT'";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        [HttpGet("get-supplier")]
        public async Task<IActionResult> GetSupplier()
        {
            return Ok(await _supplierService.GetForSelect());
        }
        [HttpGet("get-qcmasters")]
        public async Task<IActionResult> GetQCMasters()
        {
            return Ok(await _bladeService.GetQCMasters());
        }

        [HttpGet("get-bladecheck-form-mapping")]
        public async Task<IActionResult> GetMoldCheckFormMapping([FromQuery] BladeDto model)
        {
            return Ok(await _bladeService.GetBladeCheckFormMapping(model));
        }

        //[HttpPost("create-bladecheck-form-mapping")]
        //public async Task<IActionResult> CreateMoldCheckFormMapping([FromBody] BladeCheckMasterDto model)
        //{
        //    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //    var userId = _jwtService.ValidateToken(token);
        //    model.createdBy = long.Parse(userId);
        //    return Ok(await _bladeService.CreateBladeCheckFormMapping(model));
        //}

        //[HttpGet("get-bladecheck-form-history")]
        //public async Task<IActionResult> GetMoldCheckFormHistory([FromQuery] BladeDto model)
        //{
        //    return Ok(await _bladeService.GetBladeCheckFormHistory(model));
        //}

        [HttpGet("get-blade-by-id")]
        public async Task<IActionResult> GetById(long BladeId)
        {
            var returnData = await _bladeService.GetById(BladeId);
            return Ok(returnData);
        }

        [HttpGet("get-blade-history")]
        [PermissionAuthorization(PermissionConst.MOLD_READ)]
        public async Task<IActionResult> GetMoldHistory([FromQuery] BladeHistoryDto model)
        {
            var returnData = await _bladeService.GetBladeHistory(model);
            return Ok(returnData);
        }
    }
}
