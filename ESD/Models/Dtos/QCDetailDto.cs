﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class QCDetailDto : BaseModel
    {
        [Key]
        public long QCDetailId { get; set; }
        public long QCMasterId { get; set; }
        public long QCId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string QCMasterCode { get; set; } = string.Empty;
        public string QCCode { get; set; } = string.Empty;
        public bool showDelete { get; set; }
    }
    public partial class QCDetailExcelDto
    {
        public long QCMasterId { get; set; }
        public string QCCode { get; set; }

    }
}
