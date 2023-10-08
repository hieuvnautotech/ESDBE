using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.Slitting;
using ESD.Services;
using ESD.Services.APP;
using ESD.Services.Common;
using System.Text;
using ESD.Models.Dtos.FQC;

namespace ESD.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerMappingController : ControllerBase
    {
        private readonly IBuyerQRService _buyerQRService;
        private readonly ICustomService _customService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly IJwtService _jwtService;
        public BuyerMappingController(IBuyerQRService buyerQRService, ICustomService customService, ICommonMasterService commonMasterService, IJwtService jwtService)
        {
            _buyerQRService = buyerQRService;
            _customService = customService;
            _commonMasterService = commonMasterService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [PermissionAuthorization(PermissionConst.BUYERMAPPING_READ)]
        public async Task<IActionResult> GetAppMapping([FromQuery] WOSemiLotFQCDto model)
        {
            var returnData = await _buyerQRService.GetAppMapping(model);
            return Ok(returnData);
        }

        [HttpPost("mapping")]
        [PermissionAuthorization(PermissionConst.BUYERMAPPING_CREATE)]
        public async Task<IActionResult> Create([FromBody] WOSemiLotFQCDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _buyerQRService.MappingBuyerQR(model);

            return Ok(result);
        }

        [HttpPost("changeQR")]
        [PermissionAuthorization(PermissionConst.BUYERMAPPING_CHANGE)]
        public async Task<IActionResult> ChangeQR([FromBody] WOSemiLotFQCDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _buyerQRService.ChangeBuyerQR(model);

            return Ok(result);
        }

        [HttpGet("getForChageQR/{BuyerQR}")]
        [PermissionAuthorization(PermissionConst.BUYERMAPPING_CHANGE)]
        public async Task<IActionResult> GetForChange(string BuyerQR)
        {
            var returnData = await _buyerQRService.GetForChangeBuyerQR(BuyerQR);
            return Ok(returnData);
        }
    }
}
