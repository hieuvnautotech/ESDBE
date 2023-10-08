using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Standard.Information;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _ModelService;
        private readonly IJwtService _jwtService;
        public ModelController( IModelService ModelService, IJwtService jwtService)
        {
            _ModelService = ModelService;
            _jwtService = jwtService;
        }
        [HttpGet]
        [PermissionAuthorization(PermissionConst.MODEL_READ)]
        public async Task<IActionResult> GetAll([FromQuery] ModelDto model)
        {
            var returnData = await _ModelService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.MODEL_CREATE)]
        public async Task<IActionResult> Create([FromBody] ModelDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.ModelId = AutoId.AutoGenerate();

            var result = await _ModelService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.MODEL_UPDATE)]
        public async Task<IActionResult> Update([FromBody] ModelDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _ModelService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.MODEL_DELETE)]
        public async Task<IActionResult> Delete([FromBody] ModelDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _ModelService.Delete(model);

            return Ok(result);
        }
    }
}
