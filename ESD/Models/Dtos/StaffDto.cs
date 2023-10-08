using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class StaffDto : BaseModel
    {
        public long StaffId { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string StaffCode { get; set; } = string.Empty;
        [StringLength(100)]
        public string StaffName { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public long? DeptId { get; set; }
        public string DeptCode { get; set; } = string.Empty;
        public string DeptNameVI { get; set; } = string.Empty;
        public string DeptNameEN { get; set; } = string.Empty;
    }
}
