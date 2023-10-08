using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.WMS.Material
{
    public class ReturnMaterialLotDto : BaseModel
    {
        public long? RMId { get; set; }
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
        public long? LocationShelfId { get; set; }
        public string? LocationShelfCode { get; set; }
    }
}
