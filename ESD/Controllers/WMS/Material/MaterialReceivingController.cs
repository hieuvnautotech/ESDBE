using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.WMS.Material;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.WMS.Material
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialReceivingController : ControllerBase
    {
        private readonly IMaterialReceivingService _materialReceivingService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MaterialReceivingController(IWebHostEnvironment webHostEnvironment,
            ICustomService customService, IMaterialReceivingService materialReceivingService,
            IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _materialReceivingService = materialReceivingService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region IQC AND Receiving
        [HttpGet]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialReceivingDto model)
        {
            var returnData = await _materialReceivingService.GetAll(model);
            return Ok(returnData);
        }

        [HttpGet("lotno")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetLotNo([FromQuery] MaterialReceivingDto model)
        {
            var returnData = await _materialReceivingService.GetLotNo(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_CREATE)]
        public async Task<IActionResult> Create([FromBody] MaterialReceivingDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _materialReceivingService.Create(model);

            return Ok(result);
        }
        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_UPDATE)]
        public async Task<IActionResult> ModifyReceiving([FromBody] MaterialReceivingDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _materialReceivingService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_DELETE)]
        public async Task<IActionResult> Delete([FromBody] MaterialReceivingDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _materialReceivingService.Delete(model);

            return Ok(result);
        }

        [HttpDelete("delete-lot")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_DELETE)]
        public async Task<IActionResult> DeleteLot([FromBody] MaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _materialReceivingService.DeleteLot(model);

            return Ok(result);
        }

        [HttpGet("get-materialLot-all")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetMaterialLotAll([FromQuery] MaterialLotDto model)
        {
            var returnData = await _materialReceivingService.GetMaterialLotAll(model);
            return Ok(returnData);
        }
        [HttpGet("get-materialReceivingLot-all")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetMaterialReceivingLotAll([FromQuery] MaterialLotDto model)
        {
            var returnData = await _materialReceivingService.GetMaterialReceivingLotAll(model);
            return Ok(returnData);
        }

        [HttpGet("get-materialReceivingLot-all-Print")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetMaterialReceivingLotAllPrint([FromQuery] MaterialLotDto model)
        {
            var returnData = await _materialReceivingService.GetMaterialReceivingLotAllPrint(model);
            return Ok(returnData);
        }

        [HttpPost("add-lot")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_CREATE)]
        public async Task<IActionResult> AddLot([FromBody] MaterialReceivingDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _materialReceivingService.AddLot(model);

            return Ok(result);
        }
        #endregion
        #region Detail Material
        [HttpGet("get-check/{QCIQCMasterId}/{MaterialLotId}")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetDetailMaterialL(long? QCIQCMasterId, long? MaterialLotId)
        {
            var returnData = await _materialReceivingService.GetDetailMaterial(QCIQCMasterId, MaterialLotId);
            return Ok(returnData);
        }
        [HttpPost("check")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_CREATE)]
        public async Task<IActionResult> CreateFormMaterial([FromBody] CheckMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _materialReceivingService.CreateFormMaterial(model);

            return Ok(result);
        }

        [HttpPost("checkSUS")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_CREATE)]
        public async Task<IActionResult> CreateFormSUSMaterial([FromBody] CheckMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _materialReceivingService.CreateFormSUSMaterial(model);

            return Ok(result);
        }
        #endregion
        #region Raw Material
        [HttpGet("get-check-raw/{QCIQCMasterId}/{MaterialLotId}")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetDetailMaterialRM(long? QCIQCMasterId, long? MaterialLotId)
        {
            var returnData = await _materialReceivingService.GetDetailRawMaterial(QCIQCMasterId, MaterialLotId);
            return Ok(returnData);
        }

        [HttpPost("check-raw")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_CREATE)]
        public async Task<IActionResult> CreateRawMaterial([FromBody] CheckRawMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _materialReceivingService.CreateRawMaterial(model);

            return Ok(result);
        }
        #endregion

        [HttpGet("get-staff-check")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetStaffCheck()
        {
            string Column = "StaffId, concat(s.StaffCode, ' - ', s.StaffName) as StaffName";
            string Table = "Staff s";
            string Where = "s.isActived = 1 and (s.DeptCode = '00007')";    
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("po-for-select")]
        public async Task<IActionResult> GetPOForSelect([FromQuery] PageModel model, string? POOrderCode, string? MaterialCode)
        {
            var returnData = await _materialReceivingService.GetPO(model, POOrderCode, MaterialCode);
            return Ok(returnData);
        }

        [HttpGet("get-material-type")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetMaterialType()
        {
            var list = await _commonMasterService.GetForSelect("MATERIALTYPE");
            return Ok(list);
        }
        [HttpGet("get-iqc-form/{IQCType}")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetIQCForm(string? IQCType)
        {

            string Column = "QCIQCMasterId, QCIQCMasterName";
            string Table = "QCIQCMaster";
            string Where = @"IsConfirm = 1 and isActived = 1 and IQCType = '" + IQCType + "'";

            return Ok(await _customService.GetForSelect<QCIQCMasterDto>(Column, Table, Where, ""));
        }
        [HttpGet("get-all-detail-id/{MaterialReceivingId}")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetAllDetailId(long? MaterialReceivingId)
        {
            var list = await _materialReceivingService.GetAllDetailId(MaterialReceivingId);
            return Ok(list);
        }

        [HttpPost("get-list-print-qr")]
        [PermissionAuthorization(PermissionConst.IQCRECEIVING_READ)]
        public async Task<IActionResult> GetListPrintQR([FromBody] List<long> listQR)
        {

            return Ok(await _materialReceivingService.GetListPrintQR(listQR));
        }


        [HttpPost("print-material-lot")]
        public async Task<IActionResult> PrintMaterialLot([FromBody] List<long> listQR)
        {
            return Ok(await _materialReceivingService.PrintMaterialLot(listQR));
        }
    }
}
