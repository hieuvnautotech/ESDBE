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
using MiniExcelLibs;
using ESD.Models.Dtos.Common;

namespace ESD.Controllers.Cutting
{
    [Route("api/[controller]")]
    [ApiController]
    public class EffectiveController : ControllerBase
    {
        private readonly IMaterialStockService _MaterialStockService;
        private readonly IAnalyticsService _analyticsService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EffectiveController(IWebHostEnvironment webHostEnvironment, IMaterialStockService MaterialStockService, IAnalyticsService analyticsService,ICustomService customService)
        {
            _MaterialStockService = MaterialStockService;
            _analyticsService = analyticsService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("getWO")]
        public async Task<IActionResult> GetWO([FromQuery] DateTime StartDate, DateTime EndDate)
        {
            var returnData = await _analyticsService.GetWOForDisplay(StartDate, EndDate);
            return Ok(returnData);
        }

        [HttpGet("getValueMMS")]
        public async Task<IActionResult> GetValueMMS([FromQuery] EffectiveDto model)
        {
            var returnData = await _analyticsService.GetWOProcessMMS(model);
            return Ok(returnData);
        }

        [HttpGet("getValueFQC")]
        public async Task<IActionResult> GetValueFQC([FromQuery] EffectiveDto model)
        {
            var returnData = await _analyticsService.GetWOProcessFQC(model);
            return Ok(returnData);
        }

        [HttpGet("download")]
        [AllowAnonymous]
        public async Task<IActionResult> DownLoad([FromQuery] EffectiveDto model)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/Effective.xlsx");

            var MMS = await _analyticsService.GetWOProcessMMS(model);
            var FQC = await _analyticsService.GetWOProcessFQC(model);

            var sheets = new Dictionary<string, object>();
            var ListWO = new List<EffectiveDto>();
            var ListMMS = new List<ProcessQty>();
            var ListFQC = new List<ProcessQty>();

            MMS.Data.Effective = decimal.Parse(Math.Round(((decimal)MMS.Data.OKQty / model.Target) * 100, 2).ToString("0.##"));
            FQC.Data.Effective = decimal.Parse(Math.Round(((decimal)FQC.Data.OKQty / model.Target) * 100, 2).ToString("0.##"));

            ListWO.Add(model);
            ListMMS.Add(MMS.Data); 
            ListFQC.Add(FQC.Data);

            sheets.Add("WO", ListWO);
            sheets.Add("MMS", ListMMS);
            sheets.Add("FQC", ListFQC);

            var memoryStream = new MemoryStream();
            MiniExcel.SaveAsByTemplate(memoryStream, filePath, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }


        [HttpGet("getProcessMMS/{ProductCode}")]
        public async Task<IActionResult> GetProcessMMS(string? ProductCode)
        {
            string Column = "distinct ProcessCode";
            string Table = "WOProcess";
            string Where = "isActived = 1 and AreaCode = 'WORKSHOP' ";
            if (ProductCode != null)
                Where = Where + " and WOId in (SELECT WOId FROM WO WHERE isActived = 1 and ProductCode = '" + ProductCode + "')";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("getProcessFQC/{ProductCode}")]
        public async Task<IActionResult> GetProcessFQC(string? ProductCode)
        {
            string Column = "distinct ProcessCode";
            string Table = "WOProcess";
            string Where = "isActived = 1 and AreaCode = 'FQC' ";
            if (ProductCode != null)
                Where = Where + " and WOId in (SELECT WOId FROM WO WHERE isActived = 1 and ProductCode = '" + ProductCode + "')";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
