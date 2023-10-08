using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESD.Models.Dtos
{
    public class MoldDto : BaseModel
    {
        public long MoldId { get; set; }
        public string? MoldCode { get; set; }
        public string? MoldName { get; set; }
        public string? MoldVersion { get; set; }
        public string? ProductCode { get; set; }
        public string? LineType { get; set; }
        public string? MoldStatus { get; set; }
        public int? CurrentNumber { get; set; }
        public int? MaxNumber { get; set; }
        public int? UsingNumber { get; set; }
        public int? PeriodNumber { get; set; }
        public DateTime? ProductionDate { get; set; }
        public string? Remark { get; set; }
        public long? SupplierId { get; set; }
        public byte? CheckNo { get; set; }
        public long? QCMasterId { get; set; }
        public long? MoldType { get; set; }
        public string? MoldTypeName { get; set; }
        public bool CheckUpdateMoldType { get; set; }

        ////Additional Props
        public long? ProductId { get; set; }
        public string? MoldStatusName { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierCode { get; set; }
        public string? QCMasterName { get; set; }
        public IList<ProductDto>? Products { get; set; }
        public IList<CommonDetailDto>? LineTypes { get; set; }

        //check pqc
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public long? StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
    }
    public partial class MoldExcelDto
    {
        public string MoldSerial { get; set; }
        public string MoldCode { get; set; }
        public string ModelCode { get; set; }
        public decimal Inch { get; set; }
        public string MoldTypeCode { get; set; }
        public string MachineTypeCode { get; set; }
        public DateTime ETADate { get; set; }
        public decimal MachineTon { get; set; }
        public int Cabity { get; set; }
        public bool ETAStatus { get; set; }
        public string Remark { get; set; }

    }

}
