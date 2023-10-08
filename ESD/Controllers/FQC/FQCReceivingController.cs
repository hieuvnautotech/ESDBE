using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services.FQC;
using ESD.Services.Common;

namespace ESD.Controllers.FQC
{
    [Route("api/fqc-receiving")]
    [ApiController]
    public class FQCReceivingController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IFQCReceivingService _FQCReceivingService;

        public FQCReceivingController(IJwtService jwtService, IFQCReceivingService FQCReceivingService)
        {
            _jwtService = jwtService;
            _FQCReceivingService = FQCReceivingService;
        }

        [HttpGet]
        [PermissionAuthorization(PermissionConst.FQCRECEIVING_READ)]
        public async Task<IActionResult> GetAll([FromQuery] PageModel pageInfo, string WONo, string ProductCode, string SemiLotCode, string Shift, DateTime? StartDate, DateTime? EndDate , bool showDelete = true)
        {
            var returnData = await _FQCReceivingService.GetAll(pageInfo, WONo, ProductCode, SemiLotCode, Shift, StartDate, EndDate);
            return Ok(returnData);
        }
        [HttpGet("history-receiving")]
        [PermissionAuthorization(PermissionConst.FQCRECEIVING_READ)]
        public async Task<IActionResult> GetAllHistory([FromQuery] PageModel pageInfo, string SemiLotCode, string ProductCode, string Shift, DateTime? StartDate, DateTime? EndDate)
        {
            var returnData = await _FQCReceivingService.GetAllHistory(pageInfo, SemiLotCode, ProductCode, Shift, StartDate, EndDate);
            return Ok(returnData);
        }

        [HttpPost("scan-fqc-receiving")]
        [PermissionAuthorization(PermissionConst.FQCRECEIVING_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] SemiMMSDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _FQCReceivingService.ScanLot(model);

            return Ok(result);
        }
    }
}
