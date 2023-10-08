using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Common.Standard.Information;
using ESD.Services.QMS.Holding;

namespace ESD.Controllers.QMS.Holding
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoldRawMaterialController : ControllerBase
    {
        private readonly IHoldRawMaterialService _HoldRawMaterialService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HoldRawMaterialController(IWebHostEnvironment webHostEnvironment, ICustomService customService, IHoldRawMaterialService HoldRawMaterialService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _HoldRawMaterialService = HoldRawMaterialService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Master
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.HOLD_RAWMATERIAL_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialLotDto model)
        {
            var returnData = await _HoldRawMaterialService.GetAll(model);
            return Ok(returnData);
        }
        [HttpGet("get-material-type")]
        public async Task<IActionResult> GetMaterialType()
        {
            var list = await _commonMasterService.GetForSelect("MATERIAL TYPE");
            return Ok(list);
        }
        #endregion

        [HttpPost("hold")]
        [PermissionAuthorization(PermissionConst.HOLD_RAWMATERIAL_CREATE)]
        public async Task<IActionResult> Hold([FromForm] HoldLogRawMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.IsPicture = false;

            if (model.file != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "QMS\\RawMaterial");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                //check image
                List<string> ImageExtensions = new List<string> { "JPG", "JPEG", "JPE", "BMP", "GIF", "PNG" };
                if (ImageExtensions.Contains(model.file.FileName.Split('.').Last().ToUpperInvariant()))
                    model.IsPicture = true;

                string newFileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.file.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.file.CopyToAsync(stream);
                }

                model.FileName = newFileName;
            }

            var result = await _HoldRawMaterialService.Hold(model);

            return Ok(result);
        }

        [HttpPost("unHold")]
        [PermissionAuthorization(PermissionConst.HOLD_RAWMATERIAL_CREATE)]
        public async Task<IActionResult> UnHold([FromForm] HoldLogRawMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.IsPicture = false;

            if (model.file != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "QMS\\RawMaterial");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                //check image
                List<string> ImageExtensions = new List<string> { "JPG", "JPEG", "JPE", "BMP", "GIF", "PNG" };
                if (ImageExtensions.Contains(model.file.FileName.Split('.').Last().ToUpperInvariant()))
                    model.IsPicture = true;

                string newFileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.file.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.file.CopyToAsync(stream);
                }

                model.FileName = newFileName;
            }

            var result = await _HoldRawMaterialService.UnHold(model);

            return Ok(result);
        }

        [HttpPost("scrap")]
        [PermissionAuthorization(PermissionConst.HOLD_RAWMATERIAL_CREATE)]
        public async Task<IActionResult> Scrap([FromBody] HoldLogRawMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.IsPicture = false;
            var result = await _HoldRawMaterialService.Scrap(model);

            return Ok(result);
        }

        [HttpGet("get-check/{QCIQCMasterId}/{MaterialLotId}")]
        public async Task<IActionResult> GetDetailMaterialRM(long? QCIQCMasterId, long? MaterialLotId)
        {
            var returnData = await _HoldRawMaterialService.GetDetailRawMaterial(QCIQCMasterId, MaterialLotId);
            return Ok(returnData);
        }

        [HttpPost("reCheck")]
        public async Task<IActionResult> CreateRawMaterial([FromBody] CheckRawMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _HoldRawMaterialService.CreateRawMaterial(model);

            return Ok(result);
        }
    }
}
