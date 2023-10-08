using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.FQC;
using ESD.Models.Dtos;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Common.Standard.Information;
using Microsoft.AspNetCore.Mvc;
using ESD.Services.FQC;

namespace ESD.Controllers.FQC
{
    [Route("api/fqc-routing")]
    [ApiController]
    public class FQCRoutingController : ControllerBase
    {
        private readonly IFQCRoutingService _fqcRoutingService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly IJwtService _jwtService;
        private readonly IProductService _productService;
        private readonly ICustomService _customService;
        public FQCRoutingController(IProductService productService, ICustomService customService, IFQCRoutingService fqcRoutingService,  ICommonMasterService commonMasterService, IJwtService jwtService)
        {
            _productService = productService;
            _fqcRoutingService = fqcRoutingService;
            _commonMasterService = commonMasterService;
            _jwtService = jwtService;
            _customService = customService;
        }

        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.FQCROUTING_READ)]
        public async Task<IActionResult> GetAll([FromQuery] ProductDto item)
        {
            var returnData = await _productService.GetAll(item);
            return Ok(returnData);
        }
        [HttpGet("get-detail")]
        [PermissionAuthorization(PermissionConst.FQCROUTING_READ)]
        public async Task<IActionResult> GetCutOrderDetail([FromQuery] ProductRoutingDto model)
        {
            var returnData = await _fqcRoutingService.GetRoutingDetail(model);
            return Ok(returnData);
        }

        [HttpGet("get-process")]
        public async Task<IActionResult> GetModel()
        {
            //var list = await _commonMasterService.GetForSelect("FQCPROCESS");
            //return Ok(list);

            string Column = "commonDetailCode, CONCAT(commonDetailName,' - ',commonDetailLanguge) as commonDetailLanguge";
            string Table = "sysTbl_CommonDetail";
            string Where = "isActived = 1 and commonMasterCode = 'FQCPROCESS'";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpPost("create-detail")]
        [PermissionAuthorization(PermissionConst.FQCROUTING_CREATE)]
        public async Task<IActionResult> CreateRoutingDetail([FromBody] ProductRoutingDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.RoutingId = AutoId.AutoGenerate();

            var result = await _fqcRoutingService.CreateRoutingDetail(model);

            return Ok(result);
        }
        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.FQCROUTING_UPDATE)]
        public async Task<IActionResult> Update([FromBody] ProductRoutingDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _fqcRoutingService.ModifyDetail(model);

            return Ok(result);
        }
        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.FQCROUTING_DELETE)]
        public async Task<IActionResult> Delete([FromBody] ProductRoutingDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _fqcRoutingService.DeleteDetail(model);

            return Ok(result);
        }
    }
}
