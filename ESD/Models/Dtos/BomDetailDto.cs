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
    public partial class BomDetailDto : BaseModel
    {
        public long? BomDetailId { get; set; }
        public long? BomId { get; set; }
        public long? MaterialId { get; set; }
        public decimal? Amount { get; set; }
        public string Remark { get; set; }
        public string MaterialCode { get; set; }
    }
}