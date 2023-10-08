using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class LocationDto : BaseModel
    {
        public long? LocationId { get; set; }
        public string? LocationCode { get; set; }
        public string? AreaCode { get; set; }
        public string? AreaName { get; set; }
    }
    public partial class LocationExcelDto
    {
        public string LocationCode { get; set; }
        public string AreaCode { get; set; }

    }
}
