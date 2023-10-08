using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Standard.Information;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class QCOQCController : ControllerBase
    {
        private readonly IQCOQCService _QCOQCService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public QCOQCController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, IQCOQCService QCOQCService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _QCOQCService = QCOQCService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.QCOQC_READ)]
        public async Task<IActionResult> GetAll([FromQuery] QCOQCMasterDto model)
        {
            var returnData = await _QCOQCService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.QCOQC_CREATE)]
        public async Task<IActionResult> Create([FromBody] QCOQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCOQCMasterId = AutoId.AutoGenerate();

            var result = await _QCOQCService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.QCOQC_UPDATE)]
        public async Task<IActionResult> Update([FromBody] QCOQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _QCOQCService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.QCOQC_DELETE)]
        public async Task<IActionResult> Delete([FromBody] QCOQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCOQCService.Delete(model);

            return Ok(result);
        }

        [HttpDelete("confirm")]
        [PermissionAuthorization(PermissionConst.QCOQC_UPDATE)]
        public async Task<IActionResult> Confirm([FromBody] QCOQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCOQCService.Confirm(model);

            return Ok(result);
        }

        [HttpPost("copy")]
        [PermissionAuthorization(PermissionConst.QCOQC_CREATE)]
        public async Task<IActionResult> Copy([FromBody] QCOQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCOQCService.Copy(model);

            return Ok(result);
        }

        [HttpGet("get-product/{QCOQCMasterId}")]
        public async Task<IActionResult> GetProductForId(long QCOQCMasterId)
        {
            string Column = "ProductId, ProductCode";
            string Table = "Product";
            string Where = "isActived = 1 and ProductCode in (select ProductCode from QCOQCMasterProduct where QCOQCMasterId = " + QCOQCMasterId + ")";
            return Ok(await _customService.GetForSelect<ProductDto>(Column, Table, Where, ""));
        }
        #endregion

        #region Detail
        [HttpGet("get-detail")]
        [PermissionAuthorization(PermissionConst.QCOQC_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] QCOQCDetailDto model)
        {
            var returnData = await _QCOQCService.GetDetail(model);
            return Ok(returnData);
        }

        [HttpPost("create-detail")]
        [PermissionAuthorization(PermissionConst.QCOQC_CREATE)]
        public async Task<IActionResult> CreateSL([FromBody] QCOQCDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCOQCDetailId = AutoId.AutoGenerate();

            var result = await _QCOQCService.CreateDetail(model);

            return Ok(result);
        }
     
        [HttpDelete("delete-detail")]
        [PermissionAuthorization(PermissionConst.QCOQC_DELETE)]
        public async Task<IActionResult> DeleteSL([FromBody] QCOQCDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCOQCService.DeleteDetail(model);

            return Ok(result);
        }
        [HttpDelete("clear-detail/{QCPQCMasterId}")]
        [PermissionAuthorization(PermissionConst.QCOQC_DELETE)]
        public async Task<IActionResult> ClearSL(long QCPQCMasterId)
        {
            var result = await _QCOQCService.ClearDetail(QCPQCMasterId);
            return Ok(result);
        }
        #endregion

        [HttpGet("get-product-list")]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, concat(ProductCode, ' - ', ProductName) ProductCode";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        [HttpGet("get-qc-type")]
        public async Task<IActionResult> GetQCType()
        {
            return Ok(await _customService.GetQCTypeForSelectByApply("OQC"));
        }
        [HttpGet("get-qc-item/{QCTypeId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCItem(long? QCTypeId)
        {
            var list = await _customService.GetQCItemForSelectByApply(QCTypeId, "OQC");
            return Ok(list);
        }
        [HttpGet("get-qc-standard/{QCTypeId}/{QCItemId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCStandard(long? QCTypeId, long? QCItemId)
        {
            var list = await _customService.GetQCStandardForSelectByApply(QCTypeId, QCItemId, "OQC");
            return Ok(list);
        }
        [HttpGet("get-qc-tool")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCTool(long? QCTypeId, long? QCItemId, long? QCStandardId)
        {
            var list = await _customService.GetQCToolForSelectByApply(QCTypeId, QCItemId, QCStandardId, "OQC");
            return Ok(list);
        }
        [HttpGet("get-qc-frequency")]
        public async Task<IActionResult> GetQCFrequency()
        {
            return Ok(await _customService.GetQCFrequencyForSelectByApply("OQC"));
        }
        [HttpGet("get-Type-list")]
        public async Task<IActionResult> GetTypeList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Type", "OQC"));
        }

        [HttpGet("get-Item-list")]
        public async Task<IActionResult> GetItemList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Item", "OQC"));
        }

        [HttpGet("get-Standard-list")]
        public async Task<IActionResult> GetStandardList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Standard", "OQC"));
        }

        [HttpGet("get-Tool-list")]
        public async Task<IActionResult> GetToolList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Tool", "OQC"));
        }

        [HttpGet("get-TypeQC-list")]
        public async Task<IActionResult> GetTypeQC()
        {
            var list = await _commonMasterService.GetForSelect("TYPEOQC");
            return Ok(list);
        }
    }
}
