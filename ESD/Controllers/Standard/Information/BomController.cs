using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Standard.Information;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class BomController : ControllerBase
    {
        private readonly IBomService _BomService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly ICommonMasterService _commonMasterService;

        public BomController(IBomService BomService, IJwtService jwtService,  ICustomService customService, ICommonMasterService commonMasterService)
        {
            _BomService = BomService;
            _jwtService = jwtService;
            _customService = customService;
            _commonMasterService = commonMasterService;
        }

        #region BOM
        [HttpGet]
        [PermissionAuthorization(PermissionConst.BOM_READ)]
        public async Task<IActionResult> GetAll([FromQuery] BomDto model)
        {
            var returnData = await _BomService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.BOM_CREATE)]
        public async Task<IActionResult> Create([FromBody] BomDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.BomId = AutoId.AutoGenerate();

            var result = await _BomService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.BOM_UPDATE)]
        public async Task<IActionResult> Update([FromBody] BomDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _BomService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.BOM_DELETE)]
        public async Task<IActionResult> Delete([FromBody] BomDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _BomService.Delete(model);

            return Ok(result);
        }
        #endregion

        #region BOM Process
        [HttpGet("get-Process")]
        [PermissionAuthorization(PermissionConst.BOM_READ)]
        public async Task<IActionResult> GetProcess([FromQuery] BomDto model)
        {
            var returnData = await _BomService.GetProcess(model);
            return Ok(returnData);
        }

        [HttpPost("create-Process")]
        [PermissionAuthorization(PermissionConst.BOM_CREATE)]
        public async Task<IActionResult> CreateProcess([FromBody] BomProcessDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.BomProcessId = AutoId.AutoGenerate();

            var result = await _BomService.CreateProcess(model);

            return Ok(result);
        }

        [HttpPut("update-Process")]
        [PermissionAuthorization(PermissionConst.BOM_UPDATE)]
        public async Task<IActionResult> UpdateProcess([FromBody] BomProcessDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _BomService.ModifyProcess(model);

            return Ok(result);
        }

        [HttpDelete("delete-Process")]
        [PermissionAuthorization(PermissionConst.BOM_DELETE)]
        public async Task<IActionResult> DeleteProcess([FromBody] BomProcessDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _BomService.DeleteProcess(model);

            return Ok(result);
        }
        #endregion

        #region BOM Process Material
        [HttpGet("get-Process-material")]
        [PermissionAuthorization(PermissionConst.BOM_READ)]
        public async Task<IActionResult> GetProcessMaterial([FromQuery] BomDto model)
        {
            var returnData = await _BomService.GetProcessMaterial(model);
            return Ok(returnData);
        }

        [HttpPost("create-Process-material")]
        [PermissionAuthorization(PermissionConst.BOM_CREATE)]
        public async Task<IActionResult> CreateProcessMaterial([FromBody] BomProcessMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.ProcessMaterialId = AutoId.AutoGenerate();

            var result = await _BomService.CreateProcessMaterial(model);

            return Ok(result);
        }

        [HttpPost("create-Process-material-by-excel")]
        [PermissionAuthorization(PermissionConst.BOM_CREATE)]
        public async Task<IActionResult> CreateByExcel([FromBody] List<BomProcessMaterialExcelDto> model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            var createdBy = long.Parse(userId);
            var result = await _BomService.CreateProcessMaterialByExcel(model, createdBy);

            return Ok(result);
        }

        [HttpPut("update-Process-material")]
        [PermissionAuthorization(PermissionConst.BOM_UPDATE)]
        public async Task<IActionResult> UpdateProcessMaterial([FromBody] BomProcessMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _BomService.ModifyProcessMaterial(model);

            return Ok(result);
        }

        [HttpDelete("delete-Process-material")]
        [PermissionAuthorization(PermissionConst.BOM_DELETE)]
        public async Task<IActionResult> DeleteProcessMaterial([FromBody] BomProcessMaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _BomService.DeleteProcessMaterial(model);

            return Ok(result);
        }
        #endregion

        [HttpGet("get-product-list/{Model}")]
        public async Task<IActionResult> GetProductByModel(long? Model)
        {
            string Column = "p.ProductId, p.ProductCode";
            string Table = "Product p";
            string Where = "p.isActived = 1 and exists (select BomId from BOMVer where ProductCode = p.ProductCode)";
            if (Model != null && Model > 0)
            { 
                Where += " and p.ModelId = " + Model; 
            }
            return Ok(await _customService.GetForSelect<ProductDto>(Column, Table, Where, ""));
        }

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "p.ProductId, concat(p.ProductCode, ' - ', p.ProductName) ProductLabel, p.ProductCode, p.ProductName, m.ModelCode";
            string Table = "Product p join Model m on p.ModelId = m.ModelId";
            string Where = "p.isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-buyer-list")]
        public async Task<IActionResult> GetBuyerC()
        {
            string Column = "BuyerId, BuyerCode";
            string Table = "Buyer";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<BuyerDto>(Column, Table, Where, ""));
        }

        [HttpGet("get-process-list")]
        public async Task<IActionResult> GetMaterialQC()
        {
            var list = await _commonMasterService.GetForSelect("BOMPROCESS");
            return Ok(list);
        }

        [HttpGet("get-material-list")]
        public async Task<IActionResult> GetMaterial()
        {
            string Column = "MaterialId, MaterialCode, MaterialUnit";
            string Table = "Material";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<MaterialDto>(Column, Table, Where, ""));
        }

        [HttpGet("get-model-tree")]
        public async Task<IActionResult> GetModelTree()
        {
            return Ok(await _BomService.GetAllModel());
        }
    }
}
