using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos.WMS.Material
{
    public class MaterialShippingOrderLotDto : BaseModel
    {
        public long? MSODetailId { get; set; }
        public long? MaterialLotId { get; set; }
        public bool? LotStatus { get; set; }
        public string? MaterialLotCode { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public string? MaterialCode { get; set; }
        public string? LocationCode { get; set; }
        public string? AreaCode { get; set; }
        public string? Description { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public long? MMSLocationShelfId { get; set; }
        public long? SlitLocationShelfId { get; set; }
        public string? MMSLocationCode { get; set; }
        public string? SlitLocationCode { get; set; }

        public long? WOProcessId { get; set; }
    }
}
