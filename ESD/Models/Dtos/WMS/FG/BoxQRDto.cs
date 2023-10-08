using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.WMS.FG
{
    public class BoxQRDto : BaseModel
    {
        public long? BoxId { get; set; }
        public string BoxQR { get; set; } = string.Empty;
        public string LotNo { get; set; } = string.Empty;
        public string BuyerQR { get; set; } = string.Empty;
        public string BoxStatus { get; set; } = string.Empty;
        public string BoxStatusName { get; set; } = string.Empty;
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int? PackingAmount { get; set; }
        public long? BuyerQRId { get; set; }
        public string FirstBuyerQR { get; set; } = string.Empty;
        public DateTime? SearchDate { get; set; }
        public long? FGSOId { get; set; }
        public DateTime? BoxShipDate { get; set; }
        public long? locationId { get; set; }

    }
}
