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
    public class HistoryReplacementController : ControllerBase
    {
        private readonly IHistoryReplacementService _HistoryReplacementService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HistoryReplacementController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, IHistoryReplacementService HistoryReplacementService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _HistoryReplacementService = HistoryReplacementService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] HistoryReplacementDto model)
        {
            var returnData = await _HistoryReplacementService.Get(model);
            return Ok(returnData);
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail([FromQuery] HistoryReplacementDetailDto model)
        {
            var returnData = await _HistoryReplacementService.GetDetail(model);
            return Ok(returnData);
        }

    }
}
