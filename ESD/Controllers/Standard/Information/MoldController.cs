using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos;
using ESD.Models.Validators;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Common;
using ESD.Services.Standard.Information;
using Microsoft.AspNetCore.Mvc;
using ESD.Services;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/mold")]
    [ApiController]
    public class MoldController : ControllerBase
    {
        private readonly IMoldService _MoldService;
        private readonly IProductService _productService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IStaffService _staffService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;

        public MoldController(IMoldService MoldService, IJwtService jwtService, ICommonMasterService commonMasterService, IProductService productService, ISupplierService supplierService, IStaffService staffService, ICustomService customService)
        {
            _MoldService = MoldService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _productService = productService;
            _supplierService = supplierService;
            _staffService = staffService;
            _customService = customService;
        }

        [HttpGet("get")]
        [PermissionAuthorization(PermissionConst.MOLD_READ)]
        public async Task<IActionResult> Get([FromQuery] MoldDto model)
        {
            var returnData = await _MoldService.Get(model);
            return Ok(returnData);
        }

        [HttpGet("get-mold-history")]
        [PermissionAuthorization(PermissionConst.MOLD_READ)]
        public async Task<IActionResult> GetMoldHistory([FromQuery] WOMoldPressingTimesDto model)
        {
            var returnData = await _MoldService.GetMoldHistory(model);
            return Ok(returnData);
        }

        [HttpGet("get-mold-by-id")]
        [PermissionAuthorization(PermissionConst.MOLD_READ)]
        public async Task<IActionResult> GetById(long moldId)
        {
            var returnData = await _MoldService.GetById(moldId);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.MOLD_CREATE)]
        public async Task<IActionResult> Create([FromBody] MoldDto model)
        {
            var returnData = new ResponseModel<MoldDto?>();
            var validator = new MoldValidator();
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
            model.MoldId = AutoId.AutoGenerate();

            var result = await _MoldService.Create(model);

            return Ok(result);
        }
        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.MOLD_UPDATE)]
        public async Task<IActionResult> Update([FromBody] MoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);

            var result = await _MoldService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete-reuse")]
        [PermissionAuthorization(PermissionConst.MOLD_DELETE)]
        public async Task<IActionResult> DeleteReuse([FromBody] MoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            var modifiedBy = long.Parse(userId);
            var result = await _MoldService.DeleteReuse(model.MoldId, model.row_version, modifiedBy, model.isActived);

            return Ok(result);
        }

        #region check QC
        [HttpGet("get-check-qc/{QCMoldMasterId}/{MoldId}/{CheckTime}")]
        public async Task<IActionResult> GetDetailMaterialRM(long? QCMoldMasterId, long? MoldId, int? CheckTime)
        {
            var returnData = await _MoldService.getCheckQC(QCMoldMasterId, MoldId, CheckTime);
            return Ok(returnData);
        }

        [HttpGet("get-check/{MoldId}/{CheckTime}")]
        public async Task<IActionResult> GetDetailMaterialRM(long? MoldId, int? CheckTime)
        {
            var returnData = await _MoldService.GetCheckMaster(MoldId, CheckTime);
            return Ok(returnData);
        }

        [HttpPost("check-qc")]
        public async Task<IActionResult> CreateRawMaterial([FromBody] CheckMoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _MoldService.CheckQC(model);

            return Ok(result);
        }
        #endregion

        [HttpGet("get-status-crud")]
        public async Task<IActionResult> GetStatusCRUD()
        {
            string Column = "commonDetailCode, commonDetailLanguge";
            string Table = "sysTbl_CommonDetail";
            string Where = "isActived = 1 and commonMasterCode = 'MOLDSTATUS' and commonDetailCode <> '002'";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-status")]
        public async Task<IActionResult> GetStatus()
        {
            return Ok(await _commonMasterService.GetForSelect("MOLDSTATUS"));
        }
            [HttpGet("get-type")]
        public async Task<IActionResult> GetType()
        {
            return Ok(await _commonMasterService.GetForSelect("MOLDTYPE"));
        }
        [HttpGet("get-line-type")]
        public async Task<IActionResult> GetLineType()
        {
            return Ok(await _commonMasterService.GetForSelect("LINE TYPE"));
        }
        [HttpGet("get-supplier")]
        public async Task<IActionResult> GetSupplier()
        {
            string Column = "SupplierId, SupplierCode";
            string Table = "Supplier";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<SupplierDto>(Column, Table, Where, ""));
        }
        [HttpGet("get-qcmasters")]
        public async Task<IActionResult> GetQCMasters()
        {
            string Column = "QCMoldMasterId, QCMoldMasterName";
            string Table = "QCMoldMaster";
            string Where = "isActived = 1  AND isConfirm = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        [HttpGet("get-product")]
        public async Task<IActionResult> GetModel()
        {
            string Column = "ProductId, ProductCode";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        [HttpGet("get-product-mapping")]
        public async Task<IActionResult> GetProductMapping(long moldId)
        {
            return Ok(await _MoldService.GetProductMapping(moldId));
        }

        [HttpGet("get-linetype-mapping")]
        public async Task<IActionResult> GetLineTypeMapping(long moldId)
        {
            return Ok(await _MoldService.GetLineTypeMapping(moldId));
        }
    }
}
