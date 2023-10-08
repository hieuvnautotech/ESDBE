using ESD.Services.QMS.QMSReport;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;
using ESD.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using MiniExcelLibs;

namespace ESD.Controllers.QMS.QMSReport
{
    [Route("api/[controller]")]
    [ApiController]
    public class QCFQCReportController : ControllerBase
    {
        private readonly IQCFQCReportService _QCFQCReportService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICustomService _customService;

        public QCFQCReportController(ICustomService customService, IQCFQCReportService QCFQCReportService, IWebHostEnvironment webHostEnvironment)
        {
            _QCFQCReportService = QCFQCReportService;
            _webHostEnvironment = webHostEnvironment;
            _customService = customService;
        }
        [HttpGet("getFQCGeneral")]
        public async Task<IActionResult> GetAPP([FromQuery] QCReportDto model)
        {
            var returnData = await _QCFQCReportService.GetFQCGeneral(model);
            return Ok(returnData);
        }

        [HttpGet("getFQCGeneralChart")]
        public async Task<IActionResult> GetFQCChart([FromQuery] QCReportDto model)
        {
            var returnData = await _QCFQCReportService.GetFQCGeneralChart(model);
            return Ok(returnData);
        }

        [HttpGet("downloadFQCGeneral")]
        [AllowAnonymous]
        public async Task<IActionResult> DownLoadFQCGeneral([FromQuery] QCReportDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/QCReportFQCGeneral.xlsx");
            var returnData = await _QCFQCReportService.GetFQCGeneral(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            //MiniExcel.SaveAs("QCReportAPPGeneral.xlsx", returnData.Data.ToList());

            MiniExcel.SaveAs(memoryStream, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }
        [HttpGet("downloadFQCDetail")]
        [AllowAnonymous]
        public async Task<IActionResult> DownLoadFQCDetail([FromQuery] QCReportDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var returnData = await _QCFQCReportService.GetFQCDetailExcel(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();

            MiniExcel.SaveAs(memoryStream, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }
        [HttpGet("getFQCDetail")]
        public async Task<IActionResult> GetDetailFQC([FromQuery] QCReportDto model)
        {
            var returnData = await _QCFQCReportService.GetFQCDetail(model);
            return Ok(returnData);
        }

        [HttpGet("getFQCDetailChart")]
        public async Task<IActionResult> GetDetailFQCChart([FromQuery] QCReportDto model)
        {
            var returnData = await _QCFQCReportService.GetFQCDetailChart(model);
            return Ok(returnData);
        }
        [HttpGet("get-QCStandard-FQC")]
        public async Task<IActionResult> GetQCStandard()
        {
            string Column = "QCStandardId, QCName";
            string Table = "QCStandard";
            string Where = "isActived = 1 AND QCApply = 63809009999179";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
