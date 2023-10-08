using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Common;
using ESD.Services.Standard.Information;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _MaterialService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;

        public MaterialController(ICustomService customService, ISupplierService supplierService, IMaterialService MaterialService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _MaterialService = MaterialService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
        }

        [HttpGet]
        [PermissionAuthorization(PermissionConst.MATERIAL_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialDto model)
        {
            var returnData = await _MaterialService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.MATERIAL_CREATE)]
        public async Task<IActionResult> Create([FromBody] MaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.MaterialId = AutoId.AutoGenerate();

            var result = await _MaterialService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.MATERIAL_UPDATE)]
        public async Task<IActionResult> Update([FromBody] MaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _MaterialService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.MATERIAL_DELETE)]
        public async Task<IActionResult> Delete([FromBody] MaterialDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _MaterialService.Delete(model);

            return Ok(result);
        }

        [HttpGet("get-material-type")]
        public async Task<IActionResult> GetMaterialType()
        {
            var list = await _commonMasterService.GetForSelect("MATERIALTYPE");
            return Ok(list);
        }

        [HttpGet("get-supplier")]
        public async Task<IActionResult> GetSupplier()
        {
            string Column = "SupplierId, SupplierCode";
            string Table = "Supplier";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<SupplierDto>(Column, Table, Where, ""));
        }

        //[HttpGet("get-material-qc")]
        //public async Task<IActionResult> GetMaterialQC()
        //{
        //    var list = await _commonMasterService.GetForSelect("QCAPPLY");
        //    return Ok(list);
        //}

        [HttpGet("get-IQC-RawMaterial")]
        public async Task<IActionResult> GetIQCRawMaterial()
        {
            string Column = "QCIQCMasterId, QCIQCMasterName";
            string Table = "QCIQCMaster";
            string Where = "IsConfirm = 1 and isActived = 1 and IQCType = 'RM' ";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-IQC-Material")]
        public async Task<IActionResult> GetIQCMaterial()
        {
            string Column = "QCIQCMasterId, QCIQCMasterName";
            string Table = "QCIQCMaster";
            string Where = "IsConfirm = 1 and isActived = 1 and IQCType = 'M' ";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
