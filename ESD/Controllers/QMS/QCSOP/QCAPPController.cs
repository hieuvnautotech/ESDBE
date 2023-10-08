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
    public class QCAPPController : ControllerBase
    {
        private readonly IQCAPPService _QCAPPService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public QCAPPController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, IQCAPPService QCAPPService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _QCAPPService = QCAPPService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.QCAPP_READ)]
        public async Task<IActionResult> GetAll([FromQuery] QCAPPMasterDto model)
        {
            var returnData = await _QCAPPService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.QCAPP_CREATE)]
        public async Task<IActionResult> Create([FromBody] QCAPPMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCAPPMasterId = AutoId.AutoGenerate();

            var result = await _QCAPPService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.QCAPP_UPDATE)]
        public async Task<IActionResult> Update([FromBody] QCAPPMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _QCAPPService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.QCAPP_DELETE)]
        public async Task<IActionResult> Delete([FromBody] QCAPPMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCAPPService.Delete(model);

            return Ok(result);
        }

        [HttpDelete("confirm")]
        [PermissionAuthorization(PermissionConst.QCAPP_UPDATE)]
        public async Task<IActionResult> Confirm([FromBody] QCAPPMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCAPPService.Confirm(model);

            return Ok(result);
        }

        [HttpPost("copy")]
        [PermissionAuthorization(PermissionConst.QCAPP_CREATE)]
        public async Task<IActionResult> Copy([FromBody] QCAPPMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCAPPService.Copy(model);

            return Ok(result);
        }

        [HttpGet("get-product/{QCAPPMasterId}")]
        public async Task<IActionResult> GetProductForId(long QCAPPMasterId)
        {
            string Column = "p.ProductId, p.ProductCode, cm.ModelCode";
            string Table = "Product p join Model cm on p.ModelId = cm.ModelId";
            string Where = "p.isActived = 1 and p.ProductCode in (select ProductCode from QCAPPMasterProduct where QCAPPMasterId = " + QCAPPMasterId + ")";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        #endregion

        #region Detail
        [HttpGet("get-detail")]
        [PermissionAuthorization(PermissionConst.QCAPP_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] QCAPPDetailDto model)
        {
            var returnData = await _QCAPPService.GetDetail(model);
            return Ok(returnData);
        }

        [HttpPost("create-detail")]
        [PermissionAuthorization(PermissionConst.QCAPP_CREATE)]
        public async Task<IActionResult> CreateSL([FromBody] QCAPPDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCAPPDetailId = AutoId.AutoGenerate();

            var result = await _QCAPPService.CreateDetail(model);

            return Ok(result);
        }
     
        [HttpDelete("delete-detail")]
        [PermissionAuthorization(PermissionConst.QCAPP_DELETE)]
        public async Task<IActionResult> DeleteSL([FromBody] QCAPPDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCAPPService.DeleteDetail(model);

            return Ok(result);
        }
        [HttpDelete("clear-detail/{QCPQCMasterId}")]
        [PermissionAuthorization(PermissionConst.QCAPP_DELETE)]
        public async Task<IActionResult> ClearSL(long QCPQCMasterId)
        {
            var result = await _QCAPPService.ClearDetail(QCPQCMasterId);
            return Ok(result);
        }
        #endregion

        [HttpGet("get-product-list")]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "p.ProductId, concat(p.ProductCode, ' - ', p.ProductName) ProductCode, cm.ModelCode";
            string Table = "Product p join Model cm on p.ModelId = cm.ModelId";
            string Where = "p.isActived = 1 and p.ProductCode not in (select ProductCode from QCAPPMasterProduct)";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-qc-type")]
        public async Task<IActionResult> GetQCType()
        {
            return Ok(await _customService.GetQCTypeForSelectByApply("FQC"));
        }
        [HttpGet("get-qc-item/{QCTypeId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCItem(long? QCTypeId)
        {
            var list = await _customService.GetQCItemForSelectByApply(QCTypeId, "FQC");
            return Ok(list);
        }
        [HttpGet("get-qc-standard/{QCTypeId}/{QCItemId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCStandard(long? QCTypeId, long? QCItemId)
        {
            var list = await _customService.GetQCStandardForSelectByApply(QCTypeId, QCItemId, "FQC");
            return Ok(list);
        }
        [HttpGet("get-qc-tool")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCTool(long? QCTypeId, long? QCItemId, long? QCStandardId)
        {
            var list = await _customService.GetQCToolForSelectByApply(QCTypeId, QCItemId, QCStandardId, "FQC");
            return Ok(list);
        }
        [HttpGet("get-qc-frequency")]
        public async Task<IActionResult> GetQCFrequency()
        {
            return Ok(await _customService.GetQCFrequencyForSelectByApply("FQC"));
        }
        [HttpGet("get-Type-list")]
        public async Task<IActionResult> GetTypeList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Type", "FQC"));
        }

        [HttpGet("get-Item-list")]
        public async Task<IActionResult> GetItemList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Item", "FQC"));
        }

        [HttpGet("get-Standard-list")]
        public async Task<IActionResult> GetStandardList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Standard", "FQC"));
        }

        [HttpGet("get-Tool-list")]
        public async Task<IActionResult> GetToolList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Tool", "FQC"));
        }

        [HttpGet("get-Location-list")]
        public async Task<IActionResult> GetLocation()
        {
            var list = await _commonMasterService.GetForSelect("LOCATION QC APP");
            return Ok(list);
        }

        [HttpGet("get-TypeQC-list")]
        public async Task<IActionResult> GetTypeQC()
        {
            var list = await _commonMasterService.GetForSelect("TYPE APP");
            return Ok(list);
        }
    }
}
