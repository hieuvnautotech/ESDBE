using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.WMS.FG
{
    public class FGShippingOrderDto : BaseModel
    {
        public long? FGSOId { get; set; }
        public string FGSOCode { get; set; } = string.Empty;
        public string BuyerCode { get; set; } = string.Empty;
        public string FGSONumber { get; set; } = string.Empty;
        public long? BuyerId { get; set; }
        public string BuyerQR { get; set; } = string.Empty;
        public DateTime? FGSODate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string UnitCode { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public decimal? PackingAmount { get; set; }
    }
}
