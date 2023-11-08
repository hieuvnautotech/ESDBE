using Apache.IoTDB;
using Apache.IoTDB.DataStructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project1.Dtos;
using Project1.Services;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Project1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IOT3Controller : ControllerBase
    {

        private readonly ILogger<IOT3Controller> _logger;

        private readonly IIotDBService _ioTService;


        public IOT3Controller(IIotDBService ioTService, ILogger<IOT3Controller> logger)
        {
            _logger = logger;
            _ioTService = ioTService;
        }

        [HttpGet()]
        public async Task<List<Sensors_modelview>> Get(string? keyword, decimal? value_search)
        {

            var query = "";
            var lst_sensors = new List<string>();
            if (keyword !="null")
            {
               
                if ("temperature".Contains(keyword))
                    lst_sensors.Add("temperature");
                   
                else if ("ph".Contains (keyword)) {
                    lst_sensors.Add("ph");
                } else
                {
                    lst_sensors.Add("_");

                }

                query = $"select {string.Join(",", lst_sensors)} from root.ln.sensors ";

                
            } else
            {
                query= $"select * from root.ln.sensors";
            }


          
            if (value_search != null) 
            {
                query += $" where ";
                if (lst_sensors.Count > 0)
                {
                    foreach (var s in lst_sensors)
                    {
                        query += $" {s} > {value_search}";
                    }
                } else
                {
                    query += $" temperature > {value_search} or ph > {value_search} ";
                }

            }

            query += " order by time desc limit 10";

           var db = await _ioTService.get_session_pool();
            var res = await db.ExecuteQueryStatementAsync(query);
            var list_data = new List<Sensors_modelview>();

            try
            {
                var index = 0;
                while (res.HasNext())
                {
                    RowRecord row = res.Next();

                    index++;
                    var data_row = new Sensors_modelview();
                    for (var i = 0; i < row.Measurements.Count; i++)
                    {
                         var label = row.Measurements[i];
                        decimal? value = row.Values[i] == DBNull.Value ? null : Convert.ToDecimal(row.Values[i]);
                        data_row.created_date = row.GetDateTime();


                        if (label == "root.ln.sensors.ph")
                            data_row.ph = value;
                        else if (label == "root.ln.sensors.temperature")
                        {
                            data_row.temperture = value;
                        }


                    }

                    data_row.id = index;
                    list_data.Add(data_row);

                }
            } finally
            {
                await res.Close();
            }
           
            

            return list_data;
        }



        [HttpPost("temperature")]
        [AllowAnonymous]
        public async Task<IActionResult> PostTemperature([FromBody] sensor_postdata data )
        {

            if (data.value!=null)
            {
                var db = await _ioTService.get_session_pool();
               
                await db.ExecuteNonQueryStatementAsync($"insert into root.ln.sensors(temperature) values({data.value})");
            

            }


            return Ok();

        }

        [HttpPost("ph")]
        [AllowAnonymous]
        public async Task<IActionResult> PostPH([FromBody] sensor_postdata data)
        {

            if (data.value != null)
            {
                var db = await _ioTService.get_session_pool();

                await db.ExecuteNonQueryStatementAsync($"insert into root.ln.sensors(ph) values({data.value})");


            }


            return Ok();

        }






    }
}