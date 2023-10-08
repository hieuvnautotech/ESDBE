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
    public class QCMoldController : ControllerBase
    {
        private readonly IQCMoldService _QCMoldService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public QCMoldController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, IQCMoldService QCMoldService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _QCMoldService = QCMoldService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.QCMOLD_READ)]
        public async Task<IActionResult> GetAll([FromQuery] QCMoldMasterDto model)
        {
            var returnData = await _QCMoldService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.QCMOLD_CREATE)]
        public async Task<IActionResult> Create([FromBody] QCMoldMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCMoldMasterId = AutoId.AutoGenerate();

            var result = await _QCMoldService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.QCMOLD_UPDATE)]
        public async Task<IActionResult> Update([FromBody] QCMoldMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _QCMoldService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.QCMOLD_DELETE)]
        public async Task<IActionResult> Delete([FromBody] QCMoldMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCMoldService.Delete(model);

            return Ok(result);
        }

        [HttpDelete("confirm")]
        [PermissionAuthorization(PermissionConst.QCMOLD_UPDATE)]
        public async Task<IActionResult> Confirm([FromBody] QCMoldMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCMoldService.Confirm(model);

            return Ok(result);
        }

        [HttpPost("copy")]
        [PermissionAuthorization(PermissionConst.QCMOLD_CREATE)]
        public async Task<IActionResult> Copy([FromBody] QCMoldMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCMoldService.Copy(model);

            return Ok(result);
        }
        #endregion

        #region Detail
        [HttpGet("get-detail")]
        [PermissionAuthorization(PermissionConst.QCMOLD_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] QCMoldDetailDto model)
        {
            var returnData = await _QCMoldService.GetDetail(model);
            return Ok(returnData);
        }

        [HttpPost("create-detail")]
        [PermissionAuthorization(PermissionConst.QCMOLD_CREATE)]
        public async Task<IActionResult> CreateSL([FromBody] QCMoldDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCMoldDetailId = AutoId.AutoGenerate();

            var result = await _QCMoldService.CreateDetail(model);

            return Ok(result);
        }
     
        [HttpDelete("delete-detail")]
        [PermissionAuthorization(PermissionConst.QCMOLD_DELETE)]
        public async Task<IActionResult> DeleteSL([FromBody] QCMoldDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCMoldService.DeleteDetail(model);

            return Ok(result);
        }
        [HttpDelete("clear-detail/{QCPQCMasterId}")]
        [PermissionAuthorization(PermissionConst.QCMOLD_DELETE)]
        public async Task<IActionResult> ClearSL(long QCPQCMasterId)
        {
            var result = await _QCMoldService.ClearDetail(QCPQCMasterId);
            return Ok(result);
        }
        #endregion

        [HttpGet("get-qc-type")]
        public async Task<IActionResult> GetQCType()
        {
            return Ok(await _customService.GetQCTypeForSelectByApply("MOLD"));
        }
        [HttpGet("get-qc-item/{QCTypeId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCItem(long? QCTypeId)
        {
            var list = await _customService.GetQCItemForSelectByApply(QCTypeId, "MOLD");
            return Ok(list);
        }
        [HttpGet("get-qc-standard/{QCTypeId}/{QCItemId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCStandard(long? QCTypeId, long? QCItemId)
        {
            var list = await _customService.GetQCStandardForSelectByApply(QCTypeId, QCItemId, "MOLD");
            return Ok(list);
        }
    }
}
