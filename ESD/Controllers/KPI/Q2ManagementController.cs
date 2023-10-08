using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MiniExcelLibs;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services.Common;
using ESD.Services.EDI;

namespace ESD.Controllers.EDI
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class Q2ManagementController : ControllerBase
    {
        private readonly IQ2MgtService _q2MgtService;
        private readonly IJwtService _jwtService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Q2ManagementController(IQ2MgtService q2MgtServiceService, IJwtService jwtService, IWebHostEnvironment webHostEnvironment)
        {
            _q2MgtService = q2MgtServiceService;
            _jwtService = jwtService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] pportal_qual02_infoDto model)
        {
            var returnData = await _q2MgtService.GetAll(model);
            return Ok(returnData);
        }

        [HttpGet("downloadEDI")]
        public async Task<IActionResult> DownLoadEDI([FromQuery] pportal_qual02_infoDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/KPIEDI.xlsx");
            var returnData = await _q2MgtService.GetAll(model);

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
