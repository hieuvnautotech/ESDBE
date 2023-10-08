using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.MMS
{
    public class WOProcessStaffDto : BaseModel
    {
        public long? WOProcessStaffId { get; set; }
        public long? WOProcessId { get; set; }
        public long? StaffId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StaffCode { get; set; } = string.Empty;
        public string StaffName { get; set; } = string.Empty;
        public int? OKQty { get; set; }
        public int? NGQty { get; set; }
    }
}
