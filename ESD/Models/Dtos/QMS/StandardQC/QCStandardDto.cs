using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using ESD.Models.Dtos.StandardQC;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos.StandardQC
{
    public class QCStandardDto : BaseModel
    {
        [Key]
        public long QCStandardId { get; set; }
        public long QCTypeId { get; set; }
        public long QCItemId { get; set; }
        [Required]
        [StringLength(255)]
        public string QCName { get; set; } = string.Empty;
        [Required]
        public long QCApply { get; set; }
        public bool showDelete { get; set; }

        //ngoaij bien
        public string QCApplyName { get; set; } = string.Empty;
        public string QCFullName { get; set; } = string.Empty;
        public string QCTypeName { get; set; } = string.Empty;
        public string QCItemName { get; set; } = string.Empty;

    }
}
