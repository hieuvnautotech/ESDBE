using ESD.Models.Dtos;
using ESD.Services.KPI;
using ESD.Services.MMS;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;

namespace ESD.Controllers.KPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class KPIQCIQCController : ControllerBase
    {
        private readonly IKPIQCIQCService _KPIQCIQCService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAnalyticsService _analyticsService;
        public KPIQCIQCController(IKPIQCIQCService KPIQCIQCService, IWebHostEnvironment webHostEnvironment, IAnalyticsService analyticsService)
        {
            _KPIQCIQCService = KPIQCIQCService;
            _webHostEnvironment = webHostEnvironment;
            _analyticsService = analyticsService;
        }
        [HttpGet("get-data-chart")]
        public async Task<IActionResult> GetDataChart(string? MaterialCode, string LotNo)
        {
            var list = await _KPIQCIQCService.GetForChart(MaterialCode, LotNo);
            return Ok(list);
        }

        [HttpGet("get-data-grid")]
        public async Task<IActionResult> GetDataGrid([FromQuery] MaterialReceivingDto model)
        {
            var returnData = await _KPIQCIQCService.GetDataGrid(model);
            return Ok(returnData);
        }

        [HttpGet("get-data-chart-slit-cut")]
        public async Task<IActionResult> GetDataChartSlitCut(string? MaterialCode, string LotNo, long ProductId)
        {
            var list = await _KPIQCIQCService.GetForChartSlitCut(MaterialCode, LotNo, ProductId);
            return Ok(list);
        }

        [HttpGet("get-data-grid-slit-cut")]
        public async Task<IActionResult> GetDataGridSlitCut([FromQuery] string? MaterialCode, string LotNo, long ProductId)
        {
            var returnData = await _KPIQCIQCService.GetDataGridSlitCut(MaterialCode, LotNo, ProductId);
            return Ok(returnData);
        }

        [HttpGet("downloadIQCRaw")]
        public async Task<IActionResult> DownLoadIQCRaw([FromQuery] MaterialReceivingDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/KPIIQCRaw.xlsx");
            var returnData = await _KPIQCIQCService.GetDataGrid(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            MiniExcel.SaveAsByTemplate(memoryStream, filePath, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("downloadIQCSlitCut")]
        public async Task<IActionResult> DownLoadIQCSlitCut([FromQuery] string? MaterialCode, string LotNo, long ProductId)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/KPIIQCSlitCut.xlsx");
            var returnData = await _KPIQCIQCService.GetDataGridSlitCut(MaterialCode, LotNo, ProductId);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            MiniExcel.SaveAsByTemplate(memoryStream, filePath, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("downloadProductivity")]
        public async Task<IActionResult> DownLoadEDI()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/KPIProductivity.xlsx");
            var returnData = await _analyticsService.GetDisplay();

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.data);

            var memoryStream = new MemoryStream();
            MiniExcel.SaveAsByTemplate(memoryStream, filePath, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }
    }
}
