using ESD.Models.Dtos.Common;
using System.Reflection.Emit;

namespace ESD.Models.Dtos
{
    public class ForecastPODto : BaseModel
    {
        public long? FPoMasterId { get; set; }
        public string FPoMasterCode { get; set; }
        public long FPOId { get; set; }
        public string FPoCode { get; set; }
        public long? MaterialId { get; set; }
        public long? BuyerId { get; set; }
        public long? LineId { get; set; }
        public int? Week { get; set; }
        public int? Year { get; set; }
        public int? Amount { get; set; }
        public int? OrderQty { get; set; }
        public string LineName { get; set; }
        public string MaterialCode { get; set; }
        public string BuyerCode { get; set; }
        public float Inch { get; set; }

        public string Description { get; set; }
        public string DescriptionMaterial { get; set; }

        //Required Properties
        public string MaterialBuyerCode { get; set; }
        public string GroupMaterial { get; set; }

        public ForecastPODto()
        {
            FPoMasterCode= string.Empty;
            FPoCode = string.Empty;
            LineName = string.Empty;
            MaterialCode = string.Empty;
            BuyerCode = string.Empty;
            Description = string.Empty;
            DescriptionMaterial = string.Empty;
            MaterialBuyerCode = string.Empty;
            GroupMaterial = string.Empty;
        }
    }
    public class SelectMaterial
    {
        public long MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string GroupMaterial { get; set; }

    }
    public class SelectLine
    {
        public long LineId { get; set; }
        public string LineName { get; set; }

    }
    public class SelectYear
    {
        public long YearId { get; set; }
        public string YearName { get; set; }

    }
    public class SelectBuyer
    {
        public long BuyerId { get; set; }
        public string BuyerCode { get; set; }

    }
    public class SelectQC
    {
        public long QcId { get; set; }
        public string QCCode { get; set; }

    }
    public class SelectAisle
    {
        public long LocationId { get; set; }
        public string LocationCode { get; set; }

    }
    public class SelectShelf
    {
        public long ShelfId { get; set; }
        public string ShelfCode { get; set; }
    }
    public class SelectBin
    {
        public long BinId { get; set; }
        public string BinCode { get; set; }
    }

    public class SelectQCMaster
    {
        public long QCMasterId { get; set; }
        public string QCMasterCode { get; set; }
    }
    public partial class DetailForecastPOExcelDto
    {
        public long FPoMasterId { get; set; }
        public string FPoCode { get; set; }
        public string MaterialCode { get; set; }
        public int Amount { get; set; }
        public string BuyerCode { get; set; }
        public string LineName { get; set; } = string.Empty;
        public int Week { get; set; }
        public int Year { get; set; }
    }
}
