using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.APP
{
    public class OQCDto : BaseModel
    {
        public string ModelCode { get; set; } = string.Empty;
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string PressLotCode { get; set; } = string.Empty;
        public string BuyerQR { get; set; } = string.Empty;
        public string SemiLotCode { get; set; } = string.Empty;
        public string WOCode { get; set; } = string.Empty;
        public int? OriginQty { get; set; }
        public int? ActualQty { get; set; }
        //check oqc
        public long? QCOQCMasterId { get; set; }
        public string QCOQCMasterName { get; set; } = string.Empty;
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public bool? CheckResultPacking { get; set; }
        public long? StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public bool? CheckType { get; set; } //true FVI , false Packing
    }

    public class OQCCheckDto
    {
        public OQCDto Master { get; set; }
        public IEnumerable<QCOQCDetailDto>? Detail { get; set; }

        public OQCCheckDto()
        {
            Master = new OQCDto();
            Detail = new List<QCOQCDetailDto>();
        }

    }
}
