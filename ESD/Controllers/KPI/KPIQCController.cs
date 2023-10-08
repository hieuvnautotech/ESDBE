using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ESD.Services.Common;
using ESD.Services.Standard.Information;
using ESD.Services;
using ESD.Services.WMS.Material;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services.MMS;
using Microsoft.AspNetCore.Authorization;
using ESD.Services.EDI;
using MiniExcelLibs;
using ESD.Models.Dtos.Common;

namespace ESD.Controllers.Cutting
{
    [Route("api/[controller]")]
    [ApiController]
    public class KPIQCController : ControllerBase
    {
        private readonly IKPIPQCService _KPIPQCServiceService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public KPIQCController(IKPIPQCService KPIPQCServiceService, IWebHostEnvironment webHostEnvironment)
        {
            _KPIPQCServiceService = KPIPQCServiceService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("getPQC")]
        public async Task<IActionResult> GetPQC([FromQuery] KPIQCDto model)
        {
            var returnData = await _KPIPQCServiceService.GetPQC(model);
            return Ok(returnData);
        }

        [HttpGet("currentPQC")]
        public async Task<IActionResult> GetPQCCurrentDate([FromQuery] KPIQCDto model)
        {
            var returnData = await _KPIPQCServiceService.GetPQCCurrent(model);
            return Ok(returnData);
        }

        [HttpGet("downloadPQC")]
        public async Task<IActionResult> DownLoadPQC([FromQuery] KPIQCDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/KPIPQC.xlsx");
            var returnData = await _KPIPQCServiceService.GetPQCCurrent(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            MiniExcel.SaveAsByTemplate(memoryStream, filePath, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("getFQC")]
        public async Task<IActionResult> GetFQC([FromQuery] KPIQCDto model)
        {
            var returnData = await _KPIPQCServiceService.GetFQC(model);
            return Ok(returnData);
        }

        [HttpGet("currentFQC")]
        public async Task<IActionResult> GetFQCCurrentDate([FromQuery] KPIQCDto model)
        {
            var returnData = await _KPIPQCServiceService.GetFQCCurrent(model);
            return Ok(returnData);
        }

        [HttpGet("downloadFQC")]
        public async Task<IActionResult> DownLoadFQC([FromQuery] KPIQCDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/KPIFQC.xlsx");
            var returnData = await _KPIPQCServiceService.GetFQCCurrent(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            MiniExcel.SaveAsByTemplate(memoryStream, filePath, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("getOQC")]
        public async Task<IActionResult> GetOQC([FromQuery] KPIQCDto model)
        {
            var returnData = await _KPIPQCServiceService.GetOQC(model);
            return Ok(returnData);
        }

        [HttpGet("currentOQC")]
        public async Task<IActionResult> GetOQCCurrentDate([FromQuery] KPIQCDto model)
        {
            var returnData = await _KPIPQCServiceService.GetOQCCurrent(model);
            return Ok(returnData);
        }

        [HttpGet("downloadOQC")]
        public async Task<IActionResult> DownLoadOQC([FromQuery] KPIQCDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/KPIOQC.xlsx");
            var returnData = await _KPIPQCServiceService.GetOQCCurrent(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            MiniExcel.SaveAsByTemplate(memoryStream, filePath, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }
    }
}
