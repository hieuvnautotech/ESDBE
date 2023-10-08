using ESD.Services.Common.Standard.Information;
using ESD.Services.Common;
using ESD.Services.FQC;
using ESD.Services.MMS;
using ESD.Services.Standard.Information;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;
using ESD.Models.Dtos.MMS;
using ESD.Extensions;
using ESD.CustomAttributes;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.FQC;
using ESD.Models.Dtos;

namespace ESD.Controllers.FQC
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActualController : ControllerBase
    {
        private readonly IWOService _WOService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly IStaffService _staffService;
        private readonly IMoldService _MoldService;
        private readonly IActualService _ActualService;

        public ActualController(IActualService ActualService, IStaffService staffService, IWOService WOService, IJwtService jwtService, ICustomService customService, ICommonMasterService commonMasterService, IMoldService moldService)
        {
            _WOService = WOService;
            _jwtService = jwtService;
            _customService = customService;
            _commonMasterService = commonMasterService;
            _staffService = staffService;
            _MoldService = moldService;
            _ActualService = ActualService;
        }
        #region WO
        [HttpGet]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetAll([FromQuery] WODto model)
        {
            var returnData = await _WOService.GetAll(model);
            return Ok(returnData);
        }
        #endregion

        #region Process
        [HttpGet("get-Process")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetProcess([FromQuery] WOProcessDto model)
        {
            var returnData = await _ActualService.GetProcessFQC(model);
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

            var result = await _ActualService.CreateProcess(model);

            return Ok(result);
        }
        [HttpDelete("delete-Process-fqc")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteProcessApp([FromBody] WOProcessDto model)
        {
            var returnData = new ResponseModel<WOProcessDto?>();
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);


            //Xóa từ level cao -> thấp
            var CheckProcess = await _ActualService.GetProcessById(model.WOProcessId);
            if (CheckProcess == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            var ListProcess = await _ActualService.GetListProcess(CheckProcess.WOId);
            if (ListProcess.Data.Count() > 0)
            {
                var level = ListProcess.Data.OrderByDescending(x => x.ProcessLevel).Select(x => x.ProcessLevel).FirstOrDefault();
                if (level == CheckProcess.ProcessLevel)
                {
                    var result = await _ActualService.DeleteProcessFQC(model);
                    return Ok(result);
                }
                else
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "semiFqc.Error_Delete_Max_Level";
                    return Ok(returnData);
                }
            }
            return Ok(returnData);
        }
        #endregion

        #region ProcessStaff
        [HttpGet("get-Process-staff")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetProcessStaff([FromQuery] WOProcessStaffDto model)
        {
            var returnData = await _ActualService.GetProcessStaff(model);
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

            var result = await _ActualService.CreateProcessStaff(model);

            return Ok(result);
        }

        [HttpPut("update-Process-staff")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> UpdateProcessStaff([FromBody] WOProcessStaffDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _ActualService.ModifyProcessStaff(model);

            return Ok(result);
        }

        [HttpDelete("delete-Process-staff")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteProcessStaff([FromBody] WOProcessStaffDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _ActualService.DeleteProcessStaff(model);

            return Ok(result);
        }
        #endregion

        #region Semi lot
        [HttpGet("get-semi-lot")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWOSemiLot([FromQuery] WOSemiLotMMSDto model)
        {
            var returnData = await _ActualService.GetWOSemoLot(model);
            return Ok(returnData);
        }
        #endregion

        #region WO SemiLot FQC
        [HttpGet("get-wosemilot-fqc")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWoSemiLotFQC([FromQuery] WOSemiLotFQCDto model)
        {
            var returnData = await _ActualService.GetWoSemiLotFQC(model);
            return Ok(returnData);
        }
        [HttpPost("create-semi-lot")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateSemiLot([FromBody] WOSemiLotFQCDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCQCDto?>();
            bool CheckShiftOfStaff = await _ActualService.CheckShiftOfStaff(model.WOProcessStaffId);
            if (!CheckShiftOfStaff)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.WorkerOutShift";
                return Ok(returnData);
            }
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOSemiLotFQCId = AutoId.AutoGenerate();

            var result = await _ActualService.CreateSemiLot(model);

            return Ok(result);
        }

        [HttpDelete("delete-semi-lot")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteSemiLot([FromBody] WOSemiLotFQCDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var returnData = new ResponseModel<WOSemiLotFQCDto?>();

            if (model.OriginQty > 0 || model.ActualQty > 0)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "semiFqc.QCChecked";
                return Ok(returnData);
            }

            var result = await _ActualService.DeleteSemiLot(model);

            return Ok(result);
        }

        [HttpGet("get-print/{WOSemiLotFQCId}")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetPrint(long? WOSemiLotFQCId)
        {
            var returnData = await _ActualService.GetByIdWOSemiLotFQC(WOSemiLotFQCId);
            return Ok(returnData);

        }
        #endregion

        #region WO SemiLot FQC Detail
        [HttpGet("get-wosemilot-fqc-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWoSemiLotFQCDetail([FromQuery] WOSemiLotFQCDetailDto model)
        {
            var returnData = await _ActualService.GetWoSemiLotFQCDetail(model);
            return Ok(returnData);
        }

        [HttpGet("get-wo-materialLot")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWomaterialLot([FromQuery] MaterialLotDto model)
        {
            var returnData = await _ActualService.GetWomaterialLot(model);
            return Ok(returnData);
        }

        [HttpPost("create-semi-lot-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateSemiLotDetail([FromBody] WOSemiLotFQCDetailDto model)
        {
            //MaterialLotCode Input
            //WOSemiLotFQCId Ouput
            var returnData = new ResponseModel<WOSemiLotFQCDetailDto?>();
            //check Ouput
            var CheckSemilotFQC = await _ActualService.GetSemiLotCodeById(model.WOSemiLotFQCId);
            if (CheckSemilotFQC == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            bool CheckShiftOfStaff = await _ActualService.CheckShiftOfStaff(CheckSemilotFQC.WOProcessStaffId);
            if (!CheckShiftOfStaff)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.WorkerOutShift";
                return Ok(returnData);
            }
            var listProcess = await _ActualService.GetProcess(CheckSemilotFQC.WOProcessId);
            //check lot
            var CheckLotMMS = await _WOService.GetSemiLotCodeByCode(model.MaterialLotCode);
            var CheckLotFQC = await _ActualService.GetSemiLotCode(model.MaterialLotCode);
            if (CheckLotMMS == null && CheckLotFQC == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            //bool MaxSemiLot = await _ActualService.CheckMaxSemilot(CheckSemilotFQC.WOSemiLotFQCId, CheckSemilotFQC.WOProcessId);
            //if (MaxSemiLot == false)
            //{
            //    returnData.HttpResponseCode = 204;
            //    returnData.ResponseMessage = "WO.SemiNotMax";
            //    return Ok(returnData);
            //}

            if (CheckLotMMS != null)
            {
                if (!CheckLotMMS.LotStatus.Equals("010"))
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "semiFqc.Error_Allow_WaittingTest";
                    return Ok(returnData);
                }
                if (CheckLotMMS.WOProcessId == CheckSemilotFQC.WOProcessId)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "semiFqc.Error_SameProcess";
                    return Ok(returnData);
                }
                if (CheckLotMMS.ActualQty == 0)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "semiFqc.Error_QuantityIs0";
                    return Ok(returnData);
                }
                var Minlevel = await _ActualService.NameMinBTPMMS(listProcess.WOId);
                // chỉ có level 1 MỚI ĐC MMS
                if (listProcess.ProcessLevel != Minlevel.ProcessLevel)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "semiFqc.Error_WrongProcess";
                    return Ok(returnData);
                }
                // LẤY TÊN BTP CỦA CÔNG ĐOẠN TRƯỚC NÓ
                //var TenBTP = await _ActualService.NameBTPMMS(listProcess.WOId);
                //if (TenBTP != null)
                //{
                //    var mt_no = TenBTP.Product + "-" + TenBTP.NameProcess;
                //    if (CheckLotMMS.SemiLotNo != mt_no)
                //    {
                //        returnData.HttpResponseCode = 204;
                //        returnData.ResponseMessage = "semiFqc.Error_WrongProcess";
                //        return Ok(returnData);
                //    }
                //}
            }
            else
            {
                if (CheckLotFQC != null)
                {
                    if (!CheckLotFQC.LotStatus.Equals("009"))
                    {
                        returnData.HttpResponseCode = 204;
                        returnData.ResponseMessage = "semiFqc.Error_Allow_WaittingTest";
                        return Ok(returnData);
                    }
                    if (CheckLotFQC.WOProcessId == CheckSemilotFQC.WOProcessId)
                    {
                        returnData.HttpResponseCode = 204;
                        returnData.ResponseMessage = "semiFqc.Error_SameProcess";
                        return Ok(returnData);
                    }
                    if (CheckLotFQC.ActualQty == 0)
                    {
                        returnData.HttpResponseCode = 204;
                        returnData.ResponseMessage = "semiFqc.Error_QuantityIs0";
                        return Ok(returnData);
                    }

                    //LẤY TÊN BTP CỦA CÔNG ĐOẠN TRƯỚC NÓ
                    var TenBTP = await _ActualService.NameBTPFQC(listProcess.WOId, (int)listProcess.ProcessLevel);
                    if (TenBTP != null)
                    {
                        var mt_no = TenBTP.Product + "-" + TenBTP.NameProcess;
                        if (CheckLotFQC.SemiLotNo != mt_no)
                        {
                            returnData.HttpResponseCode = 204;
                            returnData.ResponseMessage = "semiFqc.Error_WrongProcess";
                            return Ok(returnData);
                        }
                    }
                    //kiểm tra đầu vào sản lượng có đúng quy cách đăng kí bên product ko
                    //var getProduct = await _ActualService.GetProduct(CheckSemilotFQC.ProductId);
                    //if (getProduct.PackingAmount != (decimal?)CheckLotFQC.ActualQty)
                    //{
                    //    returnData.HttpResponseCode = 204;
                    //    returnData.ResponseMessage = "semiFqc.Error_Qty_Incorrect";
                    //    return Ok(returnData);
                    //}
                }
            }
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOSemiLotDetailId = AutoId.AutoGenerate();

            var result = await _ActualService.CreateSemiLotDetail(model);

            return Ok(result);
        }

        [HttpDelete("delete-semi-lot-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteSemiLot([FromBody] WOSemiLotFQCDetailDto model)
        {
            //OUTPUT:SEMILOT
            //INPUT MATERIALlOTcODE
            var returnData = new ResponseModel<WOSemiLotFQCDetailDto?>();


            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);


            //KIỂM TRA MÃ ĐẦU RA TỒN TẠI KHÔNG
            var CheckLotFQC = await _ActualService.GetSemiLotCode(model.SemiLotCode);// ĐẦU RA
            if (CheckLotFQC == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            //KIỂM TRA MÃ ĐẦU RA ĐÃ QUA CÔNG ĐOẠN KHAC CHƯA
            bool CheckSemiLotMapping = await _ActualService.CheckSemiLotMapping(CheckLotFQC.SemiLotCode);
            if (CheckSemiLotMapping == true)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MappingToAnotherProcess";
                return Ok(returnData);
            }
            //KIỂM TRA CÔNG NHÂN TẠI CÔNG ĐOẠN NÀY CÒN THỜI GIAN LÀM VIỆC KHÔNG
            bool CheckShiftOfStaff = await _ActualService.CheckShiftOfStaff(CheckLotFQC.WOProcessStaffId);
            if (!CheckShiftOfStaff)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.WorkerOutShift";
                return Ok(returnData);
            }
            // ĐÂY LÀ HÀNG BÙ
            var CheckLotFQCHB = await _ActualService.GetSemiLotCode(model.MaterialLotCode);// ĐẦU RA

            if (CheckLotFQCHB != null && CheckLotFQCHB.Description == "Hangbu")
            {
                //kiểm tra mã đầu ra gr_qty = 0 và khác trạng thái 009 thì không cho cancel
                if (CheckLotFQC.ActualQty == 0)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "semiFqc.Error_QuantityIs0";
                    return Ok(returnData);
                }
                if (CheckLotFQC.LotStatus != "009")
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "WO.AllowStatusStock";
                    return Ok(returnData);
                }
                //update cuộn cha đầu vào
                //tìm cuộn cha của hàng bù
                var Cuon_cha_hang_bu = await _ActualService.GetSemiLotCode(CheckLotFQCHB.OriginSemiLotCode);
                if (Cuon_cha_hang_bu == null)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "Mã gốc của hàng bù không tìm thấy.";
                    return Ok(returnData);
                }
                //if (Cuon_cha_hang_bu.LotStatus != "009")
                //{
                //    returnData.HttpResponseCode = 204;
                //    returnData.ResponseMessage = "WO.AllowStatusStock";
                //    return Ok(returnData);
                //}

                var resultHB = await _ActualService.DeleteSemiLotDetailHB(model);

                return Ok(resultHB);
            }


            var result = await _ActualService.DeleteSemiLotDetail(model);

            return Ok(result);
        }
        [HttpPut("stopInheritance-semi-lot-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_UPDATE)]
        public async Task<IActionResult> StopInheritanceSemiLotDetail([FromBody] WOSemiLotFQCDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _ActualService.StopInheritanceSemiLotDetail(model);

            return Ok(result);
        }
        #endregion

        #region SemiLot FQC Detail Check QC
        [HttpGet("get-wosemilot-fqc-detail-qc")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWoSemiLotFQCDetailQC([FromQuery] WOSemiLotFQCQCDto model)
        {
            var returnData = await _ActualService.GetWoSemiLotFQCQC(model);
            return Ok(returnData);
        }

        [HttpPost("create-semi-lot-detail-qc")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateSemiLotDetailQC([FromBody] WOSemiLotFQCQCDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCQCDto?>();

            var GetSemiLotCode = await _ActualService.GetSemiLotCode(model.SemiLotCode);
            if (GetSemiLotCode == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            bool CheckShiftOfStaff = await _ActualService.CheckShiftOfStaff(GetSemiLotCode.WOProcessStaffId);
            if (!CheckShiftOfStaff)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.WorkerOutShift";
                return Ok(returnData);
            }
            var ProcessLast = await _ActualService.ProcessMax(GetSemiLotCode.WOId);//công đoạn cuối cùng của wo
            var getLevelSemilotDaura = await _ActualService.GetProcess(GetSemiLotCode.WOProcessId);//lấy level của cuộn đầu ra
            if (ProcessLast.ProcessLevel == getLevelSemilotDaura.ProcessLevel)
            {
                //kiểm tra đầu vào sản lượng có đúng quy cách đăng kí bên product ko
                var getProduct = await _ActualService.GetProduct(GetSemiLotCode.ProductCode);
                if (getProduct.PackingAmount < (decimal?)model.OKQty)
                {
                    returnData.HttpResponseCode = 204;
                    returnData.ResponseMessage = "semiFqc.Error_Qty_Incorrect";
                    return Ok(returnData);
                }
            }

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.WOSemiLotFQCQCId = AutoId.AutoGenerate();

            var result = await _ActualService.CreateSemiLotFQCQC(model);

            return Ok(result);
        }
        [HttpDelete("delete-wosemilot-detail-qc")]
        [PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        public async Task<IActionResult> DeleteSemiLotQC([FromBody] WOSemiLotFQCQCDto model)
        {
            var returnData = new ResponseModel<WOSemiLotFQCQCDto?>();

            var GetSemiLotCode = await _ActualService.GetSemiLotCode(model.SemiLotCode);
            if (GetSemiLotCode == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            bool CheckShiftOfStaff = await _ActualService.CheckShiftOfStaff(GetSemiLotCode.WOProcessStaffId);
            if (!CheckShiftOfStaff)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.WorkerOutShift";
                return Ok(returnData);
            }
            bool CheckSemiLotMapping = await _ActualService.CheckSemiLotMapping(GetSemiLotCode.SemiLotCode);
            if (CheckSemiLotMapping == true)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.MappingToAnotherProcess";
                return Ok(returnData);
            }

            if (!(GetSemiLotCode.LotStatus.Equals("009")))
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "semiFqc.Output_other_process";
                return Ok(returnData);
            }
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);



            var result = await _ActualService.DeleteSemiLotFQCQC(model);

            return Ok(result);
        }
        [HttpGet("get-wosemilot-qc-list-detail")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWoSemiLotFQCListDetailQC([FromQuery] QCAPPDetailDto model)
        {
            var returnData = await _ActualService.GetWoSemiLotFQCListDetailQC(model);
            return Ok(returnData);
        }
        #endregion

        //#region OQC
        //[HttpGet("get-wait-semilot-oqc-list")]
        //[PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        //public async Task<IActionResult> GetWaitSemiLotOQC([FromQuery] WOSemiLotFQCDto model)
        //{
        //    var returnData = await _ActualService.GetWaitSemiLotOQC(model);
        //    return Ok(returnData);
        //}
        //[HttpPost("create-semi-lot-wait-oqc")]
        //[PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        //public async Task<IActionResult> createSemiLotWaitOQC([FromBody] WOSemiLotFQCDto model)
        //{
        //    //model.SemiLotCode: đầu vào
        //    var returnData = new ResponseModel<WOSemiLotFQCQCDto?>();

        //    var GetSemiLotCode = await _ActualService.GetSemiLotCode(model.SemiLotCode);
        //    if (GetSemiLotCode == null)
        //    {
        //        returnData.HttpResponseCode = 204;
        //        returnData.ResponseMessage = StaticReturnValue.NO_DATA;
        //        return Ok(returnData);
        //    }

        //    bool CheckShiftOfStaff = await _ActualService.CheckShiftOfStaff(model.WOProcessStaffOQCId);
        //    if (!CheckShiftOfStaff)
        //    {
        //        returnData.HttpResponseCode = 204;
        //        returnData.ResponseMessage = "WO.WorkerOutShift";
        //        return Ok(returnData);
        //    }

        //    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //    var userId = _jwtService.ValidateToken(token);
        //    model.createdBy = long.Parse(userId);
        //    model.SemiLotCode = model.SemiLotCode;
        //    model.WOProcessOQCId = model.WOProcessId;
        //    model.WOProcessStaffId = model.WOProcessStaffId;
        //    model.WOSemiLotFQCId = GetSemiLotCode.WOSemiLotFQCId;



        //    var result = await _ActualService.CreateSemiLotWaitOQC(model);

        //    return Ok(result);
        //}
        //[HttpGet("get-semilot-oqc-list")]
        //[PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        //public async Task<IActionResult> GetSemiLotOQCList([FromQuery] WOSemiLotFQCDto model)
        //{
        //    var returnData = await _ActualService.GetSemiLotOQCList(model);
        //    return Ok(returnData);
        //}
        //[HttpDelete("delete-semi-lot-oqc")]
        //[PermissionAuthorization(PermissionConst.WORKORDER_DELETE)]
        //public async Task<IActionResult> DeleteSemiLotWaitOQC([FromBody] WOSemiLotFQCDto model)
        //{
        //    var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //    var userId = _jwtService.ValidateToken(token);
        //    model.createdBy = long.Parse(userId);
        //    var result = await _ActualService.DeleteSemiLotWaitOQC(model);

        //    return Ok(result);
        //}
        //#endregion

        //#region Check OQC
        //[HttpGet("get-check-oqc")]
        //[PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        //public async Task<IActionResult> GetCheckOQC([FromQuery] WOSemiLotFQCDto model)
        //{
        //    var returnData = await _ActualService.GetCheckOQC(model);
        //    return Ok(returnData);
        //}

        //[HttpPost("check-oqc")]
        //[PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        //public async Task<IActionResult> CheckOQC([FromBody] WOSemiLotFQCOQCMasterDto model)
        //{
        //    var result = await _ActualService.CheckOQC(model);
        //    return Ok(result);
        //}
        //#endregion

        #region Add Replacment
        [HttpGet("get-semi-lot-replacment")]
        [PermissionAuthorization(PermissionConst.WORKORDER_READ)]
        public async Task<IActionResult> GetWOSemiLotReplacement([FromQuery] WOSemiLotFQCDto model)
        {
            var returnData = await _ActualService.GetWOSemiLotReplacement(model);
            return Ok(returnData);
        }
        [HttpPost("create-semi-lot-replacement")]
        [PermissionAuthorization(PermissionConst.WORKORDER_CREATE)]
        public async Task<IActionResult> CreateSemilotReplacement([FromBody] WOSemiLotFQCDetailDto model)
        {
            //Semilot Input
            //WOSemiLotFQCIdOutput Ouput
            var returnData = new ResponseModel<WOSemiLotFQCDetailDto?>();
            //CHECK OUTPUT
            var CheckSemilotFQC = await _ActualService.GetSemiLotCodeById(model.WOSemiLotFQCIdOutput);
            if (CheckSemilotFQC == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            bool CheckShiftOfStaff = await _ActualService.CheckShiftOfStaff(CheckSemilotFQC.WOProcessStaffId);
            if (!CheckShiftOfStaff)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "WO.WorkerOutShift";
                return Ok(returnData);
            }
            if (!CheckSemilotFQC.LotStatus.Equals("009"))
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "semiFqc.Error_Allow_WaittingTest";
                return Ok(returnData);
            }
            //CHECK Input
            var CheckSemilotFQCINPUT = await _ActualService.GetSemiLotCode(model.SemiLotCode);
            if (CheckSemilotFQCINPUT == null)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = StaticReturnValue.NO_DATA;
                return Ok(returnData);
            }
            if (!CheckSemilotFQCINPUT.LotStatus.Equals("009"))
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "semiFqc.Error_Allow_WaittingTest";
                return Ok(returnData);
            }
            //kiểm tra đầu vào hàng bù đã trả qua hết công đoạn chưa
            var txlot = await _ActualService.GetWOProcessWorked(CheckSemilotFQCINPUT.SemiLotCode, CheckSemilotFQCINPUT.WOId);

            if (txlot.Data.Count() > 0)
            {
                var html = "semiFqc.Error_NotEnoughtProcess| " + string.Join(" ->", txlot.Data.Select(x => x.ProcessCode));
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = html;
                return Ok(returnData);
            }

            // kiểm tra đầu ra đầu vào có cùng product không
            if (CheckSemilotFQC.ProductCode != CheckSemilotFQCINPUT.ProductCode)
            {
                returnData.HttpResponseCode = 204;
                returnData.ResponseMessage = "semiFqc.Error_Diff_Product";
                return Ok(returnData);
            }


            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);


            model.createdBy = long.Parse(userId);
            model.WOSemiLotDetailId = AutoId.AutoGenerate();
            model.WOSemiLotFQCId = model.WOSemiLotFQCIdOutput;
            model.MaterialLotCode = CheckSemilotFQCINPUT.SemiLotCode;
            model.AddQty = model.AddQty;

            var result = await _ActualService.CreateWOSemiLotReplacement(model);

            return Ok(result);
        }
        #endregion

        [HttpGet("get-product")]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId,ProductCode";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-bom/{ProductId}")]
        public async Task<IActionResult> GetBom(long? ProductId)
        {
            string Column = "BomId, SpecNo";
            string Table = "Bom";
            string Where = "isActived = 1 and ProductId = " + ProductId;
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-line")]
        public async Task<IActionResult> GetLine()
        {
            string Column = "LineId, LineCode";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-staff")]
        public async Task<IActionResult> GetStaff()
        {
            string Column = "StaffId, StaffName";
            string Table = "Staff";
            string Where = "isActived = 1 and DeptCode ='00001'";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
            //return Ok(await _staffService.GetForSelect());
        }

        [HttpGet("get-mold")]
        public async Task<IActionResult> GetMold()
        {
            string Column = "MoldId, concat(MoldCode, ' - ', MoldName) as MoldCode";
            string Table = "Mold";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
            //return Ok(await _MoldService.GetForSelect());
        }
    }
}
