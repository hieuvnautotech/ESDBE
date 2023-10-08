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
using ESD.Services.QMS.Holding;
using ESD.Models.Dtos.FQC;

namespace ESD.Controllers.QMS.Holding
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoldLogController : ControllerBase
    {
        private readonly IHoldLogService _HoldLogService;
        private readonly ICommonMasterService _commonMasterService;
        public HoldLogController(IHoldLogService HoldLogService, ICommonMasterService commonMasterService)
        {
            _HoldLogService = HoldLogService;
            _commonMasterService = commonMasterService;
        }

        #region Master
        [HttpGet("get-raw-material")]
        public async Task<IActionResult> GetLogRawMaterial([FromQuery] HoldLogRawMaterialDto model)
        {
            var returnData = await _HoldLogService.GetLogRawMaterial(model);
            return Ok(returnData);
        }

        [HttpGet("get-material")]
        public async Task<IActionResult> GetLogMaterial([FromQuery] HoldDto model)
        {
            var returnData = await _HoldLogService.GetLogMaterial(model);
            return Ok(returnData);
        }

        [HttpGet("get-mms-semi")]
        public async Task<IActionResult> GetMMSLog([FromQuery] SemiMMSDto model)
        {
            var returnData = await _HoldLogService.GetMMSLog(model);
            return Ok(returnData);
        }

        [HttpGet("get-fqc-semi")]
        public async Task<IActionResult> GetFQCLog([FromQuery] WOSemiLotFQCDto model)
        {
            var returnData = await _HoldLogService.GetFQCLog(model);
            return Ok(returnData);
        }

        [HttpGet("get-fg")]
        public async Task<IActionResult> GetFGLog([FromQuery] HoldLogFGDto model)
        {
            var returnData = await _HoldLogService.GetFGLog(model);
            return Ok(returnData);
        }

        [HttpGet("get-hold-status")]
        public async Task<IActionResult> GetHoldStatus()
        {
            var list = await _commonMasterService.GetForSelect("HOLDSTATUS");
            return Ok(list);
        }
        #endregion

    }
}
