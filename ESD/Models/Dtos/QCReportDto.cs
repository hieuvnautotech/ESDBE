namespace ESD.Models.Dtos
{
    public partial class QCReportDto
    {
        public long? MaterialId { get; set; }
        public long? ProductId { get; set; }
        public string? Products { get; set; }
        public long? ModelId { get; set; }
        public long? ProjectId { get; set; }
        public DateTime? StartDate { get; set; } = default;
        public DateTime? EndDate { get; set; } = default;
        public int? Type { get; set; } //0 All, 1 Roll, 2 EA, 3 SUS,
        public int? LotorQty { get; set; } //1 Lot, 0 Qty
    }
}
