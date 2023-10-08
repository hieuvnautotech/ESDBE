using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services.QMS.QMSReport;
using ESD.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;

namespace ESD.Controllers.QMS.QMSReport
{
    [Route("api/[controller]")]
    [ApiController]
    public class QMSReportController : ControllerBase
    {
        private readonly IQMSReportService _QMSReportService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICustomService _customService;
        public QMSReportController(IQMSReportService QMSReportService, IWebHostEnvironment webHostEnvironment, ICustomService customService)
        {
            _QMSReportService = QMSReportService;
            _webHostEnvironment = webHostEnvironment;
            _customService = customService;
        }

        [HttpGet("get-IQCRaw-General")]
        public async Task<IActionResult> GetIQCRawGeneral([FromQuery] MaterialReceivingDto model)
        {
            var returnData = await _QMSReportService.GetIQCRawGeneral(model);
            return Ok(returnData);
        }

        [HttpGet("get-IQCRaw-Detail")]
        public async Task<IActionResult> GetIQCRawDetail([FromQuery] MaterialReceivingDto model)
        {
            var returnData = await _QMSReportService.GetIQCRawDetail(model);
            return Ok(returnData);
        }

        [HttpGet("get-IQCSlitCut-Detail")]
        public async Task<IActionResult> GetIQCSlitCutDetail([FromQuery] MaterialReceivingDto model)
        {
            var returnData = await _QMSReportService.GetIQCSlitCutDetail(model);
            return Ok(returnData);
        }

        [HttpGet("get-IQCSlitCut-General")]
        public async Task<IActionResult> GetIQCSlitCutGeneral([FromQuery] MaterialReceivingDto model)
        {
            var returnData = await _QMSReportService.GetIQCSlitCutGeneral(model);
            return Ok(returnData);
        }

        [HttpGet("get-chart-IQCRaw-General")]
        public async Task<IActionResult> GetChartIQCRaw(long? MaterialId, DateTime? StartDate, DateTime? EndDate, string Type)
        {
            var list = await _QMSReportService.GetChartIQCRaw(MaterialId, StartDate, EndDate, Type);
            return Ok(list);
        }

        [HttpGet("get-chart-IQCSlitCut-General")]
        public async Task<IActionResult> GetChartIQCSlitCut(long? MaterialId, DateTime? StartDate, DateTime? EndDate, string Type)
        {
            var list = await _QMSReportService.GetChartIQCSlitCut(MaterialId, StartDate, EndDate, Type);
            return Ok(list);
        }

        [HttpGet("get-QCStandard-IQC")]
        public async Task<IActionResult> GetQCStandard()
        {
            string Column = "QCStandardId, QCName";
            string Table = "QCStandard";
            string Where = "isActived = 1 AND QCApply = 63801230380985";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-QCItem-IQC")]
        public async Task<IActionResult> GetQCItem()
        {
            string Column = "QCItemId, QCName";
            string Table = "QCItem";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("downloadIQCRaw")]
        public async Task<IActionResult> DownLoadIQCRaw([FromQuery] MaterialReceivingDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/QCIQCRawReport.xlsx");
            var returnData = await _QMSReportService.GetIQCRawGeneral(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            MiniExcel.SaveAsByTemplate(memoryStream, filePath, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("downloadIQCRawDetail")]
        [AllowAnonymous]
        public async Task<IActionResult> DownLoadOQCDetail([FromQuery] MaterialReceivingDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/QCIQCRawReport.xlsx");
            var returnData = await _QMSReportService.GetIQCRawDetailExcel(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            //MiniExcel.SaveAs("QCReportPQCGeneral.xlsx", returnData.Data.ToList());

            MiniExcel.SaveAs(memoryStream, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("downloadIQCSlitCut")]
        public async Task<IActionResult> DownLoadIQCSlitCut([FromQuery] MaterialReceivingDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/QCIQCRawReport.xlsx");
            var returnData = await _QMSReportService.GetIQCSlitCutGeneral(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            MiniExcel.SaveAsByTemplate(memoryStream, filePath, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("downloadIQCSlitCutDetail")]
        [AllowAnonymous]
        public async Task<IActionResult> DownLoadIQCSlitCutDetail([FromQuery] MaterialReceivingDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/QCIQCRawReport.xlsx");
            var returnData = await _QMSReportService.GetIQCSlitCutDetailExcel(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            //MiniExcel.SaveAs("QCReportPQCGeneral.xlsx", returnData.Data.ToList());

            MiniExcel.SaveAs(memoryStream, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("get-material-list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "MaterialId, MaterialCode";
            string Table = "Material";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
