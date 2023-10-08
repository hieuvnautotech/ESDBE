using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.MMS
{
    public class WOPressLotMMSDto : BaseModel
    {
        public long? WOSemiLotMMSId { get; set; }
        public long? WOProcessId { get; set; }
        public string SemiLotCode { get; set; } = string.Empty;
        public string PressLotCode { get; set; } = string.Empty;
        public string ModelCode { get; set; } = string.Empty;
        public int? OriginQty { get; set; }
        public int? ActualQty { get; set; }
        public string Serial { get; set; } = string.Empty;
        public List<long>? ListId { get; set; }

    }
}
