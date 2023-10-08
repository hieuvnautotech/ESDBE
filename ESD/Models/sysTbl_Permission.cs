﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ESD.Models
{
    [Index("permissionId", "commonDetailId", Name = "UC_PermissionId_CommonDetailId", IsUnique = true)]
    [Index("permissionName", Name = "UC_sysTbl_Permission_permissionName", IsUnique = true)]
    public partial class sysTbl_Permission
    {
        [Key]
        public long permissionId { get; set; }
        [Required]
        [StringLength(50)]
        public string permissionName { get; set; }
        public long commonDetailId { get; set; }
        public bool forRoot { get; set; }
        [Required]
        public bool? isActived { get; set; }
        [Precision(3)]
        public DateTime createdDate { get; set; }
        public long createdBy { get; set; }
        [Precision(3)]
        public DateTime? modifiedDate { get; set; }
        public long? modifiedBy { get; set; }
        public byte[] row_version { get; set; }

        [ForeignKey("commonDetailId")]
        [InverseProperty("sysTbl_Permission")]
        public virtual sysTbl_CommonDetail commonDetail { get; set; }
    }
}