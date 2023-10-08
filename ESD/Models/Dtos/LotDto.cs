using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class LotDto : BaseModel
    {
        public long Id { get; set; }
        public string LotCode { get; set; }
        public string LotSerial { get; set; }
        public long MaterialId { get; set; }
        public bool LotStatus { get; set; } = false; //{1: "Received", 0: "Just Created"}
        public decimal? Qty { get; set; }
        public decimal? TotalSOQty { get; set; }
        public DateTime? QCDate { get; set; }
        public bool? QCResult { get; set; } = true; //{1: "OK", 0: "NG"}
        public long? WarehouseType { get; set; } //["MATERIAL", "WIP", "FG"]
        public long? LocationId { get; set; }
        public DateTime? IncomingDate { get; set; }
        public long? BinId { get; set; }

        public long? TrayId { get; set; }
        public string? TrayCode { get; set; }

        //Required Properties
        public string MaterialCode { get; set; }
        public string MaterialColorCode { get; set; }
        public string LocationCode { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public string MaterialDescription { get; set; }
        public string UnitName { get; set; }
        public string? BinCode { get; set; }

        public DateTime? StartSearchingDate { get; set; }
        public DateTime? EndSearchingDate { get; set; }
        public string SupplierCode { get; set; }
        public List<SelectQC>? QcIDList { get; set; }

        public string? QCResult1 { get; set; }
        public string QCCode { get; set; }
        public long? WoId { get; set; }
        public string? WoCode { get; set; }

        public decimal? RequestQty { get; set; }

        public string ESLCode { get; set; }
        public int? ActualQty { get; set; }
        public int? NGQty { get; set; }
        public int? HMIQty { get; set; }

        public string WarehouseTypeName { get; set; }
        public LotDto()
        {
            LotCode = string.Empty;
            MaterialCode = string.Empty;
            MaterialColorCode = string.Empty;
            LocationCode = string.Empty;
        }
    }

    public class WOCreateLotDto : BaseModel
    {
        public long Id { get; set; }
        public int LotNumber { get; set; }
        public long WoId { get; set; }
        public decimal Qty { get; set; }
        public string? LotSerial { get; set; }
        public string? QCResult { get; set; }
        public List<SelectQC>? QcId { get; set; }
    }

    public class ScanLotModel
    {
        public string? LotId { get; set; }
        public string? BinId { get; set; }
        public string? TrayCode { get; set; }
    }

    public class ScanLotActual
    {
        public string? lotId { get; set; }
       public long? woId { get; set; }
    }
    public class DeleteLotActual
    {
        public long? Id { get; set; }
        public long? woId { get; set; }
    }

    public class SplitLotModel
    {
        public string? LotId { get; set; }
        public string? LotId2 { get; set; }
        public decimal? Qty { get; set; }
    }
}
