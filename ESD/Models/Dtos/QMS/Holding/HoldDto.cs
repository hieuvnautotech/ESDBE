using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos
{
    public class HoldDto : BaseModel
    {
        public long? HoldLogId { get; set; }
        public long? MaterialLotId { get; set; }
        public long? WOSemiLotAPPId { get; set; }
        public long? WOSemiLotMMSId { get; set; }
        public string? Reason { get; set; }
        public bool? IsPicture { get; set; }
        public string? FileName { get; set; }
        public bool? IsHold { get; set; }
        public string? LotStatus { get; set; }
        public IFormFile? File { get; set; }
        public List<long>? ListId { get; set; }

        public string? MaterialLotCode { get; set; }
        public string? HoldStatus { get; set; }
        public string? HoldStatusName { get; set; }
        public string? LotNo { get; set; }

        public long? QCIQCMasterId { get; set; }
        public long? StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public int? CheckResult { get; set; }
        public DateTime? CheckDate { get; set; }
    }
}
