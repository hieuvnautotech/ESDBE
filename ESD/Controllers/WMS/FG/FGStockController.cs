using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services.Common;
using ESD.Services.WMS.FG;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;
using ESD.Models.Dtos.FQC;

namespace ESD.Controllers.WMS.FG
{
    [Route("api/[controller]")]
    [ApiController]
    public class FGStockController : Controller
    {
        private readonly IFGStockService _FGStockService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly ICommonMasterService _commonMasterService;
        public FGStockController(IFGStockService FGStockService, IJwtService jwtService, ICustomService customService, ICommonMasterService commonMasterService)
        {
            _FGStockService = FGStockService;
            _jwtService = jwtService;
            _customService = customService;
            _commonMasterService = commonMasterService;
        }
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.FGSTOCK_READ)]
        public async Task<IActionResult> GetProduct([FromQuery] SemiMMSDto model)
        {
            var returnData = await _FGStockService.GetStock(model);
            return Ok(returnData);
        }
        [HttpGet("get-detail")]
        [PermissionAuthorization(PermissionConst.FGSTOCK_READ)]
        public async Task<IActionResult> GetSemiLot([FromQuery] WOSemiLotFQCDto model)
        {
            var returnData = await _FGStockService.GetLotDetail(model);
            return Ok(returnData);
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId,concat(ProductCode,' - ', ProductName) as ProductCode, ProductCode as ProductCodeTemp, ProductName";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-model")]
        public async Task<IActionResult> GetModel()
        {
            var list = await _commonMasterService.GetForSelect("PRODUCT MODEL");
            return Ok(list);
        }
    }
}
