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
    public class HistoryLotTrackingController : ControllerBase
    {
        private readonly IHistoryLotTrackingService _HistoryLotTrackingService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HistoryLotTrackingController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, IHistoryLotTrackingService HistoryLotTrackingService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _HistoryLotTrackingService = HistoryLotTrackingService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] HistoryLotTrackingDto model)
        {
            var returnData = await _HistoryLotTrackingService.Get(model);
            return Ok(returnData);
        }
        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail([FromQuery] HistoryLotTrackingDetailDto model)
        {
            var returnData = await _HistoryLotTrackingService.GetDetail(model);
            return Ok(returnData);
        }
        [HttpGet("detail-slit")]
        public async Task<IActionResult> GetDetailSlit([FromQuery] HistoryLotTrackingSlitDto model)
        {
            var returnData = await _HistoryLotTrackingService.GetDetailSlit(model);
            return Ok(returnData);
        }
    }
}
