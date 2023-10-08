using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services;
using ESD.Services.APP;
using ESD.Services.Common;
using ESD.Services.WMS.WIP;
using ESD.Models.Dtos.WMS.FG;

namespace ESD.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    public class FGMappingController : ControllerBase
    {
        private readonly IFGMappingService _FGMappingService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly ICommonMasterService _commonMasterService;
        public FGMappingController(IFGMappingService FGMappingService, IJwtService jwtService, ICustomService customService, ICommonMasterService commonMasterService)
        {
            _FGMappingService = FGMappingService;
            _jwtService = jwtService;
            _customService = customService;
            _commonMasterService = commonMasterService;
        }

        [HttpGet]
        [PermissionAuthorization(PermissionConst.FGMAPPING_READ)]
        public async Task<IActionResult> GetBoxQR([FromQuery] BoxQRDto model)
        {
            var returnData = await _FGMappingService.GetBoxQR(model);
            return Ok(returnData);
        }

        [HttpGet("get-detail")]
        [PermissionAuthorization(PermissionConst.FGMAPPING_READ)]
        public async Task<IActionResult> GetBuyerQR([FromQuery] BoxQRDto model)
        {
            var returnData = await _FGMappingService.GetBuyerQR(model);
            return Ok(returnData);
        }

        [HttpPost("get-print")]
        [PermissionAuthorization(PermissionConst.FGMAPPING_READ)]
        public async Task<IActionResult> GetListPrintQR([FromBody] List<string> listQR)
        {
            return Ok(await _FGMappingService.GetPrintBoxQR(listQR));
        }

        [HttpPut("unMapping")]
        [PermissionAuthorization(PermissionConst.FGMAPPING_READ)]
        public async Task<IActionResult> UnMapping([FromBody] BoxQRDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _FGMappingService.UnMaping(model);

            return Ok(result);
        }

        [HttpPost("scan-buyerqr")]
        [PermissionAuthorization(PermissionConst.FGMAPPING_CREATE)]
        public async Task<IActionResult> ScanBuyerQR([FromBody] BoxQRDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _FGMappingService.ScanLot(model);

            return Ok(result);
        }

        [HttpPost("create-buyerqr")]
        [PermissionAuthorization(PermissionConst.FGMAPPING_CREATE)]
        public async Task<IActionResult> CreateBuyerQR([FromBody] List<BoxQRDto> model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            var createdBy = long.Parse(userId);

            var result = await _FGMappingService.CreateBoxQR(model, createdBy);

            return Ok(result);
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, concat(ProductCode,' - ', ProductName) as ProductCode, ProductCode as ProductCodeTemp, ProductName";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
