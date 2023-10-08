using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.MMS
{
    public partial class PQCWOProcessCheckMasterASDto : BaseModel
    {
        public long? WOProcessId { get; set; }
        public long? QCPQCMasterId { get; set; }
        public long? StaffId { get; set; }
        public string? StaffCode { get; set; }
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public List<CheckPQCWOProcessDto?> ValueCheck { get; set; }
    }

    public partial class CheckPQCWOProcessDto : BaseModel
    {
        public long? WOProcessId { get; set; }
        public long? QCPQCDetailASId { get; set; }
        public string? TextValue { get; set; }
        public int Sample { get; set; }
    }

    public partial class PQCWOProcessDetailASDto : BaseModel
    {
        public long? WOProcessId { get; set; }
        public long? QCPQCDetailASId { get; set; }
        public long? QCTypeId { get; set; }
        public long? QCItemId { get; set; }
        public long? QCStandardId { get; set; }
        public long? QCToolId { get; set; }
        public string? QCTypeName { get; set; }
        public string? QCItemName { get; set; }
        public string? QCStandardName { get; set; }
        public string? QCToolName { get; set; }
        public string? QCFrequencyName { get; set; }
        public string? TextValue { get; set; }
        public int Sample { get; set; }
    }
}
