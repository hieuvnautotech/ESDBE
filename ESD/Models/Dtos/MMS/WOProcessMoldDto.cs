using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.MMS
{
    public class WOProcessMoldDto : BaseModel
    {
        public long? WOProcessMoldId { get; set; }
        public long? WOProcessId { get; set; }
        public long? MoldId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string MoldSerial { get; set; } = string.Empty;
        public string MoldName { get; set; } = string.Empty;
    }
}
