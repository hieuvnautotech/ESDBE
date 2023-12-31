﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ESD.Models
{
    [Index("StaffCode", Name = "UC_Staff_StaffCode", IsUnique = true)]
    public partial class Staff
    {
        [Key]
        public long StaffId { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string StaffCode { get; set; }
        [StringLength(100)]
        public string StaffName { get; set; }
        public string Contact { get; set; }
        [Required]
        public bool? isActived { get; set; }
        [Precision(3)]
        public DateTime createdDate { get; set; }
        public long createdBy { get; set; }
        [Precision(3)]
        public DateTime? modifiedDate { get; set; }
        public long? modifiedBy { get; set; }
        public byte[] row_version { get; set; }
    }
}