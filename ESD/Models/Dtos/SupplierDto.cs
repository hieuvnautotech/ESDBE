using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class SupplierDto: BaseModel
    {
        public long SupplierId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string SupplierCode { get; set; } = string.Empty;
        [StringLength(200)]
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Tax { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime? DateSignContract { get; set; }
        public DateTime? DebtDate { get; set; }
    }
    public partial class SupplierExcelDto
    {
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string ResinULCode { get; set; }
        public string SupplierContact { get; set; }

    }
}
