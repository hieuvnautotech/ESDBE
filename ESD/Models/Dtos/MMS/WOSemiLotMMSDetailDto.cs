using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.MMS
{
    public class WOSemiLotMMSDetailDto : BaseModel
    {
        public long? WOSemiLotDetailId { get; set; }
        public long? WOSemiLotMMSId { get; set; }
        public long? WOProcessId { get; set; }
        public string SemiLotCode { get; set; } = string.Empty;
        public string MaterialLotCode { get; set; } = string.Empty;
        public bool? IsFinish { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? MappingDate { get; set; } = default;
        public string MaterialType { get; set; } = string.Empty;

        public int? OriginQty { get; set; }
        public int? ActualQty { get; set; }
        public int? UsedQty { get; set; }
        public int? RemainQty { get; set; }
    }
}
