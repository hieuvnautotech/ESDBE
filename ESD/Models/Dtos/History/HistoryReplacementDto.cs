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
    public partial class HistoryReplacementDto : BaseModel
    {
        public string? BuyerQR { get; set; }
        public string? SemiLotCode { get; set; }
        public string? MaterialLotCode { get; set; }
        public string? WOCode { get; set; }
        public string? process { get; set; }
        public string? process_cd { get; set; }
        public string? remark { get; set; }
        public string? worker { get; set; }
        public string? FactoryName { get; set; }
        public int? ok_qty { get; set; }
        public int? ng_qty { get; set; }
        public int? total { get; set; }
        public int? insertqty { get; set; }
        public int? counthangbu { get; set; }
        public DateTime? date_working { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}