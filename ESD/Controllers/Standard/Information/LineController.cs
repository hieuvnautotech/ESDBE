using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using ESD.Models.Validators;
using ESD.Services.Common;
using ESD.Services.Standard.Information;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Common;
using ESD.Services.Standard.Information;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/line")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly ILineService _lineService;
        private readonly ICommonMasterService _commonMasterService;
        public LineController(IJwtService jwtService, ILineService lineService, ICommonMasterService commonMasterService)
        {
            _jwtService = jwtService;
            _lineService = lineService;
            _commonMasterService = commonMasterService;
        }

        [HttpGet]
        [PermissionAuthorization(PermissionConst.LINE_READ)]
        public async Task<IActionResult> Get([FromQuery] LineDto model)
        {
            return Ok(await _lineService.Get(model));
        }

        [HttpPost("create-line")]
        [PermissionAuthorization(PermissionConst.LINE_CREATE)]
        public async Task<IActionResult> Create([FromBody] LineDto model)
        {
            var returnData = new ResponseModel<LineDto?>();

            var validator = new LineValidator();
            var validateResults = validator.Validate(model);
            if (!validateResults.IsValid)
            {
                returnData.HttpResponseCode = 400;
                returnData.ResponseMessage = validateResults.Errors[0].ToString();
                return Ok(returnData);

            }

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.LineId = AutoId.AutoGenerate();

            var result = await _lineService.Create(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _lineService.GetById(model.LineId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }

        [HttpPut("modify-line")]
        [PermissionAuthorization(PermissionConst.LINE_UPDATE)]
        public async Task<IActionResult> Modify([FromBody] LineDto model)
        {
            var returnData = new ResponseModel<LineDto?>();

            var validator = new LineValidator();
            var validateResults = validator.Validate(model);
            if (!validateResults.IsValid)
            {
                returnData.HttpResponseCode = 400;
                returnData.ResponseMessage = validateResults.Errors[0].ToString();
                return Ok(returnData);
            }

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _lineService.Modify(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _lineService.GetById(model.LineId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }

        [HttpPut("delete-reuse-line")]
        [PermissionAuthorization(PermissionConst.LINE_DELETE)]
        public async Task<IActionResult> DeleteReuse([FromBody] LineDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);
            var result = await _lineService.DeleteReuse(model);

            var returnData = new ResponseModel<LineDto?>();
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

        [HttpPost("create-by-excel")]
        [PermissionAuthorization(PermissionConst.LINE_CREATE)]
        public async Task<IActionResult> CreateByExcel([FromBody] List<LineExcelDto> model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            var createdBy = long.Parse(userId);
            var result = await _lineService.CreateByExcel(model, createdBy);

            return Ok(result);
        }

        [HttpGet("get-location-list")]
        public async Task<IActionResult> GetLocation()
        {
            var list = await _commonMasterService.GetForSelect("AREA");
            list.Data = list.Data.Where(x => x.commonDetailName != "LINE" && x.commonDetailName != "FQC" && x.commonDetailName != "FG");
            return Ok(list);
        }

        [HttpPost("print-line")]
        [PermissionAuthorization(PermissionConst.LINE_READ)]
        public async Task<IActionResult> Print([FromBody] List<long> listQR)
        {
            var returnData = await _lineService.GetListPrintQR(listQR);
            return Ok(returnData);
        }
    }
}
