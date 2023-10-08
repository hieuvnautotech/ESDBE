using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Common;
using ESD.Services.Standard.Information;
using ESD.Services;

namespace QuizAPI.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IStaffService _StaffService;
        private readonly ICustomService _customService;

        public StaffController(IJwtService jwtService, IStaffService StaffService, ICustomService customService)
        {
            _jwtService = jwtService;
            _StaffService = StaffService;
            _customService = customService;

        }
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.STAFF_READ)]
        public async Task<IActionResult> Get([FromQuery] StaffDto model)
        {
            return Ok(await _StaffService.Get(model));
            //var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //var userId = _jwtService.ValidateToken(token);
            //var returnData = await _StaffService.GetAll(pageInfo, long.Parse(userId));
            //return Ok(returnData);
        }
        [HttpPost("create-staff")]
        [PermissionAuthorization(PermissionConst.STAFF_CREATE)]
        public async Task<IActionResult> Create([FromBody] StaffDto model)
        {
            try
            {
                var returnData = new ResponseModel<StaffDto?>();

                var validator = new StaffValidator();
                var validateResults = validator.Validate(model);
                if (!validateResults.IsValid)
                {
                    //foreach (var failure in validateResults.Errors)
                    //{
                    //    Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                    //}

                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = validateResults.Errors[0].ToString();
                    return Ok(returnData);

                }

                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var userId = _jwtService.ValidateToken(token);
                model.createdBy = long.Parse(userId);
                model.StaffId = AutoId.AutoGenerate();

                var result = await _StaffService.Create(model);

                switch (result)
                {
                    case StaticReturnValue.SYSTEM_ERROR:
                        returnData.HttpResponseCode = 500;
                        break;
                    case StaticReturnValue.SUCCESS:
                        returnData = await _StaffService.GetById(model.StaffId);
                        break;
                    default:
                        returnData.HttpResponseCode = 400;
                        break;
                }

                returnData.ResponseMessage = result;
                return Ok(returnData);
            }
            catch (Exception e)
            {

                throw;
            }
        }
        [HttpPut("modify-staff")]
        [PermissionAuthorization(PermissionConst.STAFF_UPDATE)]
        public async Task<IActionResult> Modify([FromBody] StaffDto model)
        {
            var returnData = new ResponseModel<StaffDto?>();

            var validator = new StaffValidator();
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

            var result = await _StaffService.Modify(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _StaffService.GetById(model.StaffId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }
        [HttpPut("delete-staff")]
        [PermissionAuthorization(PermissionConst.STAFF_DELETE)]
        public async Task<IActionResult> Delete([FromBody] StaffDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);
            var result = await _StaffService.Delete(model);

            var returnData = new ResponseModel<StaffDto?>();
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

        [HttpPost("print-staff")]
        [PermissionAuthorization(PermissionConst.STAFF_READ)]
        public async Task<IActionResult> Print([FromBody] List<long> listQR)
        {
            var returnData = await _StaffService.GetListPrintQR(listQR);
            return Ok(returnData);
        }
        [HttpGet("get-department")]
        [PermissionAuthorization(PermissionConst.STAFF_READ)]
        public async Task<IActionResult> GetDepartment()
        {
            string Column = "DeptId, concat(DeptCode, ' - ', DeptNameVI) as DeptName";
            string Table = "Department ";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
      
    }
}
