using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.PO
{
    public class PODto : BaseModel
    {
        public long POId { get; set; }
        public string? POOrderCode { get; set; }
        public string? SupplierCode { get; set; }
        public DateTime PODATE { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialUnit { get; set; }
        public int Qty { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int QuantityRoll { get; set; }
        public int ActualQtyRoll { get; set; }
        public DateTime? ReceivedDate { get; set; }
    }
}
