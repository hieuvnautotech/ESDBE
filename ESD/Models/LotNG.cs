﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ESD.Models
{
    public partial class LotNG
    {
        [Key]
        public long LotId { get; set; }
        [Key]
        public long QcId { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }

        [ForeignKey("LotId")]
        [InverseProperty("LotNG")]
        public virtual Lot Lot { get; set; }
        [ForeignKey("QcId")]
        [InverseProperty("LotNG")]
        public virtual QC Qc { get; set; }
    }
}