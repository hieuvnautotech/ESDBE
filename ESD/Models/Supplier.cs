﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ESD.Models
{
    [Index("SupplierCode", Name = "UC_Supplier_SupplierCode", IsUnique = true)]
    public partial class Supplier
    {
        public Supplier()
        {
            Material_Supplier = new HashSet<Material_Supplier>();
        }

        [Key]
        public long SupplierId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string SupplierCode { get; set; }
        [StringLength(200)]
        public string SupplierName { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string ResinULCode { get; set; }
        public string SupplierContact { get; set; }
        [Required]
        public bool? isActived { get; set; }
        [Precision(3)]
        public DateTime createdDate { get; set; }
        public long createdBy { get; set; }
        [Precision(3)]
        public DateTime? modifiedDate { get; set; }
        public long? modifiedBy { get; set; }
        public byte[] row_version { get; set; }

        [InverseProperty("Supplier")]
        public virtual ICollection<Material_Supplier> Material_Supplier { get; set; }
    }
}