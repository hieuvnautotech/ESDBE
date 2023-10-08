using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.WMS.Material
{
    public class MaterialShippingOrderDto : BaseModel
    {
        public long? MSOId { get; set; }
        public string MSOName { get; set; } = string.Empty;
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public bool? MSOStatus { get; set; } //0 watting  -  1 received
        public long? LocationId { get; set; }
        public string AreaName { get; set; } = string.Empty;
        public string AreaCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? DeliveryScanQty { get; set; }
        public int? ReceivedQty { get; set; }
        public int? WattingDeliveryQty { get; set; }
        public int? WattingReceivedQty { get; set; }
        public bool? LotCheckStatus { get; set; } //0 watting  -  1 received
    }
}
