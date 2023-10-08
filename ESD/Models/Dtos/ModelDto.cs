using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos
{
    public class ModelDto : BaseModel
    {
        public long? ModelId { get; set; }
        public string? ModelCode { get; set; }

    }
}
