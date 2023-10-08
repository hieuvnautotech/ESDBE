using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class ProductDto : BaseModel
    {
        public long ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public long ModelId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string SSVersion { get; set; } = string.Empty;
        public long ProductType { get; set; }
        public string Vendor { get; set; } = string.Empty;
        public decimal? PackingAmount { get; set; }
        public int? ExpiryMonth { get; set; }
        public string Temperature { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Stamps { get; set; } = string.Empty;
        public string RemarkBuyer { get; set; } = string.Empty;

        //ngoại biến
        public string ModelCode { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;

        public bool? showDelete { get; set; }
        public decimal? ActualQty { get; set; }
    }

    public partial class ProductExcelDto
    {
        public string ProductCode { get; set; }
        public string Model { get; set; }
        public decimal Inch { get; set; }
        public string ProductTypeCode { get; set; }
        public string QCMasterCode { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}
