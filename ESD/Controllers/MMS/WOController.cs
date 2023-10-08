using ESD.Services.Common;
using ESD.Services.MMS;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;
using ESD.Services.Standard.Information;
using ESD.Services.Common.Standard.Information;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.MMS;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Models.Validators;

namespace ESD.Controllers.MMS
{
    [Route("api/[controller]")]
    [ApiController]
    public class WOController : ControllerBase
    {
        private readonly IWOService _WOService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IStaffService _staffService;
        private readonly ICommonMasterService _commonMasterService;
        public WOController(IStaffService staffService, IWOService WOService, IJwtService jwtService, ICustomService customService, ICommonMasterService commonMasterService)
        {
            _WOService = WOService;
            _jwtService = jwtService;
            _customService = customService;
            _staffService = staffService;
            _commonMasterService = commonMasterService;
        }
        #region WO
        [HttpGet]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetAll([FromQuery] WODto model)
        {
            var returnData = await _WOService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> Create([FromBody] WODto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOId = AutoId.AutoGenerate();

            var result = await _WOService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> Update([FromBody] WODto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> Delete([FromBody] WODto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.Delete(model);

            return Ok(result);
        }

        [HttpDelete("finish")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> Finish([FromBody] WODto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.Finish(model);

            return Ok(result);
        }
        #endregion

        #region WO Process
        [HttpGet("get-Process")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetProcess([FromQuery] WOProcessDto model)
        {
            var returnData = await _WOService.GetProcess(model);
            return Ok(returnData);
        }

        [HttpPost("create-Process")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateProcess([FromBody] WOProcessDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOProcessId = AutoId.AutoGenerate();

            var result = await _WOService.CreateProcess(model);

            return Ok(result);
        }

        [HttpPut("update-Process")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> UpdateProcess([FromBody] WOProcessDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.ModifyProcess(model);

            return Ok(result);
        }

        [HttpDelete("delete-Process")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteProcess([FromBody] WOProcessDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.DeleteProcess(model);

            return Ok(result);
        }
        #endregion

        #region Check PQC
        [HttpGet("get-detail-as")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetDetailAS([FromQuery] PQCWOProcessCheckMasterASDto model)
        {
            var returnData = await _WOService.GetValueCheck(model.QCPQCMasterId, model.WOProcessId);
            return Ok(returnData);
        }

        [HttpPost("check-PQC")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> CheckPQC(PQCWOProcessCheckMasterASDto model)
        {
            var returnData = await _WOService.CheckProcessPQC(model);
            return Ok(returnData);
        }

        [HttpGet("get-staff-check")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetStaffCheck()
        {
            string Column = "StaffId, concat(s.StaffCode, ' - ', s.StaffName) as StaffName";
            string Table = "Staff s";
            string Where = "s.isActived = 1 and (s.DeptCode = '00007')";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
        #endregion

        #region ProcessMold
        [HttpGet("get-process-mold")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetProcessMold([FromQuery] WOProcessMoldDto model)
        {
            var returnData = await _WOService.GetProcessMold(model);
            return Ok(returnData);
        }
        [HttpGet("get-process-mold-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetProcessMoldDetail([FromQuery] WOMoldPressingTimesDto model)
        {
            var returnData = await _WOService.GetWOProcessMold(model);
            return Ok(returnData);
        }
        [HttpPost("create-Process-mold")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateProcessMold([FromBody] WOProcessMoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOProcessMoldId = AutoId.AutoGenerate();

            var result = await _WOService.CreateProcessMold(model);

            return Ok(result);
        }

        [HttpPut("update-Process-mold")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> UpdateProcessMold([FromBody] WOProcessMoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.ModifyProcessMold(model);

            return Ok(result);
        }

        [HttpDelete("delete-Process-mold")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteProcessMold([FromBody] WOProcessMoldDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.DeleteProcessMold(model);

            return Ok(result);
        }
        #endregion

        #region ProcessStaff
        [HttpGet("get-Process-staff")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetProcessStaff([FromQuery] WOProcessStaffDto model)
        {
            var returnData = await _WOService.GetProcessStaff(model);
            return Ok(returnData);
        }

        [HttpPost("create-Process-staff")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateProcessStaff([FromBody] WOProcessStaffDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOProcessStaffId = AutoId.AutoGenerate();

            var result = await _WOService.CreateProcessStaff(model);

            return Ok(result);
        }

        [HttpPut("update-Process-staff")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> UpdateProcessStaff([FromBody] WOProcessStaffDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.ModifyProcessStaff(model);

            return Ok(result);
        }

        [HttpDelete("delete-Process-staff")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteProcessStaff([FromBody] WOProcessStaffDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.DeleteProcessStaff(model);

            return Ok(result);
        }
        #endregion

        #region ProcessLine
        [HttpGet("get-Process-line")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetProcessLine([FromQuery] WOProcessLineDto model)
        {
            var returnData = await _WOService.GetProcessLine(model);
            return Ok(returnData);
        }

        [HttpPost("create-Process-Line")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateProcessLine([FromBody] WOProcessLineDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOProcessLineId = AutoId.AutoGenerate();

            var result = await _WOService.CreateProcessLine(model);

            return Ok(result);
        }
  
        [HttpPut("update-Process-Line")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> UpdateProcessLine([FromBody] WOProcessLineDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.ModifyProcessLine(model);

            return Ok(result);
        }

        [HttpDelete("delete-Process-Line")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteProcessLine([FromBody] WOProcessLineDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.DeleteProcessLine(model);

            return Ok(result);
        }
        [HttpPost("create-Process-Line-Dup")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateProcessLineDup([FromBody] WOProcessLineDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOProcessLineId = AutoId.AutoGenerate();

            var result = await _WOService.CreateProcessLineDup(model);

            return Ok(result);
        }
        #endregion

        #region ProcessMoldStaffLine
        [HttpGet("get-process-mold-staff-line")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetProcessMoldStaffLine([FromQuery] WOProcessMoldStaffLineDto model)
        {
            var returnData = await _WOService.GetProcessMoldStaffLine(model);
            return Ok(returnData);
        }
        #endregion

        #region WO SemiLot MMS
        [HttpGet("get-check-mold-staff-line/{WOProcessId}")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> CheckNewMoldWorkerMachine(long? WOProcessId)
        {
            var returnData = await _WOService.CheckNewMoldWorkerMachine(WOProcessId);

            return Ok(returnData);

        }

        [HttpGet("get-wosemilot-mms")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWoSemiLotMMS([FromQuery] WOSemiLotMMSDto model)
        {
            var returnData = await _WOService.GetWoSemiLotMMS(model);
            return Ok(returnData);
        }

        [HttpPost("create-semi-lot")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateSemiLot([FromBody] WOSemiLotMMSDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();
            bool checkStaffMoldMachine = await _WOService.CheckMoldStaffMachineShift(model.WOProcessId);
            if (checkStaffMoldMachine == false)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MoldStaffMachineEndShift";
                return Ok(returnData);
            }

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOSemiLotMMSId = AutoId.AutoGenerate();

            var result = await _WOService.CreateSemiLot(model);

            return Ok(result);
        }

        [HttpDelete("delete-semi-lot")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteSemiLot([FromBody] WOSemiLotMMSDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var returnData = new ResponseModel<WOSemiLotMMSDto?>();

            var WOSemiLotMMS = await _WOService.GetByIdWOSemiLotMMS(model.WOSemiLotMMSId);
            if (WOSemiLotMMS.HttpResponseCode != 200)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            bool CheckShiftOfStaff = await _WOService.CheckSemiLotShiftOfStaff(WOSemiLotMMS.Data.WOProcessId, WOSemiLotMMS.Data.SemiLotCode);
            if (!CheckShiftOfStaff)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.OutShift";
                return Ok(returnData);
            }
            if (WOSemiLotMMS.Data.ActualQty > 0 || WOSemiLotMMS.Data.OriginQty > 0)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.QuantityGreater0";
                return Ok(returnData);
            }
            if (WOSemiLotMMS.Data.LotStatus != "009" && WOSemiLotMMS.Data.LotStatus != "001")//using
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.AllowStatusStock";
                return Ok(returnData);
            }
            bool checkMaterialMapping = await _WOService.CheckMaterialMapping(WOSemiLotMMS.Data.SemiLotCode);
            if (checkMaterialMapping == true)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MappingToAnotherProcess";
                return Ok(returnData);
            }

            bool CheckExistsMappingFinish = await _WOService.IsMaterialFinished(WOSemiLotMMS.Data.SemiLotCode);
            if (CheckExistsMappingFinish)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.Error_Material_Finished";
                return Ok(returnData);
            }
            bool MaxSemiLot = await _WOService.CheckMaxSemilot(model.WOSemiLotMMSId, model.WOProcessId);
            if (MaxSemiLot == false)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.SemiNotMax";
                return Ok(returnData);
            }
            //Update NVL về trang thái ban đầu
            var ds_mapping = await _WOService.GetMaterialMappingByMaterialCode(WOSemiLotMMS.Data.SemiLotCode);
            foreach (var item in ds_mapping)
            {
                var GetMaterialLotCode = await _WOService.GetMaterialLotCode(item.MaterialLotCode);
                if (GetMaterialLotCode != null)
                {
                    int count = await _WOService.GetCountMaterialMapping(item.SemiLotCode, item.MaterialLotCode);
                    if (count <= 0)
                    {
                        GetMaterialLotCode.LotStatus = "004";
                        GetMaterialLotCode.WOProcessId = null;

                    }
                    else
                    {
                        GetMaterialLotCode.LotStatus = "009";
                    }
                    GetMaterialLotCode.createdBy = long.Parse(userId);
                    await _WOService.UpdateMaterialLot(GetMaterialLotCode);
                }
            }

            var result = await _WOService.DeleteSemiLot(model);

            return Ok(result);

        }
        
        [HttpPut("update-semilot-quantity")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> modifyWOSemiLotQuantity([FromBody] WOSemiLotMMSDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.modifiedBy = long.Parse(userId);
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();

            bool checkStaffMoldMachine = await _WOService.CheckMoldStaffMachineShift(model.WOProcessId);
            if (checkStaffMoldMachine == false)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MoldStaffMachineEndShift";
                return Ok(returnData);
            }
            if (model.LotStatus != "009" && model.LotStatus != "001")//using/ not yet
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.AllowStatusStock";
                return Ok(returnData);
            }
            var result = await _WOService.ModifyWOSemiLotQuantity(model);

            return Ok(result);
        }
        [HttpPut("create-wo-mold-pressingtime")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> createWOMoldPressingTimes([FromBody] WOMoldPressingTimesDto model)
        {
            var returnData = new ResponseModel<WOMoldPressingTimesDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOMoldPressingId = AutoId.AutoGenerate();

            var result = await _WOService.CreateWOMoldPressingTimes(model);

            return Ok(result);
        }
       
        [HttpGet("get-print/{WOSemiLotMMSId}")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetPrint(long? WOSemiLotMMSId)
        {
            var returnData = await _WOService.GetByIdWOSemiLotMMSPrint(WOSemiLotMMSId);

            return Ok(returnData);

        }
        #endregion

        #region WO SemiLot MMS Detail
        [HttpGet("get-wosemilot-mms-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWoSemiLotMMSDetail([FromQuery] WOSemiLotMMSDetailDto model)
        {
            var returnData = await _WOService.GetWoSemiLotMMSDetail(model);
            return Ok(returnData);
        }

        [HttpGet("get-wo-materialLot")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWomaterialLot([FromQuery] MaterialLotDto model)
        {
            var returnData = await _WOService.GetWoMaterialLot(model);
            return Ok(returnData);
        }

        [HttpGet("get-wo-semilot")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWosemilot([FromQuery] WOSemiLotMMSDto model)
        {
            var returnData = await _WOService.GetWoSemilot(model);
            return Ok(returnData);
        }

        [HttpPost("create-semi-lot-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateSemiLotDetail([FromBody] WOSemiLotMMSDetailDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();
            var validator = new WOSemiLotMMSDetailValidator();
            var validateResults = validator.Validate(model);
            if (!validateResults.IsValid)
            {
                returnData.HttpResponseCode = 400;
                returnData.ResponseMessage = validateResults.Errors[0].ToString();
                return Ok(returnData);
            }
            var GetSemiLotCode = await _WOService.GetSemiLotCode(model.WOSemiLotMMSId);
            if (GetSemiLotCode == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.WOSemiLotMMSId_required";
                return Ok(returnData);
            }
            bool checkStaffMoldMachine = await _WOService.CheckMoldStaffMachineShift(GetSemiLotCode.WOProcessId);
            if (checkStaffMoldMachine == false)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MoldStaffMachineEndShift";
                return Ok(returnData);
            }

            var GetMaterialLotCode = await _WOService.GetMaterialLotCode(model.MaterialLotCode);

            bool MaxSemiLot = await _WOService.CheckMaxSemilot(GetSemiLotCode.WOSemiLotMMSId, GetSemiLotCode.WOProcessId);
            if (MaxSemiLot == false)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.SemiNotMax";
                return Ok(returnData);
            }
            bool checkMaterialMapping = await _WOService.CheckMaterialMapping(GetSemiLotCode.SemiLotCode);
            if (checkMaterialMapping == true)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MappingToAnotherProcess";
                return Ok(returnData);
            }
            if (GetMaterialLotCode == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MaterialLotNotYetStocked";
                return Ok(returnData);
            }
            if (GetMaterialLotCode.AreaCode != "WIP")
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MaterialLotNotYetShip";
                return Ok(returnData);
            }
            var isMaterialExistInProcess = await _WOService.IsMaterialInfoExistByProcess(model.MaterialLotCode, GetSemiLotCode.WOProcessId);
            if (!isMaterialExistInProcess)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MaterialNotYetRegisteredBOM";
                return Ok(returnData);
            }

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOSemiLotDetailId = AutoId.AutoGenerate();

            var result = await _WOService.CreateSemiLotDetail(model);

            return Ok(result);
        }

        [HttpPost("create-semi-lot-detail-semi")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateSemiLotDetailSemi([FromBody] WOSemiLotMMSDetailDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            //Output model.WOSemiLotMMSId
            //Output model.WOProcessId
            //Input model.MaterialLotCode

            var validator = new WOSemiLotMMSDetailValidator();
            var validateResults = validator.Validate(model);
            if (!validateResults.IsValid)
            {
                returnData.HttpResponseCode = 400;
                returnData.ResponseMessage = validateResults.Errors[0].ToString();
                return Ok(returnData);
            }
            var GetSemiLotCodeOutput = await _WOService.GetSemiLotCode(model.WOSemiLotMMSId);
            if (GetSemiLotCodeOutput == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            bool checkStaffMoldMachine = await _WOService.CheckMoldStaffMachineShift(model.WOProcessId);
            if (checkStaffMoldMachine == false)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MoldStaffMachineEndShift";
                return Ok(returnData);
            }
            bool MaxSemiLot = await _WOService.CheckMaxSemilot(GetSemiLotCodeOutput.WOSemiLotMMSId, GetSemiLotCodeOutput.WOProcessId);
            if (MaxSemiLot == false)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.SemiNotMax";
                return Ok(returnData);
            }
            bool checkMaterialMapping = await _WOService.CheckMaterialMapping(GetSemiLotCodeOutput.SemiLotCode);
            if (checkMaterialMapping == true)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MappingToAnotherProcess";
                return Ok(returnData);
            }

            var GetSemiLotCodeInput = await _WOService.GetSemiLotCodeByCode(model.MaterialLotCode);

            if (GetSemiLotCodeInput == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            if (GetSemiLotCodeInput.LotStatus != "009")//using
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.AllowMaterialUsing";
                return Ok(returnData);
            }
            if (GetSemiLotCodeInput.ActualQty == 0)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.SemiLot_min_value";
                return Ok(returnData);
            }
            if (GetSemiLotCodeOutput.WOProcessId == GetSemiLotCodeInput.WOProcessId)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.SemiLotSameProcess";
                return Ok(returnData);
            }
            //lấy level công đoạn output
            var WOProcess = await _WOService.GetWOProcessById(GetSemiLotCodeOutput.WOProcessId);//level công đoạn đầu ra
            var WOProcessInput = await _WOService.GetWOProcessById(GetSemiLotCodeInput.WOProcessId);///level công đoạn đầu ra
            if (WOProcess.ProcessLevel > 1)
            {
                var NameSemiNo = await _WOService.GetWOProcessName(WOProcess.WOId, WOProcess.ProcessLevel);
                if (NameSemiNo != null)
                {
                    if (WOProcessInput.ProcessLevel != NameSemiNo.ProcessLevel)
                    {
                        returnData.HttpResponseCode = 204;
                        returnData.ResponseMessage = "WO.SemiLotLevelWrong";
                        return Ok(returnData);
                    }
                }
            }


            model.WOSemiLotDetailId = AutoId.AutoGenerate();
            model.SemiLotCode = GetSemiLotCodeOutput.SemiLotCode;
            model.MaterialLotCode = model.MaterialLotCode;
            model.WOProcessId = model.WOProcessId;



            var result = await _WOService.CreateSemiLotDetailSemiLot(model);

            return Ok(result);
        }

        [HttpDelete("delete-semi-lot-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteSemiLotDetail([FromBody] WOSemiLotMMSDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();

            //đầu ra model.WOSemiLotMMSId, model.semilotCode
            //đầu vào  model.MaterialLotCode
            var GetSemiLotCode = await _WOService.GetSemiLotCode(model.WOSemiLotMMSId);
            if (GetSemiLotCode == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }

            var WOSemiLotDetail = await _WOService.GetFistWOSemiLotMMSDetail(model.WOSemiLotDetailId);
            if (WOSemiLotDetail == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            bool checkStaffMoldMachine = await _WOService.CheckMoldStaffMachineShift(GetSemiLotCode.WOProcessId);
            if (checkStaffMoldMachine == false)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MoldStaffMachineEndShift";
                return Ok(returnData);
            }
            var checkMaterialOrSemiLot = await _WOService.checkMaterialOrSemiLot(WOSemiLotDetail.MaterialLotCode);
            if (checkMaterialOrSemiLot == 0)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            #region CHECK LOT 
            //Nếu đầu ra là SEMILOT là cái cũ nhất thì khong cho mapping
            //lấy được giá trị nếu là true thì return(tức db bằng rông or null )
            bool MaxSemiLot = await _WOService.CheckMaxSemilot(WOSemiLotDetail.WOSemiLotMMSId, GetSemiLotCode.WOProcessId);
            if (MaxSemiLot == false)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.SemiNotMax";
                return Ok(returnData);
            }
            #endregion
            int check_exits_mapping = await _WOService.GetCountMaterialMapping(WOSemiLotDetail.SemiLotCode, WOSemiLotDetail.MaterialLotCode);
            if (check_exits_mapping > 0)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.InheritCannotDelete";
                return Ok(returnData);
            }
            if (checkMaterialOrSemiLot == 1)//NVL
            {
              
                var findMaterialLot = await _WOService.GetMaterialLotCode(model.MaterialLotCode);
                if (findMaterialLot == null)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                    return Ok(returnData);
                }
                if (findMaterialLot.LotStatus != "009")//using
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "WO.AllowMaterialUsing";
                    return Ok(returnData);
                }
            }
            if (checkMaterialOrSemiLot == 2)//BTP
            {
               
                var findMaterialLot = await _WOService.GetSemiLotCodeByCode(model.MaterialLotCode);
                if (findMaterialLot == null)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                    return Ok(returnData);
                }
                if (findMaterialLot.LotStatus != "009")//using
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "WO.AllowMaterialUsing";
                    return Ok(returnData);
                }
            }
            var result = await _WOService.DeleteSemiLotDetail(model);

            return Ok(result);

        }
        [HttpPut("finish-semi-lot-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> FinishSemiLotDetail([FromBody] WOSemiLotMMSDetailDto model)
        {
            //Output: model.semi Lot
            //Input : model.materialLotCode

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();
            var checkMaterialOrSemiLot = await _WOService.checkMaterialOrSemiLot(model.MaterialLotCode);
            if (checkMaterialOrSemiLot == 0)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            var GetSemiLotCodeOutput = await _WOService.GetSemiLotCodeByCode(model.SemiLotCode);

            #region CHECK LOT 
            //Nếu đầu ra là SEMILOT là cái cũ nhất thì khong cho mapping
            //lấy được giá trị nếu là true thì return(tức db bằng rông or null )
            bool MaxSemiLot = await _WOService.CheckMaxSemilot(GetSemiLotCodeOutput.WOSemiLotMMSId, GetSemiLotCodeOutput.WOProcessId);
            if (MaxSemiLot == false)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.SemiNotMax";
                return Ok(returnData);
            }
            //int check_exits_mapping = await _WOService.GetCountMaterialMapping(GetSemiLotCodeOutput.SemiLotCode, model.MaterialLotCode);
            //if (check_exits_mapping > 0)
            //{
            //    returnData.HttpResponseCode = 204;
            //    returnData.ResponseMessage = "WO.InheritCannotDelete";
            //    return Ok(returnData);
            //}
            #endregion
            if (checkMaterialOrSemiLot == 1)//NVL
            {
                var findMaterialLot = await _WOService.GetMaterialLotCode(model.MaterialLotCode);
                if (findMaterialLot.LotStatus != "009" && findMaterialLot.LotStatus != "008")//using//Finish
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "WO.AllowMaterialUsing";
                    return Ok(returnData);
                }
                //Nếu NVL NÀY ĐÃ BỊ RETURN THÌ KHÔNG ĐƯỢC FINISH
                bool checkReturn = await _WOService.IsMaterialReturn(model.MaterialLotCode);
                if (checkReturn)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "WO.Error_Returned";
                    return Ok(returnData);
                }
                var result = await _WOService.FinishSemiLotDetail(model);

                return Ok(result);
            }
            if (checkMaterialOrSemiLot == 2)//SEMI LOT
            {
                var findMaterialLot = await _WOService.GetSemiLotCodeByCode(model.MaterialLotCode);
                if (findMaterialLot.LotStatus != "009" && findMaterialLot.LotStatus != "008")//using//Finish
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "WO.AllowMaterialUsing";
                    return Ok(returnData);
                }
                var result = await _WOService.FinishSemiLotDetailSemi(model);

                return Ok(result);
            }
            return Ok();
        }
        [HttpPut("return-semi-lot-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> ReturnSemiLotDetail([FromBody] WOSemiLotMMSDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();

            var WOSemiLotDetail = await _WOService.GetFistWOSemiLotMMSDetail(model.WOSemiLotDetailId);
            if (WOSemiLotDetail == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            if (WOSemiLotDetail.IsFinish == true)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MaterialLotFinished";
                return Ok(returnData);
            }
            var checkMaterialOrSemiLot = await _WOService.checkMaterialOrSemiLot(model.MaterialLotCode);
            if (checkMaterialOrSemiLot == 0)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }

            if (model.RemainQty <= 0)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "semiFqc.Error_Qty_Incorrect";
                return Ok(returnData);
            }
            if (checkMaterialOrSemiLot == 1)
            {
                var GetSemiLotCode = await _WOService.GetSemiLotCode(model.WOSemiLotMMSId);
                if (GetSemiLotCode == null)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                    return Ok(returnData);
                }
                #region CHECK LOT 
                //Nếu đầu ra là SEMILOT là cái cũ nhất thì khong cho mapping
                //lấy được giá trị nếu là true thì return(tức db bằng rông or null )
                bool MaxSemiLot = await _WOService.CheckMaxSemilot(model.WOSemiLotMMSId, GetSemiLotCode.WOProcessId);
                if (MaxSemiLot == false)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "WO.SemiNotMax";
                    return Ok(returnData);
                }
                #endregion
                var findMaterialLot = await _WOService.GetMaterialLotCode(model.MaterialLotCode);
                if (findMaterialLot == null)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                    return Ok(returnData);
                }
                if (findMaterialLot.LotStatus != "009")//using
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "WO.AllowMaterialUsing";
                    return Ok(returnData);
                }
                if (model.RemainQty >= findMaterialLot.Length)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "general.field_max";
                    return Ok(returnData);
                }
                model.ActualQty = model.OriginQty - model.RemainQty;

                var rs = await _WOService.ReturnSemiLotDetail(model);
                return Ok(rs);
            }
            if (checkMaterialOrSemiLot == 2)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.SemiLotCannotReturn";
                return Ok(returnData);
            }

            return Ok("");
        }
        #endregion

        #region Check SemiLot
        [HttpGet("get-list-pqc-sl/{QCPQCMasterId}/{WOSemiLotMMSId}")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> GetListPQCSL(long? QCPQCMasterId, long? WOSemiLotMMSId)
        {
            var returnData = await _WOService.GetListPQCSL(QCPQCMasterId, WOSemiLotMMSId);
            return Ok(returnData);
        }

        [HttpGet("get-value-pqc-sl/{WOSemiLotMMSId}")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> GetValuePQCSL(long? WOSemiLotMMSId)
        {
            var returnData = await _WOService.GetValuePQCSL(WOSemiLotMMSId);
            return Ok(returnData);
        }

        [HttpPost("check-pqc-sl")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> CheckPQCSL(WOSemiLotMMSCheckMasterSLDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var returnData = await _WOService.CheckPQCSL(model);
            return Ok(returnData);
        }

        #endregion
     
        #region List material lot code From BOM
        [HttpGet("get-list-materialLotCode-ByBom")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetListMaterialLotFromBom([FromQuery] BomProcessMaterialDto model)
        {
            var returnData = await _WOService.GetListMaterialLotFromBom(model);
            return Ok(returnData);
        }
        #endregion



        [HttpGet("get-wosemilot-presslot-mms")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWoSemiLotPressLotMMS([FromQuery] WOPressLotMMSDto model)
        {
            var returnData = await _WOService.GetSemiPressLotMMS(model);
            return Ok(returnData);
        }
        [HttpPost("create-press-lot")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> createPressLot([FromBody] WOPressLotMMSDto model)
        {
            var returnData = new ResponseModel<WOSemiLotMMSDetailDto?>();
            //bool checkStaffMoldMachine = await _WOService.CheckMoldStaffMachineShift(model.WOProcessId);
            //if (checkStaffMoldMachine == false)
            //{
            //    returnData.HttpResponseCode = 204;
            //    returnData.ResponseMessage = "WO.MoldStaffMachineEndShift";
            //    return Ok(returnData);
            //}
            //if (model.ListId.Count <= 1)
            //{
            //    returnData.HttpResponseCode = 204;
            //    returnData.ResponseMessage = "general.two_data_at_least" +
            //        "";
            //    return Ok(returnData);
            //}
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.CreatePressLot(model);

            return Ok(result);
        }
        [HttpGet("get-press-lot")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetPressLotMMS([FromQuery] WOPressLotMMSDto model)
        {
            var returnData = await _WOService.GetPressLotMMS(model);
            return Ok(returnData);
        }
        [HttpPut("unMapping")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> UnMapping([FromBody] WOPressLotMMSDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _WOService.UnMaping(model);

            return Ok(result);
        }
        [HttpGet("get-product")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, concat(ProductCode,' - ', ProductName) as ProductCode, ProductCode as ProductCodeTemp, ProductName";
            string Table = "Product ";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-bom/{ProductId}")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetBom(long? ProductId)
        {
            return Ok(await _WOService.GetSelectBOM(ProductId));
        }

        [HttpGet("get-process-list")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetMaterialQC()
        {
            //var list = await _commonMasterService.GetForSelect("BOMPROCESS");
            //return Ok(list);
            string Column = "ProcessId, ProcessCode, ProcessName";
            string Table = "Process";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-staff")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetStaff()
        {
            string Column = "StaffId, concat(s.StaffCode, ' - ', s.StaffName) as StaffName";
            string Table = "Staff s";
            string Where = "s.isActived = 1 and DeptCode = '00001'";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-mold/{WOProcessId}")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetMold(long? WOProcessId)
        {
            //string Column = "m.MoldId, concat(m.MoldCode, ' - ', m.MoldName) as MoldName";
            //string Table = "Mold m";
            //string Where = "m.isActived = 1  and m.MoldStatus = '001'  and m.MoldId not in (SELECT MoldId from WOProcessMold as a where a.MoldId = m.MoldId and getdate() BETWEEN a.StartDate and a.EndDate and a.isActived = 1)";
            //return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
            return Ok(await _WOService.GetSelectMold(WOProcessId));
        }

        [HttpGet("get-line")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetLine()
        {
            string Column = "LineId, LineName";
            string Table = "Line";
            string Where = "isActived = 1 and AreaCode = 'WORKSHOP'";
            return Ok(await _customService.GetForSelect<LineDto>(Column, Table, Where, ""));
        }

    }
}
