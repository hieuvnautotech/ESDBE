using ESD.Models;
using ESD.Hubs;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace ESD.SubscribeTableDependencies
{
    public class SubscribeWOTableDependency : ISubscribeTableDependency
    {
        //private static readonly string connectionString = ConnectionString.CONNECTIONSTRING;

        SqlTableDependency<WO> tableDependency;
        private readonly SignalRHub _signalRHub;
        public SubscribeWOTableDependency(SignalRHub signalRHub)
        {
            _signalRHub = signalRHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<WO>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(WO)} SqlTableDependency error: {e.Error.Message}");
        }

        private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<WO> e)
        {
            if(e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                await _signalRHub.GetDisplayWO();
            }
        }
    }
}
