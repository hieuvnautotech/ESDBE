using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class Buyer2Dto: BaseModel
    {
        public long BuyerId { get; set; }
        public string BuyerCode { get; set; } = string.Empty;
        public string BuyerName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Tax { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime? DateSignContract { get; set; }
    }
    //public partial class BuyerExcelDto
    //{
    //    public string Buyer2Code { get; set; }
    //    public string Buyer2Name { get; set; }
    //    public string Description { get; set; }
    //    public string Contact { get; set; }

    //}
}
