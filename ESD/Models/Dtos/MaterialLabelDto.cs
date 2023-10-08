using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos
{
    public class MaterialLabelDto
    {
        public long? MaterialLotId { get; set; }
        public string? MaterialLotCode { get; set; }
        public string? MaterialName { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string? LotNo { get; set; }
        public int? Length { get; set; }
        public string? MaterialCode { get; set; }
    }
    public class MaterialSplitDetailDto : BaseModel
    {
        public long? MaterialLotParentId { get; set; }
        public long? MaterialId { get; set; }
        public long? MaterialLotId { get; set; }
        public long? MaterialLotChildId { get; set; }
        public string MaterialLotCode { get; set; }
        public int? Length { get; set; }
        public string Type { get; set; }
    }
    public class SplitMaterial
    {
        public string? MaterialLotId { get; set; }
        public string? MaterialLotCode { get; set; }
        public string? MaterialLotCode2 { get; set; }
        public string? MaterialLotId2 { get; set; }
        public int? Length { get; set; }
    }

    public class MergeMaterial : BaseModel
    {
        public string? MaterialLotId1 { get; set; }
        public string? MaterialLotId2 { get; set; }
    }
}
