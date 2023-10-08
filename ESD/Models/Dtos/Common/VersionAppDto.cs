using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos.Common
{
    public class VersionAppDto
    {
        public int? id_app { get; set; }
        public string name_file { get; set; } = string.Empty;
        //public int? version { get; set; }
        public string app_version { get; set; }
        public string url { get; set; }
        public string change_date { get; set; } = string.Empty;
        public bool newVersion { get; set; }
        public IFormFile? file { get; set; }
        public long? createdBy { get; set; } = default;
        public byte[] row_version { get; set; }
    }
}
