using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos.Common
{
    public class CommonMasterDto : BaseModel
    {
        public long commonMasterId { get; set; }
        public string? commonMasterName { get; set; }
        public string? commonMasterCode { get; set; }
        public bool forRoot { get; set; }
       
    }
}
