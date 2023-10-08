using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.MMS
{
    public partial class WOSemiLotMMSCheckMasterSLDto : BaseModel
    {
        public long? WOSemiLotMMSId { get; set; }
        public long? WOProcessId { get; set; }
        public long? QCPQCMasterId { get; set; }
        public long? StaffId { get; set; }
        public string StaffCode { get; set; } = string.Empty;
        public string FactoryName { get; set; } = string.Empty;
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public List<WOSemiLotMMSDetailSLDto?> ValueCheck { get; set; }
    }
    public partial class WOSemiLotMMSDetailSLDto : BaseModel
    {
        public long? WOSemiLotMMSId { get; set; }
        public long? QCPQCDetailSLId { get; set; }
        public string Location { get; set; } = string.Empty;
        public string TextValue { get; set; } = string.Empty;
    }

}
