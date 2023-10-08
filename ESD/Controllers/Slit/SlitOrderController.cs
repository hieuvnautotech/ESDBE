using ESD.Services.Common;
using ESD.Services;
using ESD.Services.Slit;
using ESD.Services.Standard.Information;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.Slit;
using ESD.Models.Dtos;
using ESD.Services.WMS.Material;

namespace ESD.Controllers.Slit
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlitOrderController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly ISlitOrderService _slitOderService;
        private readonly IStaffService _staffService;
        private readonly IMaterialReceivingService _materialReceivingService;
        public SlitOrderController(IMaterialReceivingService materialReceivingService, ICustomService customService, IJwtService jwtService, ISlitOrderService slitOrderService, IStaffService staffService)
        {
            _jwtService = jwtService;
            _customService = customService;
            _slitOderService = slitOrderService;
            _staffService = staffService;
            _materialReceivingService = materialReceivingService;
        }
        #region Slitting Order Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetAll([FromQuery] SlitOrderDto model)
        {
            var returnData = await _slitOderService.GetAll(model);
            return Ok(returnData);
        }
        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_CREATE)]
        public async Task<IActionResult> Create([FromBody] SlitOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.SlitOrderId = AutoId.AutoGenerate();

            var result = await _slitOderService.Create(model);

            return Ok(result);
        }
        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_UPDATE)]
        public async Task<IActionResult> Update([FromBody] SlitOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _slitOderService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_DELETE)]
        public async Task<IActionResult> Delete([FromBody] SlitOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _slitOderService.Delete(model);

            return Ok(result);
        }
        #endregion

        #region Slitting Order Detail
        [HttpGet("get-slitOrder-detail")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetSlitOrderDetail([FromQuery] SlitOrderDetailDto model)
        {
            var returnData = await _slitOderService.GetSlitOrderDetail(model);
            return Ok(returnData);
        }

        [HttpPost("create-slitOrder-detail")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_CREATE)]
        public async Task<IActionResult> CreateSlitOrderDetail([FromBody] SlitOrderDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.SlitOrderDetailId = AutoId.AutoGenerate();

            var result = await _slitOderService.CreateSlitOrderDetail(model);

            return Ok(result);
        }

        [HttpPut("update-slitOrder-detail")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_UPDATE)]
        public async Task<IActionResult> UpdateSlitOrderDetail([FromBody] SlitOrderDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _slitOderService.ModifySlitOrderDetail(model);

            return Ok(result);
        }

        [HttpDelete("delete-slitOrder-detail")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_DELETE)]
        public async Task<IActionResult> DeleteSlitOrderDetail([FromBody] SlitOrderDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _slitOderService.DeleteSlitOrderDetail(model);

            return Ok(result);
        }

        #endregion

        #region Sliting Turn
        [HttpPost("slit")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_CREATE)]
        public async Task<IActionResult> Slit([FromBody] SlitTurnDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _slitOderService.Slit(model);

            return Ok(result);
        }

        [HttpGet("get-slit-turn-raw")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetSlitTurnRaw([FromQuery] SlitTurnDto model)
        {
            var returnData = await _slitOderService.GetSlitRaw(model);
            return Ok(returnData);
        }

        [HttpGet("get-slit-turn-raw-detail")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetSlitTurnRawDetail([FromQuery] SlitTurnDto model)
        {
            var returnData = await _slitOderService.GetSlitDetailRaw(model);
            return Ok(returnData);
        }

        [HttpGet("get-slit-turn")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetSlitTurn([FromQuery] SlitTurnDto model)
        {
            var returnData = await _slitOderService.GetSlit(model);
            return Ok(returnData);
        }

        [HttpGet("get-slit-turn-detail")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetSlitTurnDetail([FromQuery] SlitTurnDto model)
        {
            var returnData = await _slitOderService.GetSlitDetail(model);
            return Ok(returnData);
        }

        [HttpPost("finish-slit-turn")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_CREATE)]
        public async Task<IActionResult> FinishSlitTurn([FromBody] SlitTurnDto model)
        {
            var result = await _slitOderService.FinishSlitTurn(model);
            return Ok(result);
        }

        [HttpPost("reset-slit-turn")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_CREATE)]
        public async Task<IActionResult> ResetSlitTurn([FromBody] SlitTurnDto model)
        {
            var result = await _slitOderService.ResetSlitTurn(model);
            return Ok(result);
        }

        [HttpPost("edit-slit-turn")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_CREATE)]
        public async Task<IActionResult> EditSlitTurn([FromBody] SlitTurnDto model)
        {
            var result = await _slitOderService.EditSlitTurn(model);
            return Ok(result);
        }

        [HttpPost("delete-slit-turn")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_CREATE)]
        public async Task<IActionResult> DeleteSlitTurn([FromBody] SlitTurnDto model)
        {
            var result = await _slitOderService.DeleteSlitTurn(model);
            return Ok(result);
        }
        //check IQC
        [HttpGet("get-check/{QCIQCMasterId}/{MaterialLotId}")]
        public async Task<IActionResult> GetDetailMaterialL(long? QCIQCMasterId, long? MaterialLotId)
        {
            var returnData = await _materialReceivingService.GetDetailMaterial(QCIQCMasterId, MaterialLotId);
            return Ok(returnData);
        }
        [HttpPost("check")]
        public async Task<IActionResult> CreateFormMaterial([FromBody] CheckMaterialLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _slitOderService.CheckIQC(model);

            return Ok(result);
        }
        #endregion

        #region get for action slit
        [HttpGet("get-raw-material-list/{SlitOrderId}")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetRawMaterial(long? SlitOrderId)
        {
            return Ok(await _slitOderService.GetRawLotMaterial(SlitOrderId));
        }

        [HttpGet("get-product-slit")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetProductForSlit(long? SlitOrderId, long? MaterialId)
        {
            return Ok(await _slitOderService.GetProductForLot(SlitOrderId, MaterialId));
        }

        [HttpGet("get-worker-list")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetStaff()
        {
            string Column = "StaffId, concat(StaffCode, ' - ', StaffName) as StaffName";
            string Table = "Staff";
            string Where = "isActived = 1 and DeptCode ='00038' ";
            return Ok(await _customService.GetForSelect<StaffDto>(Column, Table, Where, ""));
        }

        [HttpGet("get-machine-list")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetMachine()
        {
            string Column = "LineId, LineName";
            string Table = "Line";
            string Where = "isActived = 1 and AreaCode = 'SLIT'";
            return Ok(await _customService.GetForSelect<LineDto>(Column, Table, Where, ""));
        }

        [HttpGet("get-blade-list")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetBlade()
        {
            string Column = "BladeId, BladeName";
            string Table = "Blade";
            string Where = "isActived = 1  and (BladeStatus = '001' OR BladeStatus = '002')";
            return Ok(await _customService.GetForSelect<BladeDto>(Column, Table, Where, ""));
        }

        [HttpGet("get-turn/{SlitOrderId}/{MaterialLotId}")]
        public async Task<IActionResult> GetTurn(long? SlitOrderId, long? MaterialLotId)
        {
            return Ok(await _slitOderService.GetTurn(SlitOrderId, MaterialLotId));
        }
        #endregion

        [HttpGet("get-product-list")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, concat(ProductCode,' - ', ProductName) as ProductCode, ProductCode as ProductCodeTemp, ProductName";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }

        [HttpGet("get-material-list")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetMaterial()
        {
            string Column = "MaterialId, MaterialCode";
            string Table = "Material";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<MaterialDto>(Column, Table, Where, ""));
        }

        [HttpGet("get-staff-check-slit")]
        [PermissionAuthorization(PermissionConst.SLITTINGORDER_READ)]
        public async Task<IActionResult> GetStaffCheckSlit()
        {
            string Column = "StaffId, concat(StaffCode, ' - ', StaffName) as StaffName";
            string Table = "Staff";
            string Where = "isActived = 1 and DeptCode ='00007' ";
            return Ok(await _customService.GetForSelect<StaffDto>(Column, Table, Where, ""));
        }

    }
}
