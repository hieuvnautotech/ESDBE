﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ESD.Models
{
    public partial class FGSOPicked
    {
        [Key]
        public long FGsoPickedId { get; set; }
        public long FGsoDetailId { get; set; }
        [Column(TypeName = "decimal(10, 3)")]
        public decimal PickedQty { get; set; }
        public long StaffId { get; set; }

        [ForeignKey("FGsoDetailId")]
        [InverseProperty("FGSOPicked")]
        public virtual FGSODetail FGsoDetail { get; set; }
    }
}