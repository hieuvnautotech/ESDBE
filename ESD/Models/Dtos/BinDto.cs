using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class BinDto
    {
        public long BinId { get; set; }
        public string BinCode { get; set; }
        public long ShelfId { get; set; }
        public byte BinLevel { get; set; }
        public byte BinIndex { get; set; }
        public byte? BinStatus { get; set; }
        public string? ESLCode { get; set; }
        public bool? isActived { get; set; }
        public byte[]? row_version { get; set; }


        //Required Properties
        public string ShelfCode { get; set; }
        public byte TotalLevel { get; set; }
        public byte BinPerLevel { get; set; }

        public BinDto()
        {
            BinCode = string.Empty;
            ShelfCode = string.Empty;
        }
    }

    public class BinEditDto
    {
        public long ShelfId { get; set; }
        public int Action { get; set; } //1 tăng BinPerLevel, 2 giảm BinPerLevel, 3 tăng TotalLevel, 4 giảm TotalLevel
        public long? createdBy { get; set; } = default;
    }
}
