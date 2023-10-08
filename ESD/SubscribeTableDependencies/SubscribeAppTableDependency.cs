using ESD.Hubs;
using ESD.Models;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient;
using ESD.Services.Common;

namespace ESD.SubscribeTableDependencies
{
    public class SubscribeAppTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<sysTbl_Version_App> tableDependency;
        private readonly SignalRHub _signalRHub;
        private readonly IExpoTokenService _expoTokenService;
        public SubscribeAppTableDependency(SignalRHub signalRHub, IExpoTokenService expoTokenService)
        {
            _signalRHub = signalRHub;
            _expoTokenService = expoTokenService;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<sysTbl_Version_App>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(sysTbl_Version_App)} SqlTableDependency error: {e.Error.Message}");
        }

        private async void TableDependency_OnChanged(object sender, RecordChangedEventArgs<sysTbl_Version_App> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                await _expoTokenService.PushExpoNotification();
                await _signalRHub.SendAppVersion();
            }
        }
    }
}
