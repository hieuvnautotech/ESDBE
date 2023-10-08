using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos.Common
{
    public class ExpoTokenDto
    {
        public string ExpoToken { get; set; }
        public bool? isActived { get; set; }
    }
}
