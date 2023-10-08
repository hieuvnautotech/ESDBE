using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class DeliveryOrderDto : BaseModel
    {
        public long DoId { get; set; }
        public string DoCode { get; set; }
        public long FPOId { get; set; }
        public long MaterialId { get; set; }
        public int? OrderQty { get; set; }
        public int? RemainQty { get; set; }
        public string PackingNote { get; set; }
        public string InvoiceNo { get; set; }
        public string Dock { get; set; }
        public DateTime? ETDLoad { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string Remark { get; set; }
        public string Truck { get; set; }

        //Required Properties
        public long? FPoMasterId { get; set; }
        public string FPoMasterCode { get; set; }
        public string FPoCode { get; set; }
        public string MaterialCode { get; set; }
        public long BuyerId { get; set; }
        public string? BuyerCode { get; set; }
        public string? MaterialBuyerCode { get; set; }
        public int? Year { get; set; }
        public byte? Week { get; set; }

        public DeliveryOrderDto()
        {
            DoCode = string.Empty;
            PackingNote = string.Empty;
            InvoiceNo = string.Empty;
            Dock = string.Empty;
            Truck = string.Empty;
            Remark = string.Empty;
            FPoCode = string.Empty;
            MaterialCode = string.Empty;
        }
    }

    public partial class DeliveryOrderExcelDto
    {
        public string? FPoMasterCode { get; set; }
        public int? Year { get; set; }
        public byte? Week { get; set; }
        public string? MaterialCode { get; set; }
        //public string? DoCode { get; set; }
        public string? ETDLoad { get; set; }
        public string? DeliveryTime { get; set; }
        public int? OrderQty { get; set; }
        public string? PackingNote { get; set; }
        public string? Dock { get; set; }
        public string? Truck { get; set; }
        public string? InvoiceNo { get; set; }
        public string? Remark { get; set; }
    }
}
