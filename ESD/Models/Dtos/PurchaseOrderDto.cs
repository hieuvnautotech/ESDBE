using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class PurchaseOrderDto : BaseModel
    {
        public long? PoId { get; set; }
        public long? MaterialId { get; set; }
        public string? PoCode { get; set; }
        public string? MaterialCode { get; set; }
        public string? Description { get; set; }
        public int? TotalQty { get; set; }
        public int? RemainQty { get; set; }
        public int? Year { get; set; }
        public int? Week { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class CreateByForeCastPO
    {
        public long? POId { get; set; }
        public long? FPOId { get; set; }
        public string? PoCode { get; set; }
        public int? TotalQty { get; set; }
        public long? createdBy { get; set; }
    }
}
