using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos
{
    public partial class SemiMMSDto : BaseModel
    {
        public long? WOSemiLotMMSId { get; set; }
        public string? SemiLotCode { get; set; }
        public long? WOProcessId { get; set; }
        public int? OriginQty   { get; set; }
        public int? ActualQty { get; set; }
        public string? LotStatus { get; set; }
        public long? LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public long? ProductId { get; set; }
        public long? ModelId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? Description { get; set; }
        public string? ModelName { get; set; }
        public string? LotStatusName { get; set; }
        public string? WOCode { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string? WorkOrder { get; set; }
        public string? BuyerQR { get; set; }
        public string? ProductTypeName { get; set; }
        public long? ProductType { get; set; }
        public string? ProcessCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? FactoryName { get; set; }

    }
}
