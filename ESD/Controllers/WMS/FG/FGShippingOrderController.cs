using ESD.Services.Common;
using ESD.Services;
using ESD.Services.WMS.FG;
using Microsoft.AspNetCore.Mvc;
using ESD.Services.Standard.Information;
using Microsoft.AspNetCore.Hosting;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.WMS.FG;
using ESD.Models.Dtos;
using MiniExcelLibs;

namespace ESD.Controllers.WMS.FG
{
    [Route("api/[controller]")]
    [ApiController]
    public class FGShippingOrderController : ControllerBase
    {
        private readonly IFGShippingOrderService _FGShippingOrderService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FGShippingOrderController(IWebHostEnvironment webHostEnvironment, ICustomService customService, IFGShippingOrderService FGShippingOrderService, IJwtService jwtService)
        {
            _FGShippingOrderService = FGShippingOrderService;
            _jwtService = jwtService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }
        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.FGSHIPPING_READ)]
        public async Task<IActionResult> GetAll([FromQuery] FGShippingOrderDto model)
        {
            var returnData = await _FGShippingOrderService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.FGSHIPPING_CREATE)]
        public async Task<IActionResult> Create([FromBody] FGShippingOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.FGSOId = AutoId.AutoGenerate();

            var result = await _FGShippingOrderService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.FGSHIPPING_UPDATE)]
        public async Task<IActionResult> Update([FromBody] FGShippingOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _FGShippingOrderService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.FGSHIPPING_DELETE)]
        public async Task<IActionResult> Delete([FromBody] FGShippingOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _FGShippingOrderService.Delete(model);

            return Ok(result);
        }
        [HttpGet("get-buyer-list")]
        public async Task<IActionResult> GetBuyer()
        {
            string Column = "BuyerId, BuyerCode";
            string Table = "Buyer";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        #endregion
        [HttpGet("get-box-qr")]
        [PermissionAuthorization(PermissionConst.FGSHIPPING_READ)]
        public async Task<IActionResult> GetBoxQR([FromQuery] BoxQRDto model)
        {
            var returnData = await _FGShippingOrderService.GetBoxQR(model);
            return Ok(returnData);
        }
        [HttpGet("get-buyer-qr")]
        [PermissionAuthorization(PermissionConst.FGSHIPPING_READ)]
        public async Task<IActionResult> GetBuyerQR([FromQuery] BoxQRDto model)
        {
            var returnData = await _FGShippingOrderService.GetBuyerQR(model);
            return Ok(returnData);
        }
        [HttpPost("scan-box-qr")]
        [PermissionAuthorization(PermissionConst.FGSHIPPING_READ)]
        public async Task<IActionResult> ScanBuyerQR([FromBody] BoxQRDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _FGShippingOrderService.ScanBoxQR(model);

            return Ok(result);
        }
        [HttpPut("delete-box-qr")]
        [PermissionAuthorization(PermissionConst.FGSHIPPING_READ)]
        public async Task<IActionResult> UnMapping([FromBody] BoxQRDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _FGShippingOrderService.DeleteBoxQR(model);

            return Ok(result);
        }
        [HttpGet("get-all-buyer-qr")]
        [PermissionAuthorization(PermissionConst.FGSHIPPING_READ)]
        public async Task<IActionResult> GetAllBuyerQR([FromQuery] BoxQRDto model)
        {
            var returnData = await _FGShippingOrderService.GetAllBuyerQR(model);
            return Ok(returnData);
        }

        [HttpGet("download")]
        [PermissionAuthorization(PermissionConst.FGSHIPPING_READ)]
        public async Task<IActionResult> DownLoad([FromQuery] BoxQRDto model)
        {
            var returnData = await _FGShippingOrderService.GetAllBuyerQR(model);

            string webRootPath = _webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "TemplateReport/Excel/FGShippingOrder.xlsx");
            model.BoxStatus = model.createdDate?.ToString("yyyy-MM-dd");
            var sheets = new Dictionary<string, object>();
            var ListFG = new List<BoxQRDto>();
            ListFG.Add(model);
            sheets.Add("so", ListFG);
            sheets.Add("buyer", returnData.Data.ToList());

            var memoryStream = new MemoryStream();
            MiniExcel.SaveAsByTemplate(memoryStream, filePath, sheets);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            { FileDownloadName = "download.xlsx" };
        }
    }
}
