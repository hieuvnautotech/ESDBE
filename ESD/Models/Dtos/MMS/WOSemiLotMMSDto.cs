using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.MMS
{
    public class WOSemiLotMMSDto : BaseModel
    {
        public long? WOSemiLotMMSId { get; set; }
        public long? WOProcessId { get; set; }
        public string SemiLotNo { get; set; } = string.Empty;
        public string SemiLotCode { get; set; } = string.Empty;
        public int? OriginQty { get; set; }
        public int? ActualQty { get; set; }
        public string? LotStatus { get; set; }
        public string LotStatusName { get; set; } = string.Empty;
        public long? LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public DateTime? ReceivedDate { get; set; }
        public string Description { get; set; } = string.Empty;

        public long? WOId { get; set; }
        public int? countRowMMSDetail { get; set; }

        //check pqc
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public long? StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;

        //print 
        public string ProductCode { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string LineName { get; set; } = string.Empty;
        public string PressLotCode { get; set; } = string.Empty;
        public string AreaCode { get; set; } = string.Empty;
       
    }
}
