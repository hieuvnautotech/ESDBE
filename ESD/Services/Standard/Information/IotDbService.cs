using Apache.IoTDB;
using Apache.IoTDB.DataStructure;

namespace Project1.Services
{
    public interface IIotDBService
    {
        //Task<SessionDataSet> Query(string query);
         Task<SessionPool> get_session_pool();

    }
    public class IotDbService : IIotDBService
    {
        private SessionPool session_pool;
        string host = "127.0.0.1";
        int port = 6667;
        string user = "root";
        string passwd = "root";
        int pool_size = 2;

    

        public async Task<SessionPool> get_session_pool()
        {
            var iscallOpen = false;
            if (session_pool == null)
            {
                iscallOpen = true;
            }
            else if (!session_pool.IsOpen())
            {
                session_pool.Dispose();
                iscallOpen = true;
            }
            if (iscallOpen)
            {
                session_pool = new SessionPool(host, port, user, passwd, pool_size);
                await session_pool.Open(false);
                await session_pool.SetTimeZone("+07:00");

            //    var ifExist_ph = await session_pool.CheckTimeSeriesExistsAsync(
            // string.Format("{0}.{1}.{2}", "root.ln", "sensors", "ph"));

            //    if (!ifExist_ph)
            //    {
            //        await session_pool.ExecuteNonQueryStatementAsync($"create timeseries root.ln.sensors.ph with datatype=FLOAT,encoding=RLE");
            //    }

            //    var ifExist_temperature = await session_pool.CheckTimeSeriesExistsAsync(
            //string.Format("{0}.{1}.{2}", "root.ln", "sensors", "temperature"));
            //    if (!ifExist_temperature)
            //    {
            //        await session_pool.ExecuteNonQueryStatementAsync($"create timeseries root.ln.sensors.temperature with datatype=FLOAT,encoding=RLE");
            //    }




            }

          


            return session_pool;//.ExecuteQueryStatementAsync(query);

        }
    }


  
}
