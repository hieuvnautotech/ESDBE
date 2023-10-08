using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos
{
    public class BladeCheckMasterDto : BaseModel
    {
        public long BladeCheckMasterId { get; set; }
        public long BladeId { get; set; }
        public long QCMoldMasterId { get; set; }
        public string? QCMoldMasterName { get; set; }
        public byte CheckNo { get; set; }
        public bool? UpdateAvailable { get; set; }
        public long? StaffId { get; set; }
        public string? StaffName { get; set; }
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }

        public long QCMoldDetailId { get; set; }
        public long? QCTypeId { get; set; }
        public long? QCItemId { get; set; }
        public long? QCStandardId { get; set; }
        public string? QCTypeName { get; set; }
        public string? QCItemName { get; set; }
        public string? QCStandardName { get; set; }
        public int TextValue { get; set; }
    }
}
