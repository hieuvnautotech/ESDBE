using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class WorkOrderDto : BaseModel
    {
        public long WoId { get; set; }
        public string WoCode { get; set; }
        public bool? WOProcess { get; set; }
        public long? FPOId { get; set; }
        public long? MaterialId { get; set; }
        public long BomId { get; set; }
        public long? LineId { get; set; }
        public long? MoldId { get; set; }
        public int OrderQty { get; set; }
        public DateTime? StartDate { get; set; }
        public int? ActualQty { get; set; }
        public int? NGQty { get; set; }
        public int? HMIQty { get; set; }
        public long? HMIStatus { get; set; }

        public bool? isChecked { get; set; } //1 đủ liệu để tạo actual - 0 chưa đủ liệu 
        public bool? IsInputScan { get; set; } //1 cần scan liệu đầu vào - 0 không cần liệu đầu vào (dựa vào bom của Material)

        //Required Properties
        public int? Year { get; set; }
        public byte? Week { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialBuyerCode { get; set; }
        public string BomVersion { get; set; }
        public long? FPoMasterId { get; set; }
        public string? FPoMasterCode { get; set; }
        public string? LineName { get; set; }
        public DateTime? StartSearchingDate { get; set; }
        public DateTime? EndSearchingDate { get; set; }
        public decimal? TotalLotQty { get; set; }
        public long? QCMasterId { get; set; }
        public string? HMIStatusName { get; set; }
        public string? HMIRemark { get; set; }
        public string? MoldCode { get; set; }
        public int? Cabity { get; set; }
        public string? HMIMacAddress { get; set; }
        public WorkOrderDto()
        {
            WoCode = "";
            BomVersion = "";
        }
    }

    public class WorkOrderDisplayDto
    {
        public int totalOrderQty { get; set; }
        public int totalActualQty { get; set; }
        public int totalNGQty { get; set; }

        public int totalGoodQtyInjection { get; set; }
        public int totalNGQtyInjection { get; set; }
        public int totalGoodQtyAssy { get; set; }
        public int totalNGQtyAssy { get; set; }
        public decimal totalEfficiency { get; set; }
        public List<WorkOrderDto>? data { get; set; }
    }
}
