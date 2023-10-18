using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;
using ESD.Services.Common;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Standard.Information;
using FluentValidation;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]

    public class Buyer2Controller : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IBuyer2Service _Buyer2Service;

        public Buyer2Controller(IJwtService jwtService, IBuyer2Service Buyer2Service)
        {
            _jwtService = jwtService;
            _Buyer2Service = Buyer2Service;

        }
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.BUYER_READ)]
        public async Task<IActionResult> Get([FromQuery] Buyer2Dto model)
        {
            return Ok(await _Buyer2Service.Get(model));
        }



        [HttpPost("create-buyer")]
        [PermissionAuthorization(PermissionConst.BUYER_CREATE)]
        public async Task<IActionResult> Create([FromBody] Buyer2Dto model)
        {
            var returnData = new ResponseModel<Buyer2Dto?>();

            var validator = new Buyer2Validator();
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
            model.BuyerId = AutoId.AutoGenerate();

            var result = await _Buyer2Service.Create(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _Buyer2Service.GetById(model.BuyerId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }


        [HttpPut("modify-buyer")]
        [PermissionAuthorization(PermissionConst.BUYER_UPDATE)]
        public async Task<IActionResult> Modify([FromBody] Buyer2Dto model)
        {
            var returnData = new ResponseModel<Buyer2Dto?>();

            var validator = new Buyer2Validator();
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

            var result = await _Buyer2Service.Modify(model);

            switch (result)
            {
                case StaticReturnValue.SYSTEM_ERROR:
                    returnData.HttpResponseCode = 500;
                    break;
                case StaticReturnValue.SUCCESS:
                    returnData = await _Buyer2Service.GetById(model.BuyerId);
                    break;
                default:
                    returnData.HttpResponseCode = 400;
                    break;
            }

            returnData.ResponseMessage = result;
            return Ok(returnData);
        }





    }
}
