using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.Slit;
using ESD.Models.Dtos.WMS.Material;
using ESD.Services.Common;
using ESD.Services.WMS.WIP;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.WMS.WIP
{
    [Route("api/wip-receiving")]
    [ApiController]
    public class WIPReceivingController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IWIPReceivingService _WIPReceivingService;
        public WIPReceivingController(IWIPReceivingService WIPReceivingService, IJwtService jwtService)
        {
            _WIPReceivingService = WIPReceivingService;
            _jwtService = jwtService;
        }

        //SLIT
        
        [HttpGet]
        [PermissionAuthorization(PermissionConst.WIPREICEIVING_READ)]
        public async Task<IActionResult> GetAll([FromQuery] SlitShippingOrderDto model)
        {
            var returnData = await _WIPReceivingService.GetAll(model);
            return Ok(returnData);
        }

        [HttpGet("detail")]
        [PermissionAuthorization(PermissionConst.WIPREICEIVING_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] SlitShippingOrderDetailDto model)
        {
            var returnData = await _WIPReceivingService.GetDetail(model);
            return Ok(returnData);
        }

        [HttpGet("wip-detail-history")]
        [PermissionAuthorization(PermissionConst.WIPREICEIVING_READ)]
        public async Task<IActionResult> GetDetailHistory([FromQuery] SlitShippingOrderDetailDto model)
        {
            var returnData = await _WIPReceivingService.GetDetailHistory(model);
            return Ok(returnData);
        }

        [HttpPost("wip-scan-detail-lot")]
        [PermissionAuthorization(PermissionConst.WIPREICEIVING_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] SlitShippingOrderLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WIPReceivingService.WIPScanLot(model);

            return Ok(result);
        }

        [HttpPut("delete-scan")]
        [PermissionAuthorization(PermissionConst.WIPREICEIVING_DELETE)]
        public async Task<IActionResult> Modify([FromBody] SlitShippingOrderLotDto model)
        {
            var returnData = new ResponseModel<SlitShippingOrderLotDto?>();

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _WIPReceivingService.DeleteScan(model);
            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    //returnData = await _containerService.GetContainerById(model.ContainerId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }
        //WMS

        [HttpGet("get-wms")]
        [PermissionAuthorization(PermissionConst.WIPREICEIVING_READ)]
        public async Task<IActionResult> GetAllWMS([FromQuery] MaterialShippingOrderDto model)
        {
            var returnData = await _WIPReceivingService.GetAllWMS(model);
            return Ok(returnData);
        }

        [HttpGet("detail-wms")]
        [PermissionAuthorization(PermissionConst.WIPREICEIVING_READ)]
        public async Task<IActionResult> GetDetailWMS([FromQuery] MaterialShippingOrderDetailDto model)
        {
            var returnData = await _WIPReceivingService.GetDetailWMS(model);
            return Ok(returnData);
        }

        [HttpGet("detail-history-wms")]
        [PermissionAuthorization(PermissionConst.WIPREICEIVING_READ)]
        public async Task<IActionResult> GetDetailHistoryWMS([FromQuery] MaterialShippingOrderDetailDto model)
        {
            var returnData = await _WIPReceivingService.GetDetailHistoryWMS(model);
            return Ok(returnData);
        }

        [HttpGet("get-detail-lot-wms")]
        [PermissionAuthorization(PermissionConst.WIPREICEIVING_READ)]
        public async Task<IActionResult> GetDetailLot([FromQuery] MaterialShippingOrderLotDto model)
        {
            var returnData = await _WIPReceivingService.GetDetailLotWMS(model);
            return Ok(returnData);
        }

        [PermissionAuthorization(PermissionConst.WIPREICEIVING_CREATE)]
        [HttpPost("wip-scan-detail-lot-wms")]
        public async Task<IActionResult> CreateDetailLot([FromBody] MaterialShippingOrderLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WIPReceivingService.WIPScanLotWMS(model);

            return Ok(result);
        }
        [HttpPut("delete-scan-wms")]
        [PermissionAuthorization(PermissionConst.WIPREICEIVING_DELETE)]
        public async Task<IActionResult> Modify([FromBody] MaterialShippingOrderLotDto model)
        {
            var returnData = new ResponseModel<MaterialShippingOrderLotDto?>();

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _WIPReceivingService.DeleteScanWMS(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    //returnData = await _containerService.GetContainerById(model.ContainerId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }
        [HttpGet("get-detail-lot-by-MSOId")]
        public async Task<IActionResult> GetDetailLotBySlitSOId(long MSOId)
        {
            var returnData = await _WIPReceivingService.GetDetailLotByMSOId(MSOId);
            return Ok(returnData);
        }
    }
}
