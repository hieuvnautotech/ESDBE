using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.MMS
{
    public class WOProcessLineDto : BaseModel
    {
        public long? WOProcessLineId { get; set; }
        public long? WOProcessId { get; set; }
        public long? LineId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string LineName { get; set; } = string.Empty;
    }
}
