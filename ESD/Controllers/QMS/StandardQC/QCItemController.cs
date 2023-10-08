using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.StandardQC;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Common;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Standard.Information;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using ESD.Services.Standard.Information.StandardQC;
using ESD.Services;

namespace QuizAPI.Controllers.Standard.Information
{
    [Route("api/QCItem")]
    [ApiController]
    public class QCItemController : ControllerBase
    {
        private readonly IQCItemService _QCItemService;
        private readonly IJwtService _jwtService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ICustomService _customService;
        public QCItemController(IQCItemService QCItemService, IJwtService jwtService, ICommonMasterService commonMasterService, ICustomService customService)
        {
            _QCItemService = QCItemService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _customService = customService;
        }
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_READ)]
        public async Task<IActionResult> GetAll([FromQuery] QCItemDto item)
        {
            var returnData = await _QCItemService.GetAll(item);
            return Ok(returnData);
        }
        [HttpGet("get-qc-apply")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCApply()
        {
            return Ok(await _commonMasterService.GetForSelect("QCAPPLY"));
        }
        [HttpPost("create-QCItem")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> Create([FromBody] QCItemDto model)
        {
            var returnData = new ResponseModel<QCItemDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCItemId = AutoId.AutoGenerate();
            var result = await _QCItemService.Create(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _QCItemService.GetById(model.QCItemId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }

        [HttpPut("modify-QCItem")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_UPDATE)]
        public async Task<IActionResult> Modify([FromBody] QCItemDto model)
        {
            var returnData = new ResponseModel<QCItemDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _QCItemService.Modify(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _QCItemService.GetById(model.QCItemId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }
        [HttpDelete("delete-redo-QCItem")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_DELETE)]
        public async Task<IActionResult> Delete([FromBody] QCItemDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _QCItemService.Delete(model);

            var returnData = new ResponseModel<QCItemDto?>();
            returnData.ResponseMessage = result;
            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            return Ok(returnData);
        }
        [HttpGet("get-qc-type")]
        public async Task<IActionResult> GetQCType()
        {
            return Ok(await _customService.GetQCTypeForSelect());
        }
       
    }
}
