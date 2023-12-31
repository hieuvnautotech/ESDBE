﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ESD.Models
{
    public partial class Shelf
    {
        public Shelf()
        {
            Bin = new HashSet<Bin>();
        }

        [Key]
        public long ShelfId { get; set; }
        [Required]
        [StringLength(10)]
        [Unicode(false)]
        public string ShelfCode { get; set; }
        public long LocationId { get; set; }
        public byte TotalLevel { get; set; }
        public byte BinPerLevel { get; set; }
        [Required]
        public bool? isActived { get; set; }
        [Precision(3)]
        public DateTime createdDate { get; set; }
        public long createdBy { get; set; }
        [Precision(3)]
        public DateTime? modifiedDate { get; set; }
        public long? modifiedBy { get; set; }
        public byte[] row_version { get; set; }

        [ForeignKey("LocationId")]
        [InverseProperty("Shelf")]
        public virtual Location Location { get; set; }
        [InverseProperty("Shelf")]
        public virtual ICollection<Bin> Bin { get; set; }
    }
}