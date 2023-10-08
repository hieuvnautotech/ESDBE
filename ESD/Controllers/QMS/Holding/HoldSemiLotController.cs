using Microsoft.AspNetCore.Mvc;
using ESD.Services.FQC;
using ESD.Services.Common;
using ESD.Services;
using ESD.Services.QMS.Holding;
using ESD.Models.Dtos;
using ESD.CustomAttributes;
using ESD.Extensions;
using Microsoft.AspNetCore.Hosting;
using ESD.Models.Dtos.FQC;
using ESD.Models.Dtos.MMS;

namespace ESD.Controllers.QMS.Holding
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoldSemiLotController : ControllerBase
    {
        private readonly IHoldSemiLotService _HoldSemiLotService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HoldSemiLotController(IHoldSemiLotService HoldSemiLotService, IJwtService jwtService, ICustomService customService, ICommonMasterService commonMasterService, IWebHostEnvironment webHostEnvironment)
        {
            _HoldSemiLotService = HoldSemiLotService;
            _jwtService = jwtService;
            _customService = customService;
            _commonMasterService = commonMasterService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("get-all-semi-fqc")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIFQC_READ)]
        public async Task<IActionResult> GetSemiLot([FromQuery] WOSemiLotFQCDto model)
        {
            var returnData = await _HoldSemiLotService.GetAllSemiFQC(model);
            return Ok(returnData);
        }

        [HttpGet("get-all-semi-mms")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIMMS_READ)]
        public async Task<IActionResult> GetSemiLot([FromQuery] SemiMMSDto model)
        {
            var returnData = await _HoldSemiLotService.GetAllSemiMMS(model);
            return Ok(returnData);
        }

        [HttpPost("hold-fqc")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIFQC_CREATE)]
        public async Task<IActionResult> Hold([FromForm] HoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();

            if (model.File != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "Image\\HoldSemiFQC");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string newFileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.File.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.File.CopyToAsync(stream);
                }

                model.FileName = newFileName;
            }

            var result = await _HoldSemiLotService.HoldFQC(model);

            return Ok(result);
        }

        [HttpPost("unHold-fqc")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIFQC_CREATE)]
        public async Task<IActionResult> UnHold([FromForm] HoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();

            if (model.File != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "Image\\HoldSemiFQC");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string newFileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.File.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.File.CopyToAsync(stream);
                }

                model.FileName = newFileName;
            }

            var result = await _HoldSemiLotService.UnHoldFQC(model);

            return Ok(result);
        }

        [HttpPost("scrap-fqc")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIFQC_CREATE)]
        public async Task<IActionResult> Scrap([FromBody] HoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();
            model.IsPicture = false;
            var result = await _HoldSemiLotService.ScrapFQC(model);

            return Ok(result);
        }


        [HttpPost("get-print")]
        public async Task<IActionResult> GetPrint([FromBody] List<long> listQR)
        {
            return Ok(await _HoldSemiLotService.GetPrintFQC(listQR));
        }

        #region MMS
        [HttpPost("hold-mms")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIMMS_CREATE)]
        public async Task<IActionResult> HoldMMS([FromForm] HoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();

            if (model.File != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "Image\\HoldSemiMMS");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string newFileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.File.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.File.CopyToAsync(stream);
                }

                model.FileName = newFileName;
            }

            var result = await _HoldSemiLotService.HoldMMS(model);

            return Ok(result);
        }

        [HttpPost("unHold-mms")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIMMS_CREATE)]
        public async Task<IActionResult> UnHoldMMS([FromForm] HoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();

            if (model.File != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "Image\\HoldSemiMMS");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string newFileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.File.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.File.CopyToAsync(stream);
                }

                model.FileName = newFileName;
            }

            var result = await _HoldSemiLotService.UnHoldMMS(model);

            return Ok(result);
        }

        [HttpPost("scrap-mms")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIMMS_CREATE)]
        public async Task<IActionResult> ScrapMMS([FromBody] HoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();
            model.IsPicture = false;
            var result = await _HoldSemiLotService.ScrapMMS(model);

            return Ok(result);
        }


        [HttpGet("get-list-pqc-sl/{QCPQCMasterId}/{WOSemiLotMMSId}")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIMMS_CREATE)]
        public async Task<IActionResult> GetListPQCSL(long? QCPQCMasterId, long? WOSemiLotMMSId)
        {
            var returnData = await _HoldSemiLotService.GetListPQCSL(QCPQCMasterId, WOSemiLotMMSId);
            return Ok(returnData);
        }

        [HttpGet("get-value-pqc-sl/{WOSemiLotMMSId}")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIMMS_CREATE)]
        public async Task<IActionResult> GetValuePQCSL(long? WOSemiLotMMSId)
        {
            var returnData = await _HoldSemiLotService.GetValuePQCSL(WOSemiLotMMSId);
            return Ok(returnData);
        }

        [HttpPost("check-pqc-sl")]
        [PermissionAuthorization(PermissionConst.HOLD_SEMIMMS_CREATE)]
        public async Task<IActionResult> CheckPQCSL(WOSemiLotMMSCheckMasterSLDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var returnData = await _HoldSemiLotService.CheckPQCSL(model);
            return Ok(returnData);
        }
        #endregion
    }
}
