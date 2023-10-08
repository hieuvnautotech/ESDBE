using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.MMS
{
    public class WOProcessMoldStaffLineDto : BaseModel
    {
        public long? WOProcessId { get; set; }
        public long? id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Name { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
    }
}
