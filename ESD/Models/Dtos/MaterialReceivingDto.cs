using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos
{
    public class MaterialReceivingDto : BaseModel
    {
        public long? MaterialReceiveId { get; set; }
        public long? POId { get; set; }
        public long? MaterialId { get; set; }
        public long? MaterialLotId { get; set; }
        public string? MaterialType { get; set; }
        public string IQCCheck { get; set; } = string.Empty;
        public string IQCCheckName { get; set; } = string.Empty;
        public long? QCIQCMasterId { get; set; }
        public string LotNo { get; set; } = string.Empty;
        public string BundleCode { get; set; } = string.Empty;
        public string POOrderCode { get; set; } = string.Empty;
        public int? QuantityBundle { get; set; }
        public int? QuantityInBundle { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? ExportDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public string MaterialUnit { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CuttingDate { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }
        public int? TotalLength { get; set; }
        public int? TotalArea { get; set; }
        public string MaterialTypeName { get; set; } = string.Empty;
        public string QCIQCMasterName { get; set; } = string.Empty;

        public string MaterialLotCode { get; set; } = string.Empty;
        public string CutSlit { get; set; } = string.Empty;
        public int? BundleLength { get; set; }
        public int? BundleWidth { get; set; }
        public bool? LotCheckStatus { get; set; }
        public bool? Expiration { get; set; }
        public string? Type { get; set; }
    }
}
