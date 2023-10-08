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

namespace QuizAPI.Controllers.Standard.Information
{
    [Route("api/QCType")]
    [ApiController]
    public class QCTypeController : ControllerBase
    {
        private readonly IQCTypeService _qCTypeService;
        private readonly IJwtService _jwtService;
        private readonly ICommonMasterService _commonMasterService;
        public QCTypeController(IQCTypeService qCTypeService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _qCTypeService = qCTypeService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
        }
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_READ)]
        public async Task<IActionResult> GetAll([FromQuery] QCTypeDto item)
        {
            var returnData = await _qCTypeService.GetAll(item);
            return Ok(returnData);
        }
        [HttpGet("get-qc-apply")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCApply()
        {
            return Ok(await _commonMasterService.GetForSelect("QCAPPLY"));
        }
        [HttpPost("create-QCType")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> Create([FromBody] QCTypeDto model)
        {
            var returnData = new ResponseModel<QCTypeDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCTypeId = AutoId.AutoGenerate();
            var result = await _qCTypeService.Create(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _qCTypeService.GetById(model.QCTypeId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }

        [HttpPut("modify-QCType")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_UPDATE)]
        public async Task<IActionResult> Modify([FromBody] QCTypeDto model)
        {
            var returnData = new ResponseModel<QCTypeDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _qCTypeService.Modify(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _qCTypeService.GetById(model.QCTypeId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }
        [HttpDelete("delete-redo-QCType")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_DELETE)]
        public async Task<IActionResult> Delete([FromBody] QCTypeDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _qCTypeService.Delete(model);

            var returnData = new ResponseModel<QCTypeDto?>();
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

      }
}
