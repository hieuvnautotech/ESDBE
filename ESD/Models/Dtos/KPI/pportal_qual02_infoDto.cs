using System.ComponentModel.DataAnnotations;

namespace ESD.Models.Dtos.Common
{
    public class pportal_qual02_infoDto : BaseModel
    {
        public long id { get; set; }
        public string? BUYER_COMPANY { get; set; }
        public string? BUYER_DIVISION { get; set; }
        public string? SELLER_COMPANY { get; set; }
        public string? SELLER_DIVISION { get; set; }
        public string? GBM { get; set; }
        public string? PPORTAL_ITEM_GROUP { get; set; }
        public string? QMS_ITEM_GROUP { get; set; }
        public string? ITEM_CODE { get; set; }
        public int? CTQ_NO { get; set; }
        public string? YYYYMMDDHH { get; set; }
        public string? PRC_QUAL_INFO01 { get; set; }
        public string? PRC_QUAL_INFO02 { get; set; }
        public string? PRC_QUAL_INFO03 { get; set; }
        public string? PRC_QUAL_INFO04 { get; set; }
        public string? PRC_QUAL_INFO05 { get; set; }
        public string? PRC_QUAL_INFO06 { get; set; }
        public string? PRC_QUAL_INFO07 { get; set; }
        public string? PRC_QUAL_INFO08 { get; set; }
        public string? PRC_QUAL_INFO09 { get; set; }
        public string? PRC_QUAL_INFO10 { get; set; }
        public string? PRC_QUAL_INFO11 { get; set; }
        public string? PRC_QUAL_INFO12 { get; set; }
        public string? PRC_QUAL_INFO13 { get; set; }
        public string? PRC_QUAL_INFO14 { get; set; }
        public string? PRC_QUAL_INFO15 { get; set; }
        public string? TRANSACTION_ID { get; set; }
        public string? STATUS { get; set; }
        public string? ERR_FLAG { get; set; }
        public string? SUP_CREATE_DATE { get; set; }
        public string? SUP_CREATE_TIME { get; set; }
        public string? SUP_DATE_ADDED { get; set; }
        public string? SUP_TIME_ADDED { get; set; }
        public string? SUP_SEND_DATE { get; set; }
        public string? namehmi { get; set; }
        public string? SUP_SEND_TIME { get; set; }
        public string? FIELD1 { get; set; }
        public string? FIELD2 { get; set; }
        public string? FIELD3 { get; set; }
        public string? FIELD4 { get; set; }
        public string? FIELD5 { get; set; }
        public string? file_nm { get; set; }
        public string? TRAND_TP { get; set; }
        public string? TRAND_TP_NAME { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? reg_dt { get; set; }
        public DateTime? chg_dt { get; set; }

    }
}
