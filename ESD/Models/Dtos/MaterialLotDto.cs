using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos
{
    public class MaterialLotDto : BaseModel
    {
        public long? MaterialLotId { get; set; }
        public long? MaterialReceiveId { get; set; }
        public long? WOProcessId { get; set; }
        public string MaterialLotCode { get; set; } = string.Empty;
        public string OriginMaterialLotCode { get; set; } = string.Empty;
        public long? MaterialId { get; set; }
        public long? LocationShelfId { get; set; }
        public string? LocationShelfCode { get; set; }
        public string? MaterialCode { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? ExportDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? LotStatus { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string AreaCode { get; set; } = string.Empty;
        public int? Length { get; set; }
        public int? Width { get; set; }
        public int? OriginLength { get; set; }
        public int? OriginWidth { get; set; }

        public string LotStatusName { get; set; } = string.Empty;
        public bool? IsHandCheck { get; set; }
        public string? MaterialType { get; set; }
        public string MaterialTypeName { get; set; } = string.Empty;
        public string POOrderCode { get; set; } = string.Empty;
        public int? StockQty { get; set; }
        public string? LotNo { get; set; }

        public int? MaterialWidth { get; set; }
        public int? MaterialLength { get; set; }
        public int? TotalLength { get; set; }
        public int? TotalArea { get; set; }
        public decimal? TotalStock { get; set; }
        // hold
        public string? FileName { get; set; }
        public string? Reason { get; set; }
        public bool? IsPicture { get; set; }

        // Check IQC
        public long? QCIQCMasterId { get; set; }
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public long? StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
    }
}
