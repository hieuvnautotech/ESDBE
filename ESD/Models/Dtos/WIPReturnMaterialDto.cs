using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos
{
    public class WIPReturnMaterialDto : BaseModel
    {
        public long? WIPRMId { get; set; }
        public string RMName { get; set; } = string.Empty;
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public bool? RMStatus { get; set; } //0 watting  -  1 received
        public long? LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool? LotCheckStatus { get; set; }
        public int? DeliveryScanQty { get; set; }
        public int? ReceivedQty { get; set; }
        public int? WattingDeliveryQty { get; set; }
        public int? WattingReceivedQty { get; set; }
        public long? MaterialLotId { get; set; }
    }
    public class WIPReturnMaterialLotDto : BaseModel
    {
        public long? WIPRMId { get; set; }
        public long? MaterialLotId { get; set; }
        public bool? LotStatus { get; set; }
        public string? MaterialLotCode { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public long? MaterialId { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialName { get; set; }
        public int? DeliveryScanQty { get; set; }
        public int? ReceivedQty { get; set; }
        public int? WattingReceivedQty { get; set; }
        public int? TotalLength { get; set; }
        public int? TotalWidth { get; set; }
        public DateTime? ReceivedDate { get; set; } = default;
    }
}
