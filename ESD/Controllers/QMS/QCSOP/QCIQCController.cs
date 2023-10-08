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
    public class QCIQCController : ControllerBase
    {
        private readonly IQCIQCService _QCIQCService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public QCIQCController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, IQCIQCService QCIQCService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _QCIQCService = QCIQCService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Master
        [HttpGet("get-all")]
        [PermissionAuthorization(PermissionConst.QCIQC_READ)]
        public async Task<IActionResult> GetAll([FromQuery] QCIQCMasterDto model)
        {
            var returnData = await _QCIQCService.GetAll(model);
            return Ok(returnData);
        }
        [HttpGet("get-material-type")]
        public async Task<IActionResult> GetMaterialType()
        {
            var list = await _commonMasterService.GetForSelect("MATERIALTYPE");
            return Ok(list);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.QCIQC_CREATE)]
        public async Task<IActionResult> Create([FromBody] QCIQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCIQCMasterId = AutoId.AutoGenerate();

            var result = await _QCIQCService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.QCIQC_UPDATE)]
        public async Task<IActionResult> Update([FromBody] QCIQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _QCIQCService.Modify(model);

            return Ok(result);
        }


        [HttpPost("copy")]
        [PermissionAuthorization(PermissionConst.QCIQC_CREATE)]
        public async Task<IActionResult> Copy([FromBody] QCIQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _QCIQCService.Copy(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.QCIQC_DELETE)]
        public async Task<IActionResult> Delete([FromBody] QCIQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCIQCService.Delete(model);

            return Ok(result);
        }

        [HttpDelete("confirm")]
        [PermissionAuthorization(PermissionConst.QCIQC_UPDATE)]
        public async Task<IActionResult> Confirm([FromBody] QCIQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCIQCService.Confirm(model);

            return Ok(result);
        }


        #endregion

        #region Detail Raw Material
        [HttpGet("get-detail-raw-material")]
        [PermissionAuthorization(PermissionConst.QCIQC_READ)]
        public async Task<IActionResult> GetDetailRM([FromQuery] QCIQCDetailRMDto model)
        {
            var returnData = await _QCIQCService.GetDetailRawMaterial(model);
            return Ok(returnData);
        }
        [HttpPost("create-DetailRM")]
        [PermissionAuthorization(PermissionConst.QCIQC_CREATE)]
        public async Task<IActionResult> CreateAS([FromBody] QCIQCDetailRMDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCIQCDetailRMId = AutoId.AutoGenerate();

            var result = await _QCIQCService.CreateDetailRM(model);

            return Ok(result);
        }

        [HttpDelete("delete-Detail-RM")]
        [PermissionAuthorization(PermissionConst.QCIQC_DELETE)]
        public async Task<IActionResult> DeleteRM([FromBody] QCIQCDetailRMDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCIQCService.DeleteDetailRM(model);

            return Ok(result);
        }
        [HttpDelete("clear-detail-RM/{QCIQCMasterId}")]
        [PermissionAuthorization(PermissionConst.QCIQC_DELETE)]
        public async Task<IActionResult> ClearDetailRM(long QCIQCMasterId)
        {
            var result = await _QCIQCService.ClearDetailRM(QCIQCMasterId);

            return Ok(result);
        }
        #endregion

        #region Detail Material
        [HttpGet("get-detail-material")]
        [PermissionAuthorization(PermissionConst.QCIQC_READ)]
        public async Task<IActionResult> GetDetailSL([FromQuery] QCIQCDetailMDto model)
        {
            var returnData = await _QCIQCService.GetDetailMaterial(model);
            return Ok(returnData);
        }

        [HttpPost("create-DetailMaterial")]
        [PermissionAuthorization(PermissionConst.QCIQC_CREATE)]
        public async Task<IActionResult> CreateMaterial([FromBody] QCIQCDetailMDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCIQCDetailMId = AutoId.AutoGenerate();

            var result = await _QCIQCService.CreateDetailMaterial(model);

            return Ok(result);
        }

        [HttpDelete("delete-Detail-M")]
        [PermissionAuthorization(PermissionConst.QCIQC_DELETE)]
        public async Task<IActionResult> DeleteMaterial([FromBody] QCIQCDetailMDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCIQCService.DeleteDetailMaterial(model);

            return Ok(result);
        }
        [HttpDelete("clear-detail-M/{QCIQCMasterId}")]
        [PermissionAuthorization(PermissionConst.QCIQC_DELETE)]
        public async Task<IActionResult> ClearDeleteMaterial(long QCIQCMasterId)
        {
            var result = await _QCIQCService.ClearDetailMaterial(QCIQCMasterId);

            return Ok(result);
        }
        #endregion
        [HttpGet("get-qc-type")]
        public async Task<IActionResult> GetQCType()
        {
            return Ok(await _customService.GetQCTypeForSelectByApply("IQC"));
        }
        [HttpGet("get-qc-item/{QCTypeId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCItem(long? QCTypeId)
        {
            var list = await _customService.GetQCItemForSelectByApply(QCTypeId, "IQC");
            return Ok(list);
        }
        [HttpGet("get-qc-standard/{QCTypeId}/{QCItemId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCStandard(long? QCTypeId, long? QCItemId)
        {
            var list = await _customService.GetQCStandardForSelectByApply(QCTypeId, QCItemId, "IQC");
            return Ok(list);
        }
        [HttpGet("get-qc-tool")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCTool(long? QCTypeId, long? QCItemId, long? QCStandardId)
        {
            var list = await _customService.GetQCToolForSelectByApply(QCTypeId, QCItemId, QCStandardId, "IQC");
            return Ok(list);
        }
        [HttpGet("get-qc-frequency")]
        public async Task<IActionResult> GetQCFrequency()
        {
            return Ok(await _customService.GetQCFrequencyForSelectByApply("IQC"));
        }
        [HttpGet("get-Item-list")]
        public async Task<IActionResult> GetItemList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Item", "IQC"));
        }
        [HttpGet("get-Type-list")]
        public async Task<IActionResult> GetTypeList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Type", "IQC"));
        }
        [HttpGet("get-Item-Unit-list")]
        public async Task<IActionResult> GetItemUnitList()
        {
            return Ok(await _commonMasterService.GetForSelect("MATERIAL ITEM UNIT"));
        }
    }
}
