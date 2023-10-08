using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Slit;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.Slit
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlitReceivingController : ControllerBase
    {
        private readonly ISlitReceivingService _SlitReceivingService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        public SlitReceivingController(ISlitReceivingService SlitReceivingService, ICustomService customService, IJwtService jwtService)
        {
            _SlitReceivingService = SlitReceivingService;
            _jwtService = jwtService;
            _customService = customService;
        }
        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.SLITRECEIVING_READ)]
        public async Task<IActionResult> GetAll([FromQuery] WIPReturnMaterialDto model)
        {
            var returnData = await _SlitReceivingService.GetAll(model);
            return Ok(returnData);
        }
        #endregion

        [HttpGet("detail-tab-wip")]
        [PermissionAuthorization(PermissionConst.SLITRECEIVING_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] WIPReturnMaterialLotDto model)
        {
            var returnData = await _SlitReceivingService.GetDetailTabWIP(model);
            return Ok(returnData);
        }

        [HttpPost("scan-detail-lot")]
        [PermissionAuthorization(PermissionConst.SLITRECEIVING_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] WIPReturnMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _SlitReceivingService.ScanLotReturnTabWIP(model);

            return Ok(result);
        }
    }
}
