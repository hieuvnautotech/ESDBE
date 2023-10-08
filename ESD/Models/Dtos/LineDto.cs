using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class LineDto : BaseModel
    {
        public long? LineId { get; set; }
        public string LineName { get; set; } = string.Empty;
        //public string? HMIMacAddress { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool? IsUsed { get; set; }
        public long? LocationId { get; set; }
        public string AreaCode { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
    public partial class LineExcelDto
    {
        public string LineName { get; set; }
        public string Description { get; set; }

    }
}
