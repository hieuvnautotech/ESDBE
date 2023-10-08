using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models.Dtos.Common;
using ESD.Services;
using ESD.Services.Slit;
using ESD.Services.Standard.Information;
using Microsoft.AspNetCore.Mvc;

namespace ESD.Controllers.Standard.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class POController : ControllerBase
    {
        private readonly IPOService _POService;
        public POController(IPOService POService)
        {
            _POService = POService;
        }

        [HttpGet]
        [PermissionAuthorization(PermissionConst.PO_READ)]
        public async Task<IActionResult> Get([FromQuery] PageModel model,string? POOrderCode, DateTime? searchStartDay, DateTime? searchEndDay)
        {
            var res = await _POService.GetAll(model, POOrderCode, searchStartDay, searchEndDay);
            return Ok(res);
        }
    }
}
