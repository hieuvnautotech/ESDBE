using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos.Common
{
    public class CommonDetailDto : BaseModel
    {
        public long commonDetailId { get; set; }
        public long? commonMasterId { get; set; } = default;
        public string? commonMasterCode { get; set; } = default;
        public string? commonDetailCode { get; set; } = default;
        public string? commonDetailName { get; set; } = default;
        public string? commonDetailLanguge { get; set; } = default;

    }
}
