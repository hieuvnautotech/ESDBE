﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ESD.Models
{
    public partial class Material_Buyer
    {
        [Key]
        public long MaterialId { get; set; }
        [Key]
        public long BuyerId { get; set; }
        [Required]
        public bool? isActived { get; set; }
        [Precision(3)]
        public DateTime createdDate { get; set; }
        public long createdBy { get; set; }
        [Precision(3)]
        public DateTime? modifiedDate { get; set; }
        public long? modifiedBy { get; set; }
        public byte[] row_version { get; set; }

        [ForeignKey("BuyerId")]
        [InverseProperty("Material_Buyer")]
        public virtual Buyer Buyer { get; set; }
        [ForeignKey("MaterialId")]
        [InverseProperty("Material_Buyer")]
        public virtual Material Material { get; set; }
    }
}