﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ESD.Models
{
    [Index("DoCode", Name = "UC_DeliveryOrder_DoCode", IsUnique = true)]
    public partial class DeliveryOrder
    {
        [Key]
        public long DoId { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string DoCode { get; set; }
        public long FPOId { get; set; }
        public long MaterialId { get; set; }
        public int OrderQty { get; set; }
        public int? RemainQty { get; set; }
        [StringLength(50)]
        public string PackingNote { get; set; }
        [StringLength(50)]
        public string InvoiceNo { get; set; }
        [StringLength(50)]
        public string Dock { get; set; }
        [Precision(0)]
        public DateTime? ETDLoad { get; set; }
        [Precision(0)]
        public DateTime? DeliveryTime { get; set; }
        [StringLength(200)]
        public string Remark { get; set; }
        [StringLength(50)]
        public string Truck { get; set; }
        [Required]
        public bool? isActived { get; set; }
        [Precision(3)]
        public DateTime createdDate { get; set; }
        public long createdBy { get; set; }
        [Precision(3)]
        public DateTime? modifiedDate { get; set; }
        public long? modifiedBy { get; set; }
        public byte[] row_version { get; set; }

        [ForeignKey("FPOId")]
        [InverseProperty("DeliveryOrder")]
        public virtual ForecastPO FPO { get; set; }
        [ForeignKey("MaterialId")]
        [InverseProperty("DeliveryOrder")]
        public virtual Material Material { get; set; }
    }
}