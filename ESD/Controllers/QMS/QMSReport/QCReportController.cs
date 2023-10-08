using ESD.Services.QMS.QMSReport;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;
using ESD.Models.Dtos;
using MiniExcelLibs;

namespace ESD.Controllers.QMS.QMSReport
{
    [Route("api/[controller]")]
    [ApiController]
    public class QCReportController : ControllerBase
    {
        private readonly IQCReportService _QCReportServiceService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICustomService _customService;
        public QCReportController(ICustomService customService, IQCReportService QCReportServiceService, IWebHostEnvironment webHostEnvironment)
        {
            _QCReportServiceService = QCReportServiceService;
            _webHostEnvironment = webHostEnvironment;
            _customService = customService;
        }

        [HttpGet("getPQCGeneral")]
        public async Task<IActionResult> GetPQC([FromQuery] QCReportDto model)
        {
            var returnData = await _QCReportServiceService.GetPQCGeneral(model);
            return Ok(returnData);
        }

        [HttpGet("getPQCGeneralChart")]
        public async Task<IActionResult> GetPQCChart([FromQuery] QCReportDto model)
        {
            var returnData = await _QCReportServiceService.GetPQCGeneralChart(model);
            return Ok(returnData);
        }

        [HttpGet("getPQCGeneralView")]
        public async Task<IActionResult> GetPQCView([FromQuery] QCReportDto model)
        {
            var returnData = await _QCReportServiceService.GetPQCGeneralView(model);
            return Ok(returnData);
        }

        [HttpGet("downloadPQCGeneral")]
        public async Task<IActionResult> DownLoadPQCGeneral([FromQuery] QCReportDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var returnData = await _QCReportServiceService.GetPQCGeneral(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();

            MiniExcel.SaveAs(memoryStream, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("getPQCDetail")]
        public async Task<IActionResult> GetDetailPQC([FromQuery] QCReportDto model)
        {
            var returnData = await _QCReportServiceService.GetPQCDetail(model);
            return Ok(returnData);
        }

        [HttpGet("downloadPQCDetail")]
        public async Task<IActionResult> DownLoadPQCDetail([FromQuery] QCReportDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var returnData = await _QCReportServiceService.GetPQCDetailExcel(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();

            MiniExcel.SaveAs(memoryStream, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("getOQCGeneral")]
        public async Task<IActionResult> GetOQC([FromQuery] QCReportDto model)
        {
            var returnData = await _QCReportServiceService.GetOQCGeneral(model);
            return Ok(returnData);
        }

        [HttpGet("getOQCGeneralChart")]
        public async Task<IActionResult> GetOQCChart([FromQuery] QCReportDto model)
        {
            var returnData = await _QCReportServiceService.GetOQCGeneralChart(model);
            return Ok(returnData);
        }

        [HttpGet("downloadOQCGeneral")]
        public async Task<IActionResult> DownLoadOQCGeneral([FromQuery] QCReportDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var returnData = await _QCReportServiceService.GetOQCGeneral(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();

            MiniExcel.SaveAs(memoryStream, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("getOQCDetail")]
        public async Task<IActionResult> GetDetailOQC([FromQuery] QCReportDto model)
        {
            var returnData = await _QCReportServiceService.GetOQCDetail(model);
            return Ok(returnData);
        }

        [HttpGet("downloadOQCDetail")]
        public async Task<IActionResult> DownLoadOQCDetail([FromQuery] QCReportDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var returnData = await _QCReportServiceService.GetOQCDetailExcel(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();

            MiniExcel.SaveAs(memoryStream, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        #region IQC Material
        [HttpGet("getMaterialGeneral")]
        public async Task<IActionResult> GetMaterial([FromQuery] QCReportDto model)
        {
            var returnData = await _QCReportServiceService.GetMaterialGeneral(model);
            return Ok(returnData);
        }

        [HttpGet("getMaterialGeneralChart")]
        public async Task<IActionResult> GetMaterialChart([FromQuery] QCReportDto model)
        {
            var returnData = await _QCReportServiceService.GetMaterialGeneralChart(model);
            return Ok(returnData);
        }

        [HttpGet("downloadMaterialGeneral")]
        public async Task<IActionResult> DownLoadMaterialGeneral([FromQuery] QCReportDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var returnData = await _QCReportServiceService.GetMaterialGeneral(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();

            MiniExcel.SaveAs(memoryStream, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }

        [HttpGet("getMaterialDetail")]
        public async Task<IActionResult> GetDetailMaterial([FromQuery] QCReportDto model)
        {
            var returnData = await _QCReportServiceService.GetMaterialDetail(model);
            return Ok(returnData);
        }

        [HttpGet("downloadMaterialDetail")]
        public async Task<IActionResult> DownLoadMaterialDetail([FromQuery] QCReportDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var returnData = await _QCReportServiceService.GetMaterialDetailExcel(model);

            var sheets = new Dictionary<string, object>();
            sheets.Add("QC", returnData.Data.ToList());

            var memoryStream = new MemoryStream();

            MiniExcel.SaveAs(memoryStream, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }
        #endregion


        [HttpGet("get-model")]
        public async Task<IActionResult> GetModel()
        {
            string Column = "ModelId, ModelCode";
            string Table = "Model";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-project")]
        public async Task<IActionResult> GetProject()
        {
            string Column = "ProjectId, ProjectName";
            string Table = "Project";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, ProductCode, concat(ProductCode, ' - ', ProductName) ProductLabel";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-QCStandard-PQC")]
        public async Task<IActionResult> GetQCStandard()
        {
            string Column = "QCStandardId, QCName";
            string Table = "QCStandard";
            string Where = "isActived = 1 AND QCApply = 63801658090764";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-QCStandard-OQC")]
        public async Task<IActionResult> GetQCOQCStandard()
        {
            string Column = "QCStandardId, QCName";
            string Table = "QCStandard";
            string Where = "isActived = 1 AND QCApply = 63809009989228";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-QCStandard-IQC")]
        public async Task<IActionResult> GetQCIQCStandard()
        {
            string Column = "QCStandardId, QCName";
            string Table = "QCStandard";
            string Where = "isActived = 1 AND QCApply = 63801230380985";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
