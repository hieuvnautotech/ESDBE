using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using ESD.Models.Validators;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Common;
using Microsoft.AspNetCore.Mvc;
using ESD.Services;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IJwtService _jwtService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ICustomService _customService;
        public ProductController(IProductService productService, IJwtService jwtService, ICommonMasterService commonMasterService, ICustomService customService)
        {
            _productService = productService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _customService = customService;
        }

        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.PRODUCT_READ)]
        public async Task<IActionResult> GetAll([FromQuery] ProductDto item)
        {
            var returnData = await _productService.GetAll(item);
            return Ok(returnData);
        }

        [HttpGet("get-product-model")]
        [PermissionAuthorization(PermissionConst.PRODUCT_READ)]
        public async Task<IActionResult> GetProductModel()
        {
            string Column = "ModelId, ModelCode";
            string Table = "Model";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        [HttpGet("get-product-type")]
        [PermissionAuthorization(PermissionConst.PRODUCT_READ)]
        public async Task<IActionResult> GetProductType()
        {
            return Ok(await _commonMasterService.GetForSelect("PRODUCTTYPE"));
        }
        [HttpGet("get-product-stapms")]
        [PermissionAuthorization(PermissionConst.PRODUCT_READ)]
        public async Task<IActionResult> GetProductStapms()
        {
            return Ok(await _commonMasterService.GetForSelect("STAMPS"));
        }
        [HttpPost("create-product")]
        [PermissionAuthorization(PermissionConst.PRODUCT_CREATE)]
        public async Task<IActionResult> Create(ProductDto model)
        {
            try
            {
                var returnData = new ResponseModel<ProductDto?>();

                var validator = new ProductValidator();
                var validateResults = validator.Validate(model);
                if (!validateResults.IsValid)
                {
                    returnData.HttpResponseCode = 400;
                    returnData.ResponseMessage = validateResults.Errors[0].ToString();
                    return Ok(returnData);
                }

                model.ProductId = AutoId.AutoGenerate();
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var userId = _jwtService.ValidateToken(token);
                model.createdBy = long.Parse(userId);
                var result = await _productService.Create(model);
                returnData.ResponseMessage = result;
                switch (result)
                {
                    case StaticReturnValue.SUCCESS:
                        returnData = await _productService.GetById(model.ProductId);
                        returnData.ResponseMessage = result;
                        break;
                    case StaticReturnValue.SYSTEM_ERROR:
                        returnData.HttpResponseCode = 500;
                        break;
                    default:
                        returnData.HttpResponseCode = 400;
                        break;
                }

                return Ok(returnData);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPut("modify-product")]
        [PermissionAuthorization(PermissionConst.PRODUCT_UPDATE)]
        public async Task<IActionResult> Modify(ProductDto model)
        {
            try
            {
                var returnData = new ResponseModel<ProductDto?>();

                var validator = new ProductValidator();
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
                var result = await _productService.Modify(model);

                returnData.ResponseMessage = result;
                switch (result)
                {
                    case StaticReturnValue.SYSTEM_ERROR:
                        returnData.HttpResponseCode = 500;
                        break;
                    case StaticReturnValue.SUCCESS:
                        returnData = await _productService.GetById(model.ProductId);
                        returnData.ResponseMessage = result;

                        break;
                    default:
                        returnData.HttpResponseCode = 400;
                        break;
                }

                return Ok(returnData);
            }
            catch (Exception e)
            {

                throw;
            }

        }
        [HttpDelete("delete-product")]
        [PermissionAuthorization(PermissionConst.PRODUCT_DELETE)]
        public async Task<IActionResult> Delete([FromBody] ProductDto model)
        {

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _productService.Delete(model);

            var returnData = new ResponseModel<ProductDto?>();
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
