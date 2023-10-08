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
    public class SelectOptionController : ControllerBase
    {
        private readonly IQCAPPService _QCAPPService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SelectOptionController(IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService, IQCAPPService QCAPPService, IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _QCAPPService = QCAPPService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("get-staff")]
        public async Task<IActionResult> GetStaff()
        {
            string Column = "StaffId, concat(StaffCode,' - ',StaffName) StaffName";
            string Table = "Staff";
            string Where = "isActived = 1";
            return Ok(await _customService.GetForSelect<StaffDto>(Column, Table, Where, ""));
        }
    }
}
