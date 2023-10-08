using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class ForecastPOMasterDto : BaseModel
    {
        public long? FPoMasterId { get; set; }
        public string? FPoMasterCode { get; set; }
        public int? TotalOrderQty { get; set; }
    }
}
