using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class BladeDto : BaseModel
    {
        public long? BladeId { get; set; }
        public string? BladeName { get; set; }
        public string? BladeSize { get; set; }
        public long? SupplierId { get; set; }
        public string? SupplierCode { get; set; }
        public DateTime? ImportDate { get; set; }
        public long? QCMoldMasterId { get; set; }
        public int? CutMaxNumber { get; set; }
        public int? PeriodicCheck { get; set; }
        public int? CutCurrentNumber { get; set; }
        public string? BladeStatus { get; set; }
        public long? LineId { get; set; }
        public string? LineName { get; set; }
        public string? Description { get; set; }
        public string? SupplierName { get; set; }
        public string? QCMoldMasterName { get; set; }
        public string? BladeStatusName { get; set; }
        public int? CheckNo { get; set; }
        public int UsingNumber { get; set; }
        //check pqc
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public long? StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
    }
    public class BladeHistoryDto : BaseModel
    {
        public long? Id { get; set; }
        public long? BladeId { get; set; }
        public long? MaterialId { get; set; }
        public long? SlitOrderId { get; set; }
        public string ? SlitOrder { get; set; }
        public string? BladeName { get; set; }
        public string? BladeSerial { get; set; }
        public string? MaterialCode { get; set; }
        public int TotalLength { get; set; }
    } 
}
