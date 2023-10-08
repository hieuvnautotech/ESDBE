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

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlitStockController : ControllerBase
    {
        private readonly IMaterialStockService _MaterialStockService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SlitStockController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, IMaterialStockService MaterialStockService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _MaterialStockService = MaterialStockService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

      
        [HttpGet]
        [PermissionAuthorization(PermissionConst.SLITSTOCK_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialLotDto model)
        {
            var returnData = await _MaterialStockService.GetSlit(model);
            return Ok(returnData);
        }

        [HttpGet("detail-lot")]
        [PermissionAuthorization(PermissionConst.SLITSTOCK_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] MaterialLotDto model)
        {
            var returnData = await _MaterialStockService.GetSlitDetail(model);
            return Ok(returnData);
        }

    }
}
