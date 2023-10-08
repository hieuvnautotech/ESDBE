using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESD.Models.Dtos
{
    public class MaterialSODetailDto 
    {
        public long MsoDetailId { get; set; }
        public long MsoId { get; set; }
        public long MaterialId { get; set; }
        public long? LotId { get; set; }
        public decimal? SOrderQty { get; set; } = 0;
        public bool? MsoDetailStatus { get; set; } = false;// 0: not finished, 1: finished

        //Required Properties
        public string MsoCode { get; set; } = string.Empty;
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialColorCode { get; set; } = string.Empty;
        public long? BinId { get; set; }
        public string? BinCode { get; set; }
        public string? LotSerial { get; set; }
        public decimal? Qty { get; set; } = 0; //SL Lot
        public decimal? TotalSOQty { get; set; } = 0; //SL TotalSOQty
        public DateTime? IncomingDate { get; set; }
        public byte[]? row_version { get; set; } = default;

        public List<long>? LotIds { get; set; }
        public DateTime? PickingDate { get; set; }
        public long? PickingUserId { get; set; } = default;

        public string? pickingUser { get; set; }

        public List<LotDto>? LotDtos { get; set; }

        public string LotCode { get; set; }

        public string Description { get; set; }
    }

    public class MaterialSODetailCreateDto
    {
        public long MsoId { get; set; }
        public List<LotDto>? LotDtos { get; set; }
    }
}
