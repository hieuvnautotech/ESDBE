using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos.Common
{
    public class EffectiveDto
    {
        public long ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string? ModelCode { get; set; }
        public string? ProcessCode { get; set; }
        public int Target { get; set; }
        public string? MMSCode { get; set; }
        public string? FQCCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ProcessQty
    {
        public int OKQty { get; set; }
        public int NGQty { get; set; }
        public int WaitingQty { get; set; }
        public decimal Effective { get; set; }
    }

    public class KPIQCDto
    {
        public string? WOCode { get; set; }
        public string? ProductCode { get; set; }
        public string? ProcessCode { get; set; }
    }
}
