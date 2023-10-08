using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class HMICountingDto
    {
        public long Id { get; set; }
        public long WoId { get; set; }
        public long MoldId { get; set; }
        public string MACAddress { get; set; }
        public long HMIStatus { get; set; }
        public short? PostQty { get; set; }
        public DateTime? EventTime { get; set; }
        public string Remark { get; set; }
        public byte[] row_version { get; set; }

        //Required Properties
        public string WoCode { get; set; } = string.Empty;
        public string HMIStatusName { get; set; } = string.Empty;
    }
}
