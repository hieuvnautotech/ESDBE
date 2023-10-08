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

namespace ESD.Controllers.QMS.Holding
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoldFinishGoodController : ControllerBase
    {
        private readonly IHoldFinishGoodService _HoldFinishGoodService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HoldFinishGoodController(IWebHostEnvironment webHostEnvironment, ICustomService customService, IHoldFinishGoodService HoldFinishGoodService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _HoldFinishGoodService = HoldFinishGoodService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.HOLD_FG_READ)]
        public async Task<IActionResult> GetAll([FromQuery] HoldLogFGDto model)
        {
            var returnData = await _HoldFinishGoodService.GetAll(model);
            return Ok(returnData);
        }

        [HttpGet("get-material-type")]
        [PermissionAuthorization(PermissionConst.HOLD_FG_CREATE)]
        public async Task<IActionResult> GetMaterialType()
        {
            var list = await _commonMasterService.GetForSelect("MATERIAL TYPE");
            return Ok(list);
        }

        [HttpPost("hold")]
        [PermissionAuthorization(PermissionConst.HOLD_FG_CREATE)]
        public async Task<IActionResult> Hold([FromForm] HoldLogFGDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();
            model.IsPicture = false;

            if (model.file != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "QMS\\FinishGood");
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

            var result = await _HoldFinishGoodService.Hold(model);

            return Ok(result);
        }

        [HttpPost("unHold")]
        [PermissionAuthorization(PermissionConst.HOLD_FG_CREATE)]
        public async Task<IActionResult> UnHold([FromForm] HoldLogFGDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();
            model.IsPicture = false;

            if (model.file != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "QMS\\FinishGood");
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

            var result = await _HoldFinishGoodService.UnHold(model);

            return Ok(result);
        }

        [HttpPost("scrap")]
        [PermissionAuthorization(PermissionConst.HOLD_FG_CREATE)]
        public async Task<IActionResult> Scrap([FromBody] HoldLogFGDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.HoldLogId = AutoId.AutoGenerate();
            model.IsPicture = false;
            var result = await _HoldFinishGoodService.Scrap(model);

            return Ok(result);
        }

        [HttpPost("get-print")]
        public async Task<IActionResult> GetPrint([FromBody] List<long> listQR)
        {
            return Ok(await _HoldFinishGoodService.GetPrintFG(listQR));
        }
    }
}
