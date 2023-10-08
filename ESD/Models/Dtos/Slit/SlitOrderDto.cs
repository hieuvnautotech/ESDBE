using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.Slit
{
    public class SlitOrderDto : BaseModel
    {
        public long? SlitOrderId { get; set; }
        public long? ProductId { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool? OrderStatus { get; set; } //0 not yet  -  1 finish
        public string Description { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? DeleteStatus { get; set; } //0 not yet  -  1 finish

        //ngoại biến 
        public string OrderStatusName { get; set; } = string.Empty;
    }
    public class SlitOrderDetailDto : BaseModel
    {
        public long? SlitOrderDetailId { get; set; }
        public long? SlitOrderId { get; set; }
        public long? ProductId { get; set; }
        public long? MaterialId { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }
        public int? OrderQty { get; set; }
        public int? DoneQty { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool? DeleteStatus { get; set; }
        public bool? CheckDeleteStatus { get; set; }
        //ngoai bien
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string CDLCode { get; set; } = string.Empty;
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
    }
    public class SlitTurnDto : BaseModel
    {
        public long? SlitTurnId { get; set; }
        public long? SlitOrderId { get; set; }
        public long? ProductId { get; set; }
        public List<StaffDto> StaffIds { get; set; } = new List<StaffDto>();
        public long? BladeId { get; set; }
        public long? LineId { get; set; }
        public long? ParentId { get; set; }
        public string Turn { get; set; } = string.Empty;
        public long? MaterialLotId { get; set; }
        public string MaterialLotCode { get; set; } = string.Empty;
        public long? MaterialTypeId { get; set; }
        public long? IQCMaterialId { get; set; }
        public string MaterialTypeName { get; set; } = string.Empty;
        public int? SlitQty { get; set; }
        public int? OriginWidth { get; set; }
        public int? OriginLength { get; set; }
        public int? Width { get; set; }
        public int? Length { get; set; }
        public bool? IsFinish { get; set; }
        public bool? IsParent { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string LineName { get; set; } = string.Empty;
        public long? MaterialId { get; set; }
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public string LotNo { get; set; } = string.Empty;
        public long? LotStatus { get; set; }
        public string LotStatusName { get; set; } = string.Empty;
        public string BladeSerial { get; set; } = string.Empty;
        public string BladeName { get; set; } = string.Empty;
        public string StaffNameSlit { get; set; } = string.Empty;
        public DateTime? ReceivedDate { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsFullLength { get; set; }
        public int? LossWidth { get; set; }


        //check IQC
        public string StaffName { get; set; } = string.Empty;
        public long? StaffId { get; set; }
        public long? QCIQCMasterId { get; set; }
        public DateTime? CheckDate { get; set; }
        public int? CheckResult { get; set; }
    }

    public class SlitSplitDto
    {
        public long? ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int? Width { get; set; }
        public int? Length { get; set; }

    }
}
