using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos.Slitting
{
    public class BuyerQRDto : PageModel
    {
        public long? BuyerQRId { get; set; }
        public string BuyerQR { get; set; } = string.Empty;
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Packing { get; set; }
        public int PackingAmount { get; set; }
        public string VendorLine { get; set; } = string.Empty;
        public string LabelPrinter { get; set; } = string.Empty;
        public string IsSample { get; set; } = string.Empty;
        public int? PCN { get; set; }
        public int? LabelQty { get; set; }
        public string LotNo { get; set; } = string.Empty;
        public DateTime? LotNoDate { get; set; } = DateTime.Now;
        public string SerialNumber { get; set; } = string.Empty;
        public string MachineLine { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string SSVersion { get; set; } = string.Empty;
        public string RemarkBuyer { get; set; } = string.Empty;
        public DateTime? createdDate { get; set; } = DateTime.Now;
        public DateTime? ExpiryDate { get; set; }
        public long? createdBy { get; set; } = default;
        public string? createdName { get; set; } = default;
        public string QuantityFormat { get; set; } = string.Empty;
        public string Stamps { get; set; } = string.Empty;
        public bool? LotCheckStatus { get; set; }
    }
}
