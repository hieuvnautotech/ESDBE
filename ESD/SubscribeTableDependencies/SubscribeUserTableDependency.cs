using ESD.Hubs;
using ESD.Models;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace ESD.SubscribeTableDependencies
{
    public class SubscribeUserTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<sysTbl_User> tableDependency;
        private readonly SignalRHub _signalRHub;
        public SubscribeUserTableDependency(SignalRHub signalRHub)
        {
            _signalRHub = signalRHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<sysTbl_User>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(sysTbl_User)} SqlTableDependency error: {e.Error.Message}");
        }

        private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<sysTbl_User> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                await _signalRHub.SendOnlineUsers();
            }
        }
    }
}
