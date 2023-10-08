using ESD.Models.Dtos.Common;


namespace ESD.Models.Dtos
{
    public partial class WOMoldPressingTimesDto : BaseModel
    {
        public long? WOMoldPressingId { get; set; }
        public long? MoldId { get; set; }
        public long? WOProcessId { get; set; }
        public string MoldSerial { get; set; }
        public string MoldName { get; set; }
        public string WOCode { get; set; }
        public string ProductCode { get; set; }
        public string Model { get; set; }
        public string LineName { get; set; }
        public int? CurrentNumber { get; set; }
        public int? PressingTimes { get; set; }
        public int? order_number { get; set; }
        public int? Step { get; set; }
    }
}