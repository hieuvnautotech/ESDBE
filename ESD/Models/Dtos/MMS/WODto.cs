using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.MMS
{
    public partial class WODto : BaseModel
    {
        public long? WOId { get; set; }
        public string WOCode { get; set; } = string.Empty;
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string CDLCode { get; set; } = string.Empty;
        public long? BomId { get; set; }
        public string BomVersion { get; set; } = string.Empty;
        public long? ModelId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public int? Target { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool isFinish { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ProcessQty { get; set; }
        public int? OKQty { get; set; }
        public int? NGQty { get; set; }
        public int? Efficiency { get; set; }
    }
    public partial class WOProcessDto : BaseModel
    {
        public long? WOProcessId { get; set; }
        public long? WOId { get; set; }
        public int? ProcessLevel { get; set; }
        public int? Step { get; set; }
        public string ProcessCode { get; set; } = string.Empty;
        public string ProcessName { get; set; } = string.Empty;
        public long? LineId { get; set; }
        public string LineName { get; set; } = string.Empty;
        public long? LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public long? QCPQCMasterId { get; set; }
        public string QCPQCMasterName { get; set; } = string.Empty;
        public string WOCode { get; set; } = string.Empty;
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public long? Model { get; set; }
        public string ModelCode { get; set; } = string.Empty;
        public int? OKQty { get; set; }
        public int? NGQty { get; set; }
        //check pqc
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public long? StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AreaCode { get; set; } = string.Empty;
    }
    public class WODisplayDto
    {
        public int totalWO { get; set; }
        public int totalTarget { get; set; }
        public int totalOK { get; set; }
        public int totalNG { get; set; }
        public List<WODto>? data { get; set; }
    }
}
