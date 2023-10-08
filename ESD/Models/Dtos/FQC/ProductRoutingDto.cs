using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.FQC
{
    public class ProductRoutingDto : BaseModel
    {
        public long RoutingId { get; set; }
        public long ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProcessCode { get; set; } = string.Empty;
        public int RoutingLevel { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ProcessName { get; set; } = string.Empty;
    }
}
