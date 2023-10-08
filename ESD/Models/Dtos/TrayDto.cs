using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class TrayDto : BaseModel
    {
        public long? TrayId { get; set; }
        public string? TrayCode { get; set; }
        public long? TrayType { get; set; }
        public string? TrayTypeName { get; set; }
        public bool? IsReuse { get; set; }
    }
    public partial class TrayExcelDto
    {
        public string TrayCode { get; set; }
        public string TrayTypeCode { get; set; }
        public bool IsReuse { get; set; }
   
    }
}
