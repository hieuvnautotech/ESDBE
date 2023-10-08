namespace ESD.Models.Dtos
{
    public class ESLDto
    {
        public BinData data { get; set; }
        public string id { get; set; }
        public string nfc { get; set; }
        public string stationCode { get; set; }

        public ESLDto()
        {
            data = new BinData();
            id = "L010-B-3-1";
            nfc = string.Empty;
            stationCode = "DEFAULT_STATION_CODE";
        }
    }

    public class BinData
    {
        public string? ITEM_NAME { get; set; }
        public string? LOCATION { get; set; }
        public short? SHELVE_LEVEL { get; set; }

        public BinData()
        {
            ITEM_NAME = string.Empty;
            LOCATION = string.Empty;
            SHELVE_LEVEL = short.MinValue;
        }
    }
}
