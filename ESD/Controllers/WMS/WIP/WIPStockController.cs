using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.WIP;
using ESD.Services.WMS.WIP;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.WMS.WIP
{
    [Route("api/[controller]")]
    [ApiController]
    public class WIPStockController : ControllerBase
    {
        private readonly IWIPStockService _WIPStockService;
        public WIPStockController(IWIPStockService WIPStockService)
        {
            _WIPStockService = WIPStockService;
        }

        [HttpGet]
        [PermissionAuthorization(PermissionConst.WIPSTOCK_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialLotDto model)
        {
            var returnData = await _WIPStockService.GetAll(model);
            return Ok(returnData);
        }

        [HttpGet("detail-lot")]
        [PermissionAuthorization(PermissionConst.WIPSTOCK_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] MaterialLotDto model)
        {
            var returnData = await _WIPStockService.GetDetail(model);
            return Ok(returnData);
        }
        [HttpGet("get-all-product")]
        public async Task<IActionResult> GetProduct([FromQuery] WOSemiMMSDto model)
        {
            var returnData = await _WIPStockService.GetProduct(model);
            return Ok(returnData);
        }

        [HttpGet("get-semi-lot")]
        public async Task<IActionResult> GetSemiLot([FromQuery] WOSemiMMSDto model)
        {
            var returnData = await _WIPStockService.GetSemiLotDetail(model);
            return Ok(returnData);
        }
        [HttpPost("get-list-print-qr")]
   
        public async Task<IActionResult> GetListPrintQR([FromBody] List<long> listQR)
        {
            return Ok(await _WIPStockService.GetListPrintQR(listQR));
        }
    }
}
