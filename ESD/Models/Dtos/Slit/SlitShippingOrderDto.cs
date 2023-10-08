using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.Slit
{
    public class SlitShippingOrderDto : BaseModel
    {
        public long? SlitSOId { get; set; }
        public string SlitSOName { get; set; } = string.Empty;
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public bool? SlitSOStatus { get; set; } //0 watting  -  1 received
        public long? WOId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? DeliveryScanQty { get; set; }
        public bool? LotCheckStatus { get; set; } //0 watting  -  1 received
        public string CDLCode { get; set; } = string.Empty;
    }

    public class SlitShippingOrderDetailDto : BaseModel
    {
        public long? SlitSODetailId { get; set; }
        public long? SlitSOId { get; set; }
        public long? MaterialId { get; set; }
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public int? OrderQty { get; set; }
        public int? Width { get; set; }
        public int? LengthOrEA { get; set; }
        public int? DeliveryScanQty { get; set; }
        public int? WattingDeliveryQty { get; set; }
        public int? WattingReceivedQty { get; set; }
        public int? ReceivedQty { get; set; }
    }

    public class SlitShippingOrderLotDto : BaseModel
    {
        public long? SlitSODetailId { get; set; }
        public long? MaterialLotId { get; set; }
        public bool? LotStatus { get; set; }
        public string? MaterialLotCode { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public string? MaterialCode { get; set; }
        public string? LocationCode { get; set; }
        public string? Description { get; set; }
        public string? ProductCode { get; set; }
        public string? AreaCode { get; set; }
        public long? MMSLocationShelfId { get; set; }
        public long? SlitLocationShelfId { get; set; }
        public string? MMSLocationCode { get; set; }
        public string? SlitLocationCode { get; set; }
        public long? WOProcessId { get; set; }
    }
}
