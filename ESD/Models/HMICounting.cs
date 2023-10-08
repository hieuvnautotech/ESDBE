﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ESD.Models
{
    public partial class HMICounting
    {
        [Key]
        public long Id { get; set; }
        public long WoId { get; set; }
        public long MoldId { get; set; }
        [StringLength(18)]
        [Unicode(false)]
        public string MACAddress { get; set; }
        public long HMIStatus { get; set; }
        public short? PostQty { get; set; }
        [Precision(3)]
        public DateTime? EventTime { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        public byte[] row_version { get; set; }

        [ForeignKey("WoId")]
        [InverseProperty("HMICounting")]
        public virtual WorkOrder Wo { get; set; }
    }
}