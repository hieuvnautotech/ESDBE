using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.FQC
{
    public class WOSemiLotFQCDto : BaseModel
    {
        public long? WOSemiLotFQCId { get; set; }
        public long? WOId { get; set; }
        public long? WOProcessId { get; set; }
        public string SemiLotNo { get; set; } = string.Empty;
        public string OriginSemiLotCode { get; set; } = string.Empty;
        public string SemiLotCode { get; set; } = string.Empty;
        public string WOCode { get; set; } = string.Empty;
        public int? OriginQty { get; set; } 
        public int? ActualQty { get; set; }
        public string LotStatus { get; set; } = string.Empty;
        public string LotStatusName { get; set; } = string.Empty;
        public long? LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public DateTime? ReceivedDate { get; set; }
        public DateTime? createdDateSearch { get; set; }
        public string Description { get; set; } = string.Empty;
        public long? WOProcessStaffId { get; set; }
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string PressLotCode { get; set; } = string.Empty;
        public string WorkOrder { get; set; } = string.Empty;
        public string ModelCode { get; set; } = string.Empty;
        public string BuyerQR { get; set; } = string.Empty;
        public long? WOProcessStaffOQCId { get; set; }
        public long? WOProcessOQCId { get; set; }
        public DateTime? MappingOQCDate { get; set; }
        public string Shift { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
        //check oqc
        public long? QCOQCMasterId { get; set; }
        public string QCOQCMasterName { get; set; } = string.Empty;
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public long? StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string LocationCode { get; set; } = string.Empty;
        public string AreaCode { get; set; } = string.Empty;
    }
    public partial class WOSemiLotFQCQCDto : BaseModel
    {
        public long? WOSemiLotFQCQCId { get; set; }
        public string SemiLotCode { get; set; } = string.Empty;
        public string MaterialLotCode { get; set; } = string.Empty;
        public int? ActualQty { get; set; }
        public int? CheckQty { get; set; }
        public int? OKQty { get; set; }
        public int? NGQty { get; set; }
        public int? RemainQty { get; set; }
        public List<WOSemiLotAPPCheckDetailDto?> CheckValue { get; set; }
    }
    public partial class WOSemiLotAPPCheckDetailDto
    {
        public int? Id { get; set; }
        public long? QCFQCDetailId { get; set; }
        public int? TextValue { get; set; }
    }

    public partial class WOSemiLotFQCDetailDto : BaseModel
    {
        public long? WOSemiLotFQCIdOutput { get; set; }
        public long? WOSemiLotDetailId { get; set; }
        public long? WOSemiLotFQCId { get; set; }
        public string SemiLotCode { get; set; } = string.Empty;
        public string MaterialLotCode { get; set; } = string.Empty;
        public bool? IsFinish { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? MappingDate { get; set; } = default;
        public int? OriginQty { get; set; }
        public int? ActualQty { get; set; }
        public int? AddQty { get; set; }

        //NGOẠI BIẾN 
        public long? QCFQCMasterId { get; set; }
        public string QCFQCMasterName { get; set; } = string.Empty;
    }

    public class WOProcessMaxLevelDto
    {
        public int? ProcessLevel { get; set; }
        public long? WOProcessId { get; set; }
        public string Product { get; set; } = string.Empty;
        public string NameProcess { get; set; } = string.Empty;

    }
    public partial class WOProcessWorkedDto : BaseModel
    {
        public long? WOSemiLotFQCId { get; set; }
        public string SemiLotCode { get; set; } = string.Empty;
        public string MaterialLotCode { get; set; } = string.Empty;
        public string ProcessCode { get; set; } = string.Empty;
        public string ProcessLevel { get; set; } = string.Empty;
    }
}
