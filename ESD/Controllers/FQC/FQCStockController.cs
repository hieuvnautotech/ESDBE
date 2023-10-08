using ESD.Models.Dtos;
using ESD.Services.APP;
using ESD.Services.Common;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Services.FQC;
using ESD.Models.Dtos.FQC;

namespace ESD.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController] 
    public class FQCStockController : ControllerBase
    {
        private readonly IFQCStockService _FQCStockService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly ICommonMasterService _commonMasterService;
        public FQCStockController(IFQCStockService FQCStockService, IJwtService jwtService, ICustomService customService, ICommonMasterService commonMasterService)
        {
            _FQCStockService = FQCStockService;
            _jwtService = jwtService;
            _customService = customService;
            _commonMasterService = commonMasterService;
        }
        [PermissionAuthorization(PermissionConst.FQCSTOCK_READ)]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] SemiMMSDto model)
        {
            var returnData = await _FQCStockService.GetStock(model);
            return Ok(returnData);
        }
        [HttpGet("get-detail")]
        [PermissionAuthorization(PermissionConst.FQCSTOCK_READ)]
        public async Task<IActionResult> GetSemiLot([FromQuery] WOSemiLotFQCDto model)
        {
            var returnData = await _FQCStockService.GetLotDetail(model);
            return Ok(returnData);
        }
        [HttpGet("get-product")]
        [PermissionAuthorization(PermissionConst.FQCSTOCK_READ)]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId,ProductCode";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-model")]
        [PermissionAuthorization(PermissionConst.FQCSTOCK_READ)]
        public async Task<IActionResult> GetModel()
        {
            string Column = "ModelId,ModelCode";
            string Table = "Model";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-status")]
        [PermissionAuthorization(PermissionConst.FQCSTOCK_READ)]
        public async Task<IActionResult> GetStatus()
        {
            var list = await _commonMasterService.GetForSelect("MLSTATUS");
            list.Data = list.Data.Where(x => x.commonDetailCode == "009" || x.commonDetailCode == "012" || x.commonDetailCode == "013" || x.commonDetailCode == "014" || x.commonDetailCode == "010" || x.commonDetailCode == "015");
            return Ok(list);
        }

        [HttpGet("get-product-type")]

        public async Task<IActionResult> GetLineType()
        {
            return Ok(await _commonMasterService.GetForSelect("PRODUCTTYPE"));
        }
    }
}
