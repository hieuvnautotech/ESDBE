using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.FQC
{
    public class FQCShippingDto : BaseModel
    {
        public long? FQCSOId { get; set; }
        public string FQCSOName { get; set; } = string.Empty;
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public bool? FQCSOStatus { get; set; } //0 watting  -  1 received
        public int? OrderQty { get; set; }
        public int? ScanQty { get; set; }
        public int? ReceivedQty { get; set; }
        public int? EAQty { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string Description { get; set; } = string.Empty;

        //check oqc
        public long? QCOQCMasterId { get; set; }
        public string QCOQCMasterName { get; set; } = string.Empty;
        public bool? modeCheck { get; set; }
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public long? StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
    }

    public class FQCShippingLotDto : BaseModel
    {
        public long? FQCSOId { get; set; }
        public long? WOSemiLotFQCId { get; set; }
        public bool? LotStatus { get; set; }
        public int? ActualQty { get; set; }
        public long? ProductId { get; set; }
        public long? LocationId { get; set; }
        public string LocationCode { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string SemiLotCode { get; set; } = string.Empty;
        public string BuyerQR { get; set; } = string.Empty;

    }
}
