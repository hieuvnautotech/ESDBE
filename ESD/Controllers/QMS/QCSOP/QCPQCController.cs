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
    public class QCPQCController : ControllerBase
    {
        private readonly IQCPQCService _QCPQCService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public QCPQCController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, IQCPQCService QCPQCService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _QCPQCService = QCPQCService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.QCPQC_READ)]
        public async Task<IActionResult> GetAll([FromQuery] QCPQCMasterDto model)
        {
            var returnData = await _QCPQCService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.QCPQC_CREATE)]
        public async Task<IActionResult> Create([FromForm] QCPQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCPQCMasterId = AutoId.AutoGenerate();

            if (model.file != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "Image\\QCPQC");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string newFileName =string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.file.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.file.CopyToAsync(stream);
                }

                model.ImageFile = newFileName;
            }

            var result = await _QCPQCService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.QCPQC_UPDATE)]
        public async Task<IActionResult> Update([FromForm] QCPQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            if (model.file != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "Image\\QCPQC");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string newFileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.file.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.file.CopyToAsync(stream);
                }

                model.ImageFile = newFileName;
            }

            var result = await _QCPQCService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.QCPQC_DELETE)]
        public async Task<IActionResult> Delete([FromBody] QCPQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCPQCService.Delete(model);

            return Ok(result);
        }

        [HttpDelete("confirm")]
        [PermissionAuthorization(PermissionConst.QCPQC_UPDATE)]
        public async Task<IActionResult> Confirm([FromBody] QCPQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCPQCService.Confirm(model);

            return Ok(result);
        }

        [HttpPost("copy")]
        [PermissionAuthorization(PermissionConst.QCPQC_CREATE)]
        public async Task<IActionResult> Copy([FromBody] QCPQCMasterDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCPQCService.Copy(model);

            return Ok(result);
        }

        [HttpGet("get-product/{QCPQCMasterId}")]
        public async Task<IActionResult> GetProductForId(long QCPQCMasterId)
        {
            string Column = "ProductId, ProductCode";
            string Table = "Product";
            string Where = "isActived = 1 and ProductCode in (select ProductCode from QCPQCMasterProduct where QCPQCMasterId = " + QCPQCMasterId + ")";
            return Ok(await _customService.GetForSelect<ProductDto>(Column, Table, Where, ""));
        }

        [HttpGet("get-process/{QCPQCMasterId}")]
        public async Task<IActionResult> GetProcessForId(long QCPQCMasterId)
        {
            //string Column = "ProcessId, ProcessCode, ProcessName";
            //string Table = "Process";
            //string Where = "isActived = 1 and ProcessCode in (select ProcessCode from QCPQCMasterProduct where QCPQCMasterId = " + QCPQCMasterId + ")";
            string Column = "commonDetailId ProcessId,commonDetailCode ProcessCode,commonDetailCode ProcessName";
            string Table = "sysTbl_CommonDetail";
            string Where = "isActived = 1 AND commonMasterCode ='BOMPROCESS' AND commonDetailCode in (select ProcessCode from QCPQCMasterProduct where QCPQCMasterId = " + QCPQCMasterId + ")";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        #endregion

        #region Detail AS
        [HttpGet("get-detail-as")]
        [PermissionAuthorization(PermissionConst.QCPQC_READ)]
        public async Task<IActionResult> GetDetailAS([FromQuery] QCPQCDetailASDto model)
        {
            var returnData = await _QCPQCService.GetDetailAS(model);
            return Ok(returnData);
        }

        [HttpPost("create-detail-as")]
        [PermissionAuthorization(PermissionConst.QCPQC_CREATE)]
        public async Task<IActionResult> CreateAS([FromBody] QCPQCDetailASDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCPQCDetailASId = AutoId.AutoGenerate();

            var result = await _QCPQCService.CreateDetailAS(model);

            return Ok(result);
        }

        [HttpDelete("delete-detail-as")]
        [PermissionAuthorization(PermissionConst.QCPQC_DELETE)]
        public async Task<IActionResult> DeleteAS([FromBody] QCPQCDetailASDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCPQCService.DeleteDetailAS(model);

            return Ok(result);
        }
        #endregion

        #region Detail SL
        [HttpGet("get-detail-sl")]
        [PermissionAuthorization(PermissionConst.QCPQC_READ)]
        public async Task<IActionResult> GetDetailSL([FromQuery] QCPQCDetailSLDto model)
        {
            var returnData = await _QCPQCService.GetDetailSL(model);
            return Ok(returnData);
        }

        [HttpPost("create-detail-sl")]
        [PermissionAuthorization(PermissionConst.QCPQC_CREATE)]
        public async Task<IActionResult> CreateSL([FromForm] QCPQCDetailSLDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.QCPQCDetailSLId = AutoId.AutoGenerate();

            if (model.file != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "Image\\QCPQC");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string newFileName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), ".", model.file.FileName.Split('.').Last());

                using (var stream = System.IO.File.Create(Path.Combine(folderPath, newFileName)))
                {
                    await model.file.CopyToAsync(stream);
                }

                model.ImageFile = newFileName;
            }

            var result = await _QCPQCService.CreateDetailSL(model);

            return Ok(result);
        }
     
        [HttpDelete("delete-detail-sl")]
        [PermissionAuthorization(PermissionConst.QCPQC_DELETE)]
        public async Task<IActionResult> DeleteSL([FromBody] QCPQCDetailSLDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _QCPQCService.DeleteDetailSL(model);

            return Ok(result);
        }

        [HttpDelete("clear-detail-sl/{QCPQCMasterId}")]
        [PermissionAuthorization(PermissionConst.QCPQC_DELETE)]
        public async Task<IActionResult> ClearSL(long QCPQCMasterId)
        {
            var result = await _QCPQCService.ClearDetailSL(QCPQCMasterId);

            return Ok(result);
        }
        #endregion

        [HttpGet("get-Process-list")]
        public async Task<IActionResult> GetProcessList()
        {
            //var list = await _commonMasterService.GetForSelect("BOMPROCESS");
            //return Ok(list);
            string Column = "commonDetailId ProcessId,commonDetailCode ProcessCode,commonDetailCode ProcessName";
            string Table = "sysTbl_CommonDetail";
            string Where = "isActived = 1 AND commonMasterCode ='BOMPROCESS'";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-product-list")]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, concat(ProductCode, ' - ', ProductName) ProductCode ";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        [HttpGet("get-qc-type")]
        public async Task<IActionResult> GetQCType()
        {
            return Ok(await _customService.GetQCTypeForSelectByApply("PQC"));
        }
        [HttpGet("get-qc-item/{QCTypeId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCItem(long? QCTypeId)
        {
            var list = await _customService.GetQCItemForSelectByApply(QCTypeId, "PQC");
            return Ok(list);
        }
        [HttpGet("get-qc-standard/{QCTypeId}/{QCItemId}")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCStandard(long? QCTypeId, long? QCItemId)
        {
            var list = await _customService.GetQCStandardForSelectByApply(QCTypeId, QCItemId, "PQC");
            return Ok(list);
        }
        [HttpGet("get-qc-tool")]
        [PermissionAuthorization(PermissionConst.STANDARD_QC_CREATE)]
        public async Task<IActionResult> GetQCTool(long? QCTypeId, long? QCItemId, long? QCStandardId)
        {
            var list = await _customService.GetQCToolForSelectByApply(QCTypeId, QCItemId, QCStandardId, "PQC");
            return Ok(list);
        }
        [HttpGet("get-frequency-list")]
        public async Task<IActionResult> GetFrequency()
        {
            string Column = "QCFrequencyId, QCName";
            string Table = "QCFrequency";
            string Where = "isActived = 1 and QCApply = [dbo].[sysFunc_GetCommonDetailId]('QC APPLY', 'PQC') ";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-Type-list")]
        public async Task<IActionResult> GetTypeList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Type", "PQC"));
        }

        [HttpGet("get-Item-list")]
        public async Task<IActionResult> GetItemList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Item", "PQC"));
        }

        [HttpGet("get-Standard-list")]
        public async Task<IActionResult> GetStandardList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Standard", "PQC"));
        }

        [HttpGet("get-Tool-list")]
        public async Task<IActionResult> GetToolList()
        {
            return Ok(await _customService.GetStandardQCForSelect("Tool", "PQC"));
        }

        [HttpGet("get-Location-list")]
        public async Task<IActionResult> GetLocation()
        {
            var list = await _commonMasterService.GetForSelect("LOCATIONQCPQC");
            return Ok(list);
        }
    }
}
