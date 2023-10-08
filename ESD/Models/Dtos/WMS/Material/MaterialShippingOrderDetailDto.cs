using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.WMS.Material
{
    public class MaterialShippingOrderDetailDto : BaseModel
    {
        public long? MSODetailId { get; set; }
        public long? MSOId { get; set; }
        public long? MaterialId { get; set; }
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public int? OrderQty { get; set; }
        public int? LengthOrEA { get; set; }
        public int? Width { get; set; }
        public int? DeliveryScanQty { get; set; }
        public int? WattingDeliveryQty { get; set; }
        public int? WattingReceivedQty { get; set; }
        public int? ReceivedQty { get; set; }
    }
}
