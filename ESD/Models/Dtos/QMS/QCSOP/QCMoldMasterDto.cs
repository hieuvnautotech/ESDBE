﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>;
using ESD.Models.Dtos.Common;

namespace ESD.Models.Dtos
{
    public partial class QCMoldMasterDto : BaseModel
    {
        public long? QCMoldMasterId { get; set; }
        public string QCMoldMasterName { get; set; }
        public string Explain { get; set; }
        public Boolean? IsConfirm { get; set; }
        public long? ProductId { get; set; }
    }

    public partial class QCMoldDetailDto : BaseModel
    {
        public string SemiLotCode { get; set; } = string.Empty;
        public string MaterialLotCode { get; set; } = string.Empty;
        public long QCMoldDetailId { get; set; }
        public long? QCMoldMasterId { get; set; }
        public long? QCTypeId { get; set; }
        public long? QCItemId { get; set; }
        public long? QCStandardId { get; set; }
        public string? QCTypeName { get; set; }
        public string? QCItemName { get; set; }
        public string? QCStandardName { get; set; }
        public int TextValue { get; set; }
    }
}
