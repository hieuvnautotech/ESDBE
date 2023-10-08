using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using ESD.CustomAttributes;
using ESD.Extensions;
using ESD.Models;
using ESD.Models.Dtos;
using ESD.Models.Dtos.Common;
using ESD.Services;
using ESD.Services.Common;
using ESD.Services.Common.Standard.Information;
using ESD.Services.Standard.Information;
using ESD.Services.WMS.Material;
using System.Diagnostics;

namespace ESD.Controllers.History.Information
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryStatusController : ControllerBase
    {
        private readonly IHistoryStatusService _HistoryStatusService;
        private readonly ICommonMasterService _commonMasterService;
        private readonly ISupplierService _supplierService;
        private readonly IJwtService _jwtService;
        private readonly ICustomService _customService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HistoryStatusController(IHistoryStatusService HistoryStatusService, IWebHostEnvironment webHostEnvironment, ICustomService customService, ISupplierService supplierService,  IJwtService jwtService, ICommonMasterService commonMasterService)
        {
            _HistoryStatusService = HistoryStatusService;
            _jwtService = jwtService;
            _commonMasterService = commonMasterService;
            _supplierService = supplierService;
            _customService = customService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("get-status-by-materialLotcode")]
        public async Task<IActionResult> getMaterialLotCode(string MaterialLotCode, string SemiLotCode)
        {
      
            var returnData1 = new ResponseModel<IEnumerable<HistoryStatusDto>?>();
            var returnData = new List<HistoryStatusDto>();
            var returnData2 = new HistoryStatusDto();
            if (!String.IsNullOrEmpty(MaterialLotCode))
            {
                MaterialLotCode = MaterialLotCode.Trim();
                var checkMaterialLot = await _HistoryStatusService.checkMaterialLot(MaterialLotCode);
                if (checkMaterialLot == null)
                {
                    return Ok(returnData);
                }
                if (checkMaterialLot.LotStatus == "009")
                {
                    returnData2.LotStatusName = "Đang sử dụng";
                }
                else
                {
                    returnData2.LotStatusName = await checktrangthai(checkMaterialLot.LotStatus);
                }
                returnData2.Length = checkMaterialLot.Length;
                 returnData2.LocationName = checkMaterialLot.LocationName;
                if (checkMaterialLot.WOProcessId != 0 && checkMaterialLot.WOProcessId != null)
                {
                    var listWOProcess = await _HistoryStatusService.GetWOProcess(checkMaterialLot.WOProcessId);
                    var listMaterialMappingMMS = await _HistoryStatusService.GetMaterialMappingByMaterialCode(checkMaterialLot.MaterialLotCode);
                    string resultSemi = String.Join(" ", listMaterialMappingMMS.Select(x=>x.SemiLotCode));
                    string resultTime = String.Join(" ", listMaterialMappingMMS.Select(x=>x.createdDate));
                    returnData2.WOCode = listWOProcess.WOCode;
                    returnData2.ProcessCode = listWOProcess.ProcessCode;
                    returnData2.ProductCode = listWOProcess.ProductCode;
                    returnData2.ProcessLevel = listWOProcess.ProcessLevel;
                    returnData2.SemiLotCode = resultSemi;
                    returnData2.CreateDateMapping = resultTime;
                  
                }
                else
                {
                    returnData2.Result = "Vẫn chưa được mapping";
                }
                returnData.Add(returnData2);
            }
            if (!String.IsNullOrEmpty(SemiLotCode))
            {
                SemiLotCode = SemiLotCode.Trim();
                var checkSemiLotCodeMMS = await _HistoryStatusService.checkSemiLotMMS(SemiLotCode);
                if (checkSemiLotCodeMMS != null)
                {
                    if (checkSemiLotCodeMMS == null)
                    {
                        return Ok(returnData);
                    }
                    if (checkSemiLotCodeMMS.LotStatus == "009")
                    {
                        returnData2.LotStatusName = "Đang sử dụng";
                    }
                    else
                    {
                        returnData2.LotStatusName = await checktrangthai(checkSemiLotCodeMMS.LotStatus);
                    }
                    returnData2.Length = checkSemiLotCodeMMS.ActualQty;
                    returnData2.LocationName =checkSemiLotCodeMMS.LocationName;
                    if (checkSemiLotCodeMMS.WOProcessId != 0 && checkSemiLotCodeMMS.WOProcessId != null)
                    {
                        var listWOProcess = await _HistoryStatusService.GetWOProcess(checkSemiLotCodeMMS.WOProcessId);
                        var listMaterialMappingMMS = await _HistoryStatusService.GetMaterialMappingByMaterialCode(checkSemiLotCodeMMS.SemiLotCode);
                        var listMaterialMappingTims = await _HistoryStatusService.GetSemiLotMappingByMaterialCode(checkSemiLotCodeMMS.SemiLotCode);

                        returnData2.WOCode = listWOProcess.WOCode;
                        returnData2.ProcessCode = listWOProcess.ProcessCode;
                        returnData2.ProcessLevel = listWOProcess.ProcessLevel;
                        returnData2.ProductCode = listWOProcess.ProductCode;

                        if (checkSemiLotCodeMMS != null)
                        {
                            returnData2.SemiLotCode = String.Join(" <br/>", listMaterialMappingMMS.Select(x => x.SemiLotCode));
                            returnData2.CreateDateMapping = String.Join("<br/> ", listMaterialMappingMMS.Select(x => x.createdDate));
                            returnData2.LocationName = checkSemiLotCodeMMS.LocationName ;
                        }
                        else
                        {
                            returnData2.SemiLotCode = String.Join(" <br/>", listMaterialMappingTims.Select(x => x.SemiLotCode));
                            returnData2.CreateDateMapping = String.Join("<br/> ", listMaterialMappingTims.Select(x => x.createdDate));
                        }
                    }
                    else
                    {
                        returnData2.Result = "Vẫn chưa được mapping";
                    }
                    returnData.Add(returnData2);
                    returnData1.Data = returnData;
                    return Ok(returnData1);
                }
                var checkSemiLotCodeAPP = await _HistoryStatusService.checkSemiLotAPP(SemiLotCode);
                if (checkSemiLotCodeAPP != null)
                {
                    returnData2.LotStatusName = await checktrangthai(checkSemiLotCodeAPP.LotStatus);

                    returnData2.AreaCode = checkSemiLotCodeAPP.AreaCode;

                    if (checkSemiLotCodeAPP.WOProcessId != 0 && checkSemiLotCodeAPP.WOProcessId != null)
                    {
                        var listWOProcess = await _HistoryStatusService.GetWOProcess(checkSemiLotCodeAPP.WOProcessId);
                        var listMaterialMappingTims = await _HistoryStatusService.GetMaterialMappingByMaterialCode(checkSemiLotCodeAPP.SemiLotCode);

                        returnData2.SemiLotCode = String.Join(",", listMaterialMappingTims.Select(x => x.SemiLotCode));
                        returnData2.CreateDateMapping = String.Join(",", listMaterialMappingTims.Select(x => x.createdDate));

                        returnData2.Length = checkSemiLotCodeAPP.ActualQty;
                        returnData2.WOCode = listWOProcess.WOCode;
                        returnData2.ProcessCode = listWOProcess.ProcessCode;
                        returnData2.ProcessLevel = listWOProcess.ProcessLevel;
                        returnData2.ProductCode = listWOProcess.ProductCode;
                    }
                    else
                    {
                        returnData2.Result = "Vẫn chưa được mapping";
                    }
                    returnData.Add(returnData2);
                    returnData1.Data = returnData;
                    return Ok(returnData1);
                }
                returnData.Add(returnData2);
            }
            returnData1.Data = returnData;
            return Ok(returnData1);

        }
        [HttpGet]
        public async Task<string> checktrangthai(string mt_sts_cd)
        {
            var status = await _HistoryStatusService.GetDetailNameByDetailCode(mt_sts_cd);
            return status;
        }
    }
}
