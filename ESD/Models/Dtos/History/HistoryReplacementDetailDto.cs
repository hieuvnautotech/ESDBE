﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos
{
    public partial class HistoryReplacementDetailDto : BaseModel
    {
        public long? WOSemiLotDetailId { get; set; }
        public string? SemiLotCode { get; set; }
        public string? MaterialLotCode { get; set; }
        public string? mt_cd { get; set; }
        public int? real_qty { get; set; }
        public string? worker { get; set; }
        public string? WOCode { get; set; }
        public long? WOId { get; set; }
        public string? remark { get; set; }
        public string? process { get; set; }
        public string? process_cd { get; set; }
        public string? congnhan_time { get; set; }
    }
}