using ESD.Services.Common;
using ESD.Services.Standard.Information;
using ESD.Services.WMS.Material;
using ESD.Services;
using Microsoft.AspNetCore.Mvc;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.WMS.Material;
using ESD.Models.Dtos.Common;

namespace ESD.Controllers.WMS.Material
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialShippingOrderController : ControllerBase
    {
        private readonly IMaterialShippingOrderService _MaterialShippingOrderService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MaterialShippingOrderController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, IMaterialShippingOrderService MaterialShippingOrderService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _MaterialShippingOrderService = MaterialShippingOrderService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }
        #region Master
        [HttpGet]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetAll([FromQuery] MaterialShippingOrderDto model)
        {
            var returnData = await _MaterialShippingOrderService.GetAll(model);
            return Ok(returnData);
        }

        [HttpPost("create")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_CREATE)]
        public async Task<IActionResult> Create([FromBody] MaterialShippingOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            model.MSOId = AutoId.AutoGenerate();

            var result = await _MaterialShippingOrderService.Create(model);

            return Ok(result);
        }

        [HttpPut("update")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_UPDATE)]
        public async Task<IActionResult> Update([FromBody] MaterialShippingOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _MaterialShippingOrderService.Modify(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_DELETE)]
        public async Task<IActionResult> Delete([FromBody] MaterialShippingOrderDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _MaterialShippingOrderService.Delete(model);

            return Ok(result);
        }
        #endregion

        #region Detail
        [HttpGet("detail")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetDetail([FromQuery] MaterialShippingOrderDetailDto model)
        {
            var returnData = await _MaterialShippingOrderService.GetDetail(model);
            return Ok(returnData);
        }

        [HttpGet("detail-history")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetDetailHistory([FromQuery] MaterialShippingOrderDetailDto model)
        {
            var returnData = await _MaterialShippingOrderService.GetDetailHistory(model);
            return Ok(returnData);
        }

        [HttpPost("create-detail/{MSOId}")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_CREATE)]
        public async Task<IActionResult> Create(long MSOId, [FromBody] List<MaterialShippingOrderDetailDto> model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            var createdBy = long.Parse(userId);

            var result = await _MaterialShippingOrderService.CreateDetail(model, MSOId, createdBy);

            return Ok(result);
        }

        [HttpDelete("delete-detail")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_DELETE)]
        public async Task<IActionResult> Delete([FromBody] MaterialShippingOrderDetailDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _MaterialShippingOrderService.DeleteDetail(model);

            return Ok(result);
        }
        #endregion

        #region Detail lot
        [HttpGet("get-detail-lot")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetDetailLot([FromQuery] MaterialShippingOrderLotDto model)
        {
            var returnData = await _MaterialShippingOrderService.GetDetailLot(model);
            return Ok(returnData);
        }

        [HttpGet("get-detail-lot-by-MSOId")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetDetailLotByMSOId(long MSOId)
        {
            var returnData = await _MaterialShippingOrderService.GetDetailLotByMSOId(MSOId);
            return Ok(returnData);
        }

        [HttpPost("scan-detail-lot")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_CREATE)]
        public async Task<IActionResult> CreateDetailLot([FromBody] MaterialShippingOrderLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _MaterialShippingOrderService.ScanLot(model);

            return Ok(result);
        }

        [HttpPost("scan-receiving-lot")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_CREATE)]
        public async Task<IActionResult> RecevingDetailLot([FromBody] MaterialShippingOrderLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);

            var result = await _MaterialShippingOrderService.ScanReceivingLot(model);

            return Ok(result);
        }

        [HttpDelete("delete-detail-lot")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_DELETE)]
        public async Task<IActionResult> DeleteDetailLot([FromBody] MaterialShippingOrderLotDto model)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            model.createdBy = long.Parse(userId);
            var result = await _MaterialShippingOrderService.DeleteLot(model);

            return Ok(result);
        }
        #endregion

        [HttpGet("get-material")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetMaterial([FromQuery] BaseModel model, string MaterialCode, string ProductCode, long MsoId)
        {
            var result = await _MaterialShippingOrderService.GetMaterial(model, MaterialCode, ProductCode, MsoId);
            return Ok(result);
        }

        [HttpGet("get-location-list")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetLocation()
        {
            var list = await _commonMasterService.GetForSelect("AREA");
            list.Data = list.Data.Where(x => x.commonDetailCode == "SLIT" || x.commonDetailCode == "WIP");
            return Ok(list);
        }

        [HttpGet("get-product-list")]
        [PermissionAuthorization(PermissionConst.MATERIALSO_READ)]
        public async Task<IActionResult> GetProduct()
        {
            string Column = "ProductId, concat(ProductCode,' - ', ProductName) as ProductCode, ProductCode as ProductCodeTemp, ProductName";
            string Table = "Product";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<dynamic>(Column, Table, Where, ""));
        }
    }
}
