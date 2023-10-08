using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class QCMasterDto : BaseModel
    {
        [Key]
        public long QCMasterId { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string QCMasterCode { get; set; } = string.Empty;
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        public long MaterialTypeId { get; set; }
        public long MaterialId { get; set; }
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialTypeName { get; set; } = string.Empty;
        public long QCType { get; set; }
        public string QCTypeName { get; set; } = string.Empty;
        public bool showDelete { get; set; }
    }
    public partial class QCMasterExcelDto
    {
        public string QCMasterCode { get; set; }
        public string QCType { get; set; }
        public string MaterialTypeCode { get; set; }
        public string Description { get; set; }

    }
}
