using Microsoft.AspNetCore.Mvc;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services.APP;
using ESD.Services.Common;
using ESD.Models.Dtos.FQC;
using ESD.Models.Dtos.APP;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Services;
using ESD.Models.Dtos.Slitting;

namespace ESD.Controllers.APP
{
    [Route("api/fqc-oqc")]
    [ApiController]
    public class FQCOQCController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IFQCOQCService _FQCOQCService;
        private readonly ICustomService _customService;

        public FQCOQCController(IJwtService jwtService, ICustomService customService, IFQCOQCService FQCOQCService)
        {
            _jwtService = jwtService;
            _FQCOQCService = FQCOQCService;
            _customService = customService;
        }
        [HttpGet]
        [PermissionAuthorization(PermissionConst.FQCOQC_READ)]
        public async Task<IActionResult> GetAll([FromQuery] WOSemiLotFQCDto model)
        {
            var returnData = await _FQCOQCService.GetAll(model);
            return Ok(returnData);
        }

        [HttpGet("detail")]
        [PermissionAuthorization(PermissionConst.FQCOQC_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] WOSemiLotFQCDto model)
        {
            var returnData = await _FQCOQCService.GetDetail(model);
            return Ok(returnData);
        }

        [HttpPost("scan-oqc")]
        [PermissionAuthorization(PermissionConst.FQCOQC_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] WOSemiLotFQCDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _FQCOQCService.ScanLot(model);

            return Ok(result);
        }

        [HttpDelete("delete-oqc")]
        [PermissionAuthorization(PermissionConst.FQCOQC_DELETE)]
        public async Task<IActionResult> Delete([FromBody] WOSemiLotFQCDto model)
        {
            var result = await _FQCOQCService.Delete(model);
            return Ok(result);
        }

        #region Check QC
        [HttpGet("get-check-qc")]
        [PermissionAuthorization(PermissionConst.FQCOQC_CHECKQC)]
        public async Task<IActionResult> GetCheckOQC([FromQuery] OQCDto model)
        {
            var returnData = await _FQCOQCService.GetCheckQC(model);
            return Ok(returnData);
        }

        [HttpPost("check-qc")]
        [PermissionAuthorization(PermissionConst.FQCOQC_CHECKQC)]
        public async Task<IActionResult> CheckOQC([FromBody] OQCCheckDto model)
        {

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.Master.createdBy = long.Parse(userId);

            var result = await _FQCOQCService.CheckQC(model);
            return Ok(result);
        }

        [HttpGet("get-staff-check")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetStaffCheck()
        {
            string Column = "StaffId, concat(s.StaffCode, ' - ', s.StaffName) as StaffName";
            string Table = "Staff s";
            string Where = "s.isActived = 1 and (s.DeptCode = '00007')";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        #endregion
    }
}
