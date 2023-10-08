using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Common;
using ESD.Services.Standard.Information;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutingController : ControllerBase
    {
        private readonly IRoutingService _RoutingService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;

        public RoutingController(ICustomService customService, ISupplierService supplierService, IRoutingService RoutingService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _RoutingService = RoutingService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
        }

        [HttpGet]
        [PermissionAuthorization(PermissionConst.ROUTING_READ)]
        public async Task<IActionResult> GetAll([FromQuery] RoutingDto model)
        {
            var returnData = await _RoutingService.GetAll(model);
            return Ok(returnData);
        }

        //[HttpPost("create")]
        //[PermissionAuthorization(PermissionConst.MATERIAL_CREATE)]
        //public async Task<IActionResult> Create([FromBody] RoutingDto model)
        //{
        //    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //    var userId = _jwtService.ValidateToken(token);
        //    model.createdBy = long.Parse(userId);
        //    model.RoutingId = AutoId.AutoGenerate();

        //    var result = await _RoutingService.Create(model);

        //    return Ok(result);
        //}

        //[HttpPut("update")]
        //[PermissionAuthorization(PermissionConst.MATERIAL_UPDATE)]
        //public async Task<IActionResult> Update([FromBody] RoutingDto model)
        //{
        //    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //    var userId = _jwtService.ValidateToken(token);
        //    model.createdBy = long.Parse(userId);

        //    var result = await _RoutingService.Modify(model);

        //    return Ok(result);
        //}

        //[HttpDelete("delete")]
        //[PermissionAuthorization(PermissionConst.MATERIAL_DELETE)]
        //public async Task<IActionResult> Delete([FromBody] RoutingDto model)
        //{
        //    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //    var userId = _jwtService.ValidateToken(token);
        //    model.createdBy = long.Parse(userId);
        //    var result = await _RoutingService.Delete(model);

        //    return Ok(result);
        //}

        //[HttpGet("get-material-type")]
        //public async Task<IActionResult> GetRoutingType()
        //{
        //    var list = await _commonMasterService.GetForSelect("MATERIALTYPE");
        //    return Ok(list);
        //}

        //[HttpGet("get-supplier")]
        //public async Task<IActionResult> GetSupplier()
        //{
        //    string Column = "SupplierId, SupplierCode";
        //    string Table = "Supplier";
        //    string Where = "isActived = 1";
        //    return Ok(await _customService.GetForSelect<SupplierDto>(Column, Table, Where, ""));
        //}

        ////[HttpGet("get-material-qc")]
        ////public async Task<IActionResult> GetRoutingQC()
        ////{
        ////    var list = await _commonMasterService.GetForSelect("QCAPPLY");
        ////    return Ok(list);
        ////}

        //[HttpGet("get-IQC-RawRouting")]
        //public async Task<IActionResult> GetIQCRawRouting()
        //{
        //    string Column = "QCIQCMasterId, QCIQCMasterName";
        //    string Table = "QCIQCMaster";
        //    string Where = "IsConfirm = 1 and isActived = 1 and IQCType in (select [dbo].[sysFunc_GetCommonDetailId]('MATERIAL TYPE', 'RAW MATERIAL'))";
        //    return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        //}

        //[HttpGet("get-IQC-Routing")]
        //public async Task<IActionResult> GetIQCRouting()
        //{
        //    string Column = "QCIQCMasterId, QCIQCMasterName";
        //    string Table = "QCIQCMaster";
        //    string Where = "IsConfirm = 1 and isActived = 1 and IQCType in (select [dbo].[sysFunc_GetCommonDetailId]('MATERIAL TYPE', 'MATERIAL'))";
        //    return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        //}
    }
}
