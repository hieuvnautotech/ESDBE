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
    [Route("api/QCTool")]
    [ApiController]
    public class QCToolController : ControllerBase
    {
        private readonly IQCToolService _QCToolService;
        private readonly IJwtService _jwtService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ICustomService _customService;
        public QCToolController(IQCToolService QCToolService, IJwtService jwtService, ICommonMasterService commonMasterService, ICustomService customService)
        {
            _QCToolService = QCToolService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _customService = customService;
        }
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_READ)]
        public async Task<IActionResult> GetAll([FromQuery] QCToolDto item)
        {
            var returnData = await _QCToolService.GetAll(item);
            return Ok(returnData);
        }
        [HttpGet("get-qc-apply")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCApply()
        {
            return Ok(await _commonMasterService.GetForSelect("QCAPPLY"));
        }
        [HttpPost("create-QCTool")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> Create([FromBody] QCToolDto model)
        {
            var returnData = new ResponseModel<QCToolDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCToolId = AutoId.AutoGenerate();
            var result = await _QCToolService.Create(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _QCToolService.GetById(model.QCToolId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }

        [HttpPut("modify-QCTool")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_UPDATE)]
        public async Task<IActionResult> Modify([FromBody] QCToolDto model)
        {
            var returnData = new ResponseModel<QCToolDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _QCToolService.Modify(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _QCToolService.GetById(model.QCToolId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }
        [HttpDelete("delete-redo-QCTool")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_DELETE)]
        public async Task<IActionResult> Delete([FromBody] QCToolDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _QCToolService.Delete(model);

            var returnData = new ResponseModel<QCToolDto?>();
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
        [HttpGet("get-qc-standard/{QCTypeId}/{QCItemId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCItem(long? QCTypeId , long? QCItemId)
        {
            var list = await _customService.GetQCStandardForSelect(QCTypeId, QCItemId);
            return Ok(list);
        }
        [HttpGet("get-qc-tool")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCTool(long? QCTypeId, long? QCItemId,long? QCStandardId)
        {
            var list = await _customService.GetQCToolForSelect(QCTypeId, QCItemId, QCStandardId);
            return Ok(list);
        }
    }
}
