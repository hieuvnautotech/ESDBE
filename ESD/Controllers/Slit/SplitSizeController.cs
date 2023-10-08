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
using ESD.Services.Slit;
using ESD.Models.Dtos.Slit;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class SplitSizeController : ControllerBase
    {
        private readonly ISplitSizeService _SplitSizeService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SplitSizeController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, ISplitSizeService SplitSizeService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _SplitSizeService = SplitSizeService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        [PermissionAuthorization(PermissionConst.SPLITSIZE_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialLotDto model)
        {
            var returnData = await _SplitSizeService.GetAll(model);
            return Ok(returnData);
        }


        [HttpGet("Detail")]
        [PermissionAuthorization(PermissionConst.SPLITSIZE_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] MaterialLotDto model)
        {
            var returnData = await _SplitSizeService.GetDetail(model);
            return Ok(returnData);
        }


        [HttpGet("getLotCode/{MaterialLotCode}")]
        [PermissionAuthorization(PermissionConst.SPLITSIZE_SPLIT)]
        public async Task<IActionResult> GetDetail(string MaterialLotCode)
        {
            var returnData = await _SplitSizeService.ScanLotMaterial(MaterialLotCode);
            return Ok(returnData);
        }

        [HttpPost("Split/{MaterialLotId}")]
        [PermissionAuthorization(PermissionConst.SPLITSIZE_SPLIT)]
        public async Task<IActionResult> Split(long MaterialLotId, [FromBody] List<SlitSplitDto> model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            var createdBy = long.Parse(userId);

            var result = await _SplitSizeService.SplitLot(MaterialLotId, model, createdBy);

            return Ok(result);
        }

        [HttpPost("reset")]
        [PermissionAuthorization(PermissionConst.SPLITSIZE_RESET)]
        public async Task<IActionResult> ResetSlitTurn([FromBody] MaterialLotDto model)
        {
            var result = await _SplitSizeService.Reset(model);
            return Ok(result);
        }
    }
}
