using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class ShelfDto : BaseModel
    {
        public long ShelfId { get; set; }
        public string ShelfCode { get; set; }
        public long LocationId { get; set; }
        public byte TotalLevel { get; set; }
        public byte BinPerLevel { get; set; }

        //Required Properties
        public string LocationCode { get; set; }

        public virtual ICollection<BinDto> Bin { get; set; }

        public ShelfDto()
        {
            ShelfCode = string.Empty;
            LocationCode = string.Empty;
            Bin = new HashSet<BinDto>();
        }
    }
}
