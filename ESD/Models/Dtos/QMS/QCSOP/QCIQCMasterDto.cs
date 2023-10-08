using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using ESD.Models.Dtos.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos
{
    public class QCIQCMasterDto : BaseModel
    {
        public long QCIQCMasterId { get; set; }
        public string QCIQCMasterName { get; set; } = string.Empty;
        public string IQCType { get; set; } = string.Empty;
        public string Explain { get; set; } = string.Empty;
        public bool IsConfirm { get; set; } = false;
        public bool showDelete { get; set; }

        //ngoai bien 
        public string IQCTypeName { get; set; } = string.Empty;
    }

    public class QCIQCDetailMDto : BaseModel
    {
        public long QCIQCDetailMId { get; set; }
        public long QCIQCMasterId { get; set; }
        public long QCTypeId { get; set; }
        public long QCItemId { get; set; }
        public long? QCStandardId { get; set; }

        //ngoại biến
        public string QCTypeName { get; set; } = string.Empty;
        public string QCItemName { get; set; } = string.Empty;
        public string? QCStandardName { get; set; }

        public decimal TextValue { get; set; } 
    }

    public class QCIQCDetailRMDto : BaseModel
    {
        public long? QCIQCDetailRMId { get; set; }
        public long? QCIQCMasterId { get; set; }
        public long? QCTypeId { get; set; }
        public long? QCItemId { get; set; }
        public long? QCStandardId { get; set; }

        //ngoại biến
        public string QCItemName { get; set; } = string.Empty;
        public string QCTypeName { get; set; } = string.Empty;
        public string QCStandardName { get; set; } = string.Empty;

        public int? TextValue { get; set; }
    }

    public class CheckMaterialLotDto : BaseModel
    {
        public long? MaterialLotId { get; set; }
        public long? QCIQCMasterId { get; set; }
        public long? StaffId { get; set; }
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public int? TotalQty { get; set; }
        public int? NGQty { get; set; }
        public List<QCIQCDetailMDto>? CheckValue { get; set; }
    }

    public class CheckRawMaterialLotDto : BaseModel
    {
        public long? MaterialLotId { get; set; }
        public long? QCIQCMasterId { get; set; }
        public long? StaffId { get; set; }
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public List<QCIQCDetailRMDto>? CheckValue { get; set; }
    }

    public class CheckMoldDto : BaseModel
    {
        public long? MoldId { get; set; }
        public long? BladeId { get; set; }
        public long? QCMoldMasterId { get; set; }
        public int? CheckTime { get; set; }
        public long? StaffId { get; set; }
        public DateTime? CheckDate { get; set; }
        public bool? CheckResult { get; set; }
        public List<QCMoldDetail>? CheckValue { get; set; }
    }
    public class QCMoldDetail : BaseModel
    {
        public long? QCMoldDetailId { get; set; }
        public long? QCMoldMasterId { get; set; }
        public long? QCTypeId { get; set; }
        public long? QCItemId { get; set; }
        public long? QCStandardId { get; set; }

        //ngoại biến
        public string QCItemName { get; set; } = string.Empty;
        public string QCTypeName { get; set; } = string.Empty;
        public string QCStandardName { get; set; } = string.Empty;

        public int? TextValue { get; set; }
    }

}
