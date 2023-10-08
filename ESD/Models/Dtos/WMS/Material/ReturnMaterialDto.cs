using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.WMS.Material
{
    public class ReturnMaterialDto : BaseModel
    {
        public long? RMId { get; set; }
        public string RMName { get; set; } = string.Empty;
        public bool? RMStatus { get; set; } //0 watting  -  1 received
        public long? LocationId { get; set; }
        public string AreaCode { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
        public string LocationCode { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool? LotCheckStatus { get; set; }
        public int? DeliveryScanQty { get; set; }
        public int? ReceivedQty { get; set; }
        public int? WattingDeliveryQty { get; set; }
        public int? WattingReceivedQty { get; set; }
    }
}
