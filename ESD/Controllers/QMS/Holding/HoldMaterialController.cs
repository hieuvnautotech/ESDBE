using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ESD.Services.Common;
using ESD.Services;
using ESD.Services.QMS.Holding;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;

namespace ESD.Controllers.QMS.Holding
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoldMaterialController : ControllerBase
    {
        private readonly IHoldMaterialService _HoldMaterialService;
        private readonly IJwtService _jwtService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HoldMaterialController(IHoldMaterialService HoldMaterialService, IJwtService jwtService, IWebHostEnvironment webHostEnvironment)
        {
            _HoldMaterialService = HoldMaterialService;
            _jwtService = jwtService;
            _webHostEnvironment = webHostEnvironment;
        }
        #region Master
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.HOLD_MATERIAL_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialLotDto model)
        {
            var returnData = await _HoldMaterialService.GetAll(model);
            return Ok(returnData);
        }
        [HttpPost("hold")]
        [PermissionAuthorization(PermissionConst.HOLD_MATERIAL_CREATE)]
        public async Task<IActionResult> Hold([FromForm] HoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();

            if (model.File != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "Image\\HoldMaterial");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string newFileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.File.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.File.CopyToAsync(stream);
                }

                model.FileName = newFileName;
            }

            var result = await _HoldMaterialService.Hold(model);

            return Ok(result);
        }

        [HttpPost("unHold")]
        [PermissionAuthorization(PermissionConst.HOLD_MATERIAL_CREATE)]
        public async Task<IActionResult> UnHold([FromForm] HoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();

            if (model.File != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "Image\\HoldMaterial");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string newFileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.File.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.File.CopyToAsync(stream);
                }

                model.FileName = newFileName;
            }

            var result = await _HoldMaterialService.UnHold(model);

            return Ok(result);
        }

        [HttpPost("scrap")]
        [PermissionAuthorization(PermissionConst.HOLD_MATERIAL_CREATE)]
        public async Task<IActionResult> Scrap([FromBody] HoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();
            model.IsPicture = false;
            var result = await _HoldMaterialService.Scrap(model);

            return Ok(result);
        }


        [HttpGet("get-check/{QCIQCMasterId}/{MaterialLotId}")]
        [PermissionAuthorization(PermissionConst.HOLD_MATERIAL_CREATE)]
        public async Task<IActionResult> GetDetailMaterialL(long? QCIQCMasterId, long? MaterialLotId)
        {
            var returnData = await _HoldMaterialService.GetDetailMaterial(QCIQCMasterId, MaterialLotId);
            return Ok(returnData);
        }

        [HttpPost("reCheck")]
        [PermissionAuthorization(PermissionConst.HOLD_MATERIAL_CREATE)]
        public async Task<IActionResult> CreateRawMaterial([FromBody] CheckMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _HoldMaterialService.CreateFormMaterial(model);

            return Ok(result);
        }

        #endregion
    }
}
