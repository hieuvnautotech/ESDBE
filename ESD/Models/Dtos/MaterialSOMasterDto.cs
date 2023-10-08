using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class MaterialSOMasterDto : BaseModel
    {
        public long MsoId { get; set; }
        public string MsoCode { get; set; }
        public bool? MsoStatus { get; set; }
        public string Requester { get; set; }
        public DateTime? DueDate { get; set; }
        public string Remark { get; set; }

        //Required Properties
        public DateTime? StartSearchingDate { get; set; }
        public DateTime? EndSearchingDate { get; set; }

        public MaterialSOMasterDto()
        {
            MsoCode = string.Empty;
            Requester = string.Empty;
            Remark = string.Empty;
        }
    }
}
