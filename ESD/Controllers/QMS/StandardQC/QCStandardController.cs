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
    [Route("api/QCStandard")]
    [ApiController]
    public class QCStandardController : ControllerBase
    {
        private readonly IQCStandardService _QCStandardService;
        private readonly IJwtService _jwtService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ICustomService _customService;
        public QCStandardController(IQCStandardService QCStandardService, IJwtService jwtService, ICommonMasterService commonMasterService, ICustomService customService)
        {
            _QCStandardService = QCStandardService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _customService = customService;
        }
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_READ)]
        public async Task<IActionResult> GetAll([FromQuery] QCStandardDto item)
        {
            var returnData = await _QCStandardService.GetAll(item);
            return Ok(returnData);
        }
        [HttpGet("get-qc-apply")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCApply()
        {
            return Ok(await _commonMasterService.GetForSelect("QCAPPLY"));
        }
        [HttpPost("create-QCStandard")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> Create([FromBody] QCStandardDto model)
        {
            var returnData = new ResponseModel<QCStandardDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCStandardId = AutoId.AutoGenerate();
            var result = await _QCStandardService.Create(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _QCStandardService.GetById(model.QCStandardId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }

        [HttpPut("modify-QCStandard")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_UPDATE)]
        public async Task<IActionResult> Modify([FromBody] QCStandardDto model)
        {
            var returnData = new ResponseModel<QCStandardDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _QCStandardService.Modify(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _QCStandardService.GetById(model.QCStandardId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }
        [HttpDelete("delete-redo-QCStandard")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_DELETE)]
        public async Task<IActionResult> Delete([FromBody] QCStandardDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _QCStandardService.Delete(model);

            var returnData = new ResponseModel<QCStandardDto?>();
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
        [HttpGet("get-qc-item/{QCTypeId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCItem(long? QCTypeId)
        {
            var list = await _customService.GetQCItemForSelect(QCTypeId);
            return Ok(list);
        }
    }
}
