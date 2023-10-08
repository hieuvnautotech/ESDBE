using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services.Common;
using ESD.Services.WMS.Material;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.WMS.Material
{
    [Route("api/split-merge")]
    [ApiController]
    public class MaterialSplitMergeController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IMaterialSplitMergeService _materialSplitMergeService;
        public MaterialSplitMergeController(IJwtService jwtService, IMaterialSplitMergeService materialSplitMergeService)
        {
            _jwtService = jwtService;
            _materialSplitMergeService = materialSplitMergeService;
        }

        [HttpGet("get-material")]
        [PermissionAuthorization(PermissionConst.SPLITMERGEMATERIAL_READ)]
        public async Task<IActionResult> GetMaterial(string MaterialLotCode)
        {
            var res = await _materialSplitMergeService.GetMaterial(MaterialLotCode);
            return Ok(res);
        }

        [HttpPost("split-lot")]
        [PermissionAuthorization(PermissionConst.SPLITMERGEMATERIAL_CREATE)]
        public async Task<IActionResult> SplitLot([FromBody] SplitMaterial model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            var NewMaterialLotId = AutoId.AutoGenerate();
            var createdBy = long.Parse(userId);

            var result = await _materialSplitMergeService.SplitLot(model.MaterialLotId, model.MaterialLotCode, model.MaterialLotCode2, model.Length, NewMaterialLotId, createdBy);
            return Ok(result);
        }

        [HttpPost("get-list-print-qr")]
        [PermissionAuthorization(PermissionConst.SPLITMERGEMATERIAL_READ)]
        public async Task<IActionResult> GetListPrintQR([FromBody] List<long> listQR)
        {
            return Ok(await _materialSplitMergeService.GetListPrintQR(listQR));
        }

        [HttpGet("get-list-split")]
        [PermissionAuthorization(PermissionConst.SPLITMERGEMATERIAL_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialReceivingDto model)
        {
            var returnData = await _materialSplitMergeService.GetAllSplit(model);
            return Ok(returnData);
        }

        [HttpGet("get-split-detail")]
        [PermissionAuthorization(PermissionConst.SPLITMERGEMATERIAL_READ)]
        public async Task<IActionResult> GetMaterialLotAll([FromQuery] MaterialSplitDetailDto model)
        {
            var returnData = await _materialSplitMergeService.GetSplitDetail(model);
            return Ok(returnData);
        }

        [HttpDelete("delete-split-detail")]
        [PermissionAuthorization(PermissionConst.SPLITMERGEMATERIAL_DELETE)]
        public async Task<IActionResult> Delete([FromBody] MaterialSplitDetailDto model)
        {
            var result = await _materialSplitMergeService.DeleteSplitDetail(model);
            return Ok(result);
        }

        [HttpPost("merge-lot")]
        [PermissionAuthorization(PermissionConst.SPLITMERGEMATERIAL_CREATE)]
        public async Task<IActionResult> MergeLot([FromBody] MergeMaterial model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            var NewMaterialLotId = AutoId.AutoGenerate();
            var createdBy = long.Parse(userId);

            var result = await _materialSplitMergeService.MergeLot(model.MaterialLotId1, model.MaterialLotId2, createdBy);
            return Ok(result);
        }
    }
}
